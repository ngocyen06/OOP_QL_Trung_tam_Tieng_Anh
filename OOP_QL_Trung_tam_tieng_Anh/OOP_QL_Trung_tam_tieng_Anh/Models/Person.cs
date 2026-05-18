using System;
using System.Collections.Generic;

namespace OOP_QL_Trung_tam_tieng_Anh.Models
{
    // =========================
    //  ABSTRACT CLASS Person  
    // =========================

    /// <summary>
    /// Lớp trừu tượng là nền tảng kế thừa (Inheritance) cho toàn hệ thống.
    /// Encapsulation: tất cả fields đều private, truy cập qua Properties.
    /// 'protected set' với Id: chỉ cho phép gán lúc khởi tạo qua constructor.
    /// Polymorphism: GetInfo() là abstract → bắt buộc lớp con phải override.
    /// </summary>
    public abstract class Person
    {
        // ── Private backing fields (Encapsulation) ──────────────
        private string _id;
        private string _name;      // FullName trong diagram, alias Name (giữ compat)
        private string _email;
        private string _phone;
        //private DateTime _dateOfBirth;

        // ── Public Properties ────────────────────────────────────
        public string Id { get => _id; protected set => _id = value; }
        public string Name { get => _name; set => _name = value; }   // backward-compat
        //public string FullName { get => _name; set => _name = value; }   // per diagram
        public string Email { get => _email; set => _email = value; }
        public string Phone { get => _phone; set => _phone = value; }
        //public DateTime DateOfBirth { get => _dateOfBirth; set => _dateOfBirth = value; }

        protected Person(string id, string name, string email, string phone = "")
        {
            Id = id;
            Name = name;
            Email = email;
            Phone = phone;
        }

        /// <summary>
        /// Tính đa hình: các lớp con BẮT BUỘC ghi đè để cung cấp thông tin đặc thù.
        /// </summary>
        public abstract string GetInfo();

        public override string ToString() => GetInfo();
    }

    // ===============
    //  CLASS Student  
    // ===============

    /// <summary>
    /// Học viên kế thừa Person và implement IObserver.
    /// Implement IObserver → Student tự động nhận thông báo từ Course
    ///   mà Course không cần biết cụ thể đây là Student (Loose Coupling).
    /// </summary>
    public class Student : Person, IObserver
    {
        // ── Fields theo diagram ──────────────────────────────────
        private string _studentCode;
        private DateTime _enrollDate;
        private float _attendanceScore;
        private List<Course> _courses;

        public string StudentCode { get => _studentCode; set => _studentCode = value; }
        public DateTime EnrollDate { get => _enrollDate; set => _enrollDate = value; }
        public float AttendanceScore { get => _attendanceScore; set => _attendanceScore = value; }
        public List<Course> Courses => _courses;

        // backward-compat với code cũ sinh viên đã làm
        public DateTime EnrollmentDate
        {
            get => _enrollDate;
            set => _enrollDate = value;
        }

        public Student(string id, string name, string email, string phone = "")
            : base(id, name, email, phone)
        {
            _studentCode = id;
            _enrollDate = DateTime.Now;
            _attendanceScore = 10.0f;   // Mặc định điểm tối đa (giữ nguyên logic cũ)
            _courses = new List<Course>();
        }

        // ── Ghi đè GetInfo() – Đa hình ──────────────────────────
        /// <summary>
        /// Ghi đè phương thức GetInfo() (giữ nguyên logic sinh viên đã làm).
        /// </summary>
        public override string GetInfo()
        {
            return $"[Student] ID: {Id}, Name: {Name}, Attendance: {AttendanceScore}/10";
        }

        // ── Logic nghiệp vụ ──────────────────────────────────────

        /// <summary>
        /// Phân loại điểm chuyên cần thành chuỗi mô tả.
        /// Nghiệp vụ: điểm chuyên cần phản ánh tỷ lệ có mặt thực tế.
        /// </summary>
        public string GetAttendanceStatus()
        {
            if (AttendanceScore >= 9.0f) return "Xuất sắc";
            if (AttendanceScore >= 7.0f) return "Tốt";
            if (AttendanceScore >= 5.0f) return "Trung bình";
            return "Cần cải thiện";
        }

