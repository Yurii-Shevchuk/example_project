using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.CommentDtos;
using api.Helpers;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IAuthService _authService;
        private readonly ILogger<CommentRepository> _logger;

        public CommentRepository(AppDbContext context, 
        UserManager<AppUser> userManager,
        IAuthService authService,
        ILogger<CommentRepository> logger)
        {
            _context = context;
            _userManager = userManager;
            _authService = authService;
            _logger = logger;
        }
        public async Task<Comment> Create(Comment comment)
        {
            if(await _authService.IsRealUser(comment.UserName))
            {
                await _context.Comments.AddAsync(comment);
                await _context.SaveChangesAsync();
                return comment;
            }
            return null;
        }
    
        public async Task<Comment> DeleteComment(int id, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var comment = await GetById(id);
            if(comment is null)
            {
                return null;
            }

            if(comment.UserName != user.UserName)
            {
                return null;
            }

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Comment with id {comment.CommentId} deleted successfully");
            return comment;
        }

        public async Task<List<Comment>> GetAll(CommentQueryObject query)
        {
            var comments = _context.Comments.AsQueryable();

            if(query.SearchByPost != 0 && _context.Recipes.Where(r => r.RecipeId == query.SearchByPost).Any())
            {
                comments = comments.Where(c => c.RecipeId == query.SearchByPost);
            }

            if(!string.IsNullOrWhiteSpace(query.UserName))
            {
                comments = comments.Where(c => c.UserName.ToLower() == query.UserName.ToLower());
            }

            if(query.IsSorted)
            {
                comments = comments.OrderByDescending(c => c.CreatedOn);
            }

            int commentsToSkip = (query.PageNumber - 1) * query.PageSize;

            return await comments.Skip(commentsToSkip).Take(query.PageSize).ToListAsync();
        }

        public async Task<Comment> GetById(int id)
        {
            return await _context.Comments.FirstOrDefaultAsync(c => c.CommentId == id);
        }

        public async Task<Comment> Update(int id, UpdateComment updateComment, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var existingComment = await GetById(id);

            if(existingComment is null)
            {
                return null;
            }

            if(existingComment.UserName != user.UserName)
            {
                return null;
            }
            existingComment.Text = updateComment.Text;
            await _context.SaveChangesAsync();

            return existingComment;
        }

        public async Task<bool> RecipeExists(int id)
        {
            return await _context.Recipes.Where(r => r.RecipeId == id).AnyAsync();
        }
    }
}