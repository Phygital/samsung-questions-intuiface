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
        private Category _currentCategory;
        private Question _currentQuestion;
        private AnswerGrouping _currentAnswerGrouping;


        public SpreadsheetManager()
        {
        }



        private string GetCellValue(SpreadsheetDocument doc, Cell cell)
        {
            if (cell.CellValue == null || cell.CellValue.InnerText == string.Empty) return string.Empty;

            var value = cell.CellValue.InnerText;
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
                return new List<Category>();
            }

            var categories = new List<Category>();
            int categoryId = 0;

            using (SpreadsheetDocument doc = SpreadsheetDocument.Open(fname, false))
            {
                foreach (var openXmlElement in doc.WorkbookPart.Workbook.Sheets)
                {
                    categoryId++;

                    var sheet = (Sheet) openXmlElement;

                    var worksheet = ((doc.WorkbookPart.GetPartById(sheet.Id.Value)) as WorksheetPart).Worksheet;
                    var rows = worksheet.GetFirstChild<SheetData>().Descendants<Row>();

                    _currentCategory = new Category
                                      {
                                          Name = sheet.Name,
                                          Id = categoryId
                                      };


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
                                    _currentCategory.Products.Add(new Product()
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
                            var i = 0;
                            var questionId = 0;
                            var questionText = string.Empty;
                            var questionOrder = 0;
                            var questionType = string.Empty;
                            var answerId = 0;
                            var answerText = string.Empty;
                            var productIndex = 1;
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

                                        case 6: //Answer Groups
                                            AnswersAppendDetails(answerId, answerText, cellValue, questionId, _currentCategory.Id);
                                            break;

                                        default: //Product Columns 6 and greater
                                            AnswersAppendWeighting(answerId, productIndex, int.Parse(cellValue));
                                            productIndex++;
                                            break;
                                    }
                                }

                                i++;
                            }
                        }
                    }

                    //Add Category to collection
                    categories.Add(_currentCategory);
                }

                return categories;
            }
        }

        private void QuestionAppendDetails(int questionId, string questionText, int questionOrder, string questionType)
        {
            if (!_currentCategory.Questions.Exists(x => x.Id == questionId))
            {
                _currentCategory.Questions.Add(new Question
                                              {
                                                  Id = questionId,
                                                  Text = questionText,
                                                  Order = questionOrder,
                                                  QuestionDisplayType = questionType
                                              });

                _currentQuestion = _currentCategory.Questions.FirstOrDefault(x => x.Id == questionId);
            }
        }

        private void AnswersAppendDetails(int answerId, string answerText, string answerGroupId, int questionId, int categoryId)
        {
            var groupingId = $"{categoryId}_{questionId}_{answerGroupId}";
            var answerGrouping = _currentQuestion.AnswerGroupings.SingleOrDefault(x => x.GroupId == groupingId);
            if (answerGrouping == null)
            {
                answerGrouping = new AnswerGrouping
                                 {
                                     GroupId = groupingId
                                 };
                _currentQuestion.AnswerGroupings.Add(answerGrouping);

            }

            _currentAnswerGrouping = answerGrouping;
            answerGrouping.Answers.Add(new Answer
                                       {
                                           Id = answerId,
                                           Text = answerText,
                                           QuestionId = questionId,
                                           CategoryId = categoryId,
                                           GroupId = answerGrouping.GroupId,
                                           UsesToggleGroup = _currentQuestion.QuestionDisplayType.ToLower() == "toggle"
                                       });
        }

        private void AnswersAppendWeighting(int answerId, int productId, int weighting)
        {
            if (_currentAnswerGrouping == null) return;
            Answer answer = _currentAnswerGrouping.Answers.FirstOrDefault(x => x.Id == answerId);

            if (answer != null)
            {
                answer.AnswerWeighting.Add(new AnswerWeighting
                                           {
                                               ProductId = productId,
                                               Weight = weighting
                                           });
            }
        }
    }
}
