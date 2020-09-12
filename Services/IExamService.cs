using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using word_company.Models;

namespace word_company.Services
{
    public interface IExamService
    {
        /// <summary>
        /// 获得所有的类别，且不辅助查找考试类
        /// </summary>
        /// <returns></returns>
        List<QClass> GetAllQClasses();

        /// <summary>
        /// 获得所有的题目
        /// </summary>
        /// <returns></returns>
        List<ExamModel> GetAllExamModels();

        /// <summary>
        /// 根据类别获得所有题目
        /// </summary>
        /// <param name="qClass"></param>
        /// <returns></returns>
        List<ExamModel> GetAllExamModelsByQ(QClass qClass);

        /// <summary>
        /// 根据类别id获得所有的题目
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        List<ExamModel> GetAllExamModelsByQ(long Id);

        /// <summary>
        /// 根据是否多选获得所有的题目
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        List<ExamModel> GetExamModelsIsMultiple(bool b);

        /// <summary>
        /// 根据id获得题目
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        ExamModel GetExamModelById(long Id);
        
        /// <summary>
        /// 根据id获得题目类型
        /// </summary>
        /// <param name="id">题目id</param>
        /// <returns></returns>
        QClass GetQClassById(long id);

        /// <summary>
        /// 根据类型的id获得包含该类下面的所有题目的类型模型
        /// </summary>
        /// <param name="id">类型id</param>
        /// <returns></returns>
        QClass GetQClassWithExamById(long id);

        /// <summary>
        /// 添加题目
        /// </summary>
        /// <param name="examModel">题目模型</param>
        /// <returns></returns>
        ExamModel AddExamModel(ExamModel examModel);

        /// <summary>
        /// 添加题目类别
        /// </summary>
        /// <param name="qClass">题目类型模型</param>
        /// <returns></returns>
        Task<QClass> AddQClass(QClass qClass);

        
        /// <summary>
        /// 删除题目类别
        /// </summary>
        /// <param name="id">id主键</param>
        /// <returns></returns>
        Task DeleteQClassById(long id);

        /// <summary>
        /// 删除题目类别
        /// </summary>
        /// <param name="qClass"></param>
        void DeleteQClass(QClass qClass);


        /// <summary>
        /// 根据id删除题目
        /// </summary>
        /// <param name="Id">题目id</param>
        /// <returns></returns>
        Task DeleteExamModelById(long Id);

        /// <summary>
        /// 删除题目，建议不要使用该方法，或者用于确定删除的模型是想要删除的
        /// </summary>
        /// <param name="exam">完整的题目</param>
        void DeleteExamModel(ExamModel exam);
        /// <summary>
        /// 修改类型
        /// </summary>
        /// <param name="qClass"></param>
        void EditQClass(QClass  qClass);

        /// <summary>
        /// 修改题目
        /// </summary>
        /// <param name="exam"></param>
        void EditExamModel(ExamModel exam);

        
        /// <summary>
        /// 模糊搜索
        /// </summary>
        /// <param name="information"></param>
        /// <returns></returns>
        List<ExamModel> GetExamModelsContains(string information);

        /// <summary>
        /// 获取所有的类
        /// </summary>
        /// <returns></returns>
        public IEnumerable<QClass> GetAllQClassesToIenumerable();

        /// <summary>
        /// 获取所有的类并包含题目
        /// </summary>
        /// <returns></returns>
        public IEnumerable<QClass> GetAllQClassesWithExam();

        /// <summary>
        /// 选取考试题目
        /// </summary>
        /// <param name="type">考试类型</param>
        /// <param name="id">如果type为0，则考试类别决定题目类别</param>
        /// <returns></returns>
        public List<ExamModel> ExamViewModel(long type ,long id );

        /// <summary>
        /// 添加考试结果 
        /// </summary>
        /// <param name="examination_Form">结果数据模型</param>
        public void AddExamResult_form(Examination_form examination_Form);


        public Examination_form LastExamination_Form(long id);
    }
}
