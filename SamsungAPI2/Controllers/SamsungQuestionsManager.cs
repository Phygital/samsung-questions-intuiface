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

        private int TopItemCount { get; set; } = 1;

        public SamsungQuestionsManager()
        {
            var fname = $"{CurrentDirectory}\\SamsungQuestions.xlsx";

            _productResults = new ObservableCollection<Product>();

            SpreadsheetManager spreadsheetManager = new SpreadsheetManager();
            _categories = new ObservableCollection<Category>(spreadsheetManager.ReadSpreadSheet(fname, true));

            GetTopItems(1);
        }



        public string CurrentDirectory
        {
            get => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
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

        public void GetTopItems(int categoryId)
        {
            var category = _categories.FirstOrDefault(x => x.Id == categoryId);
            if (category == null) return;

            ProductResults.Clear();

            ResetScores(category.Id);

            CalculateWeighting(category.Id);

            var topProducts = category.Products.OrderByDescending(x => x.ProductScore.Score).Take(TopItemCount);

            foreach (var product in topProducts)
            {
                ProductResults.Add(product);
            }

            OnPropertyChanged(nameof(ProductResults));
        }

        public void CalculateWeighting(int categoryId)
        {
            var category = _categories.FirstOrDefault(x => x.Id == categoryId);

            if(category!= null && category.Questions.Any())
            {
                foreach (var question in category.Questions)
                {
                    foreach (Answer answer in question.Answers.Where(x => x.IsSelected).ToList())
                    {
                        foreach (AnswerWeighting answerWeight in answer.AnswerWeighting)
                        {
                            Product product = category.Products.Find(x => x.Id == answerWeight.ProductId);

                            if (product == null) return;

                            product.ProductScore.Add(answerWeight.Weight);
                        }
                    }
                }
            }
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
            //GetTopItems(categoryId);
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
