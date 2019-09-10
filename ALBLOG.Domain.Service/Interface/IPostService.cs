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
    public interface IPostService
    {
        Task<Page> GetPageAsync(int pageSize, int pageIndex);

        Task<Page> GetPageAsync(Expression<Func<Post, bool>> filter, int pageSize, int pageIndex);

        Task<int> GetPageCountAsync(int pageSize);

        Task<int> GetPageCountAsync(Expression<Func<Post, bool>> filter, int pageSize);

        Task<IEnumerable<Post>> GetAllAsync();

        Task<IEnumerable<Post>> GetAllAsync(Expression<Func<Post, bool>> filter);

        Task<Post> GetOneAsync(Expression<Func<Post, bool>> filter);

        Task<Post> GetOneAndAddPageViewsAsync(Expression<Func<Post, bool>> filter);

        Task<long> DeleteAsync(Expression<Func<Post, bool>> filter);

        Task EditAsync(string id, string title, string content, List<string> tags);

        Task AddAsync(string title, List<string> tags, string content, bool isDraft);

        Task ChangePostToDraftAsync(Expression<Func<Post, bool>> filter);

        Task ChangeDraftToPostAsync(Expression<Func<Post, bool>> filter);
    }
}