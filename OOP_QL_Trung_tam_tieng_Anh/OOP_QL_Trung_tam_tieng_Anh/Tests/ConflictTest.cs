using System;
using OOP_QL_Trung_tam_tieng_Anh.Models;

namespace OOP_QL_Trung_tam_tieng_Anh.Tests
{
    public static class ConflictTest
    {
        public static void RunTest()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n=== CHẠY THỬ NGHIỆM THUẬT TOÁN KIỂM TRA TRÙNG LỊCH (UNIT TEST) ===");
            Console.ResetColor();

            // Tạo một giáo viên và một khóa học giả lập để test
            Teacher testTeacher = new Teacher("GV_TEST", "Giáo Viên Test", "test@lhu.edu.vn", 150000, "0123", "Test");
            Course testCourse = new Course("ENG_TEST", "Khóa Học Test", testTeacher, "A1", 10);

            // 1. Thêm một lịch học hợp lệ ban đầu
            Schedule sch1 = new Schedule("SCH01", DayOfWeek.Monday, 1, "Phòng Lab 1");
            testCourse.AddSchedule(sch1);
            Console.WriteLine("-> Thêm lịch 1 (Thứ 2, Ca 1, Phòng Lab 1): THÀNH CÔNG");

            // 2. Cố tình tạo ra lịch thứ 2 TRÙNG phòng và TRÙNG thời gian để thử nghiệm
            Schedule sch2 = new Schedule("SCH02", DayOfWeek.Monday, 1, "Phòng Lab 1");

            try
            {
                Console.WriteLine("-> Đang thử thêm lịch 2 trùng hoàn toàn với lịch 1...");
                testCourse.AddSchedule(sch2);

                // Nếu chạy xuống dòng này tức là thuật toán check trùng bị SAI
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[TEST FAILED] Thuật toán lỗi! Không phát hiện được trùng lịch.");
                Console.ResetColor();
            }
            catch (ScheduleConflictException)
            {
                // Nếu nhảy vào đây tức là thuật toán của bạn đã hoạt động HOÀN HẢO
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("[TEST PASSED] Hệ thống đã chặn và bắn ra 'ScheduleConflictException' chính xác!");
                Console.ResetColor();
            }

            Console.WriteLine("==================================================================\n");
        }
    }
}