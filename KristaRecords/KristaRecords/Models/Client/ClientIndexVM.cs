﻿using System.ComponentModel.DataAnnotations;

namespace KristaRecords.Models.Client
{
    public class ClientIndexVM
    {
        public string Id { get; set; } = null!;

        [Display(Name = "Username")]
        public string UserName { get; set; } = null!;

        [Display(Name = "First Name")]
        public string FirstName { get; set; } = null!;

        [Display(Name = "Last Name")]
        public string LastName { get; set; } = null!;

        public string Address { get; set; } = null!;

        public string Email { get; set; } = null!;

        public bool IsAdmin { get; set; }
    }
}
