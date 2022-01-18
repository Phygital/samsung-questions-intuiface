namespace SamsungAPI2
{
    public class ProductScore
    {
        public ProductScore()
        {
        }
       
        public int Score { get; set; }

        public void Add(int weighting)
        {
            Score += weighting;
        }
        
        public void Subtract(int weighting)
        {
            Score -= weighting;
        }

        public void Reset()
        {
            Score = 0;
        }
    }
}