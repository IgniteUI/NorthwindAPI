namespace NorthwindCRUD.Services
{
    using Microsoft.IdentityModel.Tokens;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using BCrypt.Net;
    using NorthwindCRUD.Helpers;
    using NorthwindCRUD.Models.DbModels;

    public class AuthService
    {
        private readonly DataContext dataContext;

        private readonly IConfiguration configuration;

        public AuthService(DataContext dataContext, IConfiguration configuration)
        {
            this.dataContext = dataContext;
            this.configuration = configuration;
        }

        public bool IsAuthenticated(string email, string password)
        {
            var user = this.GetByEmail(email);
            return this.DoesUserExists(email) && BCrypt.Verify(password, user.Password);
        }

        public bool DoesUserExists(string email)
        {
            var user = this.dataContext.Users.FirstOrDefault(x => x.Email == email);
            return user != null;
        }

        public UserDb GetById(string id)
        {
            return this.dataContext.Users.FirstOrDefault(c => c.UserId == id);
        }

        public UserDb GetByEmail(string email)
        {
            return this.dataContext.Users.FirstOrDefault(c => c.Email == email);
        }

        public UserDb RegisterUser(UserDb model)
        {
            var id = IdGenerator.CreateLetterId(10);
            var existWithId = this.GetById(id);
            while (existWithId != null)
            {
                id = IdGenerator.CreateLetterId(10);
                existWithId = this.GetById(id);
            }
            model.UserId = id;
            model.Password = BCrypt.HashPassword(model.Password);

            PropertyHelper<UserDb>.MakePropertiesEmptyIfNull(model);

            var userEntity = this.dataContext.Users.Add(model);
            this.dataContext.SaveChanges();

            return userEntity.Entity;
        }

        public string GenerateJwtToken(string email)
        {
            var issuer = this.configuration["Jwt:Issuer"];
            var audience = this.configuration["Jwt:Audience"];
            var key = Encoding.ASCII.GetBytes(this.configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                            new Claim("Id", Guid.NewGuid().ToString()),
                            new Claim(JwtRegisteredClaimNames.Sub, email),
                            new Claim(JwtRegisteredClaimNames.Email, email),
                            new Claim(JwtRegisteredClaimNames.Jti,
                            Guid.NewGuid().ToString())
                        }),
                Expires = DateTime.UtcNow.AddMinutes(5),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
