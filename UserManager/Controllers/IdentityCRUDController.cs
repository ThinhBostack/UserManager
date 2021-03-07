using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManager.Dtos;
using UserManager.Models;

namespace UserManager.Controllers
{
    //[Authorize(Roles = "Administrator")]
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityCRUDController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public IdentityCRUDController(UserManager<User> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        //Add User
        [HttpPost("AddUSer")]
        public async Task<IActionResult> AddUser([FromBody] UserRegisterDto userRegisterDto)
        {
            var user = _mapper.Map<User>(userRegisterDto);

            IdentityResult result = await _userManager.CreateAsync(user, userRegisterDto.Password);
            await _userManager.AddToRoleAsync(user, "Visitor");
            if (result.Succeeded)
                return Ok(_mapper.Map<UserReadDto>(user));
            return BadRequest("Cant add User!");
        }

        //Add Admin
        [HttpPost("AddAdmin")]
        public async Task<IActionResult> AddAdmin([FromBody] UserRegisterDto userRegisterDto)
        {
            var user = _mapper.Map<User>(userRegisterDto);

            IdentityResult result = await _userManager.CreateAsync(user, userRegisterDto.Password);
            await _userManager.AddToRoleAsync(user, "Administrator");
            if (result.Succeeded)
                return Ok(_mapper.Map<UserReadDto>(user));
            return BadRequest("Cant add Admin!");
        }

        //Update CustomId, FullName, Gender
        [HttpPut("UpdateUSer")]
        public async Task<IActionResult> UpdateUser([FromBody] UserUpdateDto userUpdateDto)
        {
            var user = await _userManager.FindByEmailAsync(userUpdateDto.Email);
            if (user != null)
            {                
                user.CustomId = userUpdateDto.CustomId;
                user.FullName = userUpdateDto.FullName;
                user.Gender = userUpdateDto.Gender;

                IdentityResult _result = await _userManager.UpdateAsync(user);
                if (_result.Succeeded)
                {
                    return Ok(_mapper.Map<UserReadDto>(user));
                }                
            }            
            return BadRequest("Cant update !");
        }

        //Delete
        [HttpDelete("DeleteUSer")]
        public async Task<IActionResult> DeleteUser(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }
            return BadRequest("Cant delete !");
        }

    }
}
