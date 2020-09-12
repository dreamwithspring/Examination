using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using word_company.Models;

namespace word_company.Services
{
    public interface IUserService
    {

        Task<UserModel> Login(LoginModel loginModel);
        

        public Task<UserModel> SignUp(SignUpModel signUpModel);



        object Logout(UserModel userModel);


        UserModel InspectGuid(TGuid guid);
        public IEnumerable<string> GetAllName();

        public IEnumerable<UserModel> GetAllUser();

        public IEnumerable<personnel_word> GetAllWordByUser(long id);

        public void DeleteUser(UserModel user);

        public UserModel EditUser(UserModel user);

        public UserModel GetUserById(long id);
        public  Task<UserModel> AddUser(AddUserModel addUserModel);

        public void ReSetPassWord(long id);



    }
}
