﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerativeAIApp.Core.DTOs
{
    public class UserLoginDto
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
