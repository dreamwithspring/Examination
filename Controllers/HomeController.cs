using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using WebApplication1.Models;
using word_company.Models;
using word_company.Services;

namespace WebApplication1.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITextService<personnel_word> textService;
        private readonly IUserService UserService;

        public HomeController(ILogger<HomeController> logger,ITextService<personnel_word> textService,IUserService userService)
        {
            _logger = logger;
            this.textService = textService;
            this.UserService = userService;
        }

  
        /// <summary>
        /// 菜单，直接查看所有人的资料
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            string user, uid;
            Request.Cookies.TryGetValue("User", out user);
            Request.Cookies.TryGetValue("Uid", out uid);
            var u = UserService.InspectGuid(new TGuid
            {
                Uid = uid,
                User = user,
            });
            if (u == null)
            {
                return RedirectToAction(nameof(ReLogin));
            }
            if (u.Role == Role.user)
            {
                return RedirectToAction("AllWordFindByUser", new {Id = u.Id});
            }
            var list = UserService.GetAllUser();
            return View(list);
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


        //public IActionResult WordIndex(string name)
        //{
        //    var list = textService.GetAllTextByName(name);
        //    var index = list
        //}
        /// <summary>
        /// 添加文档
        /// </summary>
        /// <param name="userModel">用户</param>
        /// <returns></returns>
        public IActionResult AddText(UserModel userModel)
        {
            string user, uid;
            Request.Cookies.TryGetValue("User",out user);
            Request.Cookies.TryGetValue("Uid", out uid);
            var u =  UserService.InspectGuid(new TGuid
            {
                Uid = uid,
                User = user,
            });
            if(u == null )
            {
                return RedirectToAction(nameof(ReLogin));
            }
            personnel_word personnel = new personnel_word { UserModel = u };
            return View(personnel);
        }



        [HttpPost]
        public  IActionResult AddText(personnel_word word)
        {
            if (!textService.Inspect(word))
            {
                ModelState.AddModelError(string.Empty, "文件重复");
                return View(word);
            }
            
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "上传文档出错");
                return View(word);
            }

            textService.AddText(word);

            return RedirectToAction(nameof(Index));
        }


        public IActionResult EditText(personnel_word word)
        {

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "上传文档出错");
                return View(word);
            }


            textService.EditText(word);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult EditTextById(long id)
        {
            var word = textService.GetById(id);
            if(word == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var vm = new personnel_word
            {
                Id = word.Id,
                TName = word.TName,
                Text = word.Text,
            };
            return View(vm);
        }



        [HttpPost]
        public IActionResult EditTextById(long id,personnel_word pword)
        {
            var word = textService.GetById(id);
            if(word == null)
            {
                return RedirectToAction(nameof(Index));
            }

            word.Text = pword.Text;


            textService.EditText(word);

            return RedirectToAction("index");
        }



        public IActionResult word(personnel_word word )
        {
            return View(word);
        }

        //[HttpPost] 
        //IActionResult word(string id)
        //{

        //    return View();
        //}
        //public IActionResult Findword(string name, string tname)
        //{
        //    var word = textService.GetById(tname, name);
        //    if (word == null)
        //    {

        //        return RedirectToAction("index");
        //    }
        //    return View(word);
        //}


        public IActionResult word_name()
        {
            return View();
        }

        [HttpPost]
        IActionResult word_name(string name)
        {

            return View();
        }

        //public IActionResult WordFindById()
        //{
        //    return View();
        //}


        //[HttpPost]
        //public IActionResult WordFindById(string name ,string tname)
        //{

        //    return RedirectToAction("FindWord",name,tname);
        //}

        [HttpGet]
        public IActionResult AllWordFindByUser(long Id)
        {

            //var AllWord = UserService.GetAllWordByUser(userid);
            var user = UserService.GetUserById(Id);
            return View(user);
        }


        //public IActionResult AllWordFindByName(string name )
        //{
        //    var personnel = textService.GetAllTextByName(name);
        //    return View(personnel);

        //}


        //public IActionResult DeleteWord(string name ,string tname)
        //{
        //    var word = textService.GetById(tname, name);
        //    if (word == null)
        //    {

        //        return RedirectToAction("index");
        //    }
        //    textService.Delete(word);
        //    return View("index");
        //}
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">用户Id</param>
        /// <returns></returns>
        public IActionResult DeleteWordById(long id)
        {
            var word = textService.GetById(id);
            if (word == null)
            {

                return RedirectToAction("index");
            }
            textService.Delete(word);
            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        public IActionResult AddUser(SignUpModel signUpModel)
        {
            UserService.SignUp(signUpModel);
            return RedirectToAction("index");
        }

        public IActionResult ReLogin()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AddTextById(long Id)
        {
            var User = UserService.GetUserById(Id);
            string user, uid;
            Request.Cookies.TryGetValue("User", out user);
            Request.Cookies.TryGetValue("Uid", out uid);
            var u = UserService.InspectGuid(new TGuid
            {
                Uid = uid,
                User = user,
            });
            if (u == null)
            {
                return RedirectToAction(nameof(ReLogin));
            }
            personnel_word personnel = new personnel_word { UserModel = User  , UserModelId = User.Id};
            return View(personnel);
        }



        [HttpPost]
        public IActionResult AddTextById(long id, [FromForm]personnel_word word)
        {
            var user  = UserService.GetUserById(id);
            word.UserModel = user;
            word.UserModelId = id;
            
            if (!textService.Inspect(word))
            {
                ModelState.AddModelError(string.Empty, "文件重复");
                return View(word);
            }

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "上传文档出错");
                return View(word);
            }
            

            textService.AddText(word);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult FindWordById(long Id)
        {
            var text =  textService.GetById(Id);
            return View(text);
        }
    }

   

}