        // ── IObserver.Update() ───────────────────────────────────
        /// <summary>
        /// Triển khai IObserver.Update() – giữ nguyên logic sinh viên đã làm.
        /// Cơ chế Loose Coupling: Course gọi interface Update(), không gọi Student trực tiếp.
        /// </summary>
        public void Update(string notification)
        {
            // Trong thực tế có thể là gửi email, ở đây ta in ra Console (giữ nguyên)
            Console.WriteLine($"=> Student {Name} received notification: {notification}");
        }
    }

    // ===============
    //  CLASS Teacher  
    // ===============

    /// <summary>
    /// Giáo viên kế thừa Person.
    /// Nghiệp vụ mở rộng: tính lương dựa trên số ca dạy × hệ số cấp bậc.
    /// </summary>
    public class Teacher : Person
    {
        // ── Fields theo diagram ──────────────────────────────────
        private string _teacherCode;
        private string _specialization;
        private decimal _baseSalary;      // Lương cơ bản / ca dạy
        private float _teachingHours;
        private List<Course> _courses;

        public string TeacherCode { get => _teacherCode; set => _teacherCode = value; }
        public string Specialization { get => _specialization; set => _specialization = value; }
        public List<Course> Courses => _courses;

        /// <summary>
        /// BaseSalary dùng decimal (không dùng double) để tránh sai số dấu phẩy động
        /// khi tính tiền – best practice tài chính phần mềm.
        /// Validation: lương cơ bản phải > 0 (giữ constraint trong diagram).
        /// </summary>
        public decimal BaseSalary
        {
            get => _baseSalary;
            set => _baseSalary = (value > 0)
                ? value
                : throw new ArgumentException("BaseSalary phải lớn hơn 0.");
        }

        public float TeachingHours
        {
            get => _teachingHours;
            set => _teachingHours = value;
        }

        // backward-compat với code cũ (int TeachingHours)
        public int TeachingHoursInt
        {
            get => (int)_teachingHours;
            set => _teachingHours = value;
        }

        public Teacher(string id, string name, string email, decimal baseSalary, string phone = "", string specialization = "General English")
            : base(id, name, email, phone)
        {
            _teacherCode = id;
            _specialization = specialization;
            BaseSalary = baseSalary;
            _teachingHours = 0;
            _courses = new List<Course>();
        }

        public override string GetInfo()
        {
            return $"[Teacher] ID: {Id}, Name: {Name}, Hours Logged: {TeachingHours}";
        }

        // ── Logic nghiệp vụ: Tính lương ─────────────────────────

        /// <summary>
        /// Tính lương theo số ca dạy thực tế và hệ số cấp bậc (Specialization).
        /// Hệ số: IELTS = 1.5, các môn khác = 1.0.
        /// Công thức: Lương = BaseSalary/ca × Số ca dạy × Hệ số cấp bậc.
        /// </summary>
        public decimal CalculateSalary()
        {
            // Tính hệ số cấp bậc dựa trên chuyên môn (Specialization)
            decimal heSoCapBac = (Specialization.ToUpper() == "IELTS") ? 1.5m : 1.0m;

            // Mỗi slot học thêm vào được tính là 1 ca dạy (TeachingHours ở đây đóng vai trò là số ca)
            return BaseSalary * (decimal)TeachingHours * heSoCapBac;
        }

        /// <summary>
        /// Lấy toàn bộ lịch dạy của giáo viên từ tất cả khóa học hiện tại.
        /// Phục vụ thuật toán kiểm tra trùng lịch khi xếp thêm khóa học mới.
        /// </summary>
        public List<Schedule> GetTeachingSchedule()
        {
            var all = new List<Schedule>();
            foreach (var course in _courses)
                all.AddRange(course.GetSchedules());
            return all;
        }
    }
}