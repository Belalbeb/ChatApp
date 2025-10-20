using System.ComponentModel.DataAnnotations;

namespace ChatApi.Dto
{
    public class MessageDto
    {
        [Required]
        public string From { get; set; }
        public string? To { get; set; }
        [Required]
        public string Content { get; set; }
        public  TypeOfMessage ?Type { get; set; }
    }
    public enum TypeOfMessage
    {
        text=1,
        voice=2
    }
}
