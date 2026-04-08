namespace Todos.Core.Entities;

public class EntitiesUtils
{
    public static bool IsValidNameCharacter(char x)
    {
        if (char.IsLetter(x)) return true;
        if (x == ' ' || x == '\'' || x == '-' || x == '.') return true;
        return false;
    }
}
