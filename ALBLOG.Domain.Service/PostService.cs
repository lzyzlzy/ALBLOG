using ALBLOG.Domain.Model;
using ALBLOG.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;

namespace ALBLOG.Domain.Service
{
    public class PostService
    {
        protected PostRepository repository;

        public PostService()
        {
            this.repository = new PostRepository();
        }

        public IEnumerable<Post> GetAllPosts() => this.repository.GetAll().Reverse();
        public IEnumerable<Post> GetAllPosts(Expression<Func<Post, bool>> expression) => this.repository.GetAll(expression).Reverse();

        public Post GetPost(Expression<Func<Post, bool>> expression, bool isAdmin) => isAdmin ? this.repository.GetOne(expression) : AddPageViewNum(expression);

        public bool Delete(string title)
        {
            var post = GetPost(i => i.Title == title, false);
            if (post == null)
                return false;
            this.repository.DeleteOne(post.Id);
            return true;
        }

        public void AddPost(Post post)
        {
            post.PageViews = post.PageViews ?? 0;
            this.repository.Add(post);
        }

        public Post AddPageViewNum(Expression<Func<Post, bool>> expression)
        {
            var post = this.repository.GetOne(expression);
            post.PageViews++;
            repository.UpdateAsync(post);
            return post;
        }

        public void Update(Post post)
        {
            repository.Update(post);
        }

        public long DeleteMany(Expression<Func<Post, bool>> expression) => this.repository.DeleteMany(expression);

    }
}
