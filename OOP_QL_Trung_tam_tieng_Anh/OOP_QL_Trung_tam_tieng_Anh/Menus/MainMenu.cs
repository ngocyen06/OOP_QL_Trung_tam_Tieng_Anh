using System;
using OOP_QL_Trung_tam_tieng_Anh.Models;
using OOP_QL_Trung_tam_tieng_Anh.Services;

namespace OOP_QL_Trung_tam_tieng_Anh.Menus
{
    public static class MainMenu
    {
        public static void Display()
        {
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("=========================================================");
                Console.WriteLine("   HỆ THỐNG QUẢN LÝ TRUNG TÂM TIẾNG ANH ");
                Console.WriteLine("=========================================================");
                Console.ResetColor();
                Console.WriteLine("1. Xem danh sách Khóa học & Học viên");
                Console.WriteLine("2. Thêm Lịch học mới cho khóa học");
                Console.WriteLine("3. Đăng ký Học viên vào lớp");
                Console.WriteLine("4. Tính lương Giáo viên & Điểm chuyên cần");
                Console.WriteLine("5. Module Sinh bài tập trắc nghiệm tự động & Chấm điểm");
                Console.WriteLine("6. Xuất danh sách học viên lớp mẫu ra file CSV");
                Console.WriteLine("7. Thêm học viên mới vào hệ thống");
                Console.WriteLine("0. Thoát chương trình");
                Console.WriteLine("=========================================================");
                Console.Write("Chọn tính năng (0-7): ");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        CourseMenu.ShowCenterInfo();
                        break;
                    case "2":
                        CourseMenu.AddNewScheduleWithConflictCheck();
                        break;
                    case "3":
                        StudentMenu.EnrollStudentWithObserver();
                        break;
                    case "4":
                        CalculateSalaryAndAttendance();
                        break;
                    case "5":
                        RunQuizModule();
                        break;
                    case "6":
                        CourseMenu.ExportToCsvDemo();
                        break;
                    case "7":
                        StudentMenu.AddNewStudent();
                        break;
                    case "0":
                        Console.WriteLine("\nCảm ơn bạn đã sử dụng phần mềm! Tạm biệt.");
                        return;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Lựa chọn không hợp lệ. Bấm phím bất kỳ để chọn lại...");
                        Console.ResetColor();
                        Console.ReadKey();
                        break;
                }
            }
        }


        // tính lương và quiz


        static void CalculateSalaryAndAttendance()
        {
            Console.Clear();
            Console.WriteLine("=== LOGIC NGHIỆP VỤ: TÍNH LƯƠNG GIÁO VIÊN & ĐIỂM CHUYÊN CẦN ===");

            Console.WriteLine("\n1. Báo cáo lương Giáo viên (Decimal Precision):");
            foreach (var tc in Program.teachers)
            {
                Console.WriteLine($"  - Giáo viên: {tc.Name} | Số giờ đã dạy tích lũy: {tc.TeachingHours} giờ");
                Console.WriteLine($"    => Tổng lương thực nhận: {tc.CalculateSalary():N0} VND");
            }

            Console.WriteLine("\n2. Cập nhật & Kiểm tra Điểm chuyên cần của Sinh Viên:");
            Console.WriteLine("Mặc định ban đầu sinh viên có 10.0 điểm chuyên cần.");
            Console.Write("Nhập mã sinh viên muốn trừ điểm chuyên cần do vắng học (SV01/SV02): ");
            string id = Console.ReadLine().Trim().ToUpper();

            var st = Program.students.Find(x => x.Id == id);
            if (st != null)
            {
                Console.Write("Nhập số điểm chuyên cần muốn trừ (Ví dụ: 1.5): ");
                if (float.TryParse(Console.ReadLine(), out float subScore))
                {
                    st.AttendanceScore -= subScore;
                    if (st.AttendanceScore < 0) st.AttendanceScore = 0;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"\n[SUCCESS] Cập nhật thành công!");
                    Console.ResetColor();
                    Console.WriteLine($"Thông tin mới: {st.GetInfo()} -> Xếp loại: {st.GetAttendanceStatus()}");
                }
            }
            else
            {
                Console.WriteLine("Không tìm thấy sinh viên.");
            }

            Console.WriteLine("\nBấm phím bất kỳ để tiếp tục...");
            Console.ReadKey();
        }

        static void RunQuizModule()
        {
            Console.Clear();
            Console.WriteLine("=== MODULE SINH BÀI TẬP TRẮC NGHIỆM TỰ ĐỘNG ===");
            Console.Write("Nhập mã số học viên muốn làm bài (Ví dụ: SV01, SV02, SV03): ");
            string studentId = Console.ReadLine().Trim().ToUpper();

            var st = Program.students.Find(x => x.Id == studentId);
            if (st == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Không tìm thấy học viên này trong hệ thống! Bấm phím bất kỳ để quay lại...");
                Console.ResetColor();
                Console.ReadKey();
                return;
            }

            Console.Write($"Chào {st.Name}, bạn muốn làm bài trắc nghiệm trình độ nào? (A1 / B1): ");
            string lvl = Console.ReadLine().Trim().ToUpper();

            Program.quizService.GenerateAndRunQuiz(lvl, st);
        }

        

        
    }
}