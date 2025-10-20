using ChatApi.Dto;
using ChatApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChatApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly ChatServices chatServices;

        public ChatController(ChatServices chatServices)
        {
            this.chatServices = chatServices;
        }
        [HttpPost("AddUser")]
        public IActionResult RegisterUser(UserDto userDto)
        {
            if (chatServices.AddUserToList(userDto.Name))
            {
                return Ok();
            }
            return BadRequest("the name is choosen");
        }
        //voice
        [HttpPost("upload")]
        public async Task<IActionResult> UploadVoice(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded");

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "voices");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var fileUrl = $"{Request.Scheme}://{Request.Host}/voices/{fileName}";
            return Ok(new { url = fileUrl });
        }

    }
}
