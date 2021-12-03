using System;
using System.Collections.Generic;
using System.Linq;
using UserProfile.Models;

namespace UserProfile.Helpers
{
    public class CompileSortQuestions
    {
        /// <summary>
        /// Sorts the list.
        /// </summary>
        /// <param name="questionOptions">The question options.</param>
        /// <returns>IEnumerable&lt;QuestionOptionModel&gt;.</returns>
        public IEnumerable<QuestionOptionModel> SortList(List<QuestionOptionModel> questionOptions)
        {
            questionOptions.Sort((x, y) => String.Compare(x.QuestionValue, y.QuestionValue, StringComparison.Ordinal));
            return questionOptions;
        }

        /// <summary>
        /// Compiles the answers.
        /// </summary>
        /// <param name="questionOptions">The question options.</param>
        /// <returns>System.String.</returns>
        public string CompileAnswers(IEnumerable<QuestionOptionModel> questionOptions)
        {
            IEnumerable<QuestionOptionModel> questionOptionModels = questionOptions as QuestionOptionModel[] ??
                                                                    questionOptions.ToArray();

            string answers = questionOptionModels.Where(q => q.Selected).Aggregate(
                string.Empty,
                (current, q) => current + q.QuestionValue
                );

            if (string.IsNullOrEmpty(answers))
            {
                foreach (
                    QuestionOptionModel questionOptionModel in
                        questionOptionModels.Where(questionOptionModel => questionOptionModel.DefaultAnswer))
                    answers = questionOptionModel.QuestionValue;
            }

            return answers;
        }
    }
}
