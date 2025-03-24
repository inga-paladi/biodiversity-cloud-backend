using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BiodiversityCloudApp.Repositories;
using BiodiversityCloudApp.DTOs;
using AutoMapper;

namespace BiodiversityCloudApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IObservationRepository _observationRepository;
        private readonly IMapper _mapper;

        public CommentController(ICommentRepository commentRepository, IObservationRepository observationRepository, IMapper mapper)
        {
            _commentRepository = commentRepository;
            _observationRepository = observationRepository;
            _mapper = mapper;
        }

        // POST: api/comment/{observationId}
        [HttpPost("{observationId}")]
        public async Task<IActionResult> AddComment(Guid observationId, [FromBody] CommentDto commentDto)
        {
            var observation = await _observationRepository.GetByIdAsync(observationId);
            if (observation == null)
                return NotFound(new { message = "Observation not found." });

            var comment = _mapper.Map<Comment>(commentDto);
            comment.Id = Guid.NewGuid();
            comment.ObservationId = observationId;
            comment.CreatedAt = DateTime.UtcNow;

            await _commentRepository.AddAsync(comment);
            return Ok(new { message = "Comment added successfully", comment = _mapper.Map<CommentDto>(comment) });
        }

        // GET: api/comment/{observationId}
        [HttpGet("{observationId}")]
        public async Task<IActionResult> GetComments(Guid observationId)
        {
            var comments = await _commentRepository.GetByObservationIdAsync(observationId);
            return Ok(_mapper.Map<IEnumerable<CommentDto>>(comments));
        }

        // DELETE: api/comment/{commentId}
        [HttpDelete("{commentId}")]
        public async Task<IActionResult> DeleteComment(Guid commentId)
        {
            var comment = await _commentRepository.GetByIdAsync(commentId);
            if (comment == null)
                return NotFound(new { message = "Comment not found." });

            await _commentRepository.DeleteAsync(comment);
            await _commentRepository.SaveChangesAsync();

            return Ok(new { message = "Comment deleted successfully." });
        }

        // PUT: api/comment/{commentId}
        [HttpPut("{commentId}")]
        public async Task<IActionResult> UpdateComment(Guid commentId, [FromBody] CommentDto updatedDto)
        {
            var comment = await _commentRepository.GetByIdAsync(commentId);
            if (comment == null)
                return NotFound(new { message = "Comment not found." });

            comment.Text = updatedDto.Text;
            comment.UpdatedAt = DateTime.UtcNow;

            await _commentRepository.UpdateAsync(comment);
            await _commentRepository.SaveChangesAsync();

            return Ok(new { message = "Comment updated successfully", comment = _mapper.Map<CommentDto>(comment) });
        }

    }
}
