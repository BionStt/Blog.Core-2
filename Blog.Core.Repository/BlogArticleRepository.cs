using Blog.Core.IRepository;
using Blog.Core.Model.Models;
using Blog.Core.Repository.BASE;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Core.Repository
{
    public class BlogArticleRepository : BaseRepository<BlogArticle>, IBlogArticleRepository
    {
    }
}
