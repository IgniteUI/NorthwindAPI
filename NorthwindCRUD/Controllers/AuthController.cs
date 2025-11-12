namespace NorthwindCRUD.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using NorthwindCRUD.Models.DbModels;
    using NorthwindCRUD.Models.Dtos;
    using NorthwindCRUD.Services;

    [ApiController]
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly AuthService authService;
        private readonly IMapper mapper;
        private readonly ILogger<AuthController> logger;

        public AuthController(IConfiguration configuration, AuthService authService, IMapper mapper, ILogger<AuthController> logger)
        {
            this.configuration = configuration;
            this.authService = authService;
            this.mapper = mapper;
            this.logger = logger;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public ActionResult<string> Login(LoginDto userModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (this.authService.IsAuthenticated(userModel.Email, userModel.Password))
                    {
                        var token = this.authService.GenerateJwtToken(userModel.Email);

                        return Ok(token);
                    }

                    return BadRequest("Email or password are not correct!");
                }

                return BadRequest(ModelState);
            }
            catch (Exception error)
            {
                logger.LogError(error.Message);
                return StatusCode(500);
            }
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public ActionResult<string> Register(RegisterDto userModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (userModel.Password != userModel.ConfirmedPassword)
                    {
                        return BadRequest("Passwords does not match!");
                    }

                    if (this.authService.DoesUserExists(userModel.Email))
                    {
                        return BadRequest("User does not exists!");
                    }

                    var mappedModel = this.mapper.Map<RegisterDto, UserDb>(userModel);
                    var user = this.authService.RegisterUser(mappedModel);

                    if (user != null)
                    {
                        var token = this.authService.GenerateJwtToken(user.Email);
                        return Ok(token);
                    }

                    return BadRequest("Email or password are not correct!");
                }

                return BadRequest(ModelState);
            }
            catch (Exception error)
            {
                logger.LogError(error.Message);
                return StatusCode(500);
            }
        }

        [AllowAnonymous]
        [HttpPost("LoginObject")]
        public ActionResult<AuthDto> LoginObject(LoginDto userModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (this.authService.IsAuthenticated(userModel.Email, userModel.Password))
                    {
                        var token = this.authService.GenerateJwtToken(userModel.Email);

                        return Ok(new AuthDto() { Token = token });
                    }

                    return BadRequest("Email or password are not correct!");
                }

                return BadRequest(ModelState);
            }
            catch (Exception error)
            {
                logger.LogError(error.Message);
                return StatusCode(500);
            }
        }

        [AllowAnonymous]
        [HttpPost("RegisterObject")]
        public ActionResult<AuthDto> RegisterObject(RegisterDto userModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (userModel.Password != userModel.ConfirmedPassword)
                    {
                        return BadRequest("Passwords does not match!");
                    }

                    if (this.authService.DoesUserExists(userModel.Email))
                    {
                        return BadRequest("User does not exists!");
                    }

                    var mappedModel = this.mapper.Map<RegisterDto, UserDb>(userModel);
                    var user = this.authService.RegisterUser(mappedModel);

                    if (user != null)
                    {
                        var token = this.authService.GenerateJwtToken(user.Email);
                        return Ok(new AuthDto() { Token = token });
                    }

                    return BadRequest("Email or password are not correct!");
                }

                return BadRequest(ModelState);
            }
            catch (Exception error)
            {
                logger.LogError(error.Message);
                return StatusCode(500);
            }
        }
    }
}
