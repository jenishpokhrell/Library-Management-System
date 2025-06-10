using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperLearn.DTOs.Conversation
{
    public class CreateConversationDto
    {
        //public int userId { get; set; }
        public string subject { get; set; }
        public string initialMessage { get; set; }
    }
}
