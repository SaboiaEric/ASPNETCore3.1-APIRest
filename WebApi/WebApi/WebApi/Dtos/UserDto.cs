using System;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Dtos
{
    public class UserDto
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        public bool IsEnabled { get; set; }
    }
}