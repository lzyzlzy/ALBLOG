using ALBLOG.Domain.Model;
using ALBLOG.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using ALBLOG.Domain.Service.Interface;
using System.Threading.Tasks;

namespace ALBLOG.Domain.Service
{
    public class PostService : IPostService
    {
        protected PostRepository _repository;

        public PostService()
        {
            this._repository = new PostRepository();
        }

        public async Task<IEnumerable<Post>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<IEnumerable<Post>> GetAllAsync(Expression<Func<Post, bool>> filter)
        {
            return await _repository.GetAllAsync(filter);
        }

        public async Task<Post> GetOneAsync(Expression<Func<Post, bool>> filter)
        {
            return await _repository.GetOneAsync(filter);
        }

        public async Task<Post> GetOneAndAddPageViewsAsync(Expression<Func<Post, bool>> filter)
        {
            return await _repository.GetOneAndUpdateAsync(filter, i =>
            {
                i.PageViews++;
                return i;
            });
        }

        public async Task<long> DeleteAsync(Expression<Func<Post, bool>> filter)
        {
            return await _repository.DeleteOneAsync(filter);
        }

        public async Task EditAsync(Post post)
        {
            post.EditDate = DateTime.Now;
            await _repository.UpdateAsync(post);
        }

        public async Task ChangePostToDraftAsync(Expression<Func<Post, bool>> filter)
        {
            await _repository.GetOneAndUpdateAsync(filter, i =>
            {
                i.IsDraft = true;
                return i;
            });
        }

        public async Task ChangeDraftToPostAsync(Expression<Func<Post, bool>> filter)
        {
            await _repository.GetOneAndUpdateAsync(filter, i =>
            {
                i.IsDraft = false;
                return i;
            });
        }

        public async Task AddAsync(string title, List<string> tags, string content, bool isDraft)
        {
            var post = new Post
            {
                Title = title,
                Tags = tags,
                Context = content,
                Date = DateTime.Now,
                EditDate = DateTime.Now,
                IsDraft = isDraft,
                PageViews = 0,
                UserName = ""
            };
            await _repository.AddAsync(post);
        }

        public async Task<IEnumerable<Post>> GetPageAsync(int pageSize, int pageIndex)
        {
            return await GetPageAsync(_ => true, pageSize, pageIndex);
        }

        public async Task<IEnumerable<Post>> GetPageAsync(Expression<Func<Post, bool>> filter, int pageSize, int pageIndex)
        {
            var pageCount = await GetPageCountAsync(filter, pageSize);
            pageIndex = pageIndex <= 0 ? 1 : pageIndex;
            pageIndex = pageIndex > pageCount ? pageCount : pageIndex;
            var allPost = await _repository.GetAllAsync(filter);
            return allPost.Skip(pageSize * (pageIndex - 1))
                          .Take(pageSize);
        }

        public async Task<int> GetPageCountAsync(int pageSize)
        {
            return await GetPageCountAsync(_ => true, pageSize);
        }

        public async Task<int> GetPageCountAsync(Expression<Func<Post, bool>> filter, int pageSize)
        {
            var postCount = (await _repository.GetAllAsync(filter)).Count();
            var num = postCount / pageSize;
            var pageCount = postCount % pageSize > 0 ? num + 1
                                                     : num;
            return pageCount;
        }
    }
}
