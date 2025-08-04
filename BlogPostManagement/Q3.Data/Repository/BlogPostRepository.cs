using Microsoft.EntityFrameworkCore;
using Q3.Data.Entities;
using Q3.Data.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Q3.Data.Repository
{
    public class BlogPostRepository:IBlogPostRepository
    {
        private readonly BlogDbContext _blogdbcontext;
        public BlogPostRepository(BlogDbContext context)
        {
            _blogdbcontext = context;
        }

        public async Task<BlogPost> GetBlogPostByIdAsync(int id)
        {
            return await _blogdbcontext.BlogPosts.FindAsync(id);

        }

        public async Task<IEnumerable<BlogPost>> GetAllBlogPostsAsync()
        {
            return await _blogdbcontext.BlogPosts.ToListAsync();
        }

        public async Task AddAsync(BlogPost blogPost)
        {
            await _blogdbcontext.BlogPosts.AddAsync(blogPost);
            await _blogdbcontext.SaveChangesAsync();
        }

        public async Task UpdateAsync(BlogPost blogPost)
        {
            _blogdbcontext.BlogPosts.Update(blogPost);
            await _blogdbcontext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var blogPost = await GetBlogPostByIdAsync(id);
            _blogdbcontext.BlogPosts.Remove(blogPost);
            await _blogdbcontext.SaveChangesAsync();
        }
    }
}
