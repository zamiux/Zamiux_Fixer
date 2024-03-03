using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

            //List<ShowQuestionViewModel> res =
            //    context.Questions.Select(q => new ShowQuestionViewModel
            //    {
            //        AnswerCount = context.Answers.Count(a => a.QuestionId == q.QuestionId),
            //        FavCount = context.QuestionFavorites.Count(a => a.QuestionId == q.QuestionId),
            //        LastUserAnswer = context.Answers.OrderByDescending(a => a.CreateDate).Include(a => a.User).Where(a => a.QuestionId == q.QuestionId)
            //        .Select(a => a.User).FirstOrDefault(),
            //        Question = q,
            //        Tags = context.QuestionTags.Where(t => t.QuestionId == q.QuestionId).
            //        Select(s => s.Tag).ToList(),
            //        UserName = context.Users.First(u => u.UserId == q.UserId).UserName,
            //        VoteCount = context.QuestionVotes.Count(a => a.QuestionId == q.QuestionId),
            //    }).Take(10).ToList();

            var question_list = _Context.Questions
                .Include(q=>q.User)
                .Include(q=>q.QuestionTags)
                .Take(10).ToList();
            return View(question_list);
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

        #region SMS
        private bool SendSms(string mobile, string text)
        {
            try
            {
                Kavenegar.KavenegarApi api = new Kavenegar.KavenegarApi("7A35614C6E5176697577574D36615832476F4D5734386E7A3136736D71554F796166655A6B6A504A484F343D");
                var result = api.Send("10008663", mobile, text);
                return true;
            }
            catch (Kavenegar.Exceptions.ApiException ex)
            {
                // در صورتی که خروجی وب سرویس 200 نباشد این خطارخ می دهد.
                _logger.LogError("اس ام اس ارسال نشد");
                return false; 
            }
            catch (Kavenegar.Exceptions.HttpException ex)
            {
                _logger.LogError("اس ام اس ارسال نشد");
                // در زمانی که مشکلی در برقرای ارتباط با وب سرویس وجود داشته باشد این خطا رخ می دهد
                return false;
            }

        }
        #endregion
    }
}