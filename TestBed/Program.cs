using System;
using System.Collections.Generic;
using SamsungAPI2;

namespace TestBed
{
    
    class Program
    {
        
        private static string fname = "../../../../SamsungAPI2/Assets/Spreadsheets/SamsungQuestions.xlsx";
        
        static void Main(string[] args)
        {
            
            Console.WriteLine("Hello World!");
            SpreadsheetManager spreadsheetManager = new SpreadsheetManager();
            
            bool readOk = spreadsheetManager.ReadSpreadSheet(fname, true);

            if (readOk)
            {
                Console.WriteLine(spreadsheetManager.CategoryQaMatrix.Category);
                Console.WriteLine(spreadsheetManager.CategoryQaMatrix.Questions);
                Console.WriteLine(spreadsheetManager.CategoryQaMatrix.Products);

                spreadsheetManager.CategoryQaMatrix.Products[0].ProductScore.Add(4);
                Console.WriteLine(
                    $"Product score add 4: {spreadsheetManager.CategoryQaMatrix.Products[0].ProductScore.Score}");

                spreadsheetManager.CategoryQaMatrix.Products[1].ProductScore.Add(3);
                Console.WriteLine(
                    $"Product score add 4: {spreadsheetManager.CategoryQaMatrix.Products[0].ProductScore.Score}");

                spreadsheetManager.CategoryQaMatrix.Products[2].ProductScore.Add(5);
                Console.WriteLine(
                    $"Product score add 4: {spreadsheetManager.CategoryQaMatrix.Products[0].ProductScore.Score}");

                spreadsheetManager.CategoryQaMatrix.Products[0].ProductScore.Subtract(3);
                Console.WriteLine(
                    $"Product score subtract 3: {spreadsheetManager.CategoryQaMatrix.Products[0].ProductScore.Score}");

                List<Product> toplist = spreadsheetManager.CategoryQaMatrix.GetTopItems(3);
                foreach (Product item in toplist)
                {
                    Console.WriteLine($"Top scores: {item.Name} : {item.ProductScore.Score} ");
                }

                spreadsheetManager.CategoryQaMatrix.Products[1].ProductScore.Reset();
                Console.WriteLine($"Product score reset: {spreadsheetManager.CategoryQaMatrix.Products[1].ProductScore.Score}");
                
                spreadsheetManager.CategoryQaMatrix.ResetScores();
                Console.WriteLine($"Product score reset: {spreadsheetManager.CategoryQaMatrix.Products[0].ProductScore.Score}");
            }
            else
            {
                Console.WriteLine($"Failed to read file {fname}");
            }
        }
    }
}