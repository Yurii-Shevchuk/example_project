using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.CommentDtos;
using api.Models;

namespace api.Mappers
{
    public static class CommentMappers
    {
        public static GetComment ToGetCommentFromModel(this Comment comment)
        {
            return new GetComment
            {
                CommentId = comment.CommentId,
                RecipeId = comment.RecipeId,
                UserName = comment.UserName,
                Text = comment.Text,
                CreatedOn = comment.CreatedOn
            };
        }

        public static Comment ToCommentFromCreateDto(this CreateComment comment, string username, string userId)
        {
            return new Comment
            {
                UserId = userId,
                UserName = username,
                Text = comment.Text,
                RecipeId = comment.RecipeId
            };
        }
        
    }
}