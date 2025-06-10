using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperLearn.Entities
{
    public class Conversation
    {
        public int conversationId { get; set; }
        public int userId { get; set; }
        public string subject { get; set; }
        public DateTime createdAt { get; set; }
        public IList<Message> messages { get; set; }
    }
}
