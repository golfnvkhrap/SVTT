using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace SVTT
{
    public static class JsonParser
    {
        /// <summary>
        /// แปลง JS Object Literal ให้เป็น JSON จริงๆ ก่อน Parse
        /// รองรับ: unquoted keys, // comments, /* */ comments, single quotes, trailing commas, multiple keys per line
        /// </summary>
        private static string SanitizeToJson(string input)
        {
            var sb = new StringBuilder();
            int i = 0;
            int len = input.Length;

            while (i < len)
            {
                // ข้าม // comment จนถึง end of line
                if (i + 1 < len && input[i] == '/' && input[i + 1] == '/')
                {
                    while (i < len && input[i] != '\n')
                        i++;
                    continue;
                }

                // ข้าม /* ... */ comment
                if (i + 1 < len && input[i] == '/' && input[i + 1] == '*')
                {
                    i += 2;
                    while (i + 1 < len && !(input[i] == '*' && input[i + 1] == '/'))
                        i++;
                    i += 2;
                    continue;
                }

                // ถ้าเจอ string (ทั้ง " และ ') → คัดลอกเนื้อหาใน string ออกมาตรงๆ
                if (input[i] == '"' || input[i] == '\'')
                {
                    char quote = input[i];
                    sb.Append('"'); // เปลี่ยนเป็น double quote เสมอ
                    i++;
                    while (i < len)
                    {
                        if (input[i] == '\\' && i + 1 < len)
                        {
                            // escape sequence → คัดลอกทั้งคู่ออกมา
                            sb.Append(input[i]);
                            sb.Append(input[i + 1]);
                            i += 2;
                        }
                        else if (input[i] == quote)
                        {
                            sb.Append('"');
                            i++;
                            break;
                        }
                        else
                        {
                            sb.Append(input[i]);
                            i++;
                        }
                    }
                    continue;
                }

                // ถ้าเจอตัวอักษรที่เป็น unquoted key (A-Z, a-z, 0-9, _, $)
                if (char.IsLetterOrDigit(input[i]) || input[i] == '_' || input[i] == '$')
                {
                    int start = i;
                    while (i < len && (char.IsLetterOrDigit(input[i]) || input[i] == '_' || input[i] == '$' || input[i] == '.'))
                        i++;

                    // ตรวจว่าเป็น key (ตามด้วย ':') หรือแค่ค่า value (true, false, null, number)
                    int lookahead = i;
                    while (lookahead < len && (input[lookahead] == ' ' || input[lookahead] == '\t'))
                        lookahead++;

                    if (lookahead < len && input[lookahead] == ':')
                    {
                        // เป็น unquoted key → ใส่ " ครอบ
                        sb.Append('"');
                        sb.Append(input, start, i - start);
                        sb.Append('"');
                    }
                    else
                    {
                        // ไม่ใช่ key → คัดลอกออกมาตรงๆ (true, false, null, number)
                        sb.Append(input, start, i - start);
                    }
                    continue;
                }

                sb.Append(input[i]);
                i++;
            }

            string sanitized = sb.ToString();

            // ลบ trailing comma ก่อน } หรือ ] (JSON ไม่ยอมรับ)
            sanitized = Regex.Replace(sanitized, @",\s*([}\]])", "$1");

            return sanitized;
        }

        /// <summary>
        /// อ่านไฟล์ JSON เดี่ยว หรือไฟล์ในโฟลเดอร์ แล้วแปลงเป็น List ของ TranslationEntry
        /// รองรับทั้ง JSON มาตรฐาน และ JS Object Literal ที่ mod ทั่วไปใช้
        /// </summary>
        public static List<TranslationEntry> ParseJsonFile(string filePath, string categoryTag)
        {
            var entries = new List<TranslationEntry>();

            try
            {
                string rawText = File.ReadAllText(filePath);

                // ใส่ {} หุ้มถ้าไฟล์ไม่ได้เริ่มด้วย { หรือ [
                // (บาง mod ส่งมาแค่ content ภายใน ไม่มี wrapper)
                string trimmed = rawText.Trim();
                if (!trimmed.StartsWith("{") && !trimmed.StartsWith("["))
                    trimmed = "{" + trimmed + "}";

                // Sanitize ก่อน: แปลง JS Object → JSON
                string jsonText = SanitizeToJson(trimmed);

                var options = new JsonDocumentOptions
                {
                    CommentHandling = JsonCommentHandling.Skip,
                    AllowTrailingCommas = true
                };

                using (JsonDocument doc = JsonDocument.Parse(jsonText, options))
                {
                    if (doc.RootElement.ValueKind == JsonValueKind.Object)
                    {
                        foreach (JsonProperty property in doc.RootElement.EnumerateObject())
                        {
                            if (property.Value.ValueKind == JsonValueKind.String)
                            {
                                entries.Add(new TranslationEntry(
                                    property.Name, categoryTag, property.Value.GetString()));
                            }
                            else if (property.Value.ValueKind == JsonValueKind.Number)
                            {
                                entries.Add(new TranslationEntry(
                                    property.Name, categoryTag, property.Value.GetRawText()));
                            }
                            else if (property.Value.ValueKind == JsonValueKind.Object)
                            {
                                foreach (JsonProperty subProperty in property.Value.EnumerateObject())
                                {
                                    if (subProperty.Value.ValueKind == JsonValueKind.String)
                                    {
                                        entries.Add(new TranslationEntry(
                                            $"{property.Name}.{subProperty.Name}",
                                            categoryTag,
                                            subProperty.Value.GetString()));
                                    }
                                    else if (subProperty.Value.ValueKind == JsonValueKind.Number)
                                    {
                                        entries.Add(new TranslationEntry(
                                            $"{property.Name}.{subProperty.Name}",
                                            categoryTag,
                                            subProperty.Value.GetRawText()));
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (JsonException jsonEx)
            {
                System.Diagnostics.Debug.WriteLine($"JSON Parse failed after sanitize [{filePath}]: {jsonEx.Message}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error reading file [{filePath}]: {ex.Message}");
            }

            return entries;
        }

        /// <summary>
        /// ดึงข้อมูลเฉพาะฟิลด์ข้อความจาก manifest.json
        /// </summary>
        public static List<TranslationEntry> ParseManifest(string manifestPath)
        {
            var entries = new List<TranslationEntry>();

            if (!File.Exists(manifestPath)) return entries;

            try
            {
                string manifestText = File.ReadAllText(manifestPath);
                using (JsonDocument doc = JsonDocument.Parse(manifestText))
                {
                    if (doc.RootElement.ValueKind == JsonValueKind.Object)
                    {
                        string[] targetFields = { "Name", "Description", "Author" };
                        foreach (string field in targetFields)
                        {
                            if (doc.RootElement.TryGetProperty(field, out JsonElement element))
                            {
                                entries.Add(new TranslationEntry(field, "Manifest", element.GetString()));
                            }
                        }
                    }
                }
            }
            catch { }

            return entries;
        }
    }
}