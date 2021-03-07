using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManager.Dtos;
using UserManager.Models;

namespace UserManager.Profiles
{
    public class CRUDProfile : Profile
    {
        //This class is for mapping
        public CRUDProfile()
        {
            CreateMap<User, UserReadDto>();
            CreateMap<UserAddDto, User>();
            CreateMap<UserUpdateDto, User>();
        }
    }
}
