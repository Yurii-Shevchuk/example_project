using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using api.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace api.Models
{
    public class Comment
    {
        public int CommentId { get; set; }
        public int RecipeId { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; }
        public string Text { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public Recipe Recipe { get; set; }
    }
}