using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ZamiuxFixer.Application.ViewModels.Account;
using ZamiuxFixer.DataLayer.Context;

namespace ZamiuxFixer.WEB.Components
{
    public class SideBarViewComponent : ViewComponent
    {
        private readonly ZamiuxFixerDbContext context;

        public SideBarViewComponent(ZamiuxFixerDbContext context)
        {
            this.context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            int userId = int.Parse(HttpContext.User.FindFirstValue("UserID"));
            ShowSummaryProfileVW profileVW = new ShowSummaryProfileVW()
            {
                AnswerCount = 0,
                QuestionCount = 0,
                FollowerCount = context.UserFollowings.Count(f => f.Following == userId),
                FollowingCount = context.UserFollowings.Count(f => f.Follower == userId),
            };
            return View("SideBar", profileVW);
        }
    }
}
