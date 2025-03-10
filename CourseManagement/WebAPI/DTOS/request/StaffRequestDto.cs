﻿using System.ComponentModel.DataAnnotations;
using WebAPI.DTOS.Authentication;

namespace WebAPI.DTOS.request
{
    public class StaffRequestDto
    {

        [Required(ErrorMessage = "Username is required")]
        [StringLength(50, ErrorMessage = "Username cannot be longer than 50 characters")]
        public string UserName { get; set; }
        

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }
        public int SelectedRole { get; set; }

    }
}
