using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using DocumentFormat.OpenXml.Office2013.Drawing.Chart;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace SamsungAPI2
{
    public class SpreadsheetManager
    {
        Category currentCategory;

        public SpreadsheetManager()
        {
        }

        private Question CurrentQuestion { get; set; } = null;

        private int CategoryId { get; set; } = 1;

        private string GetCellValue(SpreadsheetDocument doc, Cell cell)
        {
            if (cell.CellValue == null || cell.CellValue.InnerText == string.Empty) return string.Empty;

            string value = cell.CellValue.InnerText;
            if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
            {
                return doc.WorkbookPart.SharedStringTablePart.SharedStringTable.ChildElements.GetItem(int.Parse(value))
                    .InnerText;
            }

            return value;
        }

        public List<Category> ReadSpreadSheet(string fname, bool firstRowIsHeader)
        {
            if (!File.Exists(fname))
            {
                return new List<Category>()
                {
                    new Category()
                    {
                        Id = 99, Name = $"File Not Found: {fname}"
                    },
                    new Category()
                    {
                        Id = 98,
                        Name = Assembly.GetExecutingAssembly().Location,
                    }
                };
            }

            var categories = new List<Category>();

            using (SpreadsheetDocument doc = SpreadsheetDocument.Open(fname, false))
            {
                //Sheet sheet = doc.WorkbookPart.Workbook.Sheets.GetFirstChild<Sheet>();

                foreach (var openXmlElement in doc.WorkbookPart.Workbook.Sheets)
                {
                    var sheet = (Sheet)openXmlElement;
                    
                    Worksheet worksheet = ((doc.WorkbookPart.GetPartById(sheet.Id.Value)) as WorksheetPart).Worksheet;
                    IEnumerable<Row> rows = worksheet.GetFirstChild<SheetData>().Descendants<Row>();

                    currentCategory = new Category();

                    currentCategory.Name = sheet.Name;
                    currentCategory.Id = CategoryId;

                    foreach (Row row in rows)
                    {
                        //Read the first row as header
                        if (row.RowIndex.Value == 1)
                        {
                            int iProd = 1;
                            var j = 1;
                            foreach (Cell cell in row.Descendants<Cell>())
                            {
                                var colunmName = firstRowIsHeader ? GetCellValue(doc, cell) : "Field" + j;
                                Console.WriteLine(colunmName);

                                if (j > 7) // Product columns, so create a new product for each
                                {
                                    currentCategory.Products.Add(new Product()
                                    {
                                        Id = iProd,
                                        Name = colunmName,
                                        Description = colunmName
                                    });
                                    iProd++;
                                }

                                j++;
                            }
                        }
                        else
                        {
                            int i = 0;
                            int questionId = 0;
                            string questionText = string.Empty;
                            string questionType = string.Empty;
                            int questionOrder = 0;

                            int answerId = 0;
                            string answerText = string.Empty;
                            int answerOrder = 0;

                            int iprod = 1;
                            foreach (Cell cell in row.Descendants<Cell>())
                            {
                                string cellValue = GetCellValue(doc, cell);

                                if (cellValue != string.Empty)
                                {
                                    switch (i)
                                    {
                                        case 0: //QuestionId
                                            questionId = int.Parse(cellValue);
                                            break;
                                        case 1: //Question Text
                                            questionText = cellValue;
                                            break;

                                        case 2: //Question Order
                                            questionOrder = int.Parse(cellValue);
                                            break;

                                        case 3: //Question Type
                                            questionType = cellValue;
                                            QuestionAppendDetails(questionId, questionText, questionOrder,
                                                questionType);

                                            break;

                                        case 4: //AnswerTest
                                            answerText = cellValue;
                                            break;

                                        case 5: //AnswerId
                                            answerId = int.Parse(cellValue);
                                            break;

                                        case 6: //Answer Order
                                            answerOrder = int.Parse(cellValue);
                                            AnswersAppendDetails(answerId, answerText, answerOrder);
                                            break;

                                        default: //product coloumns 6 and greater
                                            AnswersAppendWeighting(answerId, iprod, int.Parse(cellValue));
                                            iprod++;
                                            break;
                                    }
                                }

                                i++;
                            }
                        }
                    }

                    //Add Category to collection
                    categories.Add(currentCategory);
                }

                return categories;
            }
        }

        private void QuestionAppendDetails(int questionId, string questionText, int questionOrder, string questionType)
        {
            if (!currentCategory.Questions.Exists(x => x.Id == questionId))
            {
                currentCategory.Questions.Add(new Question()
                {
                    Id = questionId,
                    Text = questionText,
                    Order = questionOrder,
                    QuestionDisplayType = questionType
                    //(QuestionDisplayType)Enum.Parse(typeof(QuestionDisplayType), questionType, true)
                });

                CurrentQuestion = currentCategory.Questions.FirstOrDefault(x => x.Id == questionId);
            }
        }

        private void AnswersAppendDetails(int answerId, string answerText, int answerOrder)
        {
            CurrentQuestion.Answers.Add(new Answer()
            {
                Id = answerId,
                Text = answerText,
                Order = answerOrder
            });
        }

        private void AnswersAppendWeighting(int answerId, int productId, int weighting)
        {
            Answer answer = CurrentQuestion.Answers.FirstOrDefault(x => x.Id == answerId);

            if (answer != null)
            {
                answer.AnswerWeighting.Add(new AnswerWeighting()
                {
                    ProductId = productId,
                    Weight = weighting
                });
            }
        }
    }
}