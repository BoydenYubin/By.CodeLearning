﻿using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace ByLearning.SSO.Application.ViewModels.UserViewModels
{
    public class SaveUserRoleViewModel
    {
        [Required]
        public string Role { get; set; }
        [JsonIgnore]
        public string Username { get; set; }
    }
}