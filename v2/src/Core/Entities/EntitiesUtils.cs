namespace Todos.Core.Entities;

public class EntitiesUtils
{
    public static bool IsValidNameCharacter(char x)
    {
        if (char.IsLetter(x)) return true;
        if (x == ' ' || x == '\'' || x == '-' || x == '.') return true;
        return false;
    }

    public static bool IsValidName(string name) {
        if (name.Length == 0) return false;
        foreach (var x in name) {
            if (!IsValidNameCharacter(x)) return false;
        }
        return true;
    }

    public static bool IsValidEmailCharacter(char x)
    {
        if (char.IsLetterOrDigit(x)) return true;
        if (x == '.' || x == '_' || x == '-') return true;
        return false;
    }

    public static bool IsValidEmail(string email)
    {
        // Validate @
        var atIndex = email.IndexOf('@');
        if (atIndex < 0 || atIndex == email.Length - 1) {
            return false;
        }

        // Validate the local
        var local = email.Substring(0, atIndex);
        if (local.Length < 2) {
            return false;
        }
        foreach (var x in local) {
            if (!IsValidEmailCharacter(x)) return false;
        }

        // Validate the domain
        var domain = email.Substring(atIndex + 1);
        if (domain.Length < 3) {
            return false;
        }
        foreach (var x in domain) {
            if (!IsValidEmailCharacter(x)) return false;
        }

        return true;
    }
}
