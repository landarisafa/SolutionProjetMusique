using Microsoft.EntityFrameworkCore;
using MyMusicCore.Models;
using MyMusicCore.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMusic.Data.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(MyMusicDbContext context)
        : base(context)
        { }
        private MyMusicDbContext _myMusicDbContext
        {
            get { return _context as MyMusicDbContext; }
        }

        public async Task<User> Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;

            User user = await _myMusicDbContext.Users.SingleOrDefaultAsync(x => x.Username == username);

            // check if username exists
            if (user == null)
                return null;

            // check if password is correct
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            // authentication successful
            return user;
        }
        public async Task<User> Create(User user, string password)
        {
            // validation
            if (string.IsNullOrEmpty(password))
                throw new Exception("Password is required");

            bool resultUser = await _myMusicDbContext.Users.AnyAsync(x => x.Username == user.Username);
            if (resultUser)
                throw new Exception($"Username {user.Username} is already taken");

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _myMusicDbContext.Users.AddAsync(user);

            return user;
        }
        public void Update(User userParam, string password = null)
        {
            User user = _myMusicDbContext.Users.Find(userParam.Id);

            if (user == null)
                throw new Exception("User not found");

            if (userParam.Username != user.Username)
            {
                // username has changed so check if the new username is already taken
                if (_myMusicDbContext.Users.Any(x => x.Username == userParam.Username))
                    throw new Exception($"Username {userParam.Username} is already taken");
            }

            // update user properties
            user.FirstName = userParam.FirstName;
            user.LastName = userParam.LastName;
            user.Username = userParam.Username;

            // update password if it was entered
            if (!string.IsNullOrWhiteSpace(password))
            {
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(password, out passwordHash, out passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
            }

            _myMusicDbContext.Users.Update(user);
        }
        public async Task<IEnumerable<User>> GetAllUserAsync()
        {
            return await _myMusicDbContext.Users
             .ToListAsync();
        }
        public async Task<User> GetWithUsersByIdAsync(int id)
        {
            return await _myMusicDbContext.Users
                     .FirstOrDefaultAsync(user => user.Id == id);
        }
        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                byte[] computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }
        public void Delete(int id)
        {
            User user = _myMusicDbContext.Users.Find(id);
            if (user != null)
            {
                _myMusicDbContext.Users.Remove(user);
            }
        }
    }
}
