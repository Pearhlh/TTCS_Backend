using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using WebApplication1.Model;
using WebApplication1.Service.InheritanceService;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly AppSetting _appSettings;

        public UserController(UserService userService, IOptionsMonitor<AppSetting> optionsMonitor)
        {
            _userService = userService;
            _appSettings = optionsMonitor.CurrentValue;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Validate(LoginModel model)
        {
            return Ok(await _userService.Validate(model));
        }

        [HttpPost("CheckExist")]
        public async Task<IActionResult> CheckExist(String userName)
        {
            return Ok(await _userService.CheckExist(userName));
        }

        [HttpPost("CheckExist/email")]
        public async Task<IActionResult> CheckEmailExist(string email)
        {
            return Ok(await _userService.CheckEmailExist(email));
        }

        [HttpPost("CheckExist/reclaim")]
        public async Task<IActionResult> reclaimCheckExist(string reclaimInfo)
        {

            return Ok(await _userService.CheckReclaimExist(reclaimInfo));
        }

        [HttpPost("GoogleSignIn")]
        public async Task<IActionResult> GoogleSignIn(UserModel model)
        {
            return Ok(await _userService.GoogleSignIn(model));
        }

        [HttpPost("FacebookSignIn")]
        public async Task<IActionResult> FacebookSignIn(UserModel model)
        {
            return Ok(await _userService.FacebookSignIn(model));
        }


        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn(UserModel model)
        {
            return Ok(await _userService.SignIn(model));
        }

        [HttpPost("ReclaimReq")]
        public async Task<IActionResult> ReclaimReq(UserReClaimModel model)
        {
            return Ok(await _userService.Reclaim(model));
        }

        [Authorize]
        [HttpPost("Update")]
        public async Task<IActionResult> Update(UserUpdateModel model)
        {
            var bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            return Ok(await _userService.Update(model, bearer_token));
        }

        [Authorize]
        [HttpPost("UpdatePassword")]
        public async Task<IActionResult> UpdatePassword(UserUpdatePasswordModel model)
        {
            var bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            return Ok(await _userService.UpdatePassword(model, bearer_token));
        }

        [HttpPost("ReclaimPassword")]
        public async Task<IActionResult> ReclaimPassword(UserReclaimPasswordModel model)
        {
            return Ok(await _userService.ReclaimPassword(model));
        }

        [Authorize(Policy = "GetUserListAccess")]
        [HttpGet("GetUserList")]
        public IActionResult GetUserList(string? search, int type, string? sortBy, string? sortDirection, int page = 1, int pageSize = 5)
        {
            var bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            return Ok(_userService.GetUserList(bearer_token, search, type, sortBy, sortDirection, page, pageSize));
        }

        [Authorize(Roles = "Admin, AdminManager")]
        [HttpGet("GetUser")]
        public IActionResult GetUser(Guid id, int type)
        {
            var bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            return Ok(_userService.GetUser(id, type, bearer_token));
        }

        [Authorize]
        [HttpPost("SelfGetUser")]
        public IActionResult SelfGetUser()
        {
            var bearer_token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            return Ok(_userService.SelfGetUser(bearer_token));
        }

        [HttpPost("RenewToken")]
        public async Task<IActionResult> RenewToken(TokenModel tokenModel)
        {
            return Ok(_userService.RenewToken(tokenModel));
        }

        [HttpGet("ValidateSignIn")]
        public async Task<IActionResult> ValidateSignIn(string token)
        {
            var result = await _userService.validateSignInToken(token);

            if (result)
            {
                return Redirect(_appSettings.ClientUrl + "/guest/login?activated=1");
            }
            else return BadRequest("Token not valid");
        }

        [HttpGet("ValidateReclaim")]
        public async Task<IActionResult> ValidateReclaim(string token)
        {
            var result = await _userService.validateReclaimToken(token);

            if (result)
            {
                return Redirect(_appSettings.ClientUrl + "/guest/reclaim?reclaim-token=" + token);
            }
            else return BadRequest("Token not valid");
        }
    }
}
