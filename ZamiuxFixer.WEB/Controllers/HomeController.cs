using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using ZamiuxFixer.DataLayer.Context;
using ZamiuxFixer.WEB.Models;

namespace ZamiuxFixer.WEB.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ZamiuxFixerDbContext _Context;


        public HomeController(ILogger<HomeController> logger, ZamiuxFixerDbContext Context)
        {
            _logger = logger;
            _Context = Context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #region Show Profiel
        [Route("Profile/{id}")]
        public IActionResult ShowProfile(int id)
        {
            var user = _Context.Users.Find(id);

            if (user == null)
                return NotFound();

            user.ProfileVisit += 1;
            _Context.SaveChanges();

            if (User.Identity.IsAuthenticated)
            {
                int userId = int.Parse(User.FindFirstValue("UserId"));

                var follow = _Context.UserFollowings
                    .Any(F => F.Follower == userId && F.Following == id);

                ViewBag.isFollow = follow;


            }
            return View(user);
        }
        #endregion

        #region List Users
        [Route("users")]
        public IActionResult ShowUsers(int pageId = 1) // system paging
        {
            int take = 1; //too har page changta mikhaei neshoon bediii. 
            int skip = (pageId - 1) * take; // mizan tedad skip ya paresh items

            //namayesh natayej
            var result = _Context.Users
                .OrderByDescending(u=>u.CreateDate)
                .Skip(skip)
                .Take(take)
                .ToList();
                 
            int USCount = _Context.Users.Count();
            ViewBag.UserCount = USCount;

            ViewBag.PageId = pageId;
            ViewBag.Take = take;

            return View(result);
        }
        #endregion
    }
}