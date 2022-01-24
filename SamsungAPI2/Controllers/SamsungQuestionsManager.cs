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

        public void GetTopItems(int categoryId)
        {
            var category = _categories.FirstOrDefault(x => x.Id == categoryId);
            if (category == null) return;

            CalculateWeighting(category.Id);

            var topProducts = category.Products.OrderByDescending(x => x.IsPerfectMatch).ThenByDescending(x => x.TotalWeight).Take(TopItemCount);

            ProductResults.Clear();
            foreach (var product in topProducts)
            {
                ProductResults.Add(product);
            }

            OnPropertyChanged(nameof(ProductResults));
        }

        public void CalculateWeighting(int categoryId)
        {
            var category = _categories.FirstOrDefault(x => x.Id == categoryId);
            if (category != null && category.Questions.Any())
            {
                var products = category.Products;
                var answers = category.Questions.SelectMany(y => y.AnswerGroupings.SelectMany(x => x.Answers).Where(x => x.IsSelected)).ToList();
                foreach (var product in products)
                {
                    product.TotalWeight = 0;
                    var totalWeight = answers.SelectMany(x => x.AnswerWeighting).Where(x => x.ProductId == product.Id).Sum(x => x.Weight);
                    var isPerfectMatch = answers.SelectMany(x => x.AnswerWeighting).Where(x => x.ProductId == product.Id).All(x => x.Weight > 0);
                    product.TotalWeight = totalWeight;
                    product.IsPerfectMatch = isPerfectMatch;
                }
            }
        }


        public void ResetScores()
        {
            ProductResults.Clear();

            foreach (var category in _categories)
            {
                category.CurrentQuestionIndex = 1;
                foreach (Product product in category.Products)
                {
                    product.IsPerfectMatch = false;
                    product.TotalWeight = 0;
                }

                foreach (var question in category.Questions)
                {
                    foreach (var answerGrouping in question.AnswerGroupings)
                    {
                        foreach (var answer in answerGrouping.Answers)
                        {
                            answer.IsSelected = false;
                        }
                    }
                }
            }

            OnPropertyChanged(nameof(ProductResults));
        }

        public void SelectAnswer(int categoryId, int questionId, int answerId, bool isSelected)
        {
            var category = _categories.SingleOrDefault(x => x.Id == categoryId);
            category?.SelectAnswer(questionId, answerId, isSelected);
        }

        public void MoveNextQuestion(int categoryId)
        {
            var category = _categories.SingleOrDefault(x => x.Id == categoryId);
            if (category == null) return;
            category.CurrentQuestionIndex++;
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
