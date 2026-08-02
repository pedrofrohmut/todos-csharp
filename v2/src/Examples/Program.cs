namespace Todos.Examples;

enum Names {
    Option,
}

public static class Program
{
    public static void ShowUsage()
    {
        Console.WriteLine("Usage: dotnet run -- <example_name>");
    }

    public static void ShowPossibleOptions()
    {
        Console.WriteLine("The possible names to use are:");
        int i = 1;
        foreach (var name in Enum.GetValues(typeof(Names))) {
            Console.WriteLine($"{i} - {name.ToString()}");
        }
    }

    public static void Main(string[] args)
    {
        if (args.Length == 0) {
            ShowPossibleOptions();
            ShowUsage();
            return;
        }

        string option = args[0].ToLower();

        Console.WriteLine($"Selected option: {option}");

        switch (option) {
            case "option":
                OptionExample.Run();
                break;
            default:
                ShowPossibleOptions();
                break;
        }
    }
}
