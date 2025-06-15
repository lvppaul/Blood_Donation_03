using BD.Repositories.Models.DTOs.Requests;
using BD.Repositories.Models.DTOs.Responses;
using BD.Repositories.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD.Services.Interfaces
{
    public interface IBlogPostService
    {
        Task<IEnumerable<BlogPostResponse>> GetAllPostsAsync();
        Task<BlogPostResponse?> GetPostByIdAsync(int id);
        Task<BlogPostResponse> AddAsync(BlogPostRequest post);
        Task<BlogPostResponse?> UpdatePostAsync(int id, BlogPostRequest post);
        Task DeletePostAsync(int id);
    }
}
