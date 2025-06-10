using DapperLearn.DTOs.Conversation;
using DapperLearn.DTOs.Message;
using DapperLearn.Entities;
using DapperLearn.Interfaces.IRepositories;
using DapperLearn.Interfaces.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DapperLearn.Services
{
    public class ConversationService : IConversationService
    {
        private readonly IConversationRepository _conversationRepository;

        public ConversationService(IConversationRepository conversationRepository)
        {
            _conversationRepository = conversationRepository;
        }

        public Task<int> CreateConversationAsync(ClaimsPrincipal User, CreateConversationDto createConversationDto)
        {
            return _conversationRepository.CreateConversation(User, createConversationDto);
        }

        public Task SendMessageAsync(ClaimsPrincipal User, SendMessageDto sendMessageDto, int conversationId)
        {
            return _conversationRepository.SendMessage(User, sendMessageDto, conversationId);
        }

        public Task<IEnumerable<Message>> GetMessagesByConversationIdAsync(ClaimsPrincipal User, int conversationId)
        {
            return _conversationRepository.GetMessagesByConversationIdAsync(User, conversationId);
        }
    }
}
