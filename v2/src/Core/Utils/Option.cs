namespace Todos.Core.Utils;

/*
   -> This is a reference only. (boring csharp won't accept this code)

   internal enum Option<T> {
       None,
       Some(T),
   }
*/

/*
   -> Example usage:

   Option<string> foo = Option<string>.Some("foo");
   Option<int> bar = Option<int>.Some(1234);
   Option<int> baz = Option<int>.None;
   Option<int> bux = Option<int>.Some(0);

   -> Add `using static Option;` at the top

   var foo2 = Some("foo");
   var bar2 = Some(1234);
   var baz2 = None<int>();
   var bux2 = Some(0);
*/
public readonly struct Option<T>
{
    private readonly T val;
    private readonly bool hasVal;

    private Option(T val, bool hasVal)
    {
        this.val = val;
        this.hasVal = hasVal;
    }

    // TODO: add instance methods: HasSome, HasNone, Value

    public static Option<T> Some(T val)
    {
        return new Option<T>(val: val, hasVal: true);
    }

    public static Option<T> None()
    {
        return new Option<T>(val: default!, hasVal: false);
    }
}

/*
   This class serves as a facade to simplify the using Option<T>
*/
public static class Option
{
    public static Option<T> None<T>() => Option<T>.None();

    public static Option<T> Some<T>(T val) => Option<T>.Some(val);
}
