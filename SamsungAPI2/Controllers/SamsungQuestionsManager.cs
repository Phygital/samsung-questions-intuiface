using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Reflection;

namespace SamsungAPI2
{
    public class SamsungQuestionsManager : INotifyPropertyChanged
    {
        public SamsungQuestionsManager()
        {
#if DEBUG
            var fname = "../../../SamsungQuestions.xlsx/SamsungQuestions.xlsx";
#else
            var fname = "./SamsungQuestions.xlsx/SamsungQuestions.xlsx";
#endif
            if (!File.Exists(fname))
            {
                Console.WriteLine(Assembly.GetExecutingAssembly().Location);    
            }
            
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