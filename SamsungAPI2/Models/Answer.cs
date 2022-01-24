using System.Collections.Generic;
using System.ComponentModel;


namespace SamsungAPI2
{
    public class Answer : INotifyPropertyChanged
    {
        public string Text { get; set; }

        public int Id { get; set; }


        public int CategoryId { get; set; }

        public int QuestionId { get; set; }

        public string GroupId { get; set; }

        public bool UsesToggleGroup { get; set; }

        public List<AnswerWeighting> AnswerWeighting { get; set; } = new List<AnswerWeighting>();

        private bool _isSelected = false;

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
