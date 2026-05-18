using System;
using System.Collections.Generic;
using System.Linq;

namespace OOP_QL_Trung_tam_tieng_Anh.Models
{
    /// <summary>
    /// Course đóng vai Subject trong Observer Pattern.
    /// Aggregation: Course CHỨA List&lt;Student&gt;, Student tồn tại độc lập với Course.
    /// Implements ISubject: có Subscribe/Unsubscribe/Notify chuẩn.
    /// </summary>
    public class Course : ISubject
    {
        // ── Private fields (Encapsulation) ───────────────────────
        private string _courseId;
        private string _courseName;
        private string _level;          // A1, A2, B1, B2, C1, C2
        private int _maxStudents;
        private List<Student> _students;       // Aggregation
        private Teacher _teacher;
        private List<Schedule> _schedules;
        private List<IObserver> _observers;      // Observer Pattern list

        // ── Properties ──────────────────────────────────────────
        public string CourseId { get => _courseId; set => _courseId = value; }
        public string CourseName { get => _courseName; set => _courseName = value; }
        public string Level { get => _level; set => _level = value; }
        public Teacher Instructor { get => _teacher; set => _teacher = value; }

        public int MaxStudents
        {
            get => _maxStudents;
            set => _maxStudents = (value > 0)
                ? value
                : throw new ArgumentException("MaxStudents phải > 0.");
        }

        // ── Constructor ──────────────────────────────────────────
        public Course(string courseId, string courseName, Teacher instructor,
                      string level = "A1", int maxStudents = 30)
        {
            _courseId = courseId;
            _courseName = courseName;
            _teacher = instructor;
            _level = level;
            _maxStudents = maxStudents;
            _students = new List<Student>();
            _schedules = new List<Schedule>();
            _observers = new List<IObserver>();

            // Đăng ký khóa học này vào danh sách của giáo viên
            if (instructor != null && !instructor.Courses.Contains(this))
                instructor.Courses.Add(this);
        }

        // ── Read-only accessors ──────────────────────────────────
        public int GetStudentCount() => _students.Count;
        public List<Schedule> GetSchedules() => new List<Schedule>(_schedules);
        public List<Student> GetStudents() => new List<Student>(_students);

        // ============================================================
        //  OBSERVER PATTERN – ISubject Implementation
        // ============================================================

        /// <summary>
        /// Đăng ký một Observer (sinh viên, email, SMS...) vào danh sách nhận thông báo.
        /// Loose Coupling: chỉ cần implement IObserver, không cần sửa Course.
        /// </summary>
        public void Subscribe(IObserver observer)
        {
            if (!_observers.Contains(observer))
                _observers.Add(observer);
        }

        /// <summary>Hủy đăng ký nhận thông báo.</summary>
        public void Unsubscribe(IObserver observer)
        {
            _observers.Remove(observer);
        }

        /// <summary>
        /// Phát thông báo đến tất cả Observer đã đăng ký.
        /// Đây là hàm cốt lõi của Observer Pattern: duyệt List&lt;IObserver&gt;
        /// và gọi Update() trên từng phần tử qua interface (không gọi Student cụ thể).
        /// </summary>
        public void Notify(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\n--- NOTIFICATION FROM COURSE: {CourseName} ---");
            Console.ResetColor();
            foreach (var obs in _observers)
                obs.Update(message);
        }

        // ── Alias giữ backward-compat với code sinh viên đã làm ─
        /// <summary>
        /// Giữ nguyên NotifyStudents() mà sinh viên đã viết.
        /// </summary>
        public void NotifyStudents(string message) => Notify(message);

        // ============================================================
        //  SCHEDULE MANAGEMENT – Thuật toán trùng lịch
        // ============================================================

        /// <summary>
        /// Thêm lịch học mới sau khi kiểm tra xung đột 3 tầng:
        ///   Tầng 1 – Trùng nội bộ khóa học này
        ///   Tầng 2 – Trùng lịch dạy hiện tại của Giáo viên
        ///   Tầng 3 – Trùng lịch học hiện tại của từng Sinh viên
        ///
        /// Nếu trùng → đổi màu đỏ + ném ScheduleConflictException.
        /// Giữ nguyên logic cốt lõi sinh viên đã làm (AddSchedule + Overlaps check),
        /// mở rộng thêm kiểm tra giáo viên và sinh viên.
        /// </summary>
        public void AddSchedule(Schedule newSchedule)
        {
            // ─ Tầng 1: Kiểm tra nội bộ khóa học (giữ nguyên logic cũ) ─
            foreach (var s in _schedules)
            {
                if (s.Overlaps(newSchedule))
                    ThrowConflict(newSchedule, s, $"Khóa học '{CourseName}' (nội bộ)");
            }

            // ─ Tầng 2: Kiểm tra lịch dạy của Giáo viên ─────────
            if (_teacher != null)
            {
                foreach (var ts in _teacher.GetTeachingSchedule())
                {
                    if (ts.Overlaps(newSchedule))
                        ThrowConflict(newSchedule, ts,
                            $"Giáo viên '{_teacher.Name}' đã có lịch dạy khóa khác");
                }
            }

            // ─ Tầng 3: Kiểm tra lịch của từng Sinh viên ─────────
            foreach (var student in _students)
            {
                var studentSchedules = GetAllStudentSchedules(student);
                foreach (var ss in studentSchedules)
                {
                    if (ss.Overlaps(newSchedule))
                        ThrowConflict(newSchedule, ss,
                            $"Sinh viên '{student.Name}' đã có lịch khóa khác");
                }
            }

            _schedules.Add(newSchedule);

            // Cộng số giờ dạy cho giáo viên
            if (_teacher != null)
                _teacher.TeachingHours +=
                    (float)(newSchedule.EndTime - newSchedule.StartTime).TotalHours;

            Notify($"Lịch học mới được thêm: {newSchedule}");
        }

