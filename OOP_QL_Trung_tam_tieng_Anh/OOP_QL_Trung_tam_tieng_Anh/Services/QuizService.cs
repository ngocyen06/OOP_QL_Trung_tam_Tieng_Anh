using System;
using System.Collections.Generic;
using OOP_QL_Trung_tam_tieng_Anh.Models;

namespace OOP_QL_Trung_tam_tieng_Anh.Services
{
    public class QuizQuestion
    {
        public string QuestionText { get; set; }
        public string[] Options { get; set; }
        public int CorrectOptionIndex { get; set; }
    }

    public class QuizService
    {
        private List<QuizQuestion> _bankA1 = new List<QuizQuestion>
        {
            new QuizQuestion { QuestionText = "I ___ a student.", Options = new[] { "am", "is", "are", "be" }, CorrectOptionIndex = 0 },
            new QuizQuestion { QuestionText = "Where ___ you from?", Options = new[] { "am", "is", "are", "do" }, CorrectOptionIndex = 2 },
            new QuizQuestion { QuestionText = "She ___ a book every day.", Options = new[] { "read", "reads", "reading", "to read" }, CorrectOptionIndex = 1 }
        };

        private List<QuizQuestion> _bankB1 = new List<QuizQuestion>
        {
            new QuizQuestion { QuestionText = "If it rains tomorrow, we ___ the picnic.", Options = new[] { "cancel", "would cancel", "will cancel", "canceled" }, CorrectOptionIndex = 2 },
            new QuizQuestion { QuestionText = "He has been living here ___ 2020.", Options = new[] { "for", "since", "during", "in" }, CorrectOptionIndex = 1 },
            new QuizQuestion { QuestionText = "The movie was ___ than I expected.", Options = new[] { "more interesting", "interestinger", "as interesting", "most interesting" }, CorrectOptionIndex = 0 }
        };

        // Sửa hàm để truyền thêm đối tượng Student vào chấm điểm
        public void GenerateAndRunQuiz(string level, Student student)
        {
            var questions = level.ToUpper() == "B1" ? _bankB1 : _bankA1;
            Console.Clear();
            Console.WriteLine($"=== BÀI KIỂM TRA TRẮC NGHIỆM - HỌC VIÊN: {student.Name.ToUpper()} - TRÌNH ĐỘ {level.ToUpper()} ===");
            int score = 0;

            for (int i = 0; i < questions.Count; i++)
            {
                var q = questions[i];
                Console.WriteLine($"\nCâu {i + 1}: {q.QuestionText}");
                Console.WriteLine($"A. {q.Options[0]}");
                Console.WriteLine($"B. {q.Options[1]}");
                Console.WriteLine($"C. {q.Options[2]}");
                Console.WriteLine($"D. {q.Options[3]}");

                Console.Write("Đáp án của bạn (A, B, C, D): ");
                string ans = Console.ReadLine().Trim().ToUpper();
                int chosenIndex;
                switch (ans)
                {
                    case "A":
                        chosenIndex = 0;
                        break;
                    case "B":
                        chosenIndex = 1;
                        break;
                    case "C":
                        chosenIndex = 2;
                        break;
                    case "D":
                        chosenIndex = 3;
                        break;
                    default:
                        chosenIndex = -1;
                        break;
                }
                ;

                if (chosenIndex == q.CorrectOptionIndex)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("=> Chính xác!");
                    score++;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"=> Sai rồi! Đáp án đúng là: {(char)('A' + q.CorrectOptionIndex)}");
                }
                Console.ResetColor();
            }

            double finalGrade = (double)score / questions.Count * 10;
            Console.WriteLine("\n-------------------------------------------");
            Console.WriteLine($"KẾT QUẢ CỦA HỌC VIÊN: {student.Name}");
            Console.WriteLine($"Số câu đúng: {score}/{questions.Count} câu.");
            Console.WriteLine($"Điểm số trắc nghiệm đạt được: {finalGrade:F1}/10.0");
            Console.WriteLine("-------------------------------------------");
            Console.WriteLine("Bấm phím bất kỳ để quay lại Menu...");
            Console.ReadKey();
        }
    }
}