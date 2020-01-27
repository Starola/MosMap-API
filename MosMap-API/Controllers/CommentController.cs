using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MosMap_API.Dtos;
using MosMap_API.Models;
using MosMap_API.ServiceInterfaces;

namespace MosMap_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _service;
        private IMapper _mapper;
        private IUserService _uservice;

        public CommentController(IConfiguration config, ICommentService service, IMapper mapper, IUserService uservice)
        {
            _service = service;
            _mapper = mapper;
            _uservice = uservice;
        }

        [HttpGet("location/{locationid}")]
        public async Task<IActionResult> GetCommentsByLocationId(int locationId)
        {
            try
            {
                // returns all comments from database
                IEnumerable<CommentDto> commentsResult = await _service.GetCommentsByLocaitonId(locationId);

                // Ok = status code 200
                return Ok(commentsResult);
            }
            catch (Exception ex)
            {
                // Something went wrong inside GetAllComments action: {ex.Message}
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCommentsById(int id)
        {
            try
            {
                Comment comment = await _service.GetCommentById(id);

                if (comment == null)
                {
                    // Comment with id hasn't been found in db
                    return NotFound();
                }
                else
                {
                    CommentDto commentResult = _mapper.Map<CommentDto>(comment);
                    commentResult.UserName = _uservice.GetUserName();
                    return Ok(commentResult);
                }
            }
            catch (Exception ex)
            {
                // Something went wrong inside GetCategoryById action: {ex.Message}
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpPost]
        public async Task<IActionResult> CreateComment([FromBody] CommentForCreationDto commentDto)
        {
            try
            {
                if (commentDto == null)
                {
                    // comment object sent from client is null
                    return BadRequest("Comment object is null");
                }

                if (!ModelState.IsValid)
                {
                    // Invalid comment object sent from client
                    return BadRequest("Invalid model object");
                }

                Comment createdComment = await _service.CreateComment(commentDto);

                return Ok();
            }
            catch (Exception ex)
            {
                //Something went wrong inside CreateComment action
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [Authorize(Roles = "administrator")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            try
            {
                Comment commentEntity = await _service.GetCommentById(id);
                if (commentEntity == null)
                {
                    // Comment with id hasn't been found in db
                    return NotFound();
                }
                _service.DeleteComment(commentEntity);

                return NoContent();
            }
            catch (Exception ex)
            {
                // Something went wrong inside DeleteComment action
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


    }
}