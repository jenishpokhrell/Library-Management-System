using Dapper;
using DapperLearn.Context;
using DapperLearn.DTOs.Conversation;
using DapperLearn.DTOs.Message;
using DapperLearn.Entities;
using DapperLearn.Interfaces.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DapperLearn.Repositories
{
    public class ConversationRepository : IConversationRepository
    {
        private readonly DapperContext _dbo;

        public ConversationRepository(DapperContext dbo)
        {
            _dbo = dbo;
        }

        public async Task<int> CreateConversation(ClaimsPrincipal User, CreateConversationDto createConversationDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var query1 = "INSERT INTO Conversation (userId, subject) VALUES (@userId, @subject) SELECT CAST(SCOPE_IDENTITY() AS INT)";

            var query2 = "INSERT INTO Message (conversationId, senderId, message) VALUES (@conversationId, @senderId, @message)";

            using(var connection = _dbo.CreateConnection())
            {
                var conversationId = await connection.ExecuteScalarAsync<int>(query1, new { userId, createConversationDto.subject });
                await connection.ExecuteAsync(query2, new { conversationId, senderId = userId, message = createConversationDto.initialMessage });

                return conversationId;
            }
        }

        public async Task SendMessage(ClaimsPrincipal User, SendMessageDto sendMessageDto, int conversationId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var query = "INSERT INTO Message (conversationId, senderId, message) VALUES (@conversationId, @senderId, @message)";

            using (var connection = _dbo.CreateConnection())
            {
                await connection.ExecuteAsync(query, new { 
                    conversationId = conversationId,
                    senderId = userId,
                    message = sendMessageDto.message
                } );
            }
        }

        public async Task<IEnumerable<Message>> GetMessagesByConversationIdAsync(ClaimsPrincipal User, int conversationId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var role = User.FindFirstValue(ClaimTypes.Role);

            var checkQuery = "SELECT userId FROM Conversation WHERE conversationId = @conversationId";

            var query = "SELECT m.messageId, m.conversationId, m.senderId, m.message, u.name AS sender, m.sentAt FROM Message m " +
                "INNER JOIN Users u ON m.senderId = u.userId " +
                "WHERE m.conversationId = @conversationId " +
                "ORDER BY SentAt ASC";

            using(var connection = _dbo.CreateConnection())
            {
                var conversationOwnerId = await connection.QueryFirstOrDefaultAsync<int?>(checkQuery, new { conversationId });

                if(conversationOwnerId == null)
                {
                    throw new Exception("Conversation not Found");
                }

                if(role != "Admin" && conversationOwnerId != userId)
                {
                    throw new UnauthorizedAccessException("You are not authorized to access this conversation.");
                }

                return await connection.QueryAsync<Message>(query, new { conversationId });
            }
        }
    }
}
