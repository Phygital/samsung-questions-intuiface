using System.Collections.Generic;

namespace SamsungAPI2
{
    public class Question
    {
        public string Text { get; set; }

        public int Id { get; set; }

        public int Order { get; set; }

        public int QuestionDisplayType { get; set; } //0 = Radio, 1 = Checkbox

        public List<Answer> Answers { get; set; } = new List<Answer>();
    }
}
