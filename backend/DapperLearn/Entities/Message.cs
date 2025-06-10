using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperLearn.Entities
{
    public class Message
    {
        public int messageId { get; set; }
        public int conversationId { get; set; }
        public int senderId { get; set; }
        public string message { get; set; }
        public DateTime sentAt { get; set; }
        public string sender { get; set; }
    }
}
