using Puissance4.API.Enums;

namespace Puissance4.API.DTO
{
    public class MessageDTO
    {
        public MessageDTO(string content, Severity severity)
        {
            Content = content;
            Severity = severity;
        }

        public MessageDTO(string content, Severity severity, bool sticky)
        {
            Content = content;
            Severity = severity;
            Sticky = sticky;
        }

        public string Content { get; set; } = string.Empty;
        public Severity Severity { get; set; }
        public bool Sticky { get; set; }
    }
}
