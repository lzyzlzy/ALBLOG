using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using ALBLOG.Domain.Model;
using ALBLOG.Domain.Repository;

namespace ALBLOG.Domain.Service
{
    public class UserService
    {
        protected UserRepository userRepository { get; set; }

        public UserService()
        {
            this.userRepository = new UserRepository();
        }

        public bool AddUser(User user)
        {
            if (IsUserExist(user.UserName))
                return false;
            userRepository.AddAsync(user);
            return true;
        }

        public User GetOne(Expression<Func<User, bool>> expression) => userRepository.GetOne(expression);


        public bool DeleteOne(string userName)
        {
            var user = GetOne(i => i.UserName == userName);
            if (user == null)
                return false;
            userRepository.DeleteOne(user.Id);
            return true;
        }



        public bool ChangePassword(ChangeUserPasswordModel model)
        {
            if (!IsUserExist(model.UserName))
                return false;
            var user = GetOne(i => i.UserName == model.UserName);
            if (user.Password != model.Password)
                return false;
            DeleteOne(model.UserName);
            AddUser(new User { UserName = model.UserName, Password = model.NewPassword });
            return true;
        }

        private bool IsUserExist(string userName) => GetOne(i => i.UserName == userName) != null ? true : false;

    }
}
