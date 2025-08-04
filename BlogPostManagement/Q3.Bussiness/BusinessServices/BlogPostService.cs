using AutoMapper;

using Microsoft.AspNetCore.Http;

using Q3.Business.IBusinessServices;
using Q3.Data.Entities;
using Q3.Data.IRepository;
using Q3.Shared.DTO.MainData;

using System.Security.Claims;


namespace Q3.Business.BusinessServices
{
    public class BlogPostService : IBlogPostService
    {
        private readonly IBlogPostRepository _blogPostRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public BlogPostService(IBlogPostRepository blogPostRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _blogPostRepository = blogPostRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<BlogPostDto> GetBlogPostByIdAsync(int id)
        {
            var blogPost = await _blogPostRepository.GetBlogPostByIdAsync(id);
            if (blogPost == null)
            {
                throw new KeyNotFoundException("Blog post not found.");
            }
            //return new BlogPostDto
            //{
            //    Id = blogPost.Id,
            //    Title = blogPost.Title,
            //    Author = blogPost.Author,
            //    Content = blogPost.Content,
            //    CreatedAt = blogPost.CreatedAt,
            //    UpdatedAt = blogPost.UpdatedAt,
            //    IsPublished= blogPost.IsPublished
            //};
            return _mapper.Map<BlogPostDto>(blogPost);

        }

        public async Task<IEnumerable<BlogPostDto>> GetAllBlogPostsAsync()
        {
            var blogPosts = await _blogPostRepository.GetAllBlogPostsAsync();
            return _mapper.Map<IEnumerable<BlogPostDto>>(blogPosts);
        }

        public async Task<BlogPostDto> CreateBlogPostAsync(BlogPostDto blogPostDto)
        {

            var blogPost = _mapper.Map<BlogPost>(blogPostDto);
            blogPost.CreatedAt = DateTime.UtcNow;
            blogPost.UpdatedAt = DateTime.UtcNow;

            // Accessing current user id from JWT claims
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                throw new UnauthorizedAccessException("User ID not found in token.");
            }
            blogPost.UserId = int.Parse(userIdClaim.Value);

            await _blogPostRepository.AddAsync(blogPost);
            return _mapper.Map<BlogPostDto>(blogPost);
        }

        public async Task UpdateBlogPostAsync(int id, BlogPostDto blogPostDto)
        {
            var previousBlogPost = await _blogPostRepository.GetBlogPostByIdAsync(id);
            if (previousBlogPost == null)
            {
                throw new KeyNotFoundException("Blog post not found.");
            }

            _mapper.Map(blogPostDto, previousBlogPost);
            previousBlogPost.UpdatedAt = DateTime.UtcNow;

            await _blogPostRepository.UpdateAsync(previousBlogPost);
        }

        public async Task DeleteBlogPostAsync(int id)
        {
            var blogPost = await _blogPostRepository.GetBlogPostByIdAsync(id);
            if (blogPost == null)
            {
                throw new KeyNotFoundException("Blog post not found.");
            }
            await _blogPostRepository.DeleteAsync(id);

        }


    }
}
