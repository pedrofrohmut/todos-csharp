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
public readonly struct Option<T> where T : notnull
{
    private readonly T val;
    private readonly bool hasVal;

    private Option(T val, bool hasVal)
    {
        this.val = val;
        this.hasVal = hasVal;
    }

    public bool HasSome { get => this.hasVal; }

    public bool HasNone { get => !this.hasVal; }

    public T Value
    {
        get {
            if (!this.hasVal) {
                throw new OptionalHasNoValueException();
            }
            return this.val;
        }
    }

    public static Option<T> Some(T val)
    {
        if (val == null) {
            throw new OptionalSomeNullArgumentException();
        }
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
    // Creates an option with no value
    public static Option<T> None<T>() where T : notnull
    {
        return Option<T>.None();
    }

    // Creates an option with a value
    public static Option<T> Some<T>(T val) where T : notnull
    {
        return Option<T>.Some(val);
    }

    // Creates a option for a optional value
    public static Option<T> New<T>(T? val) where T : notnull
    {
        return val != null ? Some(val) : None<T>();
    }
}

public class OptionalSomeNullArgumentException : Exception
{
    public OptionalSomeNullArgumentException() : base("Some cannot be called with a null argument.") {}
    public OptionalSomeNullArgumentException(string message) : base(message) {}
}

public class OptionalHasNoValueException : Exception
{
    public OptionalHasNoValueException() : base("Trying to access the value of an Optional that has no value.") {}
    public OptionalHasNoValueException(string message) : base(message) {}
}
