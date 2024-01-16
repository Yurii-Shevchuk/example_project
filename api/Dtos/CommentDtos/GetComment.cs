using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.CommentDtos
{
    public class GetComment
    {
        public int CommentId { get; set; }
        public int RecipeId { get; set; }
        public string UserName { get; set; }
        public string Text { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}