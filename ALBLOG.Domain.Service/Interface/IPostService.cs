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
        Task<IEnumerable<Post>> GetAllAsync();

        Task<IEnumerable<Post>> GetAllAsync(Expression<Func<Post, bool>> method);

        Task<Post> GetAsync(Expression<Func<Post, bool>> method);

        Task<Post> GetAndAddPageViewsAsync(Expression<Func<Post, bool>> method);

        Task<bool> DeleteAsync(Expression<Func<Post, bool>> method);

        Task<bool> EditAsync(Post post);

        Task<bool> AddAsync(Post newPost);
    }
}