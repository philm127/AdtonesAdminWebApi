//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace AdtonesAdminWebApi.ViewModels.AdProfilrModels
//{
//    /// <summary>
//    /// Class ArtharFormModel.
//    /// </summary>
//    public abstract class ArtharFormModel
//    {
//        /// <summary>
//        /// The _answer values
//        /// </summary>
//        private readonly string[] _answerValues = new[]
//                                                      {
//                                                          "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M",
//                                                          "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"
//                                                      };

//        /// <summary>
//        /// Sorts the list.
//        /// </summary>
//        /// <param name="questionOptions">The question options.</param>
//        /// <returns>IEnumerable&lt;QuestionOptionModel&gt;.</returns>
//        internal IEnumerable<QuestionOptionModel> SortList(List<QuestionOptionModel> questionOptions)
//        {
//            questionOptions.Sort((x, y) => String.Compare(x.QuestionValue, y.QuestionValue, StringComparison.Ordinal));
//            return questionOptions;
//        }

//        /// <summary>
//        /// Compiles the answers.
//        /// </summary>
//        /// <param name="questionOptions">The question options.</param>
//        /// <returns>System.String.</returns>
//        internal string CompileAnswers(IEnumerable<QuestionOptionModel> questionOptions)
//        {
//            IEnumerable<QuestionOptionModel> questionOptionModels = questionOptions as QuestionOptionModel[] ??
//                                                                    questionOptions.ToArray();

//            string answers = questionOptionModels.Where(q => q.Selected).Aggregate(
//                string.Empty,
//                (current, q) => current + q.QuestionValue
//                );

//            if (string.IsNullOrEmpty(answers))
//            {
//                foreach (
//                    QuestionOptionModel questionOptionModel in
//                        questionOptionModels.Where(questionOptionModel => questionOptionModel.DefaultAnswer))
//                    answers = questionOptionModel.QuestionValue;
//            }

//            return answers;
//        }

//        /// <summary>
//        /// Compiles the questions.
//        /// </summary>
//        /// <param name="answers">The answers. A Dictionary contains a string for each answer 
//        /// and a bool for whether or not that answer should be the default if no answer is selected</param>
//        /// <returns>List&lt;QuestionOptionModel&gt;.</returns>
//        internal List<QuestionOptionModel> CompileQuestions(Dictionary<string, bool> answers)
//        {
//            var questions = new List<QuestionOptionModel>();
//            int counter = 0;

//            foreach (var answer in answers)
//            {
//                questions.Add(new QuestionOptionModel
//                {
//                    DefaultAnswer = answer.Value,
//                    QuestionName = answer.Key,
//                    QuestionValue = _answerValues[counter],
//                    Selected = false
//                });

//                counter++;
//            }

//            return questions;
//        }

//        internal List<QuestionOptionModel> CompileQuestionsDynamic(List<Dictionary<string, bool>> answers)
//        {
//            var questions = new List<QuestionOptionModel>();
//            int counter = 0;

//            foreach (var answer in answers)
//            {
//                foreach (var item in answer)
//                {
//                    questions.Add(new QuestionOptionModel
//                    {
//                        DefaultAnswer = item.Value,
//                        QuestionName = item.Key,
//                        QuestionValue = _answerValues[counter],
//                        Selected = false
//                    });
//                }

//                counter++;
//            }

//            return questions;
//        }
//    }
//}