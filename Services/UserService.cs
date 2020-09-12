using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using word_company.Models;
using word_company.Data;
using IdentityServer4.Models;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Http.Headers;

namespace word_company.Services
{
    public class UserService : IUserService
    {
        public UserService(DataContext dataContext)
        {
            DataContext = dataContext;

        }

         public DataContext DataContext { get; }

        //public readonly HttpClient HttpClient;



        public async Task<UserModel> Login(LoginModel  loginModel)
        {
            UserModel user = new UserModel();

                user = await DataContext.userModels
                    .Where(x =>  x.User == loginModel.User )
                    .Include(x => x.personnel_Words)
                    .SingleOrDefaultAsync();
                if(user == null)
                {
                    throw new Exception("该用户名无人使用");
                }else if(loginModel.PassWord != user.PassWord){

                    throw new Exception("密码错误");
                    
                }
            UserViewModel userViewModel =  new  UserViewModel
            {
                Uid = user.Uid,
                Id = user.Id,
                User = user.User,
                UserName = user.UserName,
                DateTime = user.DateTime,
                PassWord = user.PassWord,
                personnel_Words = user.personnel_Words,
                Role = user.Role,
            };
                return userViewModel;

        }

        public object Logout(UserModel userModel)
        {
            
            
            throw new NotImplementedException();
        }

        public async  Task<UserModel> SignUp(SignUpModel signUpModel)
        {
            UserModel userModel = new UserModel
            {
                User = signUpModel.User,
                Role = Role.user,
                UserName = signUpModel.UserName,
                PassWord = signUpModel.PassWord,
                DateTime = DateTime.Now,
                Uid = Guid.NewGuid().ToString(),
            };
            DataContext.userModels.Add(userModel);
            await DataContext.SaveChangesAsync();
            return userModel;
        }

        public UserModel InspectGuid(TGuid uid)
        {
            string re;

            var user = DataContext.userModels
                .Where(x => x.User == uid.User)
                .Include(x => x.personnel_Words)
                .SingleOrDefault();
            if(user == null)
            {
                return null;
            }else if ( user.Uid != uid.Uid)
            {
                return null;
            }

            if(DateTime.Now >= user.DateTime.AddDays(1))
            {
                user.DateTime = DateTime.Now;
                re =   Guid.NewGuid().ToString();
                user.Uid = re;
                DataContext.Attach(user);
                DataContext.SaveChanges();
            }
            return user;
        }


        public IEnumerable<string> GetAllName()
        {
            return DataContext.userModels.Select(p => p.UserName).Distinct().ToList();
        }

        public IEnumerable<personnel_word> GetAllWordByUser(long id)
        {
            return DataContext.Personnels.Where(p => p.UserModel.Id == id)
                .Include(x => x.UserModel)
                .ToList();
        }

        public IEnumerable<UserModel> GetAllUser()
        {
            return DataContext.userModels
                .Where(p => p.Role != Role.SuperAdmin)
                .Include(x => x.personnel_Words)
                .Distinct().ToList();
        }

        public void DeleteUser(UserModel user)
        {
            
            var u =   DataContext.userModels.Where(p => p.Id == user.Id).SingleOrDefault();
            //foreach(var x in u.personnel_Words)
            //{
            //    DataContext.Personnels.Remove(x);
            //}
            DataContext.userModels.Remove(user);
            DataContext.SaveChanges();
        }

        public UserModel EditUser(UserModel user)
        {

            var u = DataContext.userModels
                .Where(p => p.Id == user.Id)
                .Include(X => X.personnel_Words)
                .SingleOrDefault();
            
            u.UserName = user.UserName;
            u.PassWord = user.PassWord;
            u.Role = user.Role;
            //foreach(var x in u.personnel_Words)
            //{
            //    DataContext.Personnels.Remove(x);
            //}
            DataContext.userModels.Attach(u);
            DataContext.SaveChanges();
            return u;
        }


        public UserModel GetUserById(long id)
        {
            var u = DataContext.userModels.Where(p => p.Id == id)
                .Include(x => x.personnel_Words).SingleOrDefault();
            return u;
        }

        public async Task<UserModel> AddUser(AddUserModel addUserModel)
        {
            UserModel userModel = new UserModel
            {
                User = addUserModel.User,
                Role = addUserModel.Role,
                UserName = addUserModel.UserName,
                PassWord = "123456",
                DateTime = DateTime.Now,
                Uid = Guid.NewGuid().ToString(),
            };
            DataContext.userModels.Add(userModel);
            await DataContext.SaveChangesAsync();
            return userModel;
        }

        public void ReSetPassWord(long id   )
        {
            UserModel user = DataContext.userModels.Where(p => p.Id == id).SingleOrDefault();
            user.PassWord = "123456";
            DataContext.SaveChanges();
        }
    }
}
