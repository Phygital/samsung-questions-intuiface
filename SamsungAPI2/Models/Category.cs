using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace SamsungAPI2
{
    public class Category : INotifyPropertyChanged
    {
        public string Name { get; set; }

        public int Id { get; set; }

        public List<Question> Questions { get; set; } = new List<Question>();

        public List<Product> Products { get; set; } = new List<Product>();

        private int _currentQuestionIndex = 1;
        public int CurrentQuestionIndex {
            get => _currentQuestionIndex;
            set
            {
                _currentQuestionIndex = value;
                OnPropertyChanged(nameof(CurrentQuestionIndex));
                OnPropertyChanged(nameof(NextQuestion));
            }
        }

        public bool NextQuestion
        {
            get => CurrentQuestionIndex < QuestionsLength;
        }

        public int QuestionsLength
        {
            get => Questions.Count;
        }

        public void SelectAnswer(int questionId, int answerId, bool isSelected)
        {
            var question = Questions.Find(x => x.Id == questionId);
            var answer = question.AnswerGroupings.SelectMany(x => x.Answers).ToList().Find(x => x.Id == answerId);

            if (answer != null && answer.AnswerWeighting != null)
            {
                answer.IsSelected = isSelected;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
