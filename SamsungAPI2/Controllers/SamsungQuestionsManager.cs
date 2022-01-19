using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace SamsungAPI2
{
    public class SamsungQuestionsManager : INotifyPropertyChanged
    {
        private string fname = "./Assets/Spreadsheets/SamsungQuestions.xlsx";

        private ObservableCollection<Category> _categories = new ObservableCollection<Category>
                                                             {
                                                                 new Category
                                                                 {
                                                                     Id = 0,
                                                                     Name = "Laptop",
                                                                     Questions = new List<Question>
                                                                                 {
                                                                                     new Question
                                                                                     {
                                                                                         Id = 0,
                                                                                         QuestionDisplayType = 0,
                                                                                         Text = "Test Question 1?",
                                                                                         Answers = new List<Answer>
                                                                                                   {
                                                                                                       new Answer
                                                                                                       {
                                                                                                           Id = 0,
                                                                                                           Text = "Answer A"
                                                                                                       },
                                                                                                       new Answer
                                                                                                       {
                                                                                                           Id = 1,
                                                                                                           Text = "Answer B"
                                                                                                       },
                                                                                                       new Answer
                                                                                                       {
                                                                                                           Id = 2,
                                                                                                           Text = "Answer C"
                                                                                                       }
                                                                                                   }
                                                                                     },
                                                                                     new Question
                                                                                     {
                                                                                         Id = 2,
                                                                                         QuestionDisplayType = 1,
                                                                                         Text = "Test Question 1?",
                                                                                         Answers = new List<Answer>
                                                                                                   {
                                                                                                       new Answer
                                                                                                       {
                                                                                                           Id = 3,
                                                                                                           Text = "Answer A"
                                                                                                       },
                                                                                                       new Answer
                                                                                                       {
                                                                                                           Id = 4,
                                                                                                           Text = "Answer B"
                                                                                                       },
                                                                                                       new Answer
                                                                                                       {
                                                                                                           Id = 5,
                                                                                                           Text = "Answer C"
                                                                                                       },
                                                                                                       new Answer
                                                                                                       {
                                                                                                           Id = 6,
                                                                                                           Text = "Answer D"
                                                                                                       }
                                                                                                   }
                                                                                     },
                                                                                 }
                                                                 },
                                                                 new Category
                                                                 {
                                                                     Id = 1,
                                                                     Name = "Tablet"
                                                                 },
                                                                 new Category
                                                                 {
                                                                     Id = 2,
                                                                     Name = "Mobile"
                                                                 },
                                                                 new Category
                                                                 {
                                                                     Id = 3,
                                                                     Name = "Hearable"
                                                                 },
                                                                 new Category
                                                                 {
                                                                     Id = 4,
                                                                     Name = "Wearables"
                                                                 },
                                                             };

        public ObservableCollection<Category> Categories
        {
            get { return _categories; }
            set
            {
                _categories = value;
                OnPropertyChanged(nameof(Categories));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
