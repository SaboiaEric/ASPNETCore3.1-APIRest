using Domain.Models;
using System;

namespace WebApi.Dtos
{
    public static class UserDtoExtensions
    {
        public static User ToUser(this UserDto dto)
        {
            return new User(Guid.NewGuid(), dto.Name, dto.Email, dto.IsEnabled);
        }

        public static UserDto ToUserDto(this User model)
        {
            return new UserDto
            {
                Id = model.Id,
                Name = model.Name,
                Email = model.Email,
                IsEnabled = model.IsEnabled
            };
        }
    }
}