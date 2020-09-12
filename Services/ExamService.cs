using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using word_company.Data;
using word_company.Models;
using word_company.Extensions;

namespace word_company.Services
{
    /// <summary>
    /// 考试Service
    /// </summary>
    public class ExamService : IExamService
    {
        protected readonly DataContext DataContext;
        public ExamService(DataContext dataContext)
        {
            DataContext = dataContext;
        }

        /// <summary>
        /// 添加考试题目
        /// </summary>
        /// <param name="examModel"></param>
        /// <returns></returns>
        public ExamModel  AddExamModel(ExamModel examModel)
        {
             DataContext.examModels.Add(examModel);
            var qclass = DataContext.qClasses.Where(p => p.Id == examModel.QClassId).SingleOrDefault();
            qclass.Count += 1;
            DataContext.SaveChanges();
            
            return examModel;
        }

        /// <summary>
        /// 添加题目类型
        /// </summary>
        /// <param name="qClass"></param>
        /// <returns></returns>
        public async Task<QClass> AddQClass(QClass qClass)
        {
            DataContext.qClasses.Add(qClass);
            await DataContext.SaveChangesAsync();
            return qClass;
        }

        /// <summary>
        /// 删除题目
        /// </summary>
        /// <param name="exam"></param>
        public void DeleteExamModel(ExamModel exam)
        {
            
            var exammodel = DataContext.examModels.Where(p => p.Id == exam.Id)
                .Include(p => p.QClass)
                .SingleOrDefault();
            exammodel.QClass.Count -= 1;
            DataContext.examModels.Remove(exammodel);
            DataContext.SaveChanges();
        }

        /// <summary>
        /// 根据id删除考试题目
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task DeleteExamModelById(long Id)
        {
            var exam = DataContext.examModels.Where(p => p.Id == Id)
                .Include(p => p.QClass)
                .SingleOrDefault();
            exam.QClass.Count -= 1;
            DataContext.examModels.Remove(exam);
            await DataContext.SaveChangesAsync();

        }

        /// <summary>
        /// 删除类
        /// </summary>
        /// <param name="qClass"></param>
        public void DeleteQClass(QClass qClass)
        {
            DataContext.qClasses.Remove(qClass);
            DataContext.SaveChanges();
        }


        /// <summary>
        /// 删除类
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteQClassById(long id)
        {
            var qclass =  DataContext.qClasses.Where(p => p.Id == id).SingleOrDefault() ;
                DataContext.qClasses.Remove(qclass);
                await DataContext.SaveChangesAsync();

            
        }

        /// <summary>
        /// 修改考试模型
        /// </summary>
        /// <param name="exam">考试模型</param>
        public void EditExamModel(ExamModel exam)
        {
            DataContext.examModels.Attach(exam);
            DataContext.SaveChanges();
            
        }

        /// <summary>
        /// 修改类别
        /// </summary>
        /// <param name="qClass">题目类</param>
        public void EditQClass(QClass qClass)
        {
            DataContext.qClasses.Attach(qClass);
            DataContext.SaveChanges();
        }

        /// <summary>
        /// 获得全部的考试题目
        /// </summary>
        /// <returns></returns>
        public List<ExamModel> GetAllExamModels()
        {
            return DataContext.examModels.Distinct().ToList();
        }

        /// <summary>
        /// 根据类别获得所有逇考试题目
        /// </summary>
        /// <param name="qClass"></param>
        /// <returns></returns>
        public List<ExamModel> GetAllExamModelsByQ(QClass qClass)
        {
            return DataContext.examModels
                .Where(p => p.QClassId == qClass.Id)
                .Include(p => p.QClass)
                .ToList();
        }

        /// <summary>
        /// 获得所有的类别
        /// </summary>
        /// <returns></returns>
        public IEnumerable<QClass> GetAllQClassesToIenumerable()
        {
            var qClasses = DataContext.qClasses.Distinct().ToList();
            DataContext.Dispose();
            return qClasses;

        }

        /// <summary>
        /// 根据类别的id获得所有的问题
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public List<ExamModel> GetAllExamModelsByQ(long Id)
        {
            return DataContext.examModels
                .Where(p => p.QClassId == Id)
                .Include(p => p.QClass)
                .ToList();
        }

        /// <summary>
        /// 获得所有的类别，且不辅助查找考试类
        /// </summary>
        /// <returns></returns>
        public List<QClass> GetAllQClasses()
        {
            var qClasses = DataContext.qClasses.Distinct().ToList();
            return qClasses;
        }

        /// <summary>
        /// 根据id获得完整的考试模型
        /// </summary>
        /// <param name="Id">id</param>
        /// <returns>完整模型</returns>
        public ExamModel GetExamModelById(long Id)
        {
            return DataContext.examModels
                .Where(p => p.Id == Id)
                .Include(p => p.QClass)
                .SingleOrDefault();
        }

