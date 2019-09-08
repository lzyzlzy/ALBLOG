using ALBLOG.Domain.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ALBLOG.Domain.Service.Interface
{
    interface IPostService
    {
        IEnumerable<Post> GetAll();

        IEnumerable<Post> GetAll(Expression<Func<Post, bool>> method);

        Post Get(Expression<Func<Post, bool>> method);

        Post GetAndAddPageViews(Expression<Func<Post, bool>> method);

        bool Delete(Expression<Func<Post, bool>> method);

        bool Edit(Post post);

        bool Add(Post newPost);
    }
}