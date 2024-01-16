using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using api.Dtos.CommentDtos;
using api.Helpers;
using api.Models;

namespace api.Interfaces
{
    public interface ICommentRepository
    {
        Task<List<Comment>> GetAll(CommentQueryObject query);
        Task<Comment> GetById(int id);
        Task<Comment> Create(Comment comment);
        Task<Comment> Update(int id, UpdateComment updateComment, string userId);
        Task<Comment> DeleteComment(int id, string userId);

        Task<bool> RecipeExists(int id);
    }
}