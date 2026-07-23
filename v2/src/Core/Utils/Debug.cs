using System.Text.Json;

namespace Todos.Core.Utils;

public class Debug
{
    /* Doesnt work for Result, but it is not my fault is the Serializers fault.
       But this can be used for other stuff */
    public static void DebugPrint(object obj, string label = "")
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string jsonObj = JsonSerializer.Serialize(obj, options);

        Console.WriteLine("--------------------------------------------------------------------------------");
        if (label != "") {
            Console.WriteLine($"# {label} => ");
        }
        Console.WriteLine(jsonObj);
        Console.WriteLine("--------------------------------------------------------------------------------");
    }
}
