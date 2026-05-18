using OOP_QL_Trung_tam_tieng_Anh.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace OOP_QL_Trung_tam_tieng_Anh.Data
{
    public static class CsvHandler
    {
        public static void ExportStudents(List<Student> students, string filePath)
        {
            try
            {
                using (var writer = new StreamWriter(filePath, false, Encoding.UTF8))
                {
                    writer.WriteLine("Id,Name,Email,Phone,AttendanceScore");
                    foreach (var s in students)
                    {
                        writer.WriteLine($"{s.Id},{s.Name},{s.Email},{s.Phone},{s.AttendanceScore}");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi xuất file CSV: {ex.Message}");
            }
        }

        public static List<Student> ImportStudents(string filePath)
        {
            var list = new List<Student>();
            if (!File.Exists(filePath)) return list;

            string[] lines = File.ReadAllLines(filePath);
            // Bỏ qua dòng header đầu tiên
            for (int i = 1; i < lines.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i])) continue;
                string[] parts = lines[i].Split(',');
                if (parts.Length >= 4)
                {
                    var s = new Student(parts[0], parts[1], parts[2], parts[3]);
                    if (parts.Length == 5 && float.TryParse(parts[4], out float score))
                    {
                        s.AttendanceScore = score;
                    }
                    list.Add(s);
                }
            }
            return list;
        }
    }
}