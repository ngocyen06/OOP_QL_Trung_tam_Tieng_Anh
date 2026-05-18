// ============================================================
//  FILE: Models/Schedule.cs
//  Mục đích: Lịch học theo Slot (1–6), thuật toán trùng lịch.
//  Giữ nguyên logic Overlaps() sinh viên đã làm, mở rộng thêm
//  ScheduleId, Room, và hệ thống Slot 1-6 theo đề bài.
// ============================================================

using System;

namespace OOP_QL_Trung_tam_tieng_Anh.Models
{
    /// <summary>
    /// Lịch học: xác định bởi Thứ trong tuần + Ca học (Slot 1–6).
    /// Slot tương ứng thời gian cố định theo thực tế học đường.
    /// </summary>
    public class Schedule
    {
        // ── Bảng mapping Slot → TimeRange ───────────────────────
        //  Slot 1: 07:00–09:00  |  Slot 4: 13:00–15:00
        //  Slot 2: 09:00–11:00  |  Slot 5: 15:00–17:00
        //  Slot 3: 11:00–13:00  |  Slot 6: 17:00–19:00
        private static readonly TimeSpan[] SlotStarts =
        {
            new TimeSpan(7,  0, 0),
            new TimeSpan(9,  0, 0),
            new TimeSpan(11, 0, 0),
            new TimeSpan(13, 0, 0),
            new TimeSpan(15, 0, 0),
            new TimeSpan(17, 0, 0),
        };

        private static readonly TimeSpan[] SlotEnds =
        {
            new TimeSpan(9,  0, 0),
            new TimeSpan(11, 0, 0),
            new TimeSpan(13, 0, 0),
            new TimeSpan(15, 0, 0),
            new TimeSpan(17, 0, 0),
            new TimeSpan(19, 0, 0),
        };

        // ── Properties ──────────────────────────────────────────
        public string ScheduleId { get; set; }
        public DayOfWeek Day { get; set; }
        public int SlotNumber { get; private set; }   // 1..6 (0 = custom time)
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Room { get; set; }

        // ── Constructor theo Slot (cách dùng chuẩn) ─────────────
        /// <summary>
        /// Khởi tạo lịch học theo số Slot (1–6).
        /// Slot xác định giờ bắt đầu và kết thúc tự động – tránh nhập sai giờ.
        /// </summary>
        public Schedule(string scheduleId, DayOfWeek day, int slotNumber, string room = "TBD")
        {
            if (slotNumber < 1 || slotNumber > 6)
                throw new ArgumentException("Slot phải từ 1 đến 6.");

            ScheduleId = scheduleId;
            Day = day;
            SlotNumber = slotNumber;
            StartTime = SlotStarts[slotNumber - 1];
            EndTime = SlotEnds[slotNumber - 1];
            Room = room;
        }

        // ── Constructor tùy chỉnh TimeSpan (backward-compat) ────
        /// <summary>
        /// Giữ backward-compatible với Schedule(day, startTime, endTime) mà sinh viên đã làm.
        /// Validate: StartTime phải trước EndTime (giữ nguyên logic cũ).
        /// </summary>
        public Schedule(DayOfWeek day, TimeSpan startTime, TimeSpan endTime, string room = "TBD")
        {
            // ─── Giữ nguyên validation logic của sinh viên ───────
            if (startTime >= endTime)
                throw new ArgumentException("StartTime must be earlier than EndTime.");

            Day = day;
            StartTime = startTime;
            EndTime = endTime;
            Room = room;
            ScheduleId = Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper();
            SlotNumber = DetectSlot(startTime);
        }

        // ── Phát hiện slot từ StartTime ──────────────────────────
        private int DetectSlot(TimeSpan start)
        {
            for (int i = 0; i < SlotStarts.Length; i++)
                if (SlotStarts[i] == start) return i + 1;
            return 0; // custom time, không khớp slot cố định
        }

        // ── Thuật toán kiểm tra trùng lịch ──────────────────────

        /// <summary>
        /// Kiểm tra hai lịch có trùng nhau không.
        /// Giữ nguyên thuật toán sinh viên đã làm (Interval Overlap).
        ///
        /// LOGIC (Interval Arithmetic):
        ///   Hai khoảng [A_start, A_end) và [B_start, B_end) KHÔNG giao nhau khi:
        ///     A_end <= B_start  HOẶC  B_end <= A_start
        ///   Phủ định → Trùng nhau khi:
        ///     A_start &lt; B_end  VÀ  B_start &lt; A_end
        ///   Điều kiện tiên quyết: cùng DayOfWeek.
        /// </summary>
        public bool Overlaps(Schedule other)
        {
            // ─── Giữ nguyên logic sinh viên đã làm ───────────────
            if (this.Day != other.Day)
                return false;

            return (this.StartTime < other.EndTime) && (other.StartTime < this.EndTime);
        }

        /// <summary>Alias ngữ nghĩa rõ ràng hơn khi dùng trong báo cáo.</summary>
        public bool CheckConflict(Schedule other) => Overlaps(other);

        // ── Hiển thị ─────────────────────────────────────────────
        public override string ToString()
        {
            string slotLabel = SlotNumber > 0 ? $"Slot {SlotNumber} " : "";
            return $"{Day} {slotLabel}({StartTime:hh\\:mm}–{EndTime:hh\\:mm}) @ {Room}";
        }

        /// <summary>Tên thứ bằng tiếng Việt để hiển thị menu.</summary>
        public string DayNameVi()
        {
            switch (Day)
            {
                case DayOfWeek.Monday:
                    return "Thứ 2";
                case DayOfWeek.Tuesday:
                    return "Thứ 3";
                case DayOfWeek.Wednesday:
                    return "Thứ 4";
                case DayOfWeek.Thursday:
                    return "Thứ 5";
                case DayOfWeek.Friday:
                    return "Thứ 6";
                case DayOfWeek.Saturday:
                    return "Thứ 7";
                case DayOfWeek.Sunday:
                    return "Chủ nhật";
                default:
                    return Day.ToString();
            }
        }
    }
}