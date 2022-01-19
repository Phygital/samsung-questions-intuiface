using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace SamsungAPI2
{
    public class Category
    {
        public string Name { get; set; }

        public int Id { get; set; }

        public List<Question> Questions { get; set; } = new List<Question>();

        public List<Product> Products { get; set; } = new List<Product>();

        public List<Product> ProductResults { get; set; } = new List<Product>();

        public void GetTopItems(int topItemCount)
        {
            ProductResults = Products.OrderByDescending(x => x.ProductScore.Score).Take(topItemCount).ToList();
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
