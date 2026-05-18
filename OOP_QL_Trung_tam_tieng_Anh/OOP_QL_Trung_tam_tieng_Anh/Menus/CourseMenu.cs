using System;
using OOP_QL_Trung_tam_tieng_Anh.Models;
using OOP_QL_Trung_tam_tieng_Anh.Services;

namespace OOP_QL_Trung_tam_tieng_Anh.Menus
{
    public static class CourseMenu
    {
        public static void ShowCenterInfo()
        {
            Console.Clear();
            Console.WriteLine("=== DANH SÁCH CÁC KHÓA HỌC ĐANG MỞ ===");
            // Gọi qua lớp Program để lấy dữ liệu thực tế
            foreach (var course in Program.courses)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\n{course}");
                Console.ResetColor();

                Console.WriteLine("--- Lịch học của khóa này ---");
                foreach (var sch in course.GetSchedules())
                {
                    Console.WriteLine($"  + {sch}");
                }

                Console.WriteLine("--- Danh sách học viên đăng ký ---");
                if (course.GetStudentCount() == 0) Console.WriteLine("  (Chưa có học viên)");
                foreach (var st in course.GetStudents())
                {
                    Console.WriteLine($"  => {st.GetInfo()} [Xếp loại chuyên cần: {st.GetAttendanceStatus()}]");
                }
            }
            Console.WriteLine("\nBấm phím bất kỳ để tiếp tục...");
            Console.ReadKey();
        }

        public static void AddNewScheduleWithConflictCheck()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("=========================================================");
            Console.WriteLine("               THÊM LỊCH HỌC MỚI CHO KHÓA HỌC            ");
            Console.WriteLine("=========================================================");
            Console.ResetColor();

            Console.WriteLine("Danh sách các khóa học hiện có:");
            for (int i = 0; i < Program.courses.Count; i++)
            {
                Console.WriteLine($"{i + 1}. Khóa {Program.courses[i].CourseId} - {Program.courses[i].CourseName}");
            }
            Console.Write("\nChọn số thứ tự lớp muốn xếp lịch: ");

            if (!int.TryParse(Console.ReadLine(), out int courseChoice) || courseChoice < 1 || courseChoice > Program.courses.Count)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Lựa chọn lớp không hợp lệ! Bấm phím bất kỳ để quay lại...");
                Console.ResetColor();
                Console.ReadKey();
                return;
            }
            Course targetCourse = Program.courses[courseChoice - 1];

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"\n--- LỊCH HỌC HIỆN TẠI CỦA LỚP {targetCourse.CourseName} ---");
            var currentSchedules = targetCourse.GetSchedules();
            if (currentSchedules.Count == 0)
            {
                Console.WriteLine("  (Lớp này hiện chưa có lịch học nào)");
            }
            else
            {
                foreach (var sch in currentSchedules)
                {
                    Console.WriteLine($"  * {sch}");
                }
            }
            Console.ResetColor();
            Console.WriteLine("---------------------------------------------------------");

            Console.WriteLine("\nNhập thông tin lịch học mới:");
            Console.Write("Thứ mấy trong tuần (2 -> Thứ hai, 3 -> Thứ ba... 8 -> Chủ Nhật): ");
            if (!int.TryParse(Console.ReadLine(), out int dayInt)) dayInt = 2;

            DayOfWeek day;
            switch (dayInt)
            {
                case 2: day = DayOfWeek.Monday; break;
                case 3: day = DayOfWeek.Tuesday; break;
                case 4: day = DayOfWeek.Wednesday; break;
                case 5: day = DayOfWeek.Thursday; break;
                case 6: day = DayOfWeek.Friday; break;
                case 7: day = DayOfWeek.Saturday; break;
                default: day = DayOfWeek.Sunday; break;
            }

            Console.Write("Ca học / Slot (Nhập từ 1 đến 6): ");
            if (!int.TryParse(Console.ReadLine(), out int slot)) slot = 1;

            Console.Write("Phòng học xếp lớp: ");
            string room = Console.ReadLine().Trim();

            if (string.IsNullOrEmpty(room))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Tên phòng học không được để trống!");
                Console.ResetColor();
                Console.ReadKey();
                return;
            }

            try
            {
                string newScheduleId = "SCH_" + Guid.NewGuid().ToString("N").Substring(0, 4).ToUpper();
                Schedule newSch = new Schedule(newScheduleId, day, slot, room);

                targetCourse.AddSchedule(newSch);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\n[Thành công] Đã thêm lịch học mới cho lớp {targetCourse.CourseId}!");
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("\n--- DANH SÁCH LỊCH HỌC CHÍNH THỨC CỦA LỚP ---");
                foreach (var sch in targetCourse.GetSchedules())
                {
                    if (sch.ScheduleId == newScheduleId)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"  => {sch} (Vừa thêm)");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.WriteLine($"  * {sch}");
                    }
                }
            }
            catch (ScheduleConflictException)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n[THẤT BẠI] Không thể thêm lịch! Thời gian hoặc phòng học này đã bị trùng với lịch khác.");
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n[LỖI] Có lỗi xảy ra: {ex.Message}");
            }

            Console.ResetColor();
            Console.WriteLine("\nBấm phím bất kỳ để quay lại Menu chính...");
            Console.ReadKey();
        }
        public static void ExportToCsvDemo()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("=========================================================");
            Console.WriteLine("               XUẤT DANH SÁCH HỌC VIÊN RA FILE CSV       ");
            Console.WriteLine("=========================================================");
            Console.ResetColor();

            Console.WriteLine("Danh sách các khóa học hiện có:");
            for (int i = 0; i < Program.courses.Count; i++)
            {
                Console.WriteLine($"{i + 1}. Khóa {Program.courses[i].CourseId} - {Program.courses[i].CourseName} ({Program.courses[i].GetStudentCount()} học viên)");
            }
            Console.Write("\nChọn số thứ tự lớp muốn xuất file: ");

            if (!int.TryParse(Console.ReadLine(), out int courseChoice) || courseChoice < 1 || courseChoice > Program.courses.Count)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Lựa chọn lớp không hợp lệ! Bấm phím bất kỳ để quay lại...");
                Console.ResetColor();
                Console.ReadKey();
                return;
            }
            Course targetCourse = Program.courses[courseChoice - 1];

            string path = $"DanhSachHocVien_{targetCourse.CourseId}.csv";

            try
            {
                targetCourse.ExportStudentListToCsv(path);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\n[SUCCESS] Đã xuất thành công danh sách học viên lớp {targetCourse.CourseId}!");
                Console.WriteLine($"Tên file: '{path}'");
                Console.ResetColor();
                Console.WriteLine("-> Bạn có thể vào thư mục bin/Debug của project để mở xem bằng Excel.");
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n[LỖI] Thao tác file thất bại: {ex.Message}");
            }

            Console.ResetColor();
            Console.WriteLine("\nBấm phím bất kỳ để tiếp tục...");
            Console.ReadKey();
        }
    }
}