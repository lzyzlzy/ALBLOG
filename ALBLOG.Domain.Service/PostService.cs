﻿using ALBLOG.Domain.Model;
using ALBLOG.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ALBLOG.Domain.Service
{
    public class PostService
    {
        protected PostRepository repository;

        public PostService()
        {
            this.repository = new PostRepository();
        }

        public IEnumerable<Post> GetAllPosts() => this.repository.GetAll();

        public Post GetPost(Expression<Func<Post, bool>> expression) => this.repository.GetOne(expression);

        public bool Delete(string title)
        {
            var post = GetPost(i => i.Title == title);
            if (post == null)
                return false;
            this.repository.DeleteOne(post.Id);
            return true;
        }

        public void AddPost(Post post)
        {
            this.repository.Add(post);
        }

        public long DeleteMany(Expression<Func<Post, bool>> expression) => this.repository.DeleteMany(expression);

    }
}
