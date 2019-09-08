using ALBLOG.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ALBLOG.Domain.Service.Interface
{
    interface IUserService
    {
        bool Login(string userName, string password);

        bool Add(User newUser);

        bool Delete(Expression<Func<User, bool>> user);

        bool ChangePassword(string userName, string oldPassword, string newPassword);

        bool ChangeUserName(string oldUserName, string newUserName, string password);
    }
}
