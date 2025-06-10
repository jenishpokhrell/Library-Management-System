using DapperLearn.DTOs.Conversation;
using DapperLearn.DTOs.Message;
using DapperLearn.Interfaces.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperLearn.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConversationController : ControllerBase
    {
        private readonly IConversationService _conversationService;

        public ConversationController(IConversationService conversationService)
        {
            _conversationService = conversationService;
        }

        [HttpPost]
        [Route("CreateConversation")]
        [Authorize]
        public async Task<IActionResult> CreateConversation(CreateConversationDto createConversationDto)
        {
            var id = await _conversationService.CreateConversationAsync(User, createConversationDto);
            return Ok(new { conversationId = id });
        }

        [HttpPost]
        [Route("Message/{conversationId}")]
        [Authorize]
        public async Task<IActionResult> SendMessage([FromBody] SendMessageDto sendMessageDto, int conversationId)
        {
            await _conversationService.SendMessageAsync(User, sendMessageDto, conversationId);
            return Ok();
        }

        [HttpGet]
        [Route("{conversationId}")]
        [Authorize]
        public async Task<IActionResult> GetMessages(int conversationId)
        {
            var messages = await _conversationService.GetMessagesByConversationIdAsync(User, conversationId);
            return Ok(messages);
        }
    }
}
