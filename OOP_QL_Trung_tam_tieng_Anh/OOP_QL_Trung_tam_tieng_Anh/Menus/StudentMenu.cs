using System;
using OOP_QL_Trung_tam_tieng_Anh.Models;
using OOP_QL_Trung_tam_tieng_Anh.Services;

namespace OOP_QL_Trung_tam_tieng_Anh.Menus
{
    public static class StudentMenu
    {
        public static void EnrollStudentWithObserver()
        {
            Console.Clear();
            Console.WriteLine("=== ĐĂNG KÝ HỌC VIÊN & BẮN THÔNG BÁO (OBSERVER PATTERN) ===");
            Console.Write("Nhập mã học viên muốn đăng ký vào lớp (Ví dụ: SV01, SV02, SV04...): ");
            string studentId = Console.ReadLine().Trim().ToUpper();

            var targetStudent = Program.students.Find(x => x.Id == studentId);
            if (targetStudent == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Không tìm thấy học viên này trong hệ thống! Bấm phím bất kỳ để quay lại...");
                Console.ResetColor();
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"\nChọn lớp muốn xếp học viên {targetStudent.Name} ({targetStudent.Id}) vào:");
            Console.WriteLine("1. Lớp Giao Tiếp A1 (ENG01) - Còn chỗ");
            Console.WriteLine("2. Lớp Luyện Thi B1 (ENG02) - Lớp giới hạn max 2 người");
            Console.Write("Lựa chọn (1 hoặc 2): ");
            string subChoice = Console.ReadLine();
            Course targetCourse = subChoice == "2" ? Program.courses[1] : Program.courses[0];

            try
            {
                bool success = targetCourse.AddStudent(targetStudent);
                if (success)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"\n[SUCCESS] Đăng ký thành công học viên {targetStudent.Name} vào lớp {targetCourse.CourseName}!");
                    Console.ResetColor();
                    Console.WriteLine("\nBây giờ thử gọi hàm Notify() của lớp xem học viên có tự nhận được không:");
                    targetCourse.Notify("Thông báo: Ngày mai lớp chúng ta sẽ có một bài kiểm tra nhỏ, các bạn nhớ đi học đầy đủ nhé!");
                }
            }
            catch (CourseFullException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n[LỖI] Không thể add học viên! Lý do: {ex.Message}");
            }
            catch (ScheduleConflictException)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n[LỖI] Học viên này bị trùng lịch học ở một lớp khác không thể add!");
            }

            Console.ResetColor();
            Console.WriteLine("\nBấm phím bất kỳ để tiếp tục...");
            Console.ReadKey();
        }
        public static void AddNewStudent()
        {
            Console.Clear();
            Console.WriteLine("=== THÊM HỌC VIÊN MỚI VÀO HỆ THỐNG ===");

            string id = "";
            while (true)
            {
                Console.Write("Nhập mã học viên mới (Ví dụ: SV04): ");
                id = Console.ReadLine().Trim().ToUpper();

                if (string.IsNullOrEmpty(id))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Mã học viên không được để trống!");
                    Console.ResetColor();
                    continue;
                }

                if (Program.students.Exists(x => x.Id == id))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Mã học viên {id} đã tồn tại! Vui lòng nhập mã khác.");
                    Console.ResetColor();
                }
                else
                {
                    break;
                }
            }

            Console.Write("Nhập họ và tên học viên: ");
            string name = Console.ReadLine().Trim();

            // Nếu Class Student không cần dùng đến Tuổi, bạn có thể xóa đoạn nhập tuổi này đi.
            // Ngược lại, nếu Constructor cần tuổi, bạn phải truyền thêm `age` vào sau.
            Console.Write("Nhập tuổi học viên: ");
            int age = 0;
            while (!int.TryParse(Console.ReadLine(), out age) || age <= 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Tuổi không hợp lệ! Vui lòng nhập lại số nguyên dương: ");
                Console.ResetColor();
            }

            // 1. BỔ SUNG: Nhập Email học viên
            Console.Write("Nhập email học viên: ");
            string email = Console.ReadLine().Trim();

            // 2. GIỮ NGUYÊN: Nhập Số điện thoại
            Console.Write("Nhập số điện thoại: ");
            string phone = Console.ReadLine().Trim();

            // Nhập Trình độ (Nếu class Student có thuộc tính Level, bạn có thể gán sau khi tạo object)
            Console.Write("Nhập trình độ hiện tại (Ví dụ: A1, A2, B1, B2): ");
            string level = Console.ReadLine().Trim().ToUpper();

            // 3. SỬA LẠI: Truyền đúng thứ tự (id, name, email, phone) theo Constructor yêu cầu
            Student newStudent = new Student(id, name, email, phone);

            // Nếu trong class Student của bạn có thuộc tính Level, hãy gán thêm dòng này:
            // newStudent.Level = level; 

            Program.students.Add(newStudent);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n[Thành công] Đã thêm học viên {name} ({id}) vào hệ thống!");
            Console.ResetColor();

            Console.WriteLine("\nBấm phím bất kỳ để quay lại Menu...");
            Console.ReadKey();
        }
    }
}