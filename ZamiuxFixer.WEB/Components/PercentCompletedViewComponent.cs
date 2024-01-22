using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ZamiuxFixer.Application.ViewModels.Account;
using ZamiuxFixer.DataLayer.Context;

namespace ZamiuxFixer.WEB.Components
{
    public class PercentCompletedViewComponent : ViewComponent
    {
        public ZamiuxFixerDbContext _context { get; }
        #region Ctor
        public PercentCompletedViewComponent(ZamiuxFixerDbContext context)
        {
            _context = context;
        }

        
        #endregion
        public async Task<IViewComponentResult> InvokeAsync()
        {
            int percent = 25;
            int UserId = int.Parse(HttpContext.User.FindFirstValue("UserID"));

            var user_current = _context.Users.Find(UserId);
            
            if (!string.IsNullOrEmpty(user_current.Avatar))
            {
                percent += 25;
            }

            if (!string.IsNullOrEmpty(user_current.JobTitle))
            {
                percent += 10;
            }

            if (!string.IsNullOrEmpty(user_current.Country))
            {
                percent += 20;
            }

            return View("PercentCompleted", percent);
        }
    }
}
