using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace SamsungAPI2
{
    public class SpreadsheetManager
    {
        public SpreadsheetManager()
        {
            CategoryQaMatrix = new CategoryQAMatrix();
        }

        public CategoryQAMatrix CategoryQaMatrix { get; set; }

        private Question CurrentQuestion { get; set; } = null;

        private int CategoryId { get; set; } = 1;

        List<string> Headers = new List<string>();

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

        public bool ReadSpreadSheet(string fname, bool firstRowIsHeader)
        {
            string currentLocation = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string codeBaseLocation = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;

            if (!File.Exists(fname)) return false;

            DataTable dt = new DataTable();

            using (SpreadsheetDocument doc = SpreadsheetDocument.Open(fname, false))
            {
                Sheet sheet = doc.WorkbookPart.Workbook.Sheets.GetFirstChild<Sheet>();
                Worksheet worksheet = ((doc.WorkbookPart.GetPartById(sheet.Id.Value)) as WorksheetPart).Worksheet;
                IEnumerable<Row> rows = worksheet.GetFirstChild<SheetData>().Descendants<Row>();

                CategoryQaMatrix.Category.Name = sheet.Name;
                CategoryQaMatrix.Category.Id = CategoryId;

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
                            Headers.Add(colunmName);
                            dt.Columns.Add(colunmName);

                            if (j > 7) // Product columns, so create a new product for each
                            {
                                CategoryQaMatrix.Products.Add(new Product()
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
                        dt.Rows.Add();
                        int i = 0;
                        int questionId = 0;
                        string questionText = string.Empty;
                        string questionType = string.Empty;
                        int questionOrder = 0;

                        int answerId = 0;
                        string answerText = string.Empty;
                        int answerOrder = 0;

                        int iprod = 0;
                        foreach (Cell cell in row.Descendants<Cell>())
                        {
                            string cellValue = GetCellValue(doc, cell);
                            dt.Rows[dt.Rows.Count - 1][i] = cellValue;
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
                                    QuestionAppendDetails(questionId, questionText, questionOrder, questionType);

                                    break;

                                case 4: //AnswerTest
                                    answerText = cellValue;
                                    break;

                                case 5: //AnswerId
                                    answerId = int.Parse(cellValue);
                                    break;

                                case 6: //Answer Order
                                    answerOrder = int.Parse(cellValue);
                                    AnswersAppendDetails( answerId, answerText, answerOrder);
                                    break;

                                default: //product coloumns 6 and greater
                                    AnswersAppendWeighting(answerId, iprod, int.Parse(cellValue));
                                    iprod++;
                                    break;
                            }

                            i++;
                        }
                    }
                }
            }

            return true;
        }

        private void QuestionAppendDetails(int questionId, string questionText, int questionOrder, string questionType)
        {
            if (!CategoryQaMatrix.Questions.Exists(x => x.Id == questionId))
            {
                CategoryQaMatrix.Questions.Add(new Question()
                {
                    Id = questionId,
                    Text = questionText,
                    Order = questionOrder,
                    QuestionDisplayType =
                        (QuestionDisplayType)Enum.Parse(typeof(QuestionDisplayType), questionType, true)
                });
                
                CurrentQuestion = CategoryQaMatrix.Questions.FirstOrDefault(x => x.Id == questionId);
            }
            
        }

        private void AnswersAppendDetails( int answerId, string answerText, int answerOrder)
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