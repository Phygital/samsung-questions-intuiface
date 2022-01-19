using System.Collections.ObjectModel;
using System.ComponentModel;

namespace SamsungAPI2
{
    public class SamsungQuestionsManager : INotifyPropertyChanged
    {
        private string fname = "./Assets/Spreadsheets/SamsungQuestions.xlsx";

        public SamsungQuestionsManager()
        {
        }

        public ObservableCollection<Category> Categories { get; set; } = new ObservableCollection<Category>
                                                                         {
                                                                             new Category
                                                                             {
                                                                                 Id = 0,
                                                                                 Name = "Laptop"
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

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
