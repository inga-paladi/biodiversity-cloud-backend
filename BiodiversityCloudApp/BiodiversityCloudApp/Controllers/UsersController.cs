using AutoMapper;
using BiodiversityCloudApp.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BiodiversityCloudApp.DTOs;

namespace BiodiversityCloudApp.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        
        public UsersController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        // GET: api/users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            var users = await _userRepository.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<UserDto>>(users));
        }

        // GET: api/users/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUser(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }
            return Ok(_mapper.Map<UserDto>(user));
        }

        // POST: api/users (Create User)
        [HttpPost]
        public async Task<ActionResult<UserDto>> CreateUser(UserDto userDto)
        {
            var user = _mapper.Map<User>(userDto);
            user.Id = Guid.NewGuid();

            user.Role ??= "User"; // Default role if none provided

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.PasswordHash); // Hash password before saving

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            var createdUserDto = _mapper.Map<UserDto>(user);
            return CreatedAtAction(nameof(GetUser), new { id = createdUserDto.Id }, createdUserDto);
        }
        // PUT: api/users/{id} (Update User)
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, UserDto userDto)
        {
            if (id != userDto.Id)
            {
                return BadRequest(new { message = "User ID mismatch" });
            }

            var existingUser = await _userRepository.GetByIdAsync(id);
            if (existingUser == null)
            {
                return NotFound(new { message = "User not found" });
            }

            _mapper.Map(userDto, existingUser);
            await _userRepository.UpdateAsync(existingUser);
            await _userRepository.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/users/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();

            return NoContent();
        }
    }

}
