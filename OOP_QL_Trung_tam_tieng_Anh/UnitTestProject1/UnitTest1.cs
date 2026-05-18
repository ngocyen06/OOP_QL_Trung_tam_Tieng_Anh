using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using OOP_QL_Trung_tam_tieng_Anh.Models; // Gọi namespace project chính

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Test_ThuatToan_TrungLich_HienThiDung()
        {
            // Arrange: Khởi tạo 2 lịch học trùng nhau (Cùng Thứ 2, cùng Slot 1)
            Schedule lich1 = new Schedule("SCH01", DayOfWeek.Monday, 1, "Phòng Lab 1");
            Schedule lich2 = new Schedule("SCH02", DayOfWeek.Monday, 1, "Phòng B201");

            // Act: Gọi thuật toán kiểm tra trùng lịch Overlaps đã viết ở lớp Schedule
            bool ketQuaTrungLich = lich1.Overlaps(lich2);

            // Assert: Kỳ vọng kết quả trả về phải là True (tức là phát hiện trùng nhau)
            Assert.IsTrue(ketQuaTrungLich, "Thuật toán phải phát hiện 2 ca học trùng Slot là TRÙNG NHAU.");
        }
    }
}