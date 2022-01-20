using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using SamsungAPI2;

namespace TestBed
{
    
    class Program
    {
        
        //private static string fname = "../../../../SamsungAPI2/Assets/Spreadsheets/SamsungQuestions.xlsx";
        
        // private static  ObservableCollection<Category> _categories = new ObservableCollection<Category>();
        
        static void Main(string[] args)
        {
            SamsungQuestionsManager samsungQuestionsManager = new SamsungQuestionsManager(); 
            
            Category category;
            
            
            if (samsungQuestionsManager.Categories.Any())
            {
                category = samsungQuestionsManager.Categories[0];
                
                Console.WriteLine(category.Name);
                Console.WriteLine(category.Questions);
                Console.WriteLine(category.Products);

                category.SelectAnswer(1,1,true); 
                Console.WriteLine(
                    $"Product score add 4: {category.Products[0].ProductScore.Score}");

                category.SelectAnswer(1,2,true);
                Console.WriteLine(
                    $"Product score add 4: {category.Products[0].ProductScore.Score}");

                category.SelectAnswer(1,3,true);
                Console.WriteLine(
                    $"Product score add 4: {category.Products[0].ProductScore.Score}");

                category.SelectAnswer(1,1,false);
                Console.WriteLine(
                    $"Product score subtract 3: {category.Products[0].ProductScore.Score}");

                category.GetTopItems(3);
                foreach (Product item in category.ProductResults)
                {
                    Console.WriteLine($"Top scores: {item.Name} : {item.ProductScore.Score} ");
                }

                category.Products[1].ProductScore.Reset();
                Console.WriteLine($"Product score reset: {category.Products[1].ProductScore.Score}");
                
                category.ResetScores();
                Console.WriteLine($"Product score reset: {category.Products[0].ProductScore.Score}");
            }
            else
            {
                Console.WriteLine($"Failed to read file");
            }
        }
    }
}