using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace SamsungAPI2
{
    public class Category: INotifyPropertyChanged
    {
        public string Name { get; set; }
        
        public int Id { get; set; }
        
        public List<Question> Questions { get; set; } = new List<Question>();

        public List<Product> Products { get; set; } = new List<Product>();
        
        public int QuestionsLength
        {
            get => Questions.Count;
        }
        
        //public ObservableCollection<Product> ProductResults { get; set; } = new ObservableCollection<Product>();

        public void SelectAnswer(int questionId, int answerId, bool isSelected)
        {
            Question question = Questions.Find(x => x.Id == questionId);
            Answer answer = question.Answers.Find(x => x.Id == answerId);

            if (answer != null && answer.AnswerWeighting != null)
            {
                foreach (AnswerWeighting answerWeight in answer.AnswerWeighting)
                {
                    Product product = Products.Find(x => x.Id == answerWeight.ProductId);
                    
                    if(product == null) return;
                    
                    if (isSelected)
                    {
                        product.ProductScore.Add(answerWeight.Weight);
                    }
                    else
                    {
                        product.ProductScore.Subtract(answerWeight.Weight);
                    }
                }
            }
        }
        
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        

    }
}