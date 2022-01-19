using System.Collections.ObjectModel;
using System.ComponentModel;

namespace SamsungAPI2
{
    public class SamsungQuestionsManager : INotifyPropertyChanged
    {
        private string fname = "SamsungQuestions.xlsx";
        public SamsungQuestionsManager()
        {
            SpreadsheetManager spreadsheetManager = new SpreadsheetManager();
            _categories = new ObservableCollection<Category>(spreadsheetManager.ReadSpreadSheet(fname, true));
        }

        private int _counter = 0;

        public int Counter
        {
            get { return _counter; }
            set
            {
                _counter = value; 
                OnPropertyChanged(nameof(Counter));
            }
        } 
        public int CategoriesLength
        {
            get => Categories.Count;
        }

        private ObservableCollection<Category> _categories;

        public ObservableCollection<Category> Categories
        {
            get { return _categories; }
            set
            {
                _categories = value;
                OnPropertyChanged(nameof(Categories));
            }
        }


        public void GetResults()
        {
            Counter++;
            Categories.Add(new Category()
            {
                Id = 5,
                Name = "Noel"
            });

            OnPropertyChanged(nameof(Categories));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}