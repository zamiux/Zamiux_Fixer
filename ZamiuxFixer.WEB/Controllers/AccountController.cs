using Microsoft.AspNetCore.Mvc;
using ZamiuxFixer.Application.ViewModels.Account;
using ZamiuxFixer.DataLayer.Contract;
using ZamiuxFixer.Domain.Users;
using ZamiuxFixer.Application.Security;
using Microsoft.Extensions.Configuration;
using ZamiuxFixer.Application.SendEmail;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using ZamiuxFixer.DataLayer.Context;

namespace ZamiuxFixer.WEB.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IMailSender _mail;
        private readonly ZamiuxFixerDbContext _context;
        private readonly ILogger<AccountController> _logger;

        #region Constructor
        public AccountController(IUserRepository userRepository,
            IConfiguration configuration,
            IMailSender mail,
            ZamiuxFixerDbContext context,
            ILogger<AccountController> logger)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _mail = mail;
            _context = context;
            _logger = logger;
        }
        #endregion

        #region Register

        [HttpGet]
        [Route("/Register")]
        public IActionResult Register()
        {
            
            return View();
        }

        [HttpPost]
        [Route("/Register")]
        public IActionResult Register(RegisterVM register)
        {
            if (!ModelState.IsValid)
            {
                return View(register);
            }

            #region Check User
            if (_userRepository.isExistUserByEmail(register.Email.Trim().ToLower()))
            {
                ModelState.AddModelError("Email","ایمیل وارد شده معتبر نمی باشد");
                return View(register);
            }
            #endregion

            #region Save User

            User user_data = new User() {
                UserName = register.UserName,
                Email = register.Email.Trim().ToLower(),
                CreateDate = DateTime.Now,
                IsDelete = false,
                Password = SecretHasher.Hash(register.Password),
                ActiveCode = Guid.NewGuid().ToString(),
                RoleId = 1
            };

            _userRepository.Add(user_data);
            _userRepository.Save();

            #endregion

            // Start TODO : Send Activation Email
            string BodyEmail = "<div style='direction:rtl'>" +
                $"<h1>{user_data.UserName} عزیز !</h1><br/>" +
                $"<p>جهت فعالسازی حساب خود روی لینک زیر کلیک کنید</p>" +
                $"<p><a href='{_configuration.GetSection("MyDomain").Value}/Account/ActiveUser/{user_data.ActiveCode}'>لینک فعالسازی</a></p>" +
                "</div>";

            ViewBag.isSendEmail = _mail.Send(user_data.Email,"فعالسازی حساب کاربری",BodyEmail);   

            // End TODO : Send Activation Email

            ViewBag.SuccessRegister = true;
            //return View("SuccessRegister",user_data);
            SendSms(user_data.Mobile, "Thnak you for Register");
            return View(register);
        }
        #endregion

        #region Active User

        //Active User : domain.com/Account/ActiveUser/54645645t45454
        public IActionResult ActiveUser(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            var user_info = _userRepository.GetUserByActiveCode(id);

            if (user_info == null)
            {
                return BadRequest();
            }

            //update & active User
            user_info.IsEmailActive = true;
            user_info.ActiveCode = Guid.NewGuid().ToString();
            user_info.ModifiedDate = DateTime.Now;

            _userRepository.Update(user_info);
            _userRepository.Save();

            return Redirect("/Login?ActiveUser=true");
        }
        #endregion

        #region Login

        [HttpGet]
        [Route("Login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login(LoginVM login)
        {
            if (!ModelState.IsValid)
            {
                return View(login);
            }

            //get User and check user
            var user_info = _userRepository.GetUserByEmail(login.Email.ToLower().ToString());

            if (user_info == null)
            {
                ModelState.AddModelError("Email", "اطلاعات کاربری درست نمی باشد");
                return View(login);
            }

            // check user active
            if (user_info.IsEmailActive == false)
            {
                ModelState.AddModelError("Email", "کاربر شما غیرفعال است");
                return View(login);
            }

            //check user password
            bool CheckPasswordUser = SecretHasher.Verify(login.Password,user_info.Password);
            if (CheckPasswordUser == false)
            {
                ModelState.AddModelError("Email", "اطلاعات کاربری درست نمی باشد");
                return View(login);
            }

            // start Todo Login

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,user_info.UserName),
                new Claim(ClaimTypes.Email,user_info.Email),
                new Claim("UserID",user_info.UserId.ToString()),
                new Claim("Avatar",user_info.Avatar.ToString())
            };

            var identity = new ClaimsIdentity(claims,CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            //Remember
            var properties = new AuthenticationProperties() { 
                // yani login bemanad.
                IsPersistent = login.RememberMe
            };

            HttpContext.SignInAsync(principal, properties);

            // end Todo Login

            return Redirect("/");
        }

        #endregion

        #region Logout
        [Route("Logout")]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/");
        }
        #endregion

        #region Followers
        [Authorize]
        public IActionResult Follow(int id) // id : yani in id = userid mikhad follow kone
        {
            // aval info oon user ke mikhad follow kone ro peida kon
            int UserId = int.Parse(User.FindFirstValue("UserId"));

            var follow = _context.UserFollowings
                .FirstOrDefault(f => f.Follower == UserId && f.Following == id);
             
            if (follow == null)
            {
                // Add follow

                _context.UserFollowings.Add(new UserFollowing()
                {
                    Follower = UserId,
                    Following = id
                });
            }
            else
            {
                // unfollow

                _context.UserFollowings.Remove(follow);
            }

            _context.SaveChanges();

            return Redirect($"/Profile/{id}");
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
