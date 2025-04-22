using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using GenerativeAIApp.Core.DTOs;
using GenerativeAIApp.Core.Entities;
using GenerativeAIApp.Core.Interfaces;

namespace GenerativeAIApp.BLL.Services
{
    public class UserService
    {
        #region properties

        private readonly IUserRepository _userRepository;
        #endregion

        #region constractor

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        #endregion

        #region methods

        public async Task<User?> ValidateUserAsync(string username, string password)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            if (user == null)
                return null;

            var hash = ComputeHash(password);
            return user.PasswordHash == hash ? user : null;
        }

        public async Task<bool> RegisterUserAsync(UserRegisterDto dto)
        {
            var existing = await _userRepository.GetByUsernameAsync(dto.Username);
            if (existing != null)
                return false;

            var user = new User
            {
                Username = dto.Username,
                PasswordHash = ComputeHash(dto.Password),
                Role = dto.Role,
            };

            await _userRepository.AddUserAsync(user);
            return true;
        }

        //public Task<User?> GetUserByUsernameAsync(string username)
        //{
        //    return _userRepository.GetByUsernameAsync(username);
        //}

        //public Task AddUserAsync(User user)
        //{
        //    return _userRepository.AddUserAsync(user);
        //}
        #endregion

        #region Private mathods

        private string ComputeHash(string password)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        #endregion
    }
}
