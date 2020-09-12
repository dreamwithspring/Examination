using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using word_company.Models;
using word_company.Services;
using word_company.Extensions;
using System.Net.Http;


namespace word_company.Controllers
{
    public class ExamController : Controller
    {
        protected readonly IUserService _UserService;
        protected readonly IExamService _ExamService;
        

        public ExamController(IUserService userService,IExamService examService )
        {
            _UserService = userService;
            _ExamService = examService;
        }

        public IActionResult Index()
        {
            string user;
            string Uid;
            Request.Cookies.TryGetValue("User", out user);
            Request.Cookies.TryGetValue("Uid", out Uid);
            if (user == null || Uid == null)
            {
                return RedirectToAction("Login","User");
            }
            var u = _UserService.InspectGuid(new TGuid { User = user, Uid = Uid, });
            if (u == null)
            {
                return RedirectToAction("Login", "User");
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
                });
                switch (u.Role)
                {
                    case Role.user:
                        return RedirectToAction(nameof(ExamSelect));
                    case Role.admin:
                        return View(_ExamService.GetAllQClasses());
                    case Role.SuperAdmin:
                        return View(_ExamService.GetAllQClasses());
                }
                return RedirectToAction("Login", "User");
            }
        }

        public IActionResult AddQClass()
        {
            
            return View();
        }
        [HttpPost]
        public IActionResult AddQClass([FromForm]QClass qClass)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "类型添加错误");
                return View(qClass);
            }
             _ExamService.AddQClass(qClass);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult DeleteQClass(long id)
        {
            string user;
            string Uid;
            Request.Cookies.TryGetValue("User", out user);
            Request.Cookies.TryGetValue("Uid", out Uid);
            if (user == null || Uid == null)
            {
                return RedirectToAction("Login", "Home");
            }
            var u = _UserService.InspectGuid(new TGuid { User = user, Uid = Uid, });
            if (u == null)
            {
                return RedirectToAction("Login", "Home");
            }
            if (u.Role != Role.SuperAdmin)
            {
                return View("DeleteError");
            }
            _ExamService.DeleteQClassById(id);
            return RedirectToAction(nameof(Index));
        }

       public IActionResult EditQClass(long id)
        {
            var qclass = _ExamService.GetQClassById(id);
            return View(qclass);
        }
        [HttpPost]
        public IActionResult EditQClass(long id , [FromForm]EditQClassViewModel editQClassViewModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "修改错误");
                return View(editQClassViewModel);
            }
            var qclass = _ExamService.GetQClassById(id);
            qclass.Explain = editQClassViewModel.Explain;
            qclass.Class = editQClassViewModel.Class;
            _ExamService.EditQClass(qclass);
            return RedirectToAction(nameof(Index));
        }




        public IActionResult AddExamModelMultiple(long id)
        {
            var exam = new ExamModel
            {
                QClassId = id,
            };
            return View(exam);
        }


        [HttpPost]
        public IActionResult AddExamModelMultiple(long id , [FromForm]ExamModel examModel)
        {
            string user; 
            string Uid;


            Request.Cookies.TryGetValue("User", out user);
            Request.Cookies.TryGetValue("Uid", out Uid);
            var u = _UserService.InspectGuid(new TGuid { User = user, Uid = Uid, });
            
            if (!ModelState.IsValid || u == null)
            {
                ModelState.AddModelError(string.Empty, "类型添加错误");
                return View(examModel);
            }
            var str = Request.Form["AA"]  +  Request.Form["AB"] + Request.Form["AC"] + Request.Form["AD"];
            examModel.Answer = str;
            examModel.User = u.UserName;
            examModel.QClassId = id;
            examModel.Multiple = true;
            
            _ExamService.AddExamModel(examModel);

            return RedirectToAction(nameof(Index));
        }


        public IActionResult AddExamModelNotMultiple(long id)
        {
            var exam = new ExamModel
            {
                QClassId = id,
            };
            return View(exam);
        }


        [HttpPost]
        public IActionResult AddExamModelNotMultiple(long id, [FromForm]ExamModel examModel)
        {
            string user;
            string Uid;


            Request.Cookies.TryGetValue("User", out user);
            Request.Cookies.TryGetValue("Uid", out Uid);
            var u = _UserService.InspectGuid(new TGuid { User = user, Uid = Uid, });

            if (!ModelState.IsValid || u == null)
            {
                ModelState.AddModelError(string.Empty, "类型添加错误");
                return View(examModel);
            }
            examModel.User = u.UserName;
            examModel.QClassId = id;

            _ExamService.AddExamModel(examModel);

            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public IActionResult GetAllExamModelsByQ( long id)
        {
            string user;
            string Uid;


            Request.Cookies.TryGetValue("User", out user);
            Request.Cookies.TryGetValue("Uid", out Uid);
            var u = _UserService.InspectGuid(new TGuid { User = user, Uid = Uid, });
            if (u == null || (u.Role != Role.admin &&  u.Role != Role.SuperAdmin)  )
            {
                return View("Error");
            }
            var qclass = _ExamService.GetAllExamModelsByQ(id);
            return View(qclass);
        }


        public IActionResult DeleteExamModel(long id)
        {
            var exam = _ExamService.GetExamModelById(id);
            _ExamService.DeleteExamModelById(id);
            return RedirectToAction(nameof(GetAllExamModelsByQ), exam.QClassId);
        }


        public IActionResult EditExamModel(long id)
        {
            var exam = _ExamService.GetExamModelById(id);

                return View("EditExamModel",  exam);
        }
        [HttpPost]
        public IActionResult EditExamModel(long id, [FromForm]ExamModel examModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "修改错误");
                return View(examModel);
            }
            var exam = _ExamService.GetExamModelById(id);
            exam.QClassId = examModel.QClassId;
            exam.Question = examModel.Question;
            exam.AnswerA = examModel.AnswerA;
            exam.AnswerB = examModel.AnswerB;
            exam.AnswerC = examModel.AnswerC;
            exam.AnswerD = examModel.AnswerD;
            exam.Explain = examModel.Explain;
            exam.Difficulty = examModel.Difficulty;
            _ExamService.EditExamModel(exam);

            return RedirectToAction(nameof(GetAllExamModelsByQ), exam.QClassId);

        }

        [HttpGet]
        public IActionResult ExamSelect()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Exam(long id,long type)
        {
            List<ExamModel> UserExamModels = new List<ExamModel>();
            UserExamModels = _ExamService.ExamViewModel(type, id);
            return View(UserExamModels);
        }


        [HttpPost]
        public IActionResult Exam(HttpClient httpContent)
        {
            var a = int.Parse(Request.Form["all"]);
            var resultarr = new string[3];
            var str = "";
            var arr = new string[a + 1];
            var result = 0;
            int count_all = 0;
            ExamModel answer = new ExamModel() ;
            List<Examanswer_List> examanswers = new List<Examanswer_List>();
            Examanswer_List examanswer = new Examanswer_List();
            
            //for(int i = 0;i <= a;i++ )
            //{
            //    answer = _ExamService.GetExamModelById(int.Parse(resultarr[1]));
            //    count_all += (int)answer.Score;
            //    str =   Request.Form["Ans" + i];
            //    resultarr = str.Split("%%%,");
            //    if(answer.Answer  == resultarr[0])
            //    {
            //        result += (int)answer.Score;
            //    }
            //    result_str += (resultarr + ";");
            //}
            for(int i = 0;i<= a; i++)
            {
                str = Request.Form["Ans" + i];
                resultarr = str.Split("%%%,");
                answer = _ExamService.GetExamModelById(int.Parse(resultarr[1]));
                count_all += (int)answer.Score;
                examanswer.ExamId = answer.Id;
                examanswer.Answer = resultarr[0];
                if (answer.Answer == resultarr[0])
                {
                    result += (int)answer.Score;
                    examanswer.Result = true;
                }
                else
                {
                    examanswer.Result = false;
                }
                examanswers.Add(new Examanswer_List {
                    Answer = examanswer.Answer,
                    ExamId =examanswer.ExamId,
                    Result = examanswer.Result,
                });
            }
            string result_str = ExamSplit.ListToString(examanswers);
            Examination_form form = new Examination_form();
            form.Count = (int)(100 * result / count_all);
            form.DateTime = DateTime.Now;
            form.Result = result_str;
            string user;
            string Uid;
            Request.Cookies.TryGetValue("User", out user);
            Request.Cookies.TryGetValue("Uid", out Uid);
            var u = _UserService.InspectGuid(new TGuid { User = user, Uid = Uid, });
            form.UserModel = u;
            _ExamService.AddExamResult_form(form);

            //for(int i = 0; i < a; i++)
            //{
            //    UserExamModels[]
            //}
            //return View("ExamResult");
            //return RedirectToAction(nameof(ExamResult));
            return Json(new { 
                count = form.Count,
            }) ;
        }


        public IActionResult ExamResult()
        {
            string user;
            string Uid;
            Request.Cookies.TryGetValue("User", out user);
            Request.Cookies.TryGetValue("Uid", out Uid);
            var u = _UserService.InspectGuid(new TGuid { User = user, Uid = Uid, });
            var form = _ExamService.LastExamination_Form(u.Id);
            List<Examanswer_List> examanswer_Lists = new List<Examanswer_List>();
            examanswer_Lists = ExamSplit.StringToList(form.Result);
            List<ExamResultViewModel> re = new List<ExamResultViewModel>();
            var i = 0;
            var exam = new ExamModel();
            foreach (var s in examanswer_Lists)
            {
                exam = _ExamService.GetExamModelById(s.ExamId);
                re.Add(new ExamResultViewModel
                {
                    QClass = exam.QClass,
                    Answer = exam.Answer,
                    AnswerA =exam.AnswerA,
                    AnswerB =exam.AnswerB,
                    AnswerC = exam.AnswerC,
                    AnswerD = exam.AnswerD,
                    Multiple = exam.Multiple,
                    Question = exam.Question,
                    Explain = exam.Question,
                    Result = examanswer_Lists[i++].Answer,
                });
            }

            return View(re);
        }


        public IActionResult GetExamModel(long Id)
        {
            var exammodel = _ExamService.GetExamModelById(Id);


            return View(exammodel);
        } 


    }
}
