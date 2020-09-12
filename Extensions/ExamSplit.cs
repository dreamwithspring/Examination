using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using word_company.Models;
using System.Collections;
using System.Text;
using Microsoft.AspNetCore.Mvc.Filters;

namespace word_company.Extensions
{
    public static class ExamSplit
    {
        public static string ListToString(List<Examanswer_List> lists)
        {
            StringBuilder str = new StringBuilder();
            foreach(var a in lists)
            {
                str.Append(a.Id + "%%%.");
                str.Append(a.ExamId + "%%%,");
                str.Append(a.Answer + "%%%*");
                str.Append(a.Result + "%%%;");
            }
            return str.ToString();
        }

        public static List<Examanswer_List> StringToList(string str)
        {
            List<Examanswer_List> lists = new List<Examanswer_List>();
            foreach(var a in str.Split("%%%;"))
            {
                if(a == "")
                {
                    break;
                }
                Examanswer_List exam_List = new Examanswer_List();
                var idpoint = a.IndexOf("%%%.");
                var exampoint = a.IndexOf("%%%,");
                var answerpoint = a.IndexOf("%%%*");
                if(idpoint == -1 || exampoint == -1 || answerpoint == -1)
                {
                    throw new Exception("数据错误");
                }
                exam_List.Id =  long.Parse(a.Substring(0, idpoint));
                exam_List.ExamId = long.Parse( a.Substring(idpoint + 4, exampoint - idpoint - 4));
                exam_List.Answer = a.Substring(exampoint + 4, answerpoint - exampoint - 4);
                exam_List.Result = bool.Parse( a.Substring(answerpoint + 4, a.Length - answerpoint - 4));
                lists.Add(new Examanswer_List
                {
                    Answer = exam_List.Answer,
                    ExamId = exam_List.ExamId,
                    Result = exam_List.Result,
                    Id = exam_List.Id,
                });
            }
            return lists;
        }
    }
}
