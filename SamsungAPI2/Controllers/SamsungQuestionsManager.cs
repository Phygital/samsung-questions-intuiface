using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;

namespace SamsungAPI2
{
    public class SamsungQuestionsManager : INotifyPropertyChanged
    {
        private ObservableCollection<Product> _productResults; 
        private ObservableCollection<Category> _categories;
        
        public SamsungQuestionsManager()
        {
#if DEBUG
//            var fname = "../../../SamsungQuestions.xlsx/SamsungQuestions.xlsx";
            var fname = "./SamsungQuestions.xlsx/SamsungQuestions.xlsx";
#else
            var fname = "./SamsungQuestions.xlsx/SamsungQuestions.xlsx";
#endif
            if (!File.Exists(fname))
            {
                Console.WriteLine(Assembly.GetExecutingAssembly().Location);    
            }
            
            _productResults = new ObservableCollection<Product>();
    
            SpreadsheetManager spreadsheetManager = new SpreadsheetManager();
            _categories = new ObservableCollection<Category>(spreadsheetManager.ReadSpreadSheet(fname, true));
            
            GetTopItems(3, 1);
        }

        public int CategoriesLength
        {
            get => Categories.Count;
        }

        public ObservableCollection<Category> Categories
        {
            get { return _categories; }
            set
            {
                _categories = value;
                OnPropertyChanged(nameof(Categories));
            }
        }
        public ObservableCollection<Product> ProductResults
        {
            get { return _productResults; }
            set
            {
                _productResults = value;
                OnPropertyChanged(nameof(ProductResults));
            }
        }
        
        public void GetResults()
        {
            
        }
        public void GetTopItems(int topItemCount, int categoryId)
        {
            ProductResults.Clear();
            var category = _categories.FirstOrDefault(x => x.Id == categoryId);
            var topProducts = category.Products.OrderByDescending(x => x.ProductScore.Score).Take(topItemCount);
            foreach (var product in topProducts)
            {
                ProductResults.Add(product);
            }
            OnPropertyChanged(nameof(ProductResults));
        }
        
        public void ResetScores(int categoryId)
        {
            ProductResults.Clear();
            var category = _categories.FirstOrDefault(x => x.Id == categoryId);
            
            foreach (Product product in category.Products)
            {
                product.ProductScore.Reset();                
            }
            OnPropertyChanged(nameof(ProductResults));
        }


        public void SelectAnswer(int categoryId, int questionId, int answerId, bool isSelected)
        {
            var currentCat = _categories.SingleOrDefault(x => x.Id == categoryId);
            currentCat?.SelectAnswer(questionId,answerId, isSelected);
            GetTopItems(3, categoryId);
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}