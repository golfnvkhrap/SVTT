namespace SVTT
{
    partial class Main
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            menuMain = new MenuStrip();
            menuFile = new ToolStripMenuItem();
            menuOpenJson = new ToolStripMenuItem();
            menuOpenFolder = new ToolStripMenuItem();
            sepFile1 = new ToolStripSeparator();
            menuSave = new ToolStripMenuItem();
            menuSaveAs = new ToolStripMenuItem();
            sepFile2 = new ToolStripSeparator();
            menuSaveProject = new ToolStripMenuItem();
            menuLoadProject = new ToolStripMenuItem();
            sepFile4 = new ToolStripSeparator();
            menuRecent = new ToolStripMenuItem();
            menuExit = new ToolStripMenuItem();
            menuEdit = new ToolStripMenuItem();
            menuSearch = new ToolStripMenuItem();
            menuSupport = new ToolStripMenuItem();
            menuHelp = new ToolStripMenuItem();
            menuWiki = new ToolStripMenuItem();
            menuAbout = new ToolStripMenuItem();
            statusMain = new StatusStrip();
            lblStatus = new ToolStripStatusLabel();
            pgbProgress = new ToolStripProgressBar();
            tlpLayout = new TableLayoutPanel();
            dgvTranslations = new DataGridView();
            colKey = new DataGridViewTextBoxColumn();
            colCategory = new DataGridViewTextBoxColumn();
            colOriginal = new DataGridViewTextBoxColumn();
            colTranslate = new DataGridViewTextBoxColumn();
            colStatus = new DataGridViewTextBoxColumn();
            tsMain = new ToolStrip();
            tsBtnOpen = new ToolStripButton();
            tsBtnSave = new ToolStripButton();
            tsBtnSaveAs = new ToolStripButton();
            tsSep1 = new ToolStripSeparator();
            tsLblFilter = new ToolStripLabel();
            tsCboFilter = new ToolStripComboBox();
            tsLblCategoryFilter = new ToolStripLabel();
            tsCboCategoryFilter = new ToolStripComboBox();
            tsSep3 = new ToolStripSeparator();
            tsLblSearch = new ToolStripLabel();
            tsTxtSearch = new ToolStripTextBox();
            tlpLayout2 = new TableLayoutPanel();
            rtbOriginal = new RichTextBox();
            rtbTranslate = new RichTextBox();
            menuMain.SuspendLayout();
            statusMain.SuspendLayout();
            tlpLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvTranslations).BeginInit();
            tsMain.SuspendLayout();
            tlpLayout2.SuspendLayout();
            SuspendLayout();
            // 
            // menuMain
            // 
            menuMain.BackColor = Color.White;
            menuMain.Font = new Font("Noto Sans Thai", 9F, FontStyle.Regular, GraphicsUnit.Point, 222);
            menuMain.Items.AddRange(new ToolStripItem[] { menuFile, menuEdit, menuSupport, menuHelp });
            menuMain.Location = new Point(0, 0);
            menuMain.Name = "menuMain";
            menuMain.Size = new Size(1366, 26);
            menuMain.TabIndex = 0;
            menuMain.Text = "menuMain";
            // 
            // menuFile
            // 
            menuFile.DropDownItems.AddRange(new ToolStripItem[] { menuOpenJson, menuOpenFolder, sepFile1, menuSave, menuSaveAs, sepFile2, menuSaveProject, menuLoadProject, sepFile4, menuRecent, menuExit });
            menuFile.Name = "menuFile";
            menuFile.Size = new Size(39, 22);
            menuFile.Text = "ไฟล์";
            // 
            // menuOpenJson
            // 
            menuOpenJson.Name = "menuOpenJson";
            menuOpenJson.ShortcutKeys = Keys.Control | Keys.O;
            menuOpenJson.Size = new Size(227, 22);
            menuOpenJson.Text = "Open Json...";
            menuOpenJson.Click += OpenSingleJson_Click;
            // 
            // menuOpenFolder
            // 
            menuOpenFolder.Name = "menuOpenFolder";
            menuOpenFolder.ShortcutKeys = Keys.Control | Keys.Shift | Keys.O;
            menuOpenFolder.Size = new Size(227, 22);
            menuOpenFolder.Text = "Open Folder...";
            menuOpenFolder.Click += menuOpenFolder_Click;
            // 
            // sepFile1
            // 
            sepFile1.Name = "sepFile1";
            sepFile1.Size = new Size(224, 6);
            // 
            // menuSave
            // 
            menuSave.Name = "menuSave";
            menuSave.ShortcutKeys = Keys.Control | Keys.S;
            menuSave.Size = new Size(227, 22);
            menuSave.Text = "Save";
            menuSave.Click += tsBtnSave_Click;
            // 
            // menuSaveAs
            // 
            menuSaveAs.Name = "menuSaveAs";
            menuSaveAs.ShortcutKeys = Keys.Control | Keys.Shift | Keys.S;
            menuSaveAs.Size = new Size(227, 22);
            menuSaveAs.Text = "Save As...";
            menuSaveAs.Click += tsBtnSaveAs_Click;
            // 
            // sepFile2
            // 
            sepFile2.Name = "sepFile2";
            sepFile2.Size = new Size(224, 6);
            // 
            // menuSaveProject
            // 
            menuSaveProject.Name = "menuSaveProject";
            menuSaveProject.Size = new Size(227, 22);
            menuSaveProject.Text = "Save Project";
            menuSaveProject.Click += menuSaveProject_Click;
            // 
            // menuLoadProject
            // 
            menuLoadProject.Name = "menuLoadProject";
            menuLoadProject.Size = new Size(227, 22);
            menuLoadProject.Text = "Load Project";
            menuLoadProject.Click += menuLoadProject_Click;
            // 
            // sepFile4
            // 
            sepFile4.Name = "sepFile4";
            sepFile4.Size = new Size(224, 6);
            // 
            // menuRecent
            // 
            menuRecent.Name = "menuRecent";
            menuRecent.Size = new Size(227, 22);
            menuRecent.Text = "Recent Files";
            menuRecent.DropDownOpening += MenuRecent_DropDownOpening;
            // 
            // menuExit
            // 
            menuExit.Name = "menuExit";
            menuExit.ShortcutKeys = Keys.Alt | Keys.F4;
            menuExit.Size = new Size(227, 22);
            menuExit.Text = "Exit";
            menuExit.Click += menuExit_Click;
            // 
            // menuEdit
            // 
            menuEdit.DropDownItems.AddRange(new ToolStripItem[] { menuSearch });
            menuEdit.Name = "menuEdit";
            menuEdit.Size = new Size(45, 22);
            menuEdit.Text = "แก้ไข";
            // 
            // menuSearch
            // 
            menuSearch.Name = "menuSearch";
            menuSearch.ShortcutKeys = Keys.Control | Keys.F;
            menuSearch.Size = new Size(156, 22);
            menuSearch.Text = "Search";
            menuSearch.Click += menuSearch_Click;
            // 
            // menuSupport
            // 
            menuSupport.Name = "menuSupport";
            menuSupport.Size = new Size(95, 22);
            menuSupport.Text = "สนับสนุนผลงาน";
            menuSupport.Click += menuSupport_Click;
            // 
            // menuHelp
            // 
            menuHelp.DropDownItems.AddRange(new ToolStripItem[] { menuWiki, menuAbout });
            menuHelp.Name = "menuHelp";
            menuHelp.Size = new Size(65, 22);
            menuHelp.Text = "ช่วยเหลือ";
            // 
            // menuWiki
            // 
            menuWiki.Name = "menuWiki";
            menuWiki.Size = new Size(184, 22);
            menuWiki.Text = "Stardew Valley Wiki";
            menuWiki.Click += menuWiki_Click;
            // 
            // menuAbout
            // 
            menuAbout.Name = "menuAbout";
            menuAbout.Size = new Size(184, 22);
            menuAbout.Text = "About SVTT";
            menuAbout.Click += menuAbout_Click;
            // 
            // statusMain
            // 
            statusMain.BackColor = Color.White;
            statusMain.Font = new Font("Noto Sans Thai Looped", 9F);
            statusMain.Items.AddRange(new ToolStripItem[] { lblStatus, pgbProgress });
            statusMain.Location = new Point(0, 739);
            statusMain.Name = "statusMain";
            statusMain.Size = new Size(1366, 29);
            statusMain.TabIndex = 2;
            statusMain.Text = "statusMain";
            // 
            // lblStatus
            // 
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(1218, 24);
            lblStatus.Spring = true;
            lblStatus.Text = "พร้อมใช้งาน";
            lblStatus.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // pgbProgress
            // 
            pgbProgress.Name = "pgbProgress";
            pgbProgress.Size = new Size(100, 23);
            pgbProgress.Visible = false;
            // 
            // tlpLayout
            // 
            tlpLayout.ColumnCount = 1;
            tlpLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpLayout.Controls.Add(dgvTranslations, 0, 0);
            tlpLayout.Dock = DockStyle.Fill;
            tlpLayout.Font = new Font("Noto Sans Thai Looped", 9F);
            tlpLayout.Location = new Point(0, 58);
            tlpLayout.Name = "tlpLayout";
            tlpLayout.RowCount = 1;
            tlpLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tlpLayout.Size = new Size(1366, 544);
            tlpLayout.TabIndex = 3;
            // 
            // dgvTranslations
            // 
            dgvTranslations.AllowUserToAddRows = false;
            dgvTranslations.AllowUserToDeleteRows = false;
            dgvTranslations.AllowUserToResizeColumns = false;
            dgvTranslations.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(245, 245, 245);
            dataGridViewCellStyle1.SelectionBackColor = Color.CornflowerBlue;
            dataGridViewCellStyle1.SelectionForeColor = Color.White;
            dgvTranslations.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dgvTranslations.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvTranslations.Columns.AddRange(new DataGridViewColumn[] { colKey, colCategory, colOriginal, colTranslate, colStatus });
            dgvTranslations.Dock = DockStyle.Fill;
            dgvTranslations.EditMode = DataGridViewEditMode.EditOnEnter;
            dgvTranslations.Location = new Point(3, 3);
            dgvTranslations.Name = "dgvTranslations";
            dgvTranslations.RowHeadersVisible = false;
            dataGridViewCellStyle2.BackColor = Color.White;
            dataGridViewCellStyle2.SelectionBackColor = Color.CornflowerBlue;
            dataGridViewCellStyle2.SelectionForeColor = Color.White;
            dgvTranslations.RowsDefaultCellStyle = dataGridViewCellStyle2;
            dgvTranslations.Size = new Size(1360, 538);
            dgvTranslations.TabIndex = 0;
            dgvTranslations.CellFormatting += dgvTranslations_CategoryOrStatus_CellFormatting;
            dgvTranslations.CellValueChanged += dgvTranslations_CellValueChanged;
            dgvTranslations.SelectionChanged += dgvTranslations_SelectionChanged;
            // 
            // colKey
            // 
            colKey.HeaderText = "คีย์";
            colKey.Name = "colKey";
            colKey.ReadOnly = true;
            colKey.Width = 150;
            // 
            // colCategory
            // 
            colCategory.HeaderText = "หมวดหมู่";
            colCategory.Name = "colCategory";
            colCategory.ReadOnly = true;
            colCategory.Width = 120;
            // 
            // colOriginal
            // 
            colOriginal.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colOriginal.FillWeight = 50F;
            colOriginal.HeaderText = "ต้นฉบับ (ภาษาอังกฤษ)";
            colOriginal.Name = "colOriginal";
            colOriginal.ReadOnly = true;
            // 
            // colTranslate
            // 
            colTranslate.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colTranslate.FillWeight = 50F;
            colTranslate.HeaderText = "แปล (ภาษาไทย)";
            colTranslate.Name = "colTranslate";
            // 
            // colStatus
            // 
            colStatus.HeaderText = "สถานะ";
            colStatus.Name = "colStatus";
            colStatus.ReadOnly = true;
            colStatus.Width = 80;
            // 
            // tsMain
            // 
            tsMain.Font = new Font("Noto Sans Thai Looped", 9F, FontStyle.Regular, GraphicsUnit.Point, 222);
            tsMain.GripStyle = ToolStripGripStyle.Hidden;
            tsMain.Items.AddRange(new ToolStripItem[] { tsBtnOpen, tsBtnSave, tsBtnSaveAs, tsSep1, tsLblFilter, tsCboFilter, tsLblCategoryFilter, tsCboCategoryFilter, tsSep3, tsLblSearch, tsTxtSearch });
            tsMain.Location = new Point(0, 26);
            tsMain.Name = "tsMain";
            tsMain.Size = new Size(1366, 32);
            tsMain.TabIndex = 4;
            tsMain.Text = "tsMain";
            // 
            // tsBtnOpen
            // 
            tsBtnOpen.Image = (Image)resources.GetObject("tsBtnOpen.Image");
            tsBtnOpen.Name = "tsBtnOpen";
            tsBtnOpen.Size = new Size(106, 29);
            tsBtnOpen.Text = "เปิดไฟล์ Json...";
            tsBtnOpen.ToolTipText = "Open JSON (Ctrl+O)";
            tsBtnOpen.Click += OpenSingleJson_Click;
            // 
            // tsBtnSave
            // 
            tsBtnSave.Image = (Image)resources.GetObject("tsBtnSave.Image");
            tsBtnSave.Name = "tsBtnSave";
            tsBtnSave.Size = new Size(61, 29);
            tsBtnSave.Text = "บันทึก";
            tsBtnSave.ToolTipText = "Save (Ctrl+S)";
            tsBtnSave.Click += tsBtnSave_Click;
            // 
            // tsBtnSaveAs
            // 
            tsBtnSaveAs.Image = (Image)resources.GetObject("tsBtnSaveAs.Image");
            tsBtnSaveAs.Name = "tsBtnSaveAs";
            tsBtnSaveAs.Size = new Size(90, 29);
            tsBtnSaveAs.Text = "บันทึกเป็น...";
            tsBtnSaveAs.ToolTipText = "Save As (Ctrl+Shift+S)";
            tsBtnSaveAs.Click += tsBtnSaveAs_Click;
            // 
            // tsSep1
            // 
            tsSep1.Name = "tsSep1";
            tsSep1.Size = new Size(6, 32);
            // 
            // tsLblFilter
            // 
            tsLblFilter.Name = "tsLblFilter";
            tsLblFilter.Size = new Size(45, 29);
            tsLblFilter.Text = "แสดง :";
            // 
            // tsCboFilter
            // 
            tsCboFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            tsCboFilter.FlatStyle = FlatStyle.System;
            tsCboFilter.Font = new Font("Noto Sans Thai Looped", 9F);
            tsCboFilter.ForeColor = SystemColors.WindowText;
            tsCboFilter.Items.AddRange(new object[] { "ทั้งหมด", "ยังไม่แปล", "แปลแล้ว" });
            tsCboFilter.Name = "tsCboFilter";
            tsCboFilter.Size = new Size(150, 32);
            tsCboFilter.SelectedIndexChanged += ApplyFilters_Event;
            // 
            // tsLblCategoryFilter
            // 
            tsLblCategoryFilter.Name = "tsLblCategoryFilter";
            tsLblCategoryFilter.Size = new Size(62, 29);
            tsLblCategoryFilter.Text = "หมวดหมู่ :";
            // 
            // tsCboCategoryFilter
            // 
            tsCboCategoryFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            tsCboCategoryFilter.FlatStyle = FlatStyle.System;
            tsCboCategoryFilter.Font = new Font("Noto Sans Thai Looped", 9F);
            tsCboCategoryFilter.ForeColor = SystemColors.WindowText;
            tsCboCategoryFilter.Items.AddRange(new object[] { "ทั้งหมด" });
            tsCboCategoryFilter.Name = "tsCboCategoryFilter";
            tsCboCategoryFilter.Size = new Size(150, 32);
            tsCboCategoryFilter.SelectedIndexChanged += ApplyFilters_Event;
            // 
            // tsSep3
            // 
            tsSep3.Name = "tsSep3";
            tsSep3.Size = new Size(6, 32);
            // 
            // tsLblSearch
            // 
            tsLblSearch.Name = "tsLblSearch";
            tsLblSearch.Size = new Size(46, 29);
            tsLblSearch.Text = "ค้นหา :";
            // 
            // tsTxtSearch
            // 
            tsTxtSearch.Font = new Font("Noto Sans Thai Looped", 9F);
            tsTxtSearch.ForeColor = SystemColors.WindowText;
            tsTxtSearch.Name = "tsTxtSearch";
            tsTxtSearch.Size = new Size(200, 32);
            tsTxtSearch.ToolTipText = "พิมพ์ Key หรือข้อความ (Ctrl+F)";
            tsTxtSearch.TextChanged += ApplyFilters_Event;
            // 
            // tlpLayout2
            // 
            tlpLayout2.ColumnCount = 2;
            tlpLayout2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpLayout2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpLayout2.Controls.Add(rtbOriginal, 0, 0);
            tlpLayout2.Controls.Add(rtbTranslate, 1, 0);
            tlpLayout2.Dock = DockStyle.Bottom;
            tlpLayout2.Location = new Point(0, 602);
            tlpLayout2.Name = "tlpLayout2";
            tlpLayout2.RowCount = 1;
            tlpLayout2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpLayout2.Size = new Size(1366, 137);
            tlpLayout2.TabIndex = 5;
            // 
            // rtbOriginal
            // 
            rtbOriginal.Dock = DockStyle.Fill;
            rtbOriginal.Font = new Font("Noto Sans Thai Looped", 9F, FontStyle.Regular, GraphicsUnit.Point, 222);
            rtbOriginal.ForeColor = SystemColors.WindowFrame;
            rtbOriginal.Location = new Point(3, 3);
            rtbOriginal.Name = "rtbOriginal";
            rtbOriginal.ReadOnly = true;
            rtbOriginal.ScrollBars = RichTextBoxScrollBars.Vertical;
            rtbOriginal.Size = new Size(677, 131);
            rtbOriginal.TabIndex = 0;
            rtbOriginal.Text = "ข้อความต้นฉบับ (Original)...";
            // 
            // rtbTranslate
            // 
            rtbTranslate.Dock = DockStyle.Fill;
            rtbTranslate.Font = new Font("Noto Sans Thai Looped", 9F);
            rtbTranslate.Location = new Point(686, 3);
            rtbTranslate.Name = "rtbTranslate";
            rtbTranslate.ScrollBars = RichTextBoxScrollBars.Vertical;
            rtbTranslate.Size = new Size(677, 131);
            rtbTranslate.TabIndex = 1;
            rtbTranslate.Text = "";
            rtbTranslate.TextChanged += rtbTranslate_TextChanged;
            // 
            // Main
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 246, 248);
            ClientSize = new Size(1366, 768);
            Controls.Add(tlpLayout);
            Controls.Add(tsMain);
            Controls.Add(tlpLayout2);
            Controls.Add(statusMain);
            Controls.Add(menuMain);
            DoubleBuffered = true;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuMain;
            Name = "Main";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "SVTT 1.0";
            menuMain.ResumeLayout(false);
            menuMain.PerformLayout();
            statusMain.ResumeLayout(false);
            statusMain.PerformLayout();
            tlpLayout.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvTranslations).EndInit();
            tsMain.ResumeLayout(false);
            tsMain.PerformLayout();
            tlpLayout2.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuMain;
        private ToolStripMenuItem menuFile;
        private ToolStripMenuItem menuOpenJson;
        private ToolStripMenuItem menuOpenFolder;
        private ToolStripSeparator sepFile1;
        private ToolStripMenuItem menuSave;
        private ToolStripMenuItem menuSaveAs;
        private ToolStripSeparator sepFile2;
        private ToolStripMenuItem menuRecent;
        private ToolStripMenuItem menuExit;
        private ToolStripMenuItem menuEdit;
        private ToolStripMenuItem menuHelp;
        private ToolStripMenuItem menuWiki;
        private ToolStripMenuItem menuAbout;
        private StatusStrip statusMain;
        private TableLayoutPanel tlpLayout;
        private DataGridView dgvTranslations;
        private DataGridViewTextBoxColumn colKey;
        private DataGridViewTextBoxColumn colCategory;
        private DataGridViewTextBoxColumn colOriginal;
        private DataGridViewTextBoxColumn colTranslate;
        private DataGridViewTextBoxColumn colStatus;
        private ToolStripStatusLabel lblStatus;
        private ToolStripProgressBar pgbProgress;
        private ToolStrip tsMain;
        private ToolStripButton tsBtnOpen;
        private ToolStripButton tsBtnSave;
        private ToolStripButton tsBtnSaveAs;
        private ToolStripSeparator tsSep1;
        private ToolStripLabel tsLblFilter;
        private ToolStripComboBox tsCboFilter;
        private ToolStripLabel tsLblCategoryFilter;
        private ToolStripComboBox tsCboCategoryFilter;
        private ToolStripSeparator tsSep3;
        private ToolStripLabel tsLblSearch;
        private ToolStripTextBox tsTxtSearch;
        private TableLayoutPanel tlpLayout2;
        private RichTextBox rtbOriginal;
        private ToolStripMenuItem menuSaveProject;
        private ToolStripMenuItem menuLoadProject;
        private ToolStripSeparator sepFile4;
        private ToolStripMenuItem menuSearch;
        private RichTextBox rtbTranslate;
        private ToolStripMenuItem menuSupport;
    }
}