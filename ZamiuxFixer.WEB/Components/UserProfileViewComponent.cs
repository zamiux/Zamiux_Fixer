using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ZamiuxFixer.Application.ViewModels.Account;
using ZamiuxFixer.DataLayer.Context;

namespace ZamiuxFixer.WEB.Components
{
    public class UserProfileViewComponent : ViewComponent
    {
        public ZamiuxFixerDbContext _context { get; }
        #region Ctor
        public UserProfileViewComponent(ZamiuxFixerDbContext context)
        {
            _context = context;
        }


        #endregion
        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (User.Identity.IsAuthenticated)
            {
                int UserId = int.Parse(HttpContext.User.FindFirstValue("UserID"));

                var user_current = _context.Users.Find(UserId);
                return View("UserProfile", user_current);
            }

            return View("UserProfile");
        }
    }
}
