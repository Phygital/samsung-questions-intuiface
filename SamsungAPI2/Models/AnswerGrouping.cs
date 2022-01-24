using System.Collections.Generic;

namespace SamsungAPI2
{
    public class AnswerGrouping
    {
        public string GroupId { get; set; }

        public List<Answer> Answers { get; set; } = new List<Answer>();
    }
}
