using ALBLOG.Domain.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ALBLOG.Domain.Service.Interface
{
    interface IPostService
    {
        Task<IEnumerable<Post>> GetAll();

        Task<IEnumerable<Post>> GetAll(Expression<Func<Post, bool>> method);

        Task<Post> Get(Expression<Func<Post, bool>> method);

        Task<Post> GetAndAddPageViews(Expression<Func<Post, bool>> method);

        Task<bool> Delete(Expression<Func<Post, bool>> method);

        Task<bool> Edit(Post post);

        Task<bool> Add(Post newPost);
    }
}