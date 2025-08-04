using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Q3.Data.Entities;
using Q3.Shared.DTO.MainData;

namespace Q3.AutoMapper
{
    public class BlogPostProfile : Profile
    {
        public BlogPostProfile()
        {
            CreateMap<BlogPost, BlogPostDto>().ReverseMap();
            
        }
    }
}
