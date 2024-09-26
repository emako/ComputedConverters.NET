namespace Cast;

internal class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine(Cast<double>(96110d).ToString());
        Console.WriteLine(Cast<string>("damn").ToString());

        Console.WriteLine(Cast1(96110d));
        Console.WriteLine(Cast2("damn"));
    }

    static object CastInt(object input)
    {
        return (int)input;
    }

    static T Cast<T>(object input)
    {
        return (T)input;
    }

    static double Cast1(object input)
    {
        return (double)input;
    }

    static string Cast2(object input)
    {
        return (string)input;
    }
}
