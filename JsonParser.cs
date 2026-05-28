using System;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;

namespace SVTT
{
    public static class JsonParser
    {
        /// <summary>
        /// อ่านไฟล์ JSON เดี่ยว หรือไฟล์ในโฟลเดอร์ แล้วแปลงเป็น List ของ TranslationEntry
        /// </summary>
        public static List<TranslationEntry> ParseJsonFile(string filePath, string categoryTag)
        {
            var entries = new List<TranslationEntry>();

            try
            {
                string jsonText = File.ReadAllText(filePath);

                // 💡 ตั้งค่าตัว Parser ให้ยอมรับและข้ามคอมเมนต์ (Comment) ในไฟล์ JSON ได้
                var options = new JsonDocumentOptions
                {
                    CommentHandling = JsonCommentHandling.Skip, // ข้ามคอมเมนต์ // ได้อย่างปลอดภัย
                    AllowTrailingCommas = true // ยอมรับคอมเมนต์จุลภาคตัวสุดท้ายเกินมาได้ด้วย
                };

                // ส่ง options เข้าไปตอน Parse
                using (JsonDocument doc = JsonDocument.Parse(jsonText, options))
                {
                    if (doc.RootElement.ValueKind == JsonValueKind.Object)
                    {
                        foreach (JsonProperty property in doc.RootElement.EnumerateObject())
                        {
                            if (property.Value.ValueKind == JsonValueKind.String)
                            {
                                entries.Add(new TranslationEntry(property.Name, categoryTag, property.Value.GetString()));
                            }
                            else if (property.Value.ValueKind == JsonValueKind.Object)
                            {
                                foreach (JsonProperty subProperty in property.Value.EnumerateObject())
                                {
                                    if (subProperty.Value.ValueKind == JsonValueKind.String)
                                    {
                                        entries.Add(new TranslationEntry($"{property.Name}.{subProperty.Name}", categoryTag, subProperty.Value.GetString()));
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // สามารถแอบส่องดูข้อผิดพลาดตัวอื่นได้ตรงนี้ตอน Debug
                System.Diagnostics.Debug.WriteLine($"Error parsing JSON: {ex.Message}");
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