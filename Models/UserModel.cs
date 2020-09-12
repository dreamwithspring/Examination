using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace word_company.Models
{
    /// <summary>
    /// 权限
    /// </summary>
    public enum Role
    {
        /// <summary>
        /// 管理员
        /// </summary>
        admin = 1,
        /// <summary>
        /// 用户
        /// </summary>
        user = 0,
        /// <summary>
        /// 超级管理员
        /// </summary>
        SuperAdmin = 2,
    }

    /// <summary>
    /// 用户模型
    /// </summary>
    public class UserModel
    {
        /// <summary>
        /// 主键id
        /// </summary>
        [Key]
        public long Id { get; set; }

        /// <summary>
        /// 用户账号，用于登录
        /// </summary>
        [Required(ErrorMessage = "账号不能为空")]
        [Display(Name = "用户账号")]
        public string User { get; set; }

        /// <summary>
        /// 用户姓名，可任意修改
        /// </summary>
        [Required(ErrorMessage = "用户名不能为空")]
        [Display(Name = "用户名")]
        public string UserName { get; set; }

        /// <summary>
        /// 密码，可任意修改
        /// </summary>
        [Required(ErrorMessage = "密码不能为空")]
        [Display(Name = "密码"), DataType(DataType.Password)]
        public string PassWord { get; set; }

        /// <summary>
        /// 权限
        /// </summary>
        public Role Role { get; set; }

        /// <summary>
        /// 一对多，对应文档
        /// </summary>
        public List<personnel_word> personnel_Words { get; set; }

        /// <summary>
        /// 检验用户登录的uid字符串
        /// </summary>
        public string Uid { get; set; }

        /// <summary>
        /// 登录时间
        /// </summary>
        public DateTime DateTime { get; set; }

        /// <summary>
        /// 测试次数
        /// </summary>
        public long Score_Count { get; set; }
        /// <summary>
        /// 平均分
        /// </summary>
        public long AverageScore { get; set; }
        /// <summary>
        /// 测试总时长，分钟计
        /// </summary>
        public long TestTimes { get; set; }
        /// <summary>
        /// 测试结果 
        /// </summary>
        public List<Examination_form> examination_Forms { get; set; }

        public UserStatusModel UserStatusModel { get; set; }
    }

    /// <summary>
    /// 登录模型
    /// </summary>
    public class LoginModel
    {
        /// <summary>
        /// 用户
        /// </summary>
        [Required(ErrorMessage = "账号不能为空")]
        [Display(Name = "用户账号")]
        public string User { get; set; }

        //[Required(ErrorMessage = "用户名不能为空")]
        //[Display(Name = "用户名")]
        //public string UserName { get; set; }

        /// <summary>
        /// 登录密码
        /// </summary>
        [Required(ErrorMessage = "密码不能为空")]
        [Display(Name = "密码"), DataType(DataType.Password)]
        public string PassWord { get; set; }
    }

    /// <summary>
    /// 登录检验模型
    /// </summary>
    public class TGuid
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string User { get; set; }
        /// <summary>
        /// 用户检验uid
        /// </summary>
        public string Uid { get; set; }
    }

    /// <summary>
    /// 用户展示模型
    /// </summary>
    public class UserViewModel:UserModel
    {
        /// <summary>
        /// 返回信息
        /// </summary>
        public string msg { get; set; }
    }

    /// <summary>
    /// 登录视图模型
    /// </summary>
    public class LoginViewModel: LoginModel
    {
        /// <summary>
        /// 返回信息
        /// </summary>
        public string msg { get; set; }
    }

    /// <summary>
    /// 注册模型，继承登录模型
    /// </summary>
    public class SignUpModel:LoginModel
    {
        /// <summary>
        /// 权限
        /// </summary>
        public Role Role { get; set; }

        /// <summary>
        /// 用户姓名
        /// </summary>
        [Required(ErrorMessage = "用户名不能为空")]
        [Display(Name = "用户名")]
        public string UserName { get; set; }
    }

    /// <summary>
    /// 添加用户模型
    /// </summary>
    public class AddUserModel
    {
        /// <summary>
        /// 权限
        /// </summary>
        public Role Role { get; set; }

        /// <summary>
        /// 用户姓名
        /// </summary>
        [Required(ErrorMessage = "用户名不能为空")]
        [Display(Name = "用户名")]
        public string UserName { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [Required(ErrorMessage = "账号不能为空")]
        [Display(Name = "用户账号")]
        public string User { get; set; }

        /// <summary>
        /// 用户密码，默认123456
        /// </summary>
        public string PassWord { get; set; } = "123456";
    }
}
