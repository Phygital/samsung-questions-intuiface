using System.Collections.ObjectModel;
using System.ComponentModel;

namespace SamsungAPI2
{
    public class SamsungQuestionsManager : INotifyPropertyChanged
    {
        private string fname = "./Assets/Spreadsheets/SamsungQuestions.xlsx";
        public SamsungQuestionsManager()
        {
            SpreadsheetManager spreadsheetManager = new SpreadsheetManager();
            spreadsheetManager.ReadSpreadSheet(fname, true);
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

        private ObservableCollection<Category> _categories = new ObservableCollection<Category>()
        {
            new Category()
            {
                Id = 0,
                Name = "Laptop"
            },
            new Category()
            {
                Id = 1,
                Name = "Tablet"
            },
            new Category()
            {
                Id = 2,
                Name = "Mobile"
            },
            new Category()
            {
                Id = 3,
                Name = "Hearable"
            },
            new Category()
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