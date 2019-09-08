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
        Task<bool> LoginAsync(string userName, string password);

        Task<bool> AddAsync(User newUser);

        Task<bool> DeleteAsync(Expression<Func<User, bool>> user);

        Task<bool> ChangePasswordAsync(string userName, string oldPassword, string newPassword);

        Task<bool> ChangeUserNameAsync(string oldUserName, string newUserName, string password);
    }
}
