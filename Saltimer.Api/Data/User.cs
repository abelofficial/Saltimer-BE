﻿namespace Saltimer.Api.Data
{
    public class User
    {
        public string Username { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

    }
}
