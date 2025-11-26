using Microsoft.AspNetCore.Mvc;
using Application.Services;
using Domain.Entities;
using Application.Interfaces.Services;
using Application.DTOs.User;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public ActionResult<List<UserDto>> GetAllUsers()
        {
            var users = _userService.GetAllUsers();
            var dto = users.Select(u => new UserDto
            {
                Id = u.Id,
                Email = u.Email,
                FullName = u.FullName,
                IsAdmin = u.IsAdmin
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
                FullName = user.FullName,
                IsAdmin = user.IsAdmin
            };
        }

        [HttpPost]
        public ActionResult<UserDto> CreateUser([FromBody] CreateUserDto dto)
        {
            var user = _userService.CreateUser(dto.Email, dto.Password, dto.FullName, dto.IsAdmin);

            return CreatedAtAction(nameof(GetById), new { id = user.Id }, new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                IsAdmin = user.IsAdmin,
            });
        }

        [HttpPut("{id}")]
        public IActionResult UpdateUser(Guid id, [FromBody] CreateUserDto dto)
        {
            var user = _userService.GetUserById(id);
            if (user == null) return NotFound();

            user.Email = dto.Email;
            user.PasswordHash = dto.Password;
            user.FullName = dto.FullName;
            user.IsAdmin = dto.IsAdmin;
            _userService.UpdateUser(user);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(Guid id)
        {
            var user = _userService.GetUserById(id);
            if (user == null) return NotFound();

            _userService.DeleteUser(user);
            return NoContent();
        }
    }
}
