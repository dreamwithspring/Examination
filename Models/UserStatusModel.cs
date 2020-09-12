using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace word_company.Models
{
    /// <summary>
    /// 用户状态
    /// </summary>
    public class UserStatusModel
    {
        [Key]
        public long id { get; set; }


        public bool IsOnLine { get; set; } = false;

        public bool IsExam { get; set; }

        public DateTime Exam_Start_Time { get; set; }

        public UserModel UserModel { get; set; }

        public long UserModelId { get; set; }
    }
}
