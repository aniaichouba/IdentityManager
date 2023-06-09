﻿using System.ComponentModel.DataAnnotations;

namespace IdentityManager.Models
{
    public class VerifyAuthenticatorCodeViewModel
    {
        [Required]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }
        [Display(Name ="Remember me?")]
        public bool RememberMe { get; set; }

    }
}
