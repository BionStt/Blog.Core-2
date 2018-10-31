using Blog.Core.Common.Attributes;
using Blog.Core.IRepository;
using Blog.Core.IServices;
using Blog.Core.Model.Models;
using Blog.Core.Services.BASE;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Core.Services
{
    public class BlogArticleServices : BaseServices<BlogArticle>, IBlogArticleServices
    {
        IBlogArticleRepository dal;
        public BlogArticleServices(IBlogArticleRepository dal)
        {
            this.dal = dal;
            base.baseDal = dal;
        }

        public async Task<List<BlogArticle>> getBlogs()
        {
            var blogList = await this.dal.Query(a => a.bID > 0, a => a.bID);
            return blogList;
        }

        [Caching(AbsoluteExpiration = 10)]
        public async Task<List<BlogArticle>> getBlogsById(int id)
        {
            var blogList = await this.dal.Query(a => a.bID == id, a => a.bID);
            return blogList;
        }
    }
}
