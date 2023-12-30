using Microsoft.AspNetCore.Mvc;
using ZamiuxFixer.Application.ViewModels.Account;
using ZamiuxFixer.DataLayer.Contract;
using ZamiuxFixer.Domain.Users;

namespace ZamiuxFixer.WEB.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserRepository _userRepository;
        #region Constructor
        public AccountController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
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
                Password = register.Password,
                RoleId = 1
            };

            _userRepository.Add(user_data);
            _userRepository.Save();

            #endregion


            return View("SuccessRegister",user_data);
        }
        #endregion
    }
}
