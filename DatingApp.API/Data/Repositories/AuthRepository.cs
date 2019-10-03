using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using DatingApp.API.Data.Repositories.Interfaces;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;
        public AuthRepository(DataContext context)
        {
           _context = context;
        }

        public async Task<User> Login(string username, string password)
        {
            var userDetails = await _context.Users.FirstOrDefaultAsync(it => it.UserName == username);

            if(userDetails == null)
                return null;

            if(!ValidateUser(password, userDetails.PasswordHash, userDetails.PasswordSalt))
                return null;

            return userDetails;
        }

        private bool ValidateUser(string password, byte[] passwordHash, byte[] passwordSalt)
        {            
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedPasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                for(int i = 0; i < computedPasswordHash.Length; i++)
                {
                    if(computedPasswordHash[i] != passwordHash[i])
                        return false;
                }
            }

            return true;
        }

        public async Task<User> Register(User user, string password)
        {
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task<bool> UserExists(string username)
        {
            if(await _context.Users.AnyAsync(it => it.UserName == username))
                return true;

            return false;
        }
    }
}