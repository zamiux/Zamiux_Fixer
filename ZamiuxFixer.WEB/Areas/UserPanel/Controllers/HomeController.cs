using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System.Security.Claims;
using ZamiuxFixer.Application.Security;
using ZamiuxFixer.Application.ViewModels.Account;
using ZamiuxFixer.DataLayer.Context;
using ZamiuxFixer.Domain.Users;

namespace ZamiuxFixer.WEB.Areas.UserPanel.Controllers
{
    [Area("UserPanel")]
    [Authorize] // chekc user logined
    public class HomeController : Controller
    {
        public ZamiuxFixerDbContext _context { get; }

        #region Ctor

        public HomeController(ZamiuxFixerDbContext context)
        {
            _context = context;
        }

        #endregion
        public IActionResult Index()
        {
            int UserId = int.Parse(User.FindFirstValue("UserID"));

            var myQuestion = _context.Questions
                .Include(q=>q.QuestionFavorites)
                .Where(x => x.UserId == UserId)
                .ToList();

            return View(myQuestion);
        }

        #region Edit User Profile

        [HttpGet]
        public IActionResult EditProfile()
        {
            int UserId = int.Parse(User.FindFirstValue("UserID"));
            var user = _context.Users.Find(UserId);

            return View(user);
        }

        [HttpPost]
        public IActionResult EditProfile(User user_vm, IFormFile imgUp)
        {
            int UserId = int.Parse(User.FindFirstValue("UserID"));
            var current_user = _context.Users.Find(UserId);

            current_user.UserName = user_vm.UserName;
            current_user.Email = user_vm.Email.ToString().Trim();
            current_user.Country = user_vm.Country;
            current_user.Description = user_vm.Description;
            current_user.JobTitle = user_vm.JobTitle;
            current_user.Mobile = user_vm.Mobile;

            #region File Upload
            if (imgUp != null)
            {
                string AvatarPath = Path.Combine(Directory.GetCurrentDirectory()
                    ,"wwwroot/images/Avatars");

                string AvatarName = Guid.NewGuid().ToString() + Path.GetExtension(imgUp.FileName);
                string SavePath = Path.Combine(AvatarPath, AvatarName);
                // check user profile dashtes ya na
                if (current_user.Avatar != null) // dashte
                {
                    if (System.IO.File.Exists(Path.Combine(AvatarPath, current_user.Avatar)))
                    {
                        // delete old pic
                        System.IO.File.Delete(Path.Combine(AvatarPath, current_user.Avatar));

                        // upload pic
                        using (var stream = System.IO.File.Create(SavePath))
                        {
                            imgUp.CopyTo(stream);
                        }
                    }
                }

                // upload pic
                using (var stream = System.IO.File.Create(SavePath))
                {
                    imgUp.CopyTo(stream);
                }

                current_user.Avatar = AvatarName;

            }
            #endregion

            _context.Users.Update(current_user);
            _context.SaveChanges();

            ViewBag.isEdit = true;
            return View(user_vm);
        }

        #endregion

        #region ChangePassword

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ChangePassword(ChangePasswordVM changePassword)
        {
            int UserId = int.Parse(User.FindFirstValue("UserID"));
            var user = _context.Users.Find(UserId);

            if (ModelState.IsValid)
            {
                bool CheckPasswordUser = SecretHasher.Verify(changePassword.OldPassword, user.Password);
                if (CheckPasswordUser == false)
                {
                    ModelState.AddModelError("OldPassword", "رمز عبور فعلی شما اشتباه است");
                    return View(changePassword);
                }
                else
                {
                    string HashNewPassword = SecretHasher.Hash(changePassword.NewPassword);
                    user.Password = HashNewPassword;
                    user.ModifiedDate = DateTime.Now;

                    _context.Users.Update(user);
                    _context.SaveChanges();

                    ViewBag.isChangePassword = true;
                    return View(changePassword);
                }
            }
            return View(changePassword);
        }

        #endregion

        #region Following
        public IActionResult Following()
        {
            // chekc User
            int UserId = int.Parse(User.FindFirstValue("UserID"));
            var following = _context.UserFollowings
                .Where(f=>f.Follower == UserId)
                .Include(u=>u.User2)
                .Select(s=>s.User2)
                .ToList();

            return View(following);
        }
        #endregion

        #region Followers
        public IActionResult Followers()
        {
            // chekc User
            int UserId = int.Parse(User.FindFirstValue("UserID"));
            var followers = _context.UserFollowings
                .Where(f => f.Following == UserId)
                .Include(u => u.User1)
                .Select(s => s.User1)
                .ToList();

            return View(followers);
        }
        #endregion
    }
}
