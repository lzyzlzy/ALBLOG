using ALBLOG.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ALBLOG.Domain.Service.Interface
{
    interface IUserService
    {
        Task<bool> Login(string userName, string password);

        Task<bool> Add(User newUser);

        Task<bool> Delete(Expression<Func<User, bool>> user);

        Task<bool> ChangePassword(string userName, string oldPassword, string newPassword);

        Task<bool> ChangeUserName(string oldUserName, string newUserName, string password);
    }
}
