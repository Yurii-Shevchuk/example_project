using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using api.Dtos.CommentDtos;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{   
    [Route("api/comment")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepo;

        public CommentController(ICommentRepository commentRepo)
        {
            _commentRepo = commentRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] CommentQueryObject query)
        {
            var comments = await _commentRepo.GetAll(query);
            var commentsDto = comments.Select(r => r.ToGetCommentFromModel());

            return Ok(commentsDto);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var comment = await _commentRepo.GetById(id);

            if(comment is null)
            {
                return NotFound("Comment not found");
            }

            var formattedComment = comment.ToGetCommentFromModel();

            return Ok(formattedComment);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CreateComment commentDto)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
                
            if(!await _commentRepo.RecipeExists(commentDto.RecipeId))
            {
                return BadRequest("You cannot comment on recipe that does not exist");
            }

            var comment = commentDto.ToCommentFromCreateDto(User.Identity.Name, User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            await _commentRepo.Create(comment);

            return CreatedAtAction(nameof(GetById), new {id = comment.CommentId}, comment.ToGetCommentFromModel());
        }

        [HttpDelete]
        [Authorize]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var commentToDelete = await _commentRepo.DeleteComment(id, User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            if(commentToDelete is null)
            {
                return BadRequest();
            }

            return NoContent();
        }

        [HttpPut]
        [Authorize]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateComment updateDTO)
        {
            var commentToUpdate = await _commentRepo.Update(id, updateDTO, User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            if(commentToUpdate is null)
            {
                return BadRequest();
            }

            return Ok(commentToUpdate.ToGetCommentFromModel());
        }
        
    }
}