namespace blazorwasm
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var t = typeof(Microsoft.AspNetCore.Components.ComponentBase);
            System.Console.WriteLine(t.FullName);
        }
    }
}
