using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.CommentDtos
{
    public class CreateComment
    {
        [Required]
        public int RecipeId { get; set; }
        [Required]
        [MaxLength(280, ErrorMessage = "Comment cannot be over 280 characters")]
        public string Text { get; set; }

    }
}