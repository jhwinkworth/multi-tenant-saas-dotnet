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
        public ActionResult<List<UserDto>> GetAll()
        {
            var users = _userService.GetAllUsers();
            var dto = users.Select(u => new UserDto
            {
                Id = u.Id,
                Email = u.Email,
                PasswordHash = u.PasswordHash,
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
                PasswordHash = user.PasswordHash,
                IsAdmin = user.IsAdmin,
                TenantId = user.TenantId
            };
        }

        [HttpPost]
        public ActionResult<UserDto> Create([FromBody] CreateUserDto dto)
        {
            var user = _userService.CreateUser(dto.Email, dto.PasswordHash, dto.IsAdmin);

            return CreatedAtAction(nameof(GetById), new { id = user.Id }, new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                PasswordHash = user.PasswordHash,
                IsAdmin = user.IsAdmin,
                TenantId = user.TenantId
            });
        }

        [HttpPut("{id}")]
        public IActionResult Update(Guid id, [FromBody] CreateUserDto dto)
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
