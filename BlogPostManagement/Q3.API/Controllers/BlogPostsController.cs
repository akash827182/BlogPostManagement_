using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Q3.Business.IBusinessServices;
using Q3.Shared.DTO.MainData;
using Q3.Shared.Helpers;

using System.Security.Claims;

namespace Q3.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BlogPostsController : ApiBaseController
    {
        private readonly IBlogPostService _blogPostService;


        public BlogPostsController(IBlogPostService blogPostService)
        {
            _blogPostService = blogPostService;
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BlogPostDto>> GetBlogPost(int id)
        {
            var blogPost = await _blogPostService.GetBlogPostByIdAsync(id);
            return Ok(blogPost);
        }

        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllBlogPosts()
        {
            var blogPosts = await _blogPostService.GetAllBlogPostsAsync();
            if (!blogPosts.Any())
                return NotFound("No blog posts found.");

            return Ok(blogPosts);
        }
        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateBlogPost([FromBody] BlogPostDto blogPostDto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized("User ID not found in token.");

            blogPostDto.UserId = int.Parse(userIdClaim.Value);

            var createdBlogPost = await _blogPostService.CreateBlogPostAsync(blogPostDto);
            return CreatedAtAction(nameof(GetBlogPost), new { id = createdBlogPost.Id }, createdBlogPost);
        }
        
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateBlogPost(int id, [FromBody] BlogPostDto blogPostDto)
        {
            var blog = await _blogPostService.GetBlogPostByIdAsync(id);
            if (blog == null) return NotFound();

            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (string.IsNullOrEmpty(currentUserId))
                return Unauthorized("User ID not found in token.");

            if (!(currentUserRole == "Admin" || blog.UserId.ToString() == currentUserId))
                return Forbid("You are not authorized to update this blog post.");


            await _blogPostService.UpdateBlogPostAsync(id, blogPostDto);
            return Ok("Update Successful");
        }
        
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteBlogPost(int id)
        {
            var blog = await _blogPostService.GetBlogPostByIdAsync(id);
            if (blog == null) return NotFound();

            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (string.IsNullOrEmpty(currentUserId))
                return Unauthorized("User ID not found in token.");

            if (!(currentUserRole == "Admin" || blog.UserId.ToString() == currentUserId))
                return Forbid("You are not authorized to delete this blog post.");

            await _blogPostService.DeleteBlogPostAsync(id);
            return NoContent();
        }
    }
}
