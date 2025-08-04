using Q3.Shared.DTO.MainData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q3.Business.IBusinessServices
{
    public interface IBlogPostService
    {
        Task<BlogPostDto> GetBlogPostByIdAsync(int id);
        Task<IEnumerable<BlogPostDto>> GetAllBlogPostsAsync();
        Task<BlogPostDto> CreateBlogPostAsync(BlogPostDto blogPostDto);
        Task UpdateBlogPostAsync(int id, BlogPostDto blogPostDto);
        Task DeleteBlogPostAsync(int id);
    }
}
