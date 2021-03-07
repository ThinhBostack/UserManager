using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManager.Data;
using UserManager.Dtos;
using UserManager.Models;

namespace UserManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CRUDController : ControllerBase
    {
        private readonly IUserRepo _userRepo;
        //private readonly UserManager<User> _userRepo;
        private readonly IMapper _mapper;        

        public CRUDController(IUserRepo userRepo, IMapper mapper)
        {
            _userRepo = userRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<UserReadDto>> GetAllUser()
        {
            var userList = _userRepo.GetAllUser();
            if (userList != null)
                return Ok(_mapper.Map<IEnumerable<UserReadDto>>(userList));
            return NotFound("Cant get all !");
        }

        [HttpGet("{id}", Name="GetUserByID")]
        public ActionResult<UserReadDto> GetUserByID(string Id)
        {            
            var user = _userRepo.GetUserByID(Id.ToString());
            if (user != null)
                return Ok(_mapper.Map<UserReadDto>(user));
            return NotFound("Cant get by Id !");
        }

        [HttpPost]
        public ActionResult<UserReadDto> AddUser([FromBody] UserAddDto userAddDto)
        {
            var user = _mapper.Map<User>(userAddDto);
            if (_userRepo.AddUser(user) != null)
            {                
                var userReadDto =  _mapper.Map<UserReadDto>(user);
                /*return CreatedAtRoute(nameof(GetUserByID), 
                    new { Id = userReadDto.Id}, 
                    userReadDto);*/
                return Ok(userReadDto);
            }
            return BadRequest("Cant add !");
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteUser(int Id)
        {
            var user = _userRepo.GetUserByID(Id.ToString());
            if (_userRepo.DeleteUser(user))
            {
                return Ok($"User with Id: {Id} is deleted !");
            }
            return NotFound("Cant delete !");
        }

        [HttpPut("{id}")]
        public ActionResult<UserReadDto> UpdateUser(int Id, [FromBody] UserUpdateDto userUpdateDto)
        {
            var user = _userRepo.GetUserByID(Id.ToString());
            if (user == null)
            {
                return NotFound("Cant update !");                
            }            
            _mapper.Map(userUpdateDto, user);
            _userRepo.UpdateUser(user);
            return Ok(_mapper.Map<UserReadDto>(user));
            /*var userReadDto = _mapper.Map<UserReadDto>(user);
            return CreatedAtRoute(nameof(GetUserByID),
                    new { Id = user.Id },
                    userReadDto);*/
        }
    }
}
