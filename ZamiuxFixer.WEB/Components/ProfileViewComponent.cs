using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ZamiuxFixer.Application.ViewModels.Account;
using ZamiuxFixer.DataLayer.Context;

namespace ZamiuxFixer.WEB.Components
{
    public class ProfileViewComponent:ViewComponent
    {
        public ZamiuxFixerDbContext _context { get; }
        #region Ctor
        public ProfileViewComponent(ZamiuxFixerDbContext context)
        {
            _context = context;
        }

        
        #endregion
        public async Task<IViewComponentResult> InvokeAsync()
        {
            int UserId = int.Parse(HttpContext.User.FindFirstValue("UserID"));

            var user_current = _context.Users.Where(u=>u.UserId == UserId).Select(u => new ProfileVM
            {
                Country = u.Country,
                JobTitle = u.JobTitle,
                ProfileVisit = u.ProfileVisit
            }).First();
            

            return View("Profile", user_current);
        }
    }
}
