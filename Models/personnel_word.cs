using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace word_company.Models
{
    /// <summary>
    /// 文档模型
    /// </summary>
    public class personnel_word
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        public long Id { get; set; }

        /// <summary>
        /// 文档名
        /// </summary>
        [Display(Name = "文档名")]
        [Required(ErrorMessage ="文件名不能为空")]
        public string TName{ get; set; }

        //[Display(Name = "姓名")]
        //[Required(ErrorMessage = "姓名不能为空")]
        //public string Name { get; set; }
        /// <summary>
        /// 文档内容
        /// </summary>
        [Display(Name = "文档内容")]
        [Required(ErrorMessage = "文档内容不能为空")]
        public string Text { get; set; }


        /// <summary>
        /// 用户模型多对一
        /// </summary>
        public UserModel UserModel { get; set; }
        /// <summary>
        /// 用户外键
        /// </summary>
        public long UserModelId { get; set; }
    }
}
