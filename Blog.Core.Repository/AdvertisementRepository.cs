using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Blog.Core.IRepository;
using Blog.Core.Model.Models;
using Blog.Core.Repository.BASE;
using SqlSugar;

namespace Blog.Core.Repository
{
    public class AdvertisementRepository : BaseRepository<Advertisement>, IAdvertisementRepository
    {

        public AdvertisementRepository()
        {

        }
    }
}
