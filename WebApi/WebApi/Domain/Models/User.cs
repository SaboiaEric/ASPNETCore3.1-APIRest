using System;

namespace Domain.Models
{
    public class User
    {
        public User(Guid id, string name = "", string email = "", bool isEnabled = false)
        {
            Id = id;
            Name = name;
            Email = email;
            IsEnabled = isEnabled;
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public bool IsEnabled { get; private set; }

        public void UpdateUserStatus(bool status) => IsEnabled = status;

        public bool IsValidUser()
        {
            return !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Email);
        }
    }
}