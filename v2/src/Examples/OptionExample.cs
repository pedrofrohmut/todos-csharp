using Todos.Core.Utils;

namespace Todos.Examples;

public class OptionExample
{
    public static void Run()
    {
        // If you know you got a value.
        Option<string> name = Option.Some("Bob");

        // If you know you dont have a value.
        Option<string> missing = Option<string>.None();

        // If you dont know at this point if you have or not a value you can
        // create a option with New.
        // Or you can use New everytime you dont want to explicit about.
        string? user1middleName = null;
        Option<string> maybeMiddleName1 = Option.New(user1middleName);

        string? user2middleName = "foo";
        Option<string> maybeMiddleName2 = Option.New(user2middleName);
    }
}
