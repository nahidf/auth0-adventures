using System;

namespace Models
{
    public class UserModel
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public DateTime LatestLogin { get; set; }
    }
}
