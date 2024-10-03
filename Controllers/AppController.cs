using App.Core;
using App.Exceptions;
using App.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers
{
    public class AppController:Controller
    {
        public readonly IAppService _appService;
        public readonly IUserService _userService;
        public readonly IMapper _mapper;


        public AppController(IAppService appService,IUserService userService, IMapper mapper) {
            _appService = appService;
            _userService = userService; 
            _mapper=mapper;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            string? uniqueId =string.Empty;
            User? user=null;
            if (HttpContext.Request.Cookies.TryGetValue("UserId", out uniqueId))
            {
                try
                {
                    user = await this._userService.GetUserById(uniqueId);
                }
                catch (NotFoundException ex) { 
                    uniqueId = Guid.NewGuid().ToString();
                    HttpContext.Response.Cookies.Append("UserId", uniqueId, new CookieOptions
                    {
                        HttpOnly = true, // Optional: Prevents client-side script access
                        Secure = true, // Optional: Ensure cookie is sent only over HTTPS
                        SameSite = SameSiteMode.Strict // Optional: Control cross-origin requests
                    });
                    user = await this._userService.CreateUser(uniqueId, "Anonymous User");
                }
            }
            else
            {
                uniqueId = Guid.NewGuid().ToString();
                HttpContext.Response.Cookies.Append("UserId", uniqueId, new CookieOptions
                {
                    HttpOnly = true, // Optional: Prevents client-side script access
                    Secure = true, // Optional: Ensure cookie is sent only over HTTPS
                    SameSite = SameSiteMode.Strict // Optional: Control cross-origin requests
                });
                user = await this._userService.CreateUser(uniqueId, "Anonymous User");
            }
            var queryParams = new OffsetParams { limit = 15, offset = 0 };
            var chatMessage = await this._appService.GetOffsetMessageAsync(queryParams);
            chatMessage?.Items?.Reverse();
            var messageResult=_mapper.Map<List<MessageResultDto>>(chatMessage!.Items);
            var viewModel = new ChatView {  User = user,MessageResult=messageResult };
            return View(viewModel);
        }
       
        [HttpGet("app/get-older")]
        public async Task<ActionResult<List<MessageResultDto>>> GetOlder(int offset,int limit)
        {
            var offsetParams=new OffsetParams { limit = limit,offset=offset };    
            var chatMessage = await this._appService.GetOffsetMessageAsync(offsetParams);
            var messageResult = _mapper.Map<List<MessageResultDto>>(chatMessage!.Items);
            return messageResult;
        }
        [HttpPost("user/rename-user")]
        public async Task<ActionResult<User>> RenameUser([FromBody] RenameDto userNameDto)
        {
            if (userNameDto?.userName!.Trim().Length==0)
            {
                return BadRequest();
            }
            var cookieValue = string.Empty;
            if (HttpContext.Request.Cookies.TryGetValue("UserId", out cookieValue))
            {
                var res = await this._userService.RenameUser(cookieValue, userNameDto?.userName!);
                return res;
            }
            else
            {
                return BadRequest();
            }

        }
    }
}
