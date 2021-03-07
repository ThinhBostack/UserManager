using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserManager.Dtos;
using UserManager.Models;

namespace UserManager.Profiles
{
    public class AuthProfile : Profile
    {
        //This class is for maping
        public AuthProfile()
        {
            CreateMap<UserRegisterDto, User>()
                .ForMember(u => u.UserName, opt => opt.MapFrom(x => x.Email));
            CreateMap<UserLoginDto, User>()
                .ForMember(u => u.UserName, opt => opt.MapFrom(x => x.Email));            
        }

    }
}
