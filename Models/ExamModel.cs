using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace word_company.Models
{
    /// <summary>
    /// 难度
    /// </summary>
    public enum Difficulty 
    {
        /// <summary>
        /// 简单
        /// </summary>
        easy = 0,
        /// <summary>
        /// 比较简单
        /// </summary>
        easier = 1,
        /// <summary>
        /// 普通
        /// </summary>
        common = 2,
        /// <summary>
        /// 比较困难
        /// </summary>
        difficult = 3,
        /// <summary>
        /// 困难
        /// </summary>
        difficulty = 4,
    }

    /// <summary>
    /// 问题类型
    /// </summary>
    public class QClass 
    {

        public long Id { get; set; }
        /// <summary>
        /// 类名
        /// </summary>
        [Required(ErrorMessage = "不能为空")]
        [Display(Name = "类名")]
        public string Class { get; set; }
        /// <summary>
        /// 说明
        /// </summary>
        [Required(ErrorMessage = "不能为空")]
        [Display(Name = "说明")]
        public string Explain { get; set; }

        public List<ExamModel> ExamModels { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public long Count { get; set; }
    }


    /// <summary>
    /// 问题
    /// </summary>
    public class ExamModel
    {
        /// <summary>
        /// ID
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 问题
        /// </summary>
        [Required(ErrorMessage ="不能为空")]
        [Display(Name ="问题")]
        public string Question { get; set; }
        /// <summary>
        /// 简介
        /// </summary>
        [Required(ErrorMessage = "不能为空")]
        [MaxLength(50)]
        [Display(Name = "简介")]
        public string Validity { get; set; }

        /// <summary>
        /// 分类
        /// </summary>
        public QClass QClass { get; set; }
        /// <summary>
        /// 分类的id
        /// </summary>
        [Required(ErrorMessage ="不能为空")]
        [Display(Name = "分类")]
        public long QClassId { get; set; }
        /// <summary>
        /// 解析
        /// </summary>
        [Required(ErrorMessage = "不能为空")]
        [Display(Name = "解析")]
        public string Explain { get; set; }
        /// <summary>
        /// A选项
        /// </summary>
        [Required(ErrorMessage = "不能为空")]
        [Display(Name = "A选项")]
        public string AnswerA { get; set; }
        /// <summary>
        /// B选项
        /// </summary>
        [Required(ErrorMessage = "不能为空")]
        [Display(Name = "B选项")]
        public string AnswerB { get; set; }
        /// <summary>
        /// C选项
        /// </summary>
        [Required(ErrorMessage = "不能为空")]
        [Display(Name = "C选项")]
        public string AnswerC { get; set; }
        /// <summary>
        /// D选项
        /// </summary>
        [Required(ErrorMessage = "不能为空")]
        [Display(Name = "D选项")]
        public string AnswerD { get; set; }
        /// <summary>
        /// 答案
        /// </summary>
        [Required(ErrorMessage = "不能为空")]
        [Display(Name = "答案")]
        public string Answer { get; set; }
        /// <summary>
        /// 是否多选
        /// </summary>
        [Required(ErrorMessage = "不能为空")]
        [Display(Name = "是否多选")]
        public bool Multiple { get; set; }
        /// <summary>
        /// 分值
        /// </summary>
        [Required(ErrorMessage = "不能为空")]
        [Display(Name = "分值")]
        [Range(1,20,ErrorMessage ="分值只能为20以下")]
        public long Score { get; set; }

        /// <summary>
        /// 难度
        /// </summary>
        [Required(ErrorMessage = "不能为空")]
        [Display(Name = "难度")]
        public Difficulty Difficulty { get; set; }

        /// <summary>
        /// 上传者
        /// </summary>
        [Display(Name = "上传者")]
        public string User { get; set; }
    }

    /// <summary>
    /// 修改题目类型模型
    /// </summary>
    public class EditQClassViewModel
    {
        /// <summary>
        /// 类名
        /// </summary>
        [Required(ErrorMessage = "不能为空")]
        [Display(Name = "类名")]
        public string Class { get; set; }
        /// <summary>
        /// 说明
        /// </summary>
        [Required(ErrorMessage = "不能为空")]
        [Display(Name = "说明")]
        public string Explain { get; set; }
    }

    /// <summary>
    /// 考试结果
    /// </summary>
    public class Examination_form
    {
        /// <summary>
        /// 考试结果
        /// </summary>
        [Required(ErrorMessage = "不能为空")]
        [Display(Name = "结果")]
        public string Result {get; set; }

        /// <summary>
        /// 考试时间
        /// </summary>
        public DateTime DateTime { get; set; }


        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        public long Id { get; set; }

        /// <summary>
        /// 成绩
        /// </summary>
        [Required(ErrorMessage = "不能为空")]
        [Range(0,100,ErrorMessage ="成绩不合法")]
        [Display(Name = "成绩")]
        public long Count { get; set; }

        /// <summary>
        /// 用户模型多对一
        /// </summary>
        public UserModel UserModel { get; set; }
        /// <summary>
        /// 用户外键
        /// </summary>
        public long UserModelId { get; set; }
    }

    /// <summary>
    /// 考试题目
    /// </summary>
    public class Examanswer_List
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        public long Id { get; set; }

        /// <summary>
        /// 考试题目的题库id
        /// </summary>
        public long ExamId { get; set; }

        /// <summary>
        /// 答案
        /// </summary>
        public string Answer { get; set; }

        /// <summary>
        /// 结果，是否正确
        /// </summary>
        public bool Result { get; set; }
    }

    public class ExamResultViewModel:ExamModel
    {
        public  string Result { get; set; }

    }

}
