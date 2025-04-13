using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Souq.Api.Dtos.Identity;
using Souq.Api.Helper.Errors;
using Souq.Api.Helper.Extensions;
using Souq.Core.Entites.Identity;
using Souq.Core.Service.Contarct;
using System.Security.Claims;

namespace Souq.Api.Controllers
{

    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public AccountController(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager
            , ITokenService tokenService
            ,IEmailService emailService
            ,IConfiguration configuration,IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _emailService = emailService;
            _configuration = configuration;
            _mapper = mapper;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto model)
        {
            var Email = await _userManager.FindByEmailAsync(model.Email);
            if (Email is not null)
                return BadRequest(new ApiResponse(400, "Email Is Already Esist"));
            var User = new AppUser()
            {
                DisplayName = model.DisplayName,
                Email = model.Email,
                UserName = model.Email.Split('@')[0],
                PhoneNumber = model.PhoneNumber,
            };
            var CreateUser = await _userManager.CreateAsync(User, model.Password);
            if (!CreateUser.Succeeded)
                return BadRequest(new ApiResponse(400));
            var ReturnedUsr = new UserDto()
            {
                DisplayName = User.DisplayName,
                Email = User.Email,
                Token = await _tokenService.CreateTokenAsyn(User)
            };
            return Ok(ReturnedUsr);
        }

        [HttpPost("LogIn")]
        public async Task<ActionResult<UserDto>> LogIn(LogInDto model)
        {
            var User = await _userManager.FindByEmailAsync(model.Email);
            if (User is null)
                return NotFound(new ApiResponse(404, "Not Found User"));
            var UserLoging = await _signInManager.CheckPasswordSignInAsync(User, model.Password, false);
            if (!UserLoging.Succeeded)
                return Unauthorized(new ApiResponse(401, "Username Or Password is UnCorrect"));
            var returnUser = new UserDto()
            {
                DisplayName = User.DisplayName,
                Email = User.Email,
                Token = await _tokenService.CreateTokenAsyn(User)
            };
            return returnUser;
           
        }

        [HttpPost("forgetpassword")]
        public async Task<IActionResult> ForgetPassword(ForgotPasswordDto forgotPasswordDto)
        {
            var user = await _userManager.FindByEmailAsync(forgotPasswordDto.Email);
            if (user == null)
            {
                return BadRequest("No user found with the provided email.");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetLink = Url.Action("resetpassword", "Account", new { token, email = user.Email }, Request.Scheme);

            await _emailService.SendEmailAsync(user.Email, "Password Reset", $"Click on the link to reset your password: {resetLink}");

            return Ok(new { status = "success", message = "Password reset link sent to your email." });
        }

        [HttpPost("resetpassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);
            if (user == null)
            {
                return BadRequest("Invalid email address.");
            }

            var result = await _userManager.ResetPasswordAsync(user, resetPasswordDto.Token, resetPasswordDto.NewPassword);
            if (!result.Succeeded)
            {
                return BadRequest(new { message = "Failed to reset password.", errors = result.Errors });
            }

            return Ok(new { status = "success", message = "Password reset successfully." });
        }

        [HttpGet("CurrentUser")]
        [Authorize]
        public async Task<ActionResult<UserDto>> GetUser()
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(Email!);
            var ReturnedUser = new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _tokenService.CreateTokenAsyn(user)
            };
            return Ok(ReturnedUser);

        }
        [HttpGet("address")]
        [Authorize]
        public async Task<ActionResult<UserAddress>> GetUserAddress()
        {

             var user=await _userManager.FindUserWithAddressAsync(User);
            var MappedAddress = _mapper.Map<UserAddress,AddressIdentityDto>(user.Address);
            return Ok(MappedAddress);
        }

        [HttpPut("address")]
        [Authorize]
        public async Task<ActionResult<AddressIdentityDto>> UpdateUserAddress(AddressIdentityDto address)
        {

            
            var user = await _userManager.FindUserWithAddressAsync(User);
             
            var MappedAddress = _mapper.Map<AddressIdentityDto, UserAddress>(address);
            MappedAddress.Id = user.Address.Id;
            user.Address = MappedAddress;
           var UpdateAddress= await _userManager.UpdateAsync(user);
            if (!UpdateAddress.Succeeded)
                return BadRequest(new ApiResponse(400, "Error Where Update Address"));
            return Ok(UpdateAddress);
        }



    }
}
