using OOP_QL_Trung_tam_tieng_Anh.Menus; // Thêm dòng này để nhận diện thư mục Menus
using OOP_QL_Trung_tam_tieng_Anh.Models;
using OOP_QL_Trung_tam_tieng_Anh.Services;
using OOP_QL_Trung_tam_tieng_Anh.Tests;
using System;
using System.Collections.Generic;

namespace OOP_QL_Trung_tam_tieng_Anh
{
    internal class Program
    {
        // Đã đổi thành public static để bên MainMenu dùng chung dữ liệu
        public static List<Student> students = new List<Student>();
        public static List<Teacher> teachers = new List<Teacher>();
        public static List<Course> courses = new List<Course>();
        public static QuizService quizService = new QuizService();

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;


            // Khởi tạo dữ liệu mẫu
            InitSampleData();

            // ====================================================================
            // 2. THÊM DÒNG NÀY VÀO ĐỂ CHẠY TEST THUẬT TOÁN TRƯỚC
            ConflictTest.RunTest();
            // ====================================================================

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("Bấm phím bất kỳ để tiếp tục vào Hệ thống quản lý...");
            Console.ResetColor();
            Console.ReadKey();

            // Chạy Menu chính sau khi kết thúc test
            MainMenu.Display();
        }

        static void InitSampleData()
        {
            var tc1 = new Teacher("GV01", "Nguyễn Văn A", "vana@lhu.edu.vn", 150000, "0912345678", "IELTS");
            var tc2 = new Teacher("GV02", "Trần Thị B", "thib@lhu.edu.vn", 180000, "0987654321", "TOEIC");
            teachers.Add(tc1);
            teachers.Add(tc2);

            var st1 = new Student("SV01", "Nguyễn Thị C", "C@student.com", "0123");
            var st2 = new Student("SV02", "Nguyễn Thị D", "D@student.com", "0456");
            var st3 = new Student("SV03", "Nguyễn Thị E", "E@student.com", "0789");
            students.Add(st1);
            students.Add(st2);
            students.Add(st3);

            var c1 = new Course("ENG01", "Lớp Tiếng Anh Giao Tiếp A1", tc1, "A1", 30);
            var c2 = new Course("ENG02", "Lớp Luyện Thi B1 Cấp Tốc", tc2, "B1", 2);
            courses.Add(c1);
            courses.Add(c2);

            c1.AddSchedule(new Schedule("SCH01", DayOfWeek.Monday, 1, "Phòng Lab 1"));
            c2.AddSchedule(new Schedule("SCH02", DayOfWeek.Monday, 2, "Phòng B201"));

            c1.AddStudent(st1);
            c1.AddStudent(st2);
        }
    }
}