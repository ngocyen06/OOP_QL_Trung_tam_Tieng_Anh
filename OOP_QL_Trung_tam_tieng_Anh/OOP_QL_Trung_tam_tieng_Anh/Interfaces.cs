// ============================================================
//  FILE: Models/Interfaces.cs
//  Mục đích: Khai báo tất cả Interface dùng trong hệ thống.
//  Tách riêng interface ra file này giúp dễ mở rộng và test.
// ============================================================

namespace OOP_QL_Trung_tam_tieng_Anh.Models
{
    // ----------------------------------------------------------
    //  IObserver – Hợp đồng nhận thông báo (Observer Pattern)
    //
    //  Bất kỳ class nào muốn nhận thông báo đều implement interface này.
    //  Đây là nền tảng của Loose Coupling:
    //    - Subject (Course/NotificationCenter) chỉ giữ List<IObserver>
    //    - Subject không cần biết cụ thể là Student, Email, SMS hay bất kỳ class nào khác
    //    - Muốn thêm kênh nhận thông báo mới → chỉ cần implement IObserver, 
    //      không cần sửa code Course (Open/Closed Principle)
    // ----------------------------------------------------------
    public interface IObserver
    {
        /// <summary>
        /// Được gọi tự động khi Subject phát thông báo.
        /// </summary>
        /// <param name="notification">Nội dung thông báo</param>
        void Update(string notification);
    }

    // ----------------------------------------------------------
    //  ISubject – Hợp đồng phát thông báo (Observer Pattern)
    //
    //  Định nghĩa hợp đồng chuẩn cho bất kỳ lớp nào đóng vai Subject.
    //  Giúp viết Unit Test dễ hơn vì có thể mock ISubject.
    // ----------------------------------------------------------
    public interface ISubject
    {
        void Subscribe(IObserver observer);
        void Unsubscribe(IObserver observer);
        void Notify(string message);
    }
}