        /// <summary>
        /// 模糊搜索考试题目
        /// </summary>
        /// <param name="information"></param>
        /// <returns></returns>
        public List<ExamModel> GetExamModelsContains(string information)
        {
            return DataContext.examModels
                .Where(p => p.Validity.Contains(information))
                .Include(p => p.QClass)
                .ToList();
        }

        /// <summary>
        /// 选取多选或单选的所有题目
        /// </summary>
        /// <param name="b">false或true来获取题目</param>
        /// <returns></returns>
        public List<ExamModel> GetExamModelsIsMultiple(bool b)
        {
            return DataContext.examModels
                .Where(p => p.Multiple == b)
                .Include(p => p.QClass)
                .ToList();
        }

        /// <summary>
        /// 根据id获得类别并包含所有题目
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public QClass GetQClassById(long id)
        {
           var qclass =  DataContext.qClasses
                .Where(p => p.Id == id)
                .Include(p=>p.ExamModels)
                .SingleOrDefault();
            return qclass;
        }

        /// <summary>
        /// 根据id获得类别并包含所有题目
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public QClass GetQClassWithExamById(long id)
        {
            var qclass = DataContext.qClasses
                .Where(p => p.Id == id)
                .Include(p => p.ExamModels)
                .SingleOrDefault();
            return qclass;
        }

        /// <summary>
        /// 获取所有的类别，并包含题目
        /// </summary>
        /// <returns></returns>
        public IEnumerable<QClass> GetAllQClassesWithExam()
        {
            var qClasses = DataContext.qClasses
                .Distinct()
                .Include(p=>p.ExamModels)
                .ToList();
            
            return qClasses;
        }

        
        /// <summary>
        /// 选取考试题目
        /// </summary>
        /// <param name="type">考试类型</param>
        /// <param name="id">如果type为0，则考试类别决定题目类别</param>
        /// <returns></returns>
        public List<ExamModel> ExamViewModel( long type ,long id = 0)
        {
            //如果type为0则直接返回类别题目
            if (type == 0)
            {
                var qclass = GetQClassById(id);
                return qclass.ExamModels;
            }
            var all = 100;
            //题库
            List<ExamModel> exams = new List<ExamModel>();


            //首先添加每个类别三个题目，题目类型低于5的不考虑
            foreach (var a in DataContext.qClasses.Include(p => p.ExamModels).ToList())
            {
                if (a.ExamModels.Count < 5)
                {
                    continue;
                }
                var one = new Random(Guid.NewGuid().GetHashCode()).Next(a.ExamModels.Count);
                var two = new Random(Guid.NewGuid().GetHashCode()).Next(a.ExamModels.Count);
                var three = new Random(Guid.NewGuid().GetHashCode()).Next(a.ExamModels.Count);
                while (one == two || one == three || two == three)
                {
                    two++;
                    three += 2;
                }
                exams.Add(a.ExamModels[one]);
                all -= (int)a.ExamModels[one].Score;
                exams.Add(a.ExamModels[two]);
                all -= (int)a.ExamModels[two].Score;
                exams.Add(a.ExamModels[three]);
                all -= (int)a.ExamModels[three].Score;

            }

            //根据type决定题目难度
            var examModels = DataContext.examModels
                .Where(p => ((int)p.Difficulty < (2+type)) && ((int)p.Difficulty >= type))
                .Distinct().ToList();
            if (examModels.Count < 50)
            {
                return exams;
            }
            int question = 0;
            //随机添加题目，要求题目不能重复
            for (int i = 0; i < all - 5;)
            {
                question = new Random(Guid.NewGuid().GetHashCode()).Next(examModels.Count);
                exams.Add(examModels[question]);
                examModels.RemoveAt(question);
                i -= (int)examModels[question].Score;
            }
            var exs = examModels.Where(p => p.Score == all).ToList();
            //添加最后一个
            question = new Random(Guid.NewGuid().GetHashCode()).Next(exs.Count);
            exams.Add(exs[question]);
            exams.Sort(new OrderByD());
            return exams;
        }

        /// <summary>
        /// 添加成绩
        /// </summary>
        /// <param name="examination_Form"></param>
        public void AddExamResult_form(Examination_form examination_Form)
        {
            
            var user =  DataContext.userModels.Where(p => p.Id == examination_Form.UserModel.Id)
                .Include(p=>p.examination_Forms)
                .SingleOrDefault();
            long all = 0; 
            foreach(var a in user.examination_Forms)
            {
                all += a.Count;
            }
            all += examination_Form.Count;
            user.Score_Count = all / (++user.TestTimes);
            user.examination_Forms.Add(examination_Form);
            DataContext.userModels.Attach(user);
            DataContext.SaveChanges();
        }

        public Examination_form LastExamination_Form(long id)
        {
            var re = DataContext.userModels
                .Where(p => p.Id == id)
                .Include(p => p.examination_Forms)
                .SingleOrDefault();
            
            return re.examination_Forms.LastOrDefault();
        }
    }
}
