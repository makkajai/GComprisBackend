﻿using ServiceStack.DataAnnotations;

namespace GComprisBackend.ServiceModel.Types
{
    [Alias("users")]
    public class User
    {
        [Alias("user_id")]
        public int Id { get; set; }

        [Alias("login")]
        public string Login { get; set; }

        [Alias("lastname")]
        public string LastName { get; set; }

        [Alias("firstname")]
        public string FirstName { get; set; }
    }
}
