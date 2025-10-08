using Todos.Core.Exceptions;

using System.ComponentModel.DataAnnotations;

namespace Todos.Core.Entities;

public class User
{
    public static void ValidateId(string? id)
    {
        if (id == null) {
            throw new InvalidUserException("User id cannot be null");
        }
        var canBeParsed = Guid.TryParse(id, out var output);
        if (! canBeParsed) {
            throw new InvalidUserException("User id is not a valid Guid");
        }
    }
    
    public static void ValidateName(string name)
    {
        if (String.IsNullOrWhiteSpace(name)) {
            throw new InvalidUserException("User name is required and cannot be blank");
        }
        if (name.Length < 5 || name.Length > 120) {
            throw new InvalidUserException("Name must be between 5 and 120 characters");
        }
    }
    
    public static void ValidateEmail(string email)
    {
        if (String.IsNullOrWhiteSpace(email)) {
            throw new InvalidUserException("User e-mail is required and cannot be blank");
        }
        bool isValid = new EmailAddressAttribute().IsValid(email);
        if (! isValid) {
            throw new InvalidUserException("User e-mail has an invalid format");
        }
    }
   
    public static void ValidatePassword(string password)
    {
        if (String.IsNullOrWhiteSpace(password)) {
            throw new InvalidUserException("User password is required and cannot be blank");
        }
        if (password.Length < 3 || password.Length > 32) {
            throw new InvalidUserException("User password must be between 5 and 32 characters");
        }
    }
}
