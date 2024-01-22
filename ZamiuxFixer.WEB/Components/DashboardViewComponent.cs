
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ZamiuxFixer.Application.ViewModels.Account;
using ZamiuxFixer.DataLayer.Context;

namespace ZamiuxFixer.WEB.Components
{
    public class DashboardViewComponent : ViewComponent
    {
        public ZamiuxFixerDbContext _context { get; }
        #region Ctor
        public DashboardViewComponent(ZamiuxFixerDbContext context)
        {
            _context = context;
        }

        
        #endregion
        public async Task<IViewComponentResult> InvokeAsync()
        {
            int UserId = int.Parse(HttpContext.User.FindFirstValue("UserID"));
            var user_current = _context.Users.Find(UserId);

            return View("Dashboard", user_current);
        }
    }
}
