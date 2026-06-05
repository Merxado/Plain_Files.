using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
    public class UserAccount
    {
        public string Username { get; set; } = null;
        public string Password { get; set; } = null;
        public bool IsActive { get; set; }

        public override string ToString()
        {
            return $"{Username},{Password},{IsActive.ToString().ToLower()}";
        }

        public static UserAccount Parse(string line)
        {
            var fields = line.Split(',');
            return new UserAccount
            {
                Username = fields[0],
                Password = fields[1],
                IsActive = bool.Parse(fields[2])
            };
        }
    }
}
