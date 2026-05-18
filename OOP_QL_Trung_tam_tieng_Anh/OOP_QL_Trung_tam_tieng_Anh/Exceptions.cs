// ============================================================
//  FILE: Models/Exceptions.cs
//  Mục đích: Các Custom Exception đặc thù của hệ thống.
//  Dùng Custom Exception thay vì Exception gốc để:
//    - Caller phân biệt được loại lỗi và xử lý chính xác
//    - Unit Test có thể catch đúng loại exception cần kiểm tra
// ============================================================

using System;

namespace OOP_QL_Trung_tam_tieng_Anh.Models
{
    /// <summary>
    /// Ném ra khi phát hiện hai lịch học trùng nhau (cùng Thứ + cùng Slot/TimeRange).
    /// Kèm thông tin chi tiết về lịch nào xung đột để hiển thị cảnh báo.
    /// </summary>
    public class ScheduleConflictException : Exception
    {
        /// <summary>Mô tả chi tiết về xung đột lịch (ai bị trùng, lịch nào).</summary>
        public string ConflictDetails { get; }

        public ScheduleConflictException(string conflictDetails)
            : base($"[SCHEDULE CONFLICT] {conflictDetails}")
        {
            ConflictDetails = conflictDetails;
        }
    }

    /// <summary>
    /// Ném ra khi khóa học đã đầy học viên (vượt MaxStudents).
    /// </summary>
    public class CourseFullException : Exception
    {
        public string CourseName { get; }
        public int MaxCapacity { get; }

        public CourseFullException(string courseName, int maxCapacity)
            : base($"Course '{courseName}' is full. Maximum capacity: {maxCapacity} students.")
        {
            CourseName = courseName;
            MaxCapacity = maxCapacity;
        }
    }

    /// <summary>
    /// Ném ra khi file CSV có định dạng không hợp lệ (header sai, thiếu cột, v.v.).
    /// </summary>
    public class InvalidCsvFormatException : Exception
    {
        public string FilePath { get; }
        public int LineNumber { get; }

        public InvalidCsvFormatException(string message, string filePath, int lineNumber = 0)
            : base($"CSV Error at '{filePath}' line {lineNumber}: {message}")
        {
            FilePath = filePath;
            LineNumber = lineNumber;
        }
    }
}