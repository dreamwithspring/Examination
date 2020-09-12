using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using word_company.Models;

namespace word_company.Services
{
    public interface  ITextService<T> where T:class
    {
        IEnumerable<T> GetAll();

        //IEnumerable<string> GetAllName();

        //IEnumerable<T> GetAllTextByName(string name); 


        //T GetById(string tname ,string name);
        //T GetById(T T, string name);

        T GetById(long id);

        T AddText(  T T );

        T EditText(T T);


        bool Inspect(T T);

        //void Delete(string tname, string name);

        void Delete(personnel_word word);

        
    }
}
