using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenerativeAIApp.Core.Entities;
using GenerativeAIApp.Core.Interfaces;

namespace GenerativeAIApp.BLL.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<User?> GetUserByUsernameAsync(string username)
        {
            return _userRepository.GetByUsernameAsync(username);
        }

        public Task AddUserAsync(User user)
        {
            return _userRepository.AddUserAsync(user);
        }
    }
}
