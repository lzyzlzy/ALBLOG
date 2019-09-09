using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ALBLOG.Domain.Model;
using ALBLOG.Domain.Repository;
using ALBLOG.Domain.Service.Interface;

namespace ALBLOG.Domain.Service
{
    public class UserService : IUserService
    {
        protected UserRepository _repository { get; set; }

        public UserService()
        {
            this._repository = new UserRepository();
        }

        public async Task<ReturnModel> LoginAsync(string userName, string password)
        {
            var user = await _repository.GetOneAsync(i => i.UserName == userName);
            if (user == null)
                return new ReturnModel
                {
                    IsSuccess = false,
                    Message = "用户名不存在"
                };
            if (user.Password != password)
                return new ReturnModel
                {
                    IsSuccess = false,
                    Message = "密码输入错误"
                };
            return new ReturnModel
            {
                IsSuccess = true,
                Message = "验证成功"
            };
        }

        public async Task AddAsync(User newUser)
        {
            await _repository.AddAsync(newUser);
        }

        public async Task<long> DeleteAsync(Expression<Func<User, bool>> filter)
        {
            return await _repository.DeleteOneAsync(filter);
        }

        public async Task<ReturnModel> ChangePasswordAsync(string userName, string oldPassword, string newPassword)
        {
            var user = await _repository.GetOneAsync(i => i.UserName == userName);
            if (user == null)
                return new ReturnModel
                {
                    IsSuccess = false,
                    Message = "用户名不存在"
                };
            if (user.Password != oldPassword)
                return new ReturnModel
                {
                    IsSuccess = false,
                    Message = "密码错误"
                };
            user.Password = newPassword;
            await _repository.UpdateAsync(user);
            return new ReturnModel
            {
                IsSuccess = true,
                Message = "修改密码成功"
            };
        }

        public async Task<ReturnModel> ChangeUserNameAsync(string oldUserName, string newUserName, string password)
        {
            var user = await _repository.GetOneAsync(i => i.UserName == oldUserName);
            if (user == null)
                return new ReturnModel
                {
                    IsSuccess = false,
                    Message = "用户名不存在"
                };
            if (user.Password != password)
                return new ReturnModel
                {
                    IsSuccess = false,
                    Message = "密码错误"
                };
            if ((await _repository.GetCountAsync(i => i.UserName == newUserName)) != 0)
                return new ReturnModel
                {
                    IsSuccess = false,
                    Message = "用户名已被占用"
                };
            user.UserName = newUserName;
            await _repository.UpdateAsync(user);
            return new ReturnModel
            {
                IsSuccess = true,
                Message = "修改用户名成功"
            };
        }
    }
}
