using System.Collections.Generic;

namespace SamsungAPI2
{
    public class Question
    {
        public string Text { get; set; }
        
        public int Id { get; set; }
        
        public int Order { get; set; }
       
        public QuestionDisplayType QuestionDisplayType { get; set; }

        public List<Answer> Answers { get; set; } = new List<Answer>();
    }
}