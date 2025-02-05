﻿using System.Collections.Generic;

namespace SamsungAPI2
{
    public class Question
    {
        public string Text { get; set; }

        public int Id { get; set; }

        public int Order { get; set; }

        public string QuestionDisplayType { get; set; }

        public List<AnswerGrouping> AnswerGroupings { get; set; } = new List<AnswerGrouping>();
    }
}
