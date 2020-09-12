using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using word_company.Data;
using word_company.Models;

namespace word_company.Services
{
    public class TextService : ITextService<personnel_word>
    {
        public TextService(DataContext dataContext )
        {
            DataContext = dataContext;
        }

        public DataContext DataContext { get; }


        public personnel_word AddText(personnel_word T)
        {
            DataContext.Personnels.Add(T);
            
            DataContext.SaveChanges();

            return T;
        }

        //public void Delete(string tname , string name)
        //{

        //    DataContext.Personnels.Remove(GetById(tname, name));
        //    DataContext.SaveChanges();
        //}
        public void Delete(personnel_word word)
        {

            DataContext.Personnels.Remove(word);
            DataContext.SaveChanges();
        }

        public personnel_word EditText(personnel_word T)
        {
            DataContext.Personnels.Attach(T);
            DataContext.SaveChanges();
            return T;
        }

        public  IEnumerable<personnel_word> GetAll()
        {
            return DataContext.Personnels.ToList();
        }

        //public  IEnumerable<string> GetAllName()
        //{
        //    return DataContext.Personnels.Select(p => p.Name).Distinct().ToList();
        //}

        //public IEnumerable<personnel_word> GetAllTextByName(string name)
        //{
        //    IEnumerable<personnel_word> personnels = DataContext.Personnels.Where(p => p.Name.Contains(name)).ToList();
        //    return personnels;
        //}

        //public personnel_word GetById(string tname, string name)
        //{
        //    IEnumerable<personnel_word> personnels = DataContext.Personnels.Where(p => p.Name.Contains(name)).ToList();
        //    return personnels. Where(p => p.TName==tname ).SingleOrDefault();
        //}

        public personnel_word GetById(personnel_word T, string name)
        {
            throw new NotImplementedException();
        }

        public personnel_word GetById(long id)
        {
            personnel_word personnel = DataContext.Personnels
                .Where(p => p.Id == id)
                .Include(X => X.UserModel)
                .SingleOrDefault();
            return personnel;
        }



        /// <summary>
        /// 检查该文档是否存在
        /// </summary>
        /// <param name="T"></param>
        /// <returns></returns>
        public bool Inspect(personnel_word T)
        {
            


            var w = GetById(T.Id);
            if(w != null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }


    }
}
