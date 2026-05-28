using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace SVTT
{
    public partial class Main : Form
    {
        private string currentProjectPath = string.Empty;
        private string currentExportPath = string.Empty;
        private readonly List<string> recentFiles = new List<string>();
        private const int MaxRecentFiles = 5;

        // ════════════════════════════════════════════════════════════════
        // ใช้ int counter แทน bool flag
        // bool ธรรมดาพัง: FuncA(true) → FuncB(true) → FuncB(false) → FuncA guard หลุด
        // int counter: ทุก nested call ต้อง Increment/Decrement คู่กันเสมอ
        // ════════════════════════════════════════════════════════════════
        private int _uiUpdateDepth = 0;
        private bool IsUpdatingUI => _uiUpdateDepth > 0;

        private IDisposable BeginUiUpdate()
        {
            _uiUpdateDepth++;
            return new UiUpdateScope(this);
        }

        private void EndUiUpdate() => _uiUpdateDepth = Math.Max(0, _uiUpdateDepth - 1);

        private sealed class UiUpdateScope : IDisposable
        {
            private readonly Main _form;
            private bool _disposed;
            public UiUpdateScope(Main form) => _form = form;
            public void Dispose() { if (!_disposed) { _disposed = true; _form.EndUiUpdate(); } }
        }

        private bool _isLoadingData = false;

        private readonly Dictionary<string, TranslationEntry> masterRegistry = new Dictionary<string, TranslationEntry>();
        private readonly List<string> masterKeysOrder = new List<string>();

        private CancellationTokenSource filterCts;

        private static readonly JsonSerializerOptions JsonSaveOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All)
        };

        private bool _progressUpdatePending = false;

        public Main()
        {
            InitializeComponent();
            EnableDoubleBuffer(dgvTranslations);

            tsCboFilter.SelectedIndex = 0;
            tsCboCategoryFilter.SelectedIndex = 0;
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            filterCts?.Cancel();
            filterCts?.Dispose();
            base.OnFormClosed(e);
        }

        private void EnableDoubleBuffer(Control control)
        {
            var prop = typeof(Control).GetProperty("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance);
            prop?.SetValue(control, true, null);
        }

        #region 📂 โหลดข้อมูล (Data Loading)

        private async void OpenSingleJson_Click(object sender, EventArgs e) => await LoadDataWrapperAsync(() => OpenSingleJsonLogic());
        private async void menuOpenFolder_Click(object sender, EventArgs e) => await LoadDataWrapperAsync(() => OpenFolderLogic());
        private async void menuLoadProject_Click(object sender, EventArgs e) => await LoadDataWrapperAsync(() => LoadProjectLogic());

        private async Task LoadDataWrapperAsync(Func<Task<bool>> loadingLogic)
        {
            // ป้องกัน async void ถูกเรียกซ้อนกัน เช่น กดปุ่มเปิดไฟล์ 2 ครั้งรัวๆ
            if (_isLoadingData) return;
            _isLoadingData = true;

            SetUiLoadingState(true, "กำลังประมวลผลข้อมูล... โปรดรอสักครู่");
            try
            {
                if (await loadingLogic())
                {
                    PopulateCategoryFilter();
                    PopulateDataGridView(GetMasterList());
                    UpdateTranslationProgress();
                }
                else
                {
                    // 🚀 [FIX] ถ้าผู้ใช้กดยกเลิก (loadingLogic คืนค่า false) ให้เคลียร์สถานะกลับมาเป็นพร้อมใช้งานทันที
                    lblStatus.Text = "พร้อมใช้งาน";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"เกิดข้อผิดพลาด: {ex.Message}", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = "การทำงานล้มเหลว";
            }
            finally
            {
                SetUiLoadingState(false);
                _isLoadingData = false;
            }
        }

        private async Task<bool> OpenSingleJsonLogic()
        {
            using (var ofd = new OpenFileDialog { Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*", Title = "เลือกไฟล์ JSON ม็อด Stardew Valley" })
            {
                if (ofd.ShowDialog(this) != DialogResult.OK) return false;
                var entries = await Task.Run(() => JsonParser.ParseJsonFile(ofd.FileName, Path.GetFileName(ofd.FileName)) ?? new List<TranslationEntry>());
                ResetMasterData(entries);
                currentProjectPath = string.Empty;
                currentExportPath = ofd.FileName;
                UpdateRecentFiles(ofd.FileName);
                lblStatus.Text = $"โหลดไฟล์ {Path.GetFileName(ofd.FileName)} สำเร็จ!";
                return true;
            }
        }

        private async Task<bool> OpenFolderLogic()
        {
            using (var fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog(this) != DialogResult.OK) return false;
                var loadedData = await Task.Run(() =>
                {
                    var tempList = new List<TranslationEntry>();
                    foreach (string filePath in Directory.GetFiles(fbd.SelectedPath, "*.json", SearchOption.AllDirectories))
                    {
                        var fileEntries = JsonParser.ParseJsonFile(filePath, Path.GetFileName(filePath));
                        if (fileEntries?.Count > 0) tempList.AddRange(fileEntries);
                    }
                    return tempList;
                });
                ResetMasterData(loadedData);
                currentProjectPath = string.Empty;
                currentExportPath = string.Empty;
                lblStatus.Text = $"นำเข้าโฟลเดอร์สำเร็จ! รวมทั้งหมด {masterRegistry.Count} รายการ";
                return true;
            }
        }

        private async Task<bool> LoadProjectLogic()
        {
            using (var ofd = new OpenFileDialog { Filter = "SVTT Project files (*.svtt)|*.svtt", Title = "เลือกไฟล์โปรเจกต์งานแปล SVTT" })
            {
                if (ofd.ShowDialog(this) != DialogResult.OK) return false;
                var loadedProject = await Task.Run(() =>
                {
                    string jsonContent = File.ReadAllText(ofd.FileName);
                    return JsonSerializer.Deserialize<List<TranslationEntry>>(jsonContent) ?? new List<TranslationEntry>();
                });
                ResetMasterData(loadedProject);
                currentProjectPath = ofd.FileName;
                currentExportPath = string.Empty;
                UpdateRecentFiles(ofd.FileName);
                lblStatus.Text = $"โหลดโปรเจกต์ {Path.GetFileName(ofd.FileName)} สำเร็จ!";
                return true;
            }
        }

        private void ResetMasterData(List<TranslationEntry> entries)
        {
            masterRegistry.Clear();
            masterKeysOrder.Clear();
            foreach (var entry in entries)
            {
                if (!string.IsNullOrEmpty(entry.Key) && !masterRegistry.ContainsKey(entry.Key))
                {
                    masterRegistry[entry.Key] = entry;
                    masterKeysOrder.Add(entry.Key);
                }
            }
        }

        #endregion

        #region 💾 บันทึกและส่งออกข้อมูล (Save & Export)

        private async void menuSaveProject_Click(object sender, EventArgs e)
        {
            if (masterRegistry.Count == 0) return;

            string pathToSave = currentProjectPath;
            if (string.IsNullOrEmpty(pathToSave))
            {
                using (var sfd = new SaveFileDialog { Filter = "SVTT Project files (*.svtt)|*.svtt", FileName = "my_translation_project.svtt" })
                {
                    if (sfd.ShowDialog(this) != DialogResult.OK) return;
                    pathToSave = sfd.FileName;
                }
            }

            SetUiLoadingState(true, "กำลังบันทึกโปรเจกต์...");
            try
            {
                var dataToSave = GetMasterList();
                await Task.Run(() =>
                {
                    string projectJson = JsonSerializer.Serialize(dataToSave, JsonSaveOptions);
                    File.WriteAllText(pathToSave, projectJson, Encoding.UTF8);
                });
                currentProjectPath = pathToSave;
                UpdateRecentFiles(currentProjectPath);
                lblStatus.Text = "บันทึกโปรเจกต์เรียบร้อย!";
                MessageBox.Show("บันทึกโปรเจกต์เรียบร้อย!", "สำเร็จ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"เซฟล้มเหลว: {ex.Message}");
                lblStatus.Text = "บันทึกโปรเจกต์ล้มเหลว";
            }
            finally
            {
                SetUiLoadingState(false);
                if (lblStatus.Text == "กำลังบันทึกโปรเจกต์...") lblStatus.Text = "พร้อมใช้งาน";
            }
        }

        private async Task<bool> ExportModFileAsync(string filePath)
        {
            try
            {
                var rowsData = GetMasterList();
                await Task.Run(() =>
                {
                    var jsonStructure = new Dictionary<string, string>(rowsData.Count);
                    foreach (var item in rowsData)
                        jsonStructure[item.Key] = string.IsNullOrEmpty(item.Translate) ? item.Original : item.Translate;
                    File.WriteAllText(filePath, JsonSerializer.Serialize(jsonStructure, JsonSaveOptions), Encoding.UTF8);
                });
                currentExportPath = filePath;
                return true;
            }
            catch (Exception ex) { MessageBox.Show($"ส่งออกล้มเหลว: {ex.Message}"); return false; }
        }

        private async void tsBtnSave_Click(object sender, EventArgs e)
        {
            if (masterRegistry.Count == 0) return;

            if (string.IsNullOrEmpty(currentExportPath))
            {
                using (var sfd = new SaveFileDialog { Filter = "JSON files (*.json)|*.json", FileName = "default_thai.json", Title = "เลือกสถานที่ส่งออกไฟล์ม็อดภาษาไทย" })
                {
                    if (sfd.ShowDialog(this) != DialogResult.OK)
                    {
                        // 🚀 [FIX] หากกดยกเลิกตอนเลือกที่เซฟไฟล์เดี่ยว ให้คืนสถานะทันที
                        lblStatus.Text = "พร้อมใช้งาน";
                        return;
                    }
                    currentExportPath = sfd.FileName;
                }
            }

            SetUiLoadingState(true, "กำลังส่งออกไฟล์ม็อด...");
            try
            {
                if (await ExportModFileAsync(currentExportPath))
                {
                    lblStatus.Text = $"ส่งออกไฟล์สำเร็จ: {Path.GetFileName(currentExportPath)}";
                }
                else
                {
                    lblStatus.Text = "ส่งออกไฟล์ล้มเหลว";
                }
            }
            finally
            {
                SetUiLoadingState(false);
            }
        }

        private async void tsBtnSaveAs_Click(object sender, EventArgs e)
        {
            if (masterRegistry.Count == 0) return;

            using (var sfd = new SaveFileDialog { Filter = "JSON files (*.json)|*.json", FileName = "default_thai.json", Title = "ส่งออกไฟล์ม็อดเป็นชื่อใหม่" })
            {
                if (sfd.ShowDialog(this) != DialogResult.OK)
                {
                    // 🚀 [FIX] หากกดยกเลิกตอน Save As ให้คืนสถานะทันที
                    lblStatus.Text = "พร้อมใช้งาน";
                    return;
                }

                SetUiLoadingState(true, "กำลังส่งออกไฟล์ม็อด...");
                try
                {
                    if (await ExportModFileAsync(sfd.FileName))
                    {
                        currentExportPath = sfd.FileName;
                        lblStatus.Text = $"ส่งออกไฟล์ใหม่สำเร็จ: {Path.GetFileName(currentExportPath)}";
                    }
                    else
                    {
                        lblStatus.Text = "ส่งออกไฟล์ล้มเหลว";
                    }
                }
                finally
                {
                    SetUiLoadingState(false);
                }
            }
        }

        private async Task tsBtnSaveAsLogicAsync()
        {
            await Task.CompletedTask;
        }

        #endregion

        #region 🎮 ระบบซิงค์ข้อมูลและคำแปล (Core Binding UI Logic)

        private void UpdateModelValue(string key, string textValue)
        {
            if (string.IsNullOrEmpty(key)) return;
            if (masterRegistry.TryGetValue(key, out var entry))
            {
                entry.Translate = textValue;
                entry.Status = !string.IsNullOrWhiteSpace(textValue) ? "แปลแล้ว" : "รอการแปล";
            }
        }

        public void dgvTranslations_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (IsUpdatingUI || e.RowIndex < 0) return;
            if (dgvTranslations.Columns[e.ColumnIndex].Name != "colTranslate") return;

            var row = dgvTranslations.Rows[e.RowIndex];
            string key = row.Cells["colKey"].Value?.ToString() ?? "";
            string val = row.Cells["colTranslate"].Value?.ToString() ?? "";

            UpdateModelValue(key, val);

            using (BeginUiUpdate())
            {
                row.Cells["colStatus"].Value = !string.IsNullOrWhiteSpace(val) ? "แปลแล้ว" : "รอการแปล";
                if (dgvTranslations.CurrentRow?.Index == e.RowIndex)
                    rtbTranslate.Text = val;
            }
            UpdateTranslationProgress();
        }

        private void rtbTranslate_TextChanged(object sender, EventArgs e)
        {
            if (IsUpdatingUI) return;
            if (dgvTranslations.CurrentRow == null || dgvTranslations.CurrentRow.Index < 0) return;

            var row = dgvTranslations.CurrentRow;
            string key = row.Cells["colKey"].Value?.ToString() ?? "";
            string val = rtbTranslate.Text;

            UpdateModelValue(key, val);

            using (BeginUiUpdate())
            {
                row.Cells["colTranslate"].Value = val;
                row.Cells["colStatus"].Value = !string.IsNullOrWhiteSpace(val) ? "แปลแล้ว" : "รอการแปล";
            }

            if (!_progressUpdatePending)
            {
                _progressUpdatePending = true;
                BeginInvoke(new Action(() => { _progressUpdatePending = false; UpdateTranslationProgress(); }));
            }
        }

        private void dgvTranslations_SelectionChanged(object sender, EventArgs e)
        {
            if (IsUpdatingUI) return;

            using (BeginUiUpdate())
            {
                if (dgvTranslations.CurrentRow != null && dgvTranslations.CurrentRow.Index >= 0 && !dgvTranslations.CurrentRow.IsNewRow)
                {
                    rtbOriginal.Text = dgvTranslations.CurrentRow.Cells["colOriginal"].Value?.ToString() ?? "";
                    rtbTranslate.Text = dgvTranslations.CurrentRow.Cells["colTranslate"].Value?.ToString() ?? "";
                }
                else
                {
                    rtbOriginal.Clear();
                    rtbTranslate.Clear();
                }
            }
        }

        #endregion

        #region 🗂️ ระบบกรองข้อมูลอัจฉริยะ (Smart Asynchronous Filtering)

        public async void ApplyFilters_Event(object sender, EventArgs e)
        {
            filterCts?.Cancel();
            filterCts?.Dispose();
            var cts = new CancellationTokenSource();
            filterCts = cts;
            var token = cts.Token;

            string statusFilter = tsCboFilter.SelectedItem?.ToString() ?? "ทั้งหมด";
            string categoryFilter = tsCboCategoryFilter.SelectedItem?.ToString() ?? "ทั้งหมด";
            string searchKeyword = tsTxtSearch.Text.Trim().ToLower();

            try
            {
                await Task.Delay(150, token);
                lblStatus.Text = "กำลังกรองข้อมูลคำแปล...";

                var snapshot = new List<TranslationEntry>(masterKeysOrder.Count);
                foreach (var k in masterKeysOrder)
                {
                    if (masterRegistry.TryGetValue(k, out var entry))
                        snapshot.Add(entry);
                }

                var filteredList = await Task.Run(() =>
                {
                    var tempList = new List<TranslationEntry>();
                    foreach (var entry in snapshot)
                    {
                        token.ThrowIfCancellationRequested();

                        bool matchStatus = statusFilter == "ทั้งหมด" ||
                                           (statusFilter == "ยังไม่แปล" && (entry.Status == "รอการแปล" || entry.Status == "ยังไม่แปล")) ||
                                           (statusFilter == "แปลแล้ว" && entry.Status == "แปลแล้ว") ||
                                           entry.Status == statusFilter;

                        bool matchCategory = categoryFilter == "ทั้งหมด" || entry.Category == categoryFilter;

                        bool matchSearch = string.IsNullOrEmpty(searchKeyword) ||
                                           (entry.Key?.ToLower().Contains(searchKeyword) == true) ||
                                           (entry.Original?.ToLower().Contains(searchKeyword) == true) ||
                                           (entry.Translate?.ToLower().Contains(searchKeyword) == true);

                        if (matchStatus && matchCategory && matchSearch) tempList.Add(entry);
                    }
                    return tempList;
                }, token);

                PopulateDataGridView(filteredList);
                lblStatus.Text = $"แสดงผลข้อมูล {filteredList.Count} จากทั้งหมด {masterRegistry.Count} รายการ";
            }
            catch (OperationCanceledException) { /* จงใจยกเลิกเพื่อค้นหาคำใหม่ */ }
            catch (Exception ex) { lblStatus.Text = $"ตัวกรองผิดพลาด: {ex.Message}"; }
        }

        private void PopulateDataGridView(List<TranslationEntry> targetEntries)
        {
            using (BeginUiUpdate())
            {
                dgvTranslations.SuspendLayout();
                try
                {
                    dgvTranslations.CurrentCell = null;
                    dgvTranslations.Rows.Clear();

                    var rowsToAdd = new List<DataGridViewRow>(targetEntries.Count);
                    foreach (var entry in targetEntries)
                    {
                        var row = new DataGridViewRow();
                        row.CreateCells(dgvTranslations);
                        row.Cells[0].Value = entry.Key;
                        row.Cells[1].Value = entry.Category;
                        row.Cells[2].Value = entry.Original;
                        row.Cells[3].Value = entry.Translate;
                        row.Cells[4].Value = entry.Status;
                        rowsToAdd.Add(row);
                    }
                    if (rowsToAdd.Count > 0) dgvTranslations.Rows.AddRange(rowsToAdd.ToArray());
                }
                finally { dgvTranslations.ResumeLayout(); }
            }
        }

        private void PopulateCategoryFilter()
        {
            if (tsCboCategoryFilter == null) return;

            tsCboCategoryFilter.SelectedIndexChanged -= ApplyFilters_Event;
            tsCboCategoryFilter.Items.Clear();
            tsCboCategoryFilter.Items.Add("ทั้งหมด");

            var categories = masterRegistry.Values
                .Where(e => !string.IsNullOrEmpty(e.Category))
                .Select(e => e.Category)
                .Distinct()
                .OrderBy(c => c);

            foreach (var cat in categories) tsCboCategoryFilter.Items.Add(cat);

            tsCboCategoryFilter.SelectedIndex = 0;
            tsCboCategoryFilter.SelectedIndexChanged += ApplyFilters_Event;
        }

        #endregion

        #region 🛠️ ฟังก์ชันเสริมเบ็ดเตล็ด (Helper Utilities)

        public void UpdateTranslationProgress()
        {
            int totalRows = masterRegistry.Count;
            int translatedCount = masterRegistry.Values.Count(e => !string.IsNullOrWhiteSpace(e.Translate));

            lblStatus.Text = $"ข้อความทั้งหมด: {totalRows} | แปลแล้ว: {translatedCount} | คงเหลือ: {totalRows - translatedCount}";
            pgbProgress.Style = ProgressBarStyle.Continuous;
            pgbProgress.Value = totalRows > 0 ? Math.Clamp((int)((double)translatedCount / totalRows * 100), 0, 100) : 0;
        }

        private List<TranslationEntry> GetMasterList()
        {
            var list = new List<TranslationEntry>(masterKeysOrder.Count);
            foreach (var key in masterKeysOrder)
                if (masterRegistry.TryGetValue(key, out var entry)) list.Add(entry);
            return list;
        }

        private void dgvTranslations_CategoryOrStatus_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvTranslations.Columns[e.ColumnIndex].Name != "colStatus" || e.Value == null) return;

            bool isTranslated = e.Value.ToString() == "แปลแล้ว";
            e.CellStyle.BackColor = isTranslated ? Color.FromArgb(220, 245, 220) : Color.FromArgb(255, 230, 230);
            e.CellStyle.ForeColor = isTranslated ? Color.DarkGreen : Color.DarkRed;
            e.CellStyle.SelectionBackColor = e.CellStyle.BackColor;
            e.CellStyle.SelectionForeColor = e.CellStyle.ForeColor;
        }

        private void SetUiLoadingState(bool isLoading, string statusText = "")
        {
            if (!string.IsNullOrEmpty(statusText)) lblStatus.Text = statusText;
            pgbProgress.Visible = isLoading;
            pgbProgress.Style = isLoading ? ProgressBarStyle.Marquee : ProgressBarStyle.Continuous;
            tsMain.Enabled = !isLoading;
            menuMain.Enabled = !isLoading;
        }

        private void UpdateRecentFiles(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) return;
            recentFiles.Remove(filePath);
            recentFiles.Insert(0, filePath);
            if (recentFiles.Count > MaxRecentFiles)
                recentFiles.RemoveRange(MaxRecentFiles, recentFiles.Count - MaxRecentFiles);
        }

        private void menuExit_Click(object sender, EventArgs e) => Application.Exit();

        private void MenuRecent_DropDownOpening(object sender, EventArgs e)
        {
            menuRecent.DropDownItems.Clear();

            if (recentFiles == null || recentFiles.Count == 0)
            {
                var noFilesItem = new ToolStripMenuItem("ไม่มีไฟล์ล่าสุด") { Enabled = false };
                menuRecent.DropDownItems.Add(noFilesItem);
                return;
            }

            var contextMenu = new ContextMenuStrip();
            var deleteItem = new ToolStripMenuItem("ลบออกจากรายการล่าสุด");
            contextMenu.Items.Add(deleteItem);

            deleteItem.Click += DeleteRecentItem_Click;

            menuRecent.DropDown.Closing += (s, ev) =>
            {
                if (Control.MouseButtons == MouseButtons.Right) ev.Cancel = true;
            };

            menuFile.DropDown.Closing += (s, ev) =>
            {
                if (Control.MouseButtons == MouseButtons.Right) ev.Cancel = true;
            };

            foreach (string filePath in recentFiles)
            {
                string fileName = Path.GetFileName(filePath);
                var recentItem = new ToolStripMenuItem(fileName)
                {
                    Tag = filePath,
                    ToolTipText = filePath
                };
                recentItem.Click += RecentItem_Click;

                recentItem.MouseDown += (s, ev) =>
                {
                    if (ev.Button == MouseButtons.Right && s is ToolStripMenuItem item)
                    {
                        deleteItem.Tag = item;
                        contextMenu.Show(Cursor.Position);
                    }
                };

                menuRecent.DropDownItems.Add(recentItem);
            }
        }

        private void DeleteRecentItem_Click(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem deleteMenu && deleteMenu.Tag is ToolStripMenuItem targetItem)
            {
                string filePathToRemove = targetItem.Tag?.ToString();
                if (!string.IsNullOrEmpty(filePathToRemove))
                {
                    recentFiles.Remove(filePathToRemove);
                    menuRecent.DropDownItems.Remove(targetItem);

                    if (recentFiles.Count == 0)
                    {
                        var noFilesItem = new ToolStripMenuItem("ไม่มีไฟล์ล่าสุด") { Enabled = false };
                        menuRecent.DropDownItems.Add(noFilesItem);
                    }

                    lblStatus.Text = $"ลบประวัติไฟล์ {Path.GetFileName(filePathToRemove)} ออกเรียบร้อย";
                    menuFile.DropDown.Close();
                }
            }
        }

        private async void RecentItem_Click(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem clickedItem && clickedItem.Tag is string filePath)
            {
                if (!File.Exists(filePath))
                {
                    MessageBox.Show($"ไม่พบไฟล์: {filePath}\nไฟล์นี้อาจถูกย้ายหรือลบไปแล้ว",
                                    "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    recentFiles.Remove(filePath);
                    return;
                }

                string extension = Path.GetExtension(filePath).ToLower();

                if (extension == ".svtt")
                {
                    await LoadDataWrapperAsync(() => LoadProjectFromRecentLogic(filePath));
                }
                else if (extension == ".json")
                {
                    await LoadDataWrapperAsync(() => OpenSingleJsonFromRecentLogic(filePath));
                }
            }
        }

        private async Task<bool> LoadProjectFromRecentLogic(string filePath)
        {
            var loadedProject = await Task.Run(() =>
            {
                string jsonContent = File.ReadAllText(filePath);
                return JsonSerializer.Deserialize<List<TranslationEntry>>(jsonContent) ?? new List<TranslationEntry>();
            });
            ResetMasterData(loadedProject);
            currentProjectPath = filePath;
            currentExportPath = string.Empty;
            UpdateRecentFiles(filePath);
            lblStatus.Text = $"โหลดโปรเจกต์ล่าสุด {Path.GetFileName(filePath)} สำเร็จ!";
            return true;
        }

        private async Task<bool> OpenSingleJsonFromRecentLogic(string filePath)
        {
            var entries = await Task.Run(() => JsonParser.ParseJsonFile(filePath, Path.GetFileName(filePath)) ?? new List<TranslationEntry>());
            ResetMasterData(entries);
            currentProjectPath = string.Empty;
            currentExportPath = filePath;
            UpdateRecentFiles(filePath);
            lblStatus.Text = $"โหลดไฟล์ล่าสุด {Path.GetFileName(filePath)} สำเร็จ!";
            return true;
        }

        private void menuSearch_Click(object sender, EventArgs e)
        {
            tsTxtSearch.Focus();
            tsTxtSearch.SelectAll();
        }

        #endregion

        private void menuWiki_Click(object sender, EventArgs e)
        {
            string url = "https://stardewvalleywiki.com/Stardew_Valley_Wiki";
            lblStatus.Text = "กำลังเปิดเว็บบราวเซอร์...";
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"ไม่สามารถเปิดลิงก์ได้เนื่องจาก: {ex.Message}",
                                "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                lblStatus.Text = "พร้อมใช้งาน";
            }
        }

        private void menuAbout_Click(object sender, EventArgs e)
        {
            string aboutText = "SVTT Version 1.0 (2026)\n\n" +
                               "โปรแกรมช่วยแปลม็อดภาษาไทย\n" +
                               "พัฒนาโดย: 7526xd\n\n" +
                               "ห้ามนำไปหากำไรเด็ดขาด!";

            lblStatus.Text = "กำลังแสดงหน้าต่างเกี่ยวกับโปรแกรม";
            MessageBox.Show(this, aboutText, "เกี่ยวกับโปรแกรม", MessageBoxButtons.OK, MessageBoxIcon.Information);
            lblStatus.Text = "พร้อมใช้งาน";
        }

        private void menuSupport_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "กำลังเปิดหน้าต่างสนับสนุน...";
            using (SupportForm frm = new SupportForm())
            {
                frm.ShowDialog(this);
            }
            lblStatus.Text = "พร้อมใช้งาน";
        }
    }
}