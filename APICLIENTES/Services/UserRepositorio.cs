using APICLIENTES.Models;
using APICLIENTES.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace APICLIENTES.Services
{
    public class UserRepositorio : IUserRepositorio
    {
        private readonly IMongoCollection<User> mongoCollection;
        private readonly IConfiguration _configuration; // para el token
        

        public UserRepositorio(IDatabaseSettings settings, IConfiguration configuration)//conexionBD accede a DataBaseSetting y a su vez este a appsetting.json
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            mongoCollection = database.GetCollection<User>(settings.CollectionName1);
            _configuration = configuration;
        }
        public async Task<bool> IsRegister(string userName)
        {
            User user = await mongoCollection.FindAsync(new BsonDocument { { "UserName", userName } })
             .Result.FirstOrDefaultAsync();
            if (user!=null)
            {
                return true;

            }
            return false;
        }

        public async Task<string> Login(string userName, string password)
        {
            User user = await mongoCollection.FindAsync(new BsonDocument { { "UserName", userName } })
             .Result.FirstOrDefaultAsync();
            if (user==null)
            {
                return "NoUser";
            }
            if (!VerificarPassWordHash(password,user.PassWordHash,user.PassWordSalt))
            {
                return "PassWordWrong";

            }
            return crearToken(user);
        }

        public async Task<string> Register(User user, string password)
        {
            try
            {
                if (!await IsRegister(user.UserName))
                {
                    CrearPassWordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
                    user.PassWordHash = passwordHash;
                    user.PassWordSalt = passwordSalt;
                    await mongoCollection.InsertOneAsync(user);
                    return crearToken(user);

                }
                return "Usuario ya esta registrado";
            }
            catch (Exception)
            {
                return "500";
            }
        }

        private void CrearPassWordHash(string password,out byte[] passwordHash, out byte[] passwordSalt)//encriptar pass
        {
            using (var hmac=new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

            }

        }

        public bool VerificarPassWordHash(string password,  byte[] passwordHash,  byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i])
                   {
                       return false;
                  }

                }
                return true;
            }
        }

        private string crearToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id),
                new Claim(ClaimTypes.Name,user.UserName)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8
                .GetBytes(_configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = System.DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);

        }
    }
}
