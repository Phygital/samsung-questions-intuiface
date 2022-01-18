namespace SamsungAPI2
{
    public class Product
    {
        public string Name { get; set; }
        
        public int Id { get; set; }
        
        public string Description { get; set; }

        public ProductScore ProductScore { get; set; } = new ProductScore();
    }
}