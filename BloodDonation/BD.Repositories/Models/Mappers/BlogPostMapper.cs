using BD.Repositories.Models.DTOs.Requests;
using BD.Repositories.Models.DTOs.Responses;
using BD.Repositories.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD.Repositories.Models.Mappers
{
    public static class BlogPostMapper
    {
        public static BlogPost ToEntity(BlogPostRequest blogPostRequest)
        {
            return new BlogPost
            {
                Title = blogPostRequest.Title,
                Content = blogPostRequest.Content,
                AuthorId = blogPostRequest.AuthorId,
                Category = blogPostRequest.Category,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsDeleted = false,
                IsDocument = blogPostRequest.IsDocument ?? false,
                IsPublished = blogPostRequest.IsPublished ?? false,
            };
        }

        public static BlogPostResponse ToResponse(BlogPost blogPost)
        {
            return new BlogPostResponse
            {
                PostId = blogPost.PostId,
                Title = blogPost.Title,
                Content = blogPost.Content,
                AuthorId = blogPost.AuthorId,
                AuthorName = blogPost.Author?.Name,
                CreatedAt = blogPost.CreatedAt,
                UpdatedAt = blogPost.UpdatedAt,
                Category = blogPost.Category,    
                IsDeleted = blogPost.IsDeleted,
                IsPublished = blogPost.IsPublished,
                IsDocument = blogPost.IsDocument
            };
        }
    }
}