        // ── Helper: lấy toàn bộ lịch của một sinh viên ──────────
        private List<Schedule> GetAllStudentSchedules(Student s)
        {
            var result = new List<Schedule>();
            foreach (var c in s.Courses)
                result.AddRange(c.GetSchedules());
            return result;
        }

        // ── Helper: ném exception + đổi màu đỏ ─────────────────
        private void ThrowConflict(Schedule newS, Schedule existS, string who)
        {
            string detail = $"{who} | Lịch mới: {newS} | Lịch hiện tại: {existS}";
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n!!! TRÙNG LỊCH PHÁT HIỆN !!!\n  {detail}");
            Console.ResetColor();
            throw new ScheduleConflictException(detail);
        }

        // ============================================================
        //  STUDENT MANAGEMENT
        // ============================================================

        /// <summary>
        /// Đăng ký sinh viên vào khóa học.
        /// Kiểm tra: (1) sĩ số tối đa, (2) trùng lịch của sinh viên, (3) không trùng lặp.
        /// Sau khi thành công: tự động Subscribe sinh viên làm Observer.
        ///
        /// Giữ nguyên logic EnrollStudent() sinh viên đã làm, bổ sung kiểm tra sĩ số
        /// và trùng lịch.
        /// </summary>
        public bool AddStudent(Student student)
        {
            // ─ Kiểm tra trùng lặp (giữ nguyên logic cũ) ─────────
            if (_students.Contains(student))
            {
                Console.WriteLine($"  [INFO] {student.Name} đã đăng ký khóa này rồi.");
                return false;
            }

            // ─ Kiểm tra sĩ số ────────────────────────────────────
            if (_students.Count >= _maxStudents)
                throw new CourseFullException(CourseName, _maxStudents);

            // ─ Kiểm tra trùng lịch của sinh viên ─────────────────
            var studentExisting = GetAllStudentSchedules(student);
            foreach (var ns in _schedules)
            {
                foreach (var es in studentExisting)
                {
                    if (ns.Overlaps(es))
                        ThrowConflict(ns, es,
                            $"Sinh viên '{student.Name}' khi đăng ký khóa '{CourseName}'");
                }
            }

            _students.Add(student);
            student.Courses.Add(this);
            Subscribe(student);   // Tự động thành Observer
            return true;
        }

        // ── Alias backward-compat ────────────────────────────────
        /// <summary>
        /// Giữ nguyên EnrollStudent() mà sinh viên đã viết.
        /// </summary>
        public void EnrollStudent(Student student)
        {
            if (!_students.Contains(student))
            {
                _students.Add(student);
                student.Courses.Add(this);
                Subscribe(student);
            }
        }

        /// <summary>Xóa sinh viên khỏi khóa học và hủy đăng ký Observer.</summary>
        public bool RemoveStudent(string studentId)
        {
            var s = _students.FirstOrDefault(x => x.Id == studentId);
            if (s == null) return false;
            _students.Remove(s);
            s.Courses.Remove(this);
            Unsubscribe(s);
            return true;
        }

        /// <summary>
        /// Xuất danh sách học viên ra file CSV.
        /// Ủy quyền cho CsvHandler (Single Responsibility).
        /// </summary>
        public void ExportStudentListToCsv(string filePath)
        {
            // Gọi CsvHandler – tránh Course biết chi tiết về file I/O
            Data.CsvHandler.ExportStudents(_students, filePath);
            Console.WriteLine($"  [CSV] Đã xuất {_students.Count} học viên ra '{filePath}'");
        }

        public override string ToString() =>
            $"[Course] {CourseId} | {CourseName} | Level: {Level} | " +
            $"Giáo viên: {_teacher?.Name ?? "Chưa phân công"} | " +
            $"Học viên: {_students.Count}/{_maxStudents}";
    }
}