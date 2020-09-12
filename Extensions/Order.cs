using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using word_company.Models;

namespace word_company.Extensions
{
    public class OrderByD : IComparer<ExamModel>
    {
        public int Compare([AllowNull] ExamModel A, [AllowNull] ExamModel B)
        {
            if (A == null || B == null)
            {
                return 0;
            }
            if (A.Difficulty < B.Difficulty)
            {
                return -1;
            }
            if (A.Difficulty == B.Difficulty)
            {
                return 0;
            }
            return 1;
        }

    }
}
