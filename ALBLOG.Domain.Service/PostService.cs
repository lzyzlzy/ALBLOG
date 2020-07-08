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

        public async Task EditAsync(string id, string title, string content, string markDown, List<string> tags)
        {
            await _repository.GetOneAndUpdateAsync(i => i.Id == id, j =>
            {
                j.Title = title;
                j.Context = content;
                j.MarkDown = markDown;
                j.EditDate = DateTime.Now;
                j.Tags = tags;
                return j;
            });
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

        public async Task AddAsync(string title, List<string> tags, string content, string markDown, bool isDraft)
        {
            var post = new Post
            {
                Title = title,
                Tags = tags,
                Context = content,
                MarkDown = markDown,
                Date = DateTime.Now,
                EditDate = DateTime.Now,
                IsDraft = isDraft,
                PageViews = 0,
                UserName = ""
            };
            await _repository.AddAsync(post);
        }

        public async Task<Page> GetPageAsync(int pageSize, int pageIndex)
        {
            return await GetPageAsync(_ => true, pageSize, pageIndex);
        }

        public async Task<Page> GetPageAsync(Expression<Func<Post, bool>> filter, int pageSize, int pageIndex)
        {
            var pageCount = await GetPageCountAsync(filter, pageSize);
            pageIndex = pageIndex <= 0 ? 1
                                       : pageIndex > pageCount ? pageCount
                                                               : pageIndex;
            var offset = pageSize * (pageIndex - 1);
            offset = offset < 0 ? 0 : offset;
            var result = await _repository.GetManyByPage(filter, i => i.Date, offset, pageSize);
            var page = new Page
            {
                HaveLast = pageIndex > 1,
                HaveNext = pageIndex < pageCount,
                PageCount = pageCount,
                Index = pageIndex,
                Posts = result,
                Size = pageSize
            };
            return page;
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
