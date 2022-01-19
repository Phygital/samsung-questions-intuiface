using System;
using System.Collections.Generic;
using DocumentFormat.OpenXml.Vml.Spreadsheet;
using SamsungAPI2;

namespace TestBed
{

    class Program
    {

        private static string fname = "../../../../SamsungAPI2/Assets/Spreadsheets/SamsungQuestions.xlsx";

        static void Main(string[] args)
        {
            var manager = new SamsungQuestionsManager();
            Console.WriteLine(manager.Categories.Count);
        }

    }
}
