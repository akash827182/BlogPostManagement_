using Q3.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q3.Data.IRepository
{
    public interface IBlogPostRepository
    {
        Task<BlogPost> GetBlogPostByIdAsync(int id);
        Task<IEnumerable<BlogPost>> GetAllBlogPostsAsync();
        Task AddAsync(BlogPost blogPost);
        Task UpdateAsync(BlogPost blogPost);
        Task DeleteAsync(int id);
    }
}
