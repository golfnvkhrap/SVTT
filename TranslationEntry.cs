using System;

namespace SVTT
{
    public class TranslationEntry
    {
        public string Key { get; set; }          // รหัสอ้างอิง/คีย์
        public string Category { get; set; }     // หมวดหมู่ (เช่น i18n, Manifest, assets/Strings)
        public string Original { get; set; }     // ข้อความอังกฤษต้นฉบับ
        public string Translate { get; set; }    // ข้อความภาษาไทยที่แปลแล้ว
        public string Status { get; set; }       // สถานะ (เช่น รอการแปล, แปลแล้ว)

        // Constructor สำหรับสร้าง Object ข้อมูลได้เร็วขึ้น
        public TranslationEntry(string key, string category, string original, string translate = "", string status = "รอการแปล")
        {
            Key = key;
            Category = category;
            Original = original;
            Translate = translate;
            Status = string.IsNullOrEmpty(translate) ? status : "แปลแล้ว";
        }
    }
}