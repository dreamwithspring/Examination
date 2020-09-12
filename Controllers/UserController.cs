using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using System.Web;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using word_company.Services;
using word_company.Models;
using IdentityServer4.Models;
using WebApplication1.Controllers;
using IdentityServer4.Extensions;
using System.Net;
using System.Security.Permissions;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace word_company.Controllers
{
    /// <summary>
    /// 用户操作
    /// </summary>
    public class UserController : Controller
    {
        private readonly IUserService _UserService;
        private readonly IHttpContextAccessor _HttpContext;
        /// <summary>
        /// 依赖注入
        /// </summary>
        /// <param name="userService">用户数据库操作</param>
        /// <param name="httpContext"></param>
        public UserController(IUserService userService, IHttpContextAccessor httpContext)
        {
            this._UserService = userService;
            this._HttpContext = httpContext;

        }
        /// <summary>
        /// 桌面，判断是否登录，如未登录，则跳转到login，如登录，根据权限跳转到整体的index
        /// </summary>
        /// <returns>返回页</returns>
        public IActionResult Index()
        {
            //var user = _HttpContext.HttpContext.Session.GetString("user");
            //var UID = _HttpContext.HttpContext.Session.GetString("UID");
            string user;
            string Uid;

            //取cookie，其它也可以取token是一样的
            Request.Cookies.TryGetValue("User", out user);
            Request.Cookies.TryGetValue("Uid", out Uid);
            if (user == null || Uid == null)
            {
                return RedirectToAction("Login");
            }
            //判断cookie中的数据是否符合
            var u = _UserService.InspectGuid(new TGuid { User = user, Uid = Uid, });
            if (u == null)
            {
                return RedirectToAction("Login");
            }
            else
            {
                //HttpContext.Session.SetString("UID", u.Uid);
                Response.Cookies.Append("Uid", u.Uid, new CookieOptions
                {
                    Expires = DateTime.Now.AddMinutes(30)
                });
                Response.Cookies.Append("User", u.User, new CookieOptions
                {
                    Expires = DateTime.Now.AddMinutes(30)
                }) ;
                switch (u.Role)
                {
                    case Role.admin:
                        return RedirectToAction("index", "home");
                    case Role.user:
                        return RedirectToAction("index", "home");
                    case Role.SuperAdmin:
                        return RedirectToAction("index", "home");
                }
                return RedirectToAction("Login");
            }
        }



        /// <summary>
        /// 登出
        /// </summary>
        /// <returns>login页面</returns>
        public IActionResult Logout()
        {
            Response.Cookies.Delete("User");
            Response.Cookies.Delete("Uid");
            return RedirectToAction("Login");
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Login()
        {

            return View();
        }

        /// <summary>
        /// 登录返回
        /// </summary>
        /// <param name="loginModel"></param>
        /// <returns>如登录成功跳转到home的index</returns>
        [HttpPost]
        public IActionResult Login(LoginModel loginModel)
        {
            
            var user = new UserModel();
            var lvm = new LoginViewModel()
            {
                User = loginModel.User,
                PassWord = loginModel.PassWord,
            };
            try
            {
                user = _UserService.Login(loginModel).Result;
            }
            catch (Exception ex)
            {

                lvm.msg = ex.Message;
                return View(lvm);
            }
            //添加cookie
            Response.Cookies.Append("user", user.User, new CookieOptions
            {
                Expires = DateTime.Now.AddMinutes(30),
                IsEssential = true,
            });
            Response.Cookies.Append("UID", user.Uid, new CookieOptions
            {
                Expires = DateTime.Now.AddMinutes(30),
                IsEssential = true,


            });
            //HttpContext.Session.SetString("user", user.UserName);
            //HttpContext.Session.SetString("UID", user.Uid);
            return RedirectToAction("Index", "Home", user);
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult SignUp()
        {

            return View();
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="signUpModel">登录模型</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult SignUp(SignUpModel signUpModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "注册错误");
                return View(signUpModel);
            }
            var user = _UserService.SignUp(signUpModel).Result;
            Response.Cookies.Append("Uid", user.Uid, new CookieOptions
            {
                Expires = DateTime.Now.AddMinutes(30)
            });
            Response.Cookies.Append("User", user.UserName, new CookieOptions
            {
                Expires = DateTime.Now.AddMinutes(30)
            });
            //直接登录
            return RedirectToAction(nameof(Index));

        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id">用户id</param>
        /// <returns></returns>
        public IActionResult DeleteUser(long id)
        {
            string user;
            string Uid;
            Request.Cookies.TryGetValue("User", out user);
            Request.Cookies.TryGetValue("Uid", out Uid);
            //检查用户是否有对应的权限
            var u = _UserService.InspectGuid(new TGuid { User = user, Uid = Uid, });
            if (u == null || u.Role != Role.SuperAdmin )
            {
                return View("Error");
            }
            _UserService.DeleteUser(_UserService.GetUserById(id));
            return RedirectToAction(nameof(Index), "Home");
        }


        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns>返回修改模型</returns>
        [HttpGet]
        public IActionResult EditUser(long id)
        {
            var u = _UserService.GetUserById(id);
            return View(u);
        }

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="userModel">用户模型</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult EditUser(UserModel userModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "修改错误");
                return View(userModel);
            }
            var u = _UserService.GetUserById(userModel.Id);
            if(u == null)
            {
                return RedirectToAction(nameof(Index));
            }
            _UserService.EditUser(userModel);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult AddUser( )
        {

            return View();
        }


        [HttpPost]
        public IActionResult  AddUser(List<AddUserModel>  addUserModels)
        {
            
            
            foreach(var u in addUserModels)
            {
                if (u.User == null || u.UserName == null )
                {
                    return View();
                }
                _UserService.AddUser(u);
            }


            return RedirectToAction(nameof(Index),"Home");
        }




        public IActionResult ReSetPassWord(long id)
        {
            _UserService.ReSetPassWord(id);
            return RedirectToAction(nameof(Index), "Home");
            
        }
    }
}

