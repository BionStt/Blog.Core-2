using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Core.IServices;
using Blog.Core.Model.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "Admin")]
    public class BlogController : ControllerBase
    {
        IAdvertisementServices advertisementServices;
        IBlogArticleServices blogArticleServices;

        public BlogController(IAdvertisementServices _advertisementServices, IBlogArticleServices _blogArticleServices)
        {
            this.advertisementServices = _advertisementServices;
            this.blogArticleServices = _blogArticleServices;
        }

        // GET api/values
        /// <summary>
        /// 获取一个数据列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("id")]
        public async Task<List<BlogArticle>> Get(int id)
        {
            return await this.blogArticleServices.getBlogsById(id);
        }

        // POST api/values
        /// <summary>
        /// Post 请求
        /// </summary>
        /// <param name="love"></param>
        [HttpPost]
        public void Post()
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
