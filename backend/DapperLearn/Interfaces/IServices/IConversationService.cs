using DapperLearn.DTOs.Conversation;
using DapperLearn.DTOs.Message;
using DapperLearn.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DapperLearn.Interfaces.IServices
{
    public interface IConversationService
    {
        Task<int> CreateConversationAsync(ClaimsPrincipal User, CreateConversationDto dto);
        Task SendMessageAsync(ClaimsPrincipal User, SendMessageDto dto, int conversationId);
        Task<IEnumerable<Message>> GetMessagesByConversationIdAsync(ClaimsPrincipal User, int conversationId);
    }
}
