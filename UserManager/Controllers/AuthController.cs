using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NETCore.MailKit.Core;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UserManager.Data;
using UserManager.Dtos;
using UserManager.Models;
using UserManager.Profiles;


namespace UserManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _jwtSettings;
        private readonly IEmailService _emailService;
        private readonly SignInManager<User> _signInManager;

        public AuthController(IMapper mapper,
                            UserManager<User> userManager,
                            IConfiguration jwtSettings,
                            IEmailService emailService,
                            SignInManager<User> signInManager)
        {
            _mapper = mapper;
            _userManager = userManager;
            _jwtSettings = jwtSettings;
            _emailService = emailService;
            _signInManager = signInManager;
        }

        [HttpPost("Register")]
        public async Task<ActionResult> Register([FromBody] UserRegisterDto userRegisterDto)
        {
            var user = _mapper.Map<User>(userRegisterDto);
            if (user != null && await _userManager.CheckPasswordAsync(user, userRegisterDto.Password))
            {                
                //Generate email token
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                //Confirm email
                var result = await _userManager.ConfirmEmailAsync(user, code);
                //If success
                if (result.Succeeded)
                {
                    await _userManager.CreateAsync(user, userRegisterDto.Password);
                    await _userManager.AddToRoleAsync(user, "Visitor");                    
                    return Ok("Registered !");
                }
            }
            return BadRequest("Cant create new account for registering !");
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto userLoginDto)
        {
            var user = await _userManager.FindByEmailAsync(userLoginDto.Email);
            if (user != null && await _userManager.CheckPasswordAsync(user, userLoginDto.Password))
            {
                //Sign in
                await _signInManager.PasswordSignInAsync(user, userLoginDto.Password, false, false);
                //Generate Token
                var generateTokens = new TokenSignIn(_jwtSettings, _userManager);
                var signingCredentials = generateTokens.GetSigningCredential();
                var claims = generateTokens.GetClaim(user);
                var tokenOptions = generateTokens.GenerateTokenOptions(signingCredentials, await claims);
                var token = new JwtSecurityTokenHandler();
                token.WriteToken(tokenOptions);

                return Ok(_mapper.Map<UserReadDto>(user));
            }
            return Unauthorized("Invalid Authentication");
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok("Log out !");
        }

        [HttpPost("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword(UserVerifyByEmailDto userVerifyByEmail)
        {
            var user = await _userManager.FindByEmailAsync(userVerifyByEmail.Email);
            if (user != null)
            {
                //Generate Token
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                return Ok(code);
            }
            return BadRequest("Invalid user !");
        }

        [HttpPost("ReesetPasswordVerifyEmail")]
        public async Task<IActionResult> ReesetPasswordVerifyEmail(UserVerifyByEmailDto userVerifyByEmailDto,
            UserResetPasswordDto userResetPasswordDto)
        {
            //Find the user who verify email by ID
            var user = await _userManager.FindByEmailAsync(userVerifyByEmailDto.Email);
            if (user == null)
                return BadRequest();

            //Confirm token
            IdentityResult result = await _userManager.ResetPasswordAsync(user,
                userVerifyByEmailDto.code,
                userResetPasswordDto.Password);

            if (result.Succeeded)
            {
                return Ok("Changed Password succesfully !");
            }
            return BadRequest("Cant verify email !");
        }
    }
}
