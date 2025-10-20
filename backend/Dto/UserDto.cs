using System.ComponentModel.DataAnnotations;

namespace ChatApi.Dto
{
    public class UserDto
    {
        public int Id { get; set; }
        [Required]
        [MinLength(3,ErrorMessage ="the Lenth Must be greater than 2 character")]
        public string Name { get; set; }


    }
}
