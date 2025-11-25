using Microsoft.AspNetCore.Mvc;
using Application.Services;
using Domain.Entities;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public ActionResult<List<UserDto>> GetAll()
        {
            var users = _userService.GetAllUsers();
            var dto = users.Select(u => new UserDto
            {
                Id = u.Id,
                Email = u.Email,
                IsAdmin = u.IsAdmin,
                TenantId = u.TenantId
            }).ToList();
            return Ok(dto);
        }

        [HttpGet("{id}")]
        public ActionResult<UserDto> GetById(Guid id)
        {
            var user = _userService.GetUserById(id);
            if (user == null) return NotFound();

            return new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                IsAdmin = user.IsAdmin,
                TenantId = user.TenantId
            };
        }

        [HttpPost]
        public ActionResult<UserDto> Register([FromBody] RegisterUserDto dto)
        {
            var user = _userService.RegisterUser(dto.Email, dto.IsAdmin);

            return CreatedAtAction(nameof(GetById), new { id = user.Id }, new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                IsAdmin = user.IsAdmin,
                TenantId = user.TenantId
            });
        }

        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromBody] RegisterUserDto dto)
        {
            var user = _userService.GetUserById(id);
            if (user == null) return NotFound();

            user.Email = dto.Email;
            user.IsAdmin = dto.IsAdmin;
            _userService.UpdateUser(user);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var user = _userService.GetUserById(id);
            if (user == null) return NotFound();

            _userService.DeleteUser(user);
            return NoContent();
        }
    }
}
