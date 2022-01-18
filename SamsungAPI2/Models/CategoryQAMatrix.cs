using System.Collections.Generic;
using System.Linq;

namespace SamsungAPI2
{
    public class CategoryQAMatrix
    {
        public CategoryQAMatrix()
        {
        }

        public Category Category { get; set; } = new Category();

        public List<Question> Questions { get; set; } = new List<Question>();

        public List<Product> Products { get; set; } = new List<Product>();

        public List<Product> GetTopItems(int topItemCount)
        {
            var topSelection = Products.OrderByDescending(x => x.ProductScore.Score).Take(topItemCount).ToList();
            return topSelection;
        }

        public void ResetScores()
        {
            foreach (Product product in Products)
            {
                product.ProductScore.Reset();                
            }
        }
    }
}