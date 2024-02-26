using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ZamiuxFixer.Application.ViewModels.Account;
using ZamiuxFixer.Application.ViewModels.Question;
using ZamiuxFixer.DataLayer.Context;

namespace ZamiuxFixer.WEB.Components
{
    public class SideBarQuestionViewComponent : ViewComponent
    {
        private readonly ZamiuxFixerDbContext context;

        public SideBarQuestionViewComponent(ZamiuxFixerDbContext context)
        {
            this.context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            int userId = int.Parse(HttpContext.User.FindFirstValue("UserID"));
            QuestionsVM QuestionVM = new QuestionsVM()
            {
                TypeAnswerQuestions = context.Questions.OrderByDescending(q => q.Answers.Count()).Take(4).ToList(),
                TypeRateQuestions = context.Questions.OrderByDescending(q => q.QuestionVotes.Count()).Take(4).ToList()
            };
            return View("SideBarQuestion", QuestionVM);
        }
    }
}
