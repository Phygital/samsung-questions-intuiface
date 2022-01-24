using System.ComponentModel;

namespace SamsungAPI2
{
    public class Product : INotifyPropertyChanged
    {
        public string Name { get; set; }

        public int Id { get; set; }

        public string Description { get; set; }

        private int _totalWeight = 0;
        public int TotalWeight
        {
            get => _totalWeight;
            set
            {
                _totalWeight = value;
                OnPropertyChanged(nameof(TotalWeight));
            }
        }

        private bool _isPerfectMatch = false;
        public bool IsPerfectMatch
        {
            get => _isPerfectMatch;
            set
            {
                _isPerfectMatch = value;
                OnPropertyChanged(nameof(IsPerfectMatch));
            }
        }

        public ProductScore ProductScore { get; set; } //TODO - Not used (removing it blows up Intuiface because it's shit)

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
