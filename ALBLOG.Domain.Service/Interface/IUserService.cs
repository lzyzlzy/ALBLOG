using ALBLOG.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ALBLOG.Domain.Service.Interface
{
    public interface IUserService
    {
        Task<ReturnModel> LoginAsync(string userName, string password);

        Task AddAsync(User newUser);

        Task<long> DeleteAsync(Expression<Func<User, bool>> filter);

        Task<ReturnModel> ChangePasswordAsync(string userName, string oldPassword, string newPassword);

        Task<ReturnModel> ChangeUserNameAsync(string oldUserName, string newUserName, string password);
    }
}
