using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_QL_Trung_tam_Tieng_Anh
{
    internal class Program
    {
        // Tạo interface để đảm bảo tính lỏng lẻo (loose coupling)
        // Bất kỳ class nào muốn nhận thông báo đều có thể implement interface này.
        public interface IObserver
        {
            void Update(string message);
        }

        //Lớp Trừu tượng (Abstract Class) Person
        public abstract class Person
        {
            // Tính đóng gói (Encapsulation): Dùng Properties thay vì public fields.
            // 'protected set' đối với ID để chỉ cho phép gán ID lúc khởi tạo thông qua constructor.
            public string Id { get; protected set; }
            public string Name { get; set; }
            public string Email { get; set; }

            protected Person(string id, string name, string email)
            {
                Id = id;
                Name = name;
                Email = email;
            }

            // Tính đa hình (Polymorphism): Khai báo phương thức abstract.
            // Các lớp con BẮT BUỘC phải ghi đè (override) để cung cấp thông tin đặc thù.
            public abstract string GetInfo();
        }

        //Lớp Student (Kế thừa Person & Triển khai IObserver)
        public class Student : Person, IObserver
        {
            public DateTime EnrollmentDate { get; set; }
            public double AttendanceScore { get; set; } // Điểm chuyên cần (Nghiệp vụ mở rộng)

            public Student(string id, string name, string email)
                : base(id, name, email)
            {
                EnrollmentDate = DateTime.Now;
                AttendanceScore = 10.0; // Mặc định điểm tối đa
            }

            // Ghi đè phương thức GetInfo()
            public override string GetInfo()
            {
                return $"[Student] ID: {Id}, Name: {Name}, Attendance: {AttendanceScore}/10";
            }

            // Logic từ IObserver: Cách sinh viên xử lý khi nhận được thông báo
            public void Update(string message)
            {
                // Trong thực tế có thể là gửi email, ở đây ta in ra Console
                Console.WriteLine($"=> Student {Name} received notification: {message}");
            }
        }

        //Lớp Teacher (Tính lương)
        public class Teacher : Person
        {
            public decimal BaseSalary { get; set; }
            public int TeachingHours { get; set; }

            public Teacher(string id, string name, string email, decimal baseSalary)
                : base(id, name, email)
            {
                BaseSalary = baseSalary;
                TeachingHours = 0;
            }

            public override string GetInfo()
            {
                return $"[Teacher] ID: {Id}, Name: {Name}, Hours Logged: {TeachingHours}";
            }

            // Logic nghiệp vụ: Tính lương
            // Lương = Lương cơ bản/giờ * Số giờ dạy. (Giảng viên có thể hỏi cách bạn xử lý kiểu dữ liệu, dùng 'decimal' thay vì 'double' để tính tiền là chuẩn xác nhất).
            public decimal CalculateSalary()
            {
                return BaseSalary * TeachingHours;
            }
        }


        //Lớp Schedule (Thuật toán kiểm tra trùng lịch)
        public class Schedule
        {
            public DayOfWeek Day { get; set; }
            public TimeSpan StartTime { get; set; }
            public TimeSpan EndTime { get; set; }

            public Schedule(DayOfWeek day, TimeSpan startTime, TimeSpan endTime)
            {
                if (startTime >= endTime)
                    throw new ArgumentException("StartTime must be earlier than EndTime.");

                Day = day;
                StartTime = startTime;
                EndTime = endTime;
            }

            // Thuật toán kiểm tra trùng lịch (Overlap)
            // Trùng lịch khi: Cùng ngày VÀ (thời điểm bắt đầu của lịch này nhỏ hơn thời điểm kết thúc lịch kia) 
            //                 VÀ (thời điểm bắt đầu lịch kia nhỏ hơn thời điểm kết thúc lịch này).
            public bool Overlaps(Schedule other)
            {
                if (this.Day != other.Day)
                    return false;

                return (this.StartTime < other.EndTime) && (other.StartTime < this.EndTime);
            }
        }


        //Lớp Course (Aggregation & Quản lý Observer)
        public class Course
        {
            public string CourseId { get; set; }
            public string CourseName { get; set; }
            public Teacher Instructor { get; set; }

            // Aggregation: Khóa học CHỨA danh sách sinh viên. Sinh viên tồn tại độc lập với khóa học.
            private List<Student> students;
            private List<Schedule> schedules;

            public Course(string courseId, string courseName, Teacher instructor)
            {
                CourseId = courseId;
                CourseName = courseName;
                Instructor = instructor;
                students = new List<Student>();
                schedules = new List<Schedule>();
            }

            public void AddSchedule(Schedule newSchedule)
            {
                // Kiểm tra trùng lịch trước khi thêm
                foreach (var s in schedules)
                {
                    if (s.Overlaps(newSchedule))
                        throw new Exception($"Schedule conflict detected on {newSchedule.Day}.");
                }
                schedules.Add(newSchedule);
            }

            public void EnrollStudent(Student student)
            {
                if (!students.Contains(student))
                {
                    students.Add(student);
                }
            }

            // Logic của Observer Pattern (Subject)
            // Hàm này sẽ lặp qua danh sách sinh viên và gọi phương thức Update() của họ
            public void NotifyStudents(string message)
            {
                Console.WriteLine($"\n--- NOTIFICATION FROM COURSE: {CourseName} ---");
                foreach (var student in students)
                {
                    student.Update(message);
                }
            }
        }
        static void Main(string[] args)
        {
        }
    }
}
