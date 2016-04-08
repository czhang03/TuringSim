namespace TuringSim
{
    class Program
    {
        static void Main(string[] args)
        {
            var turing = new TuringMachine();
            string[] s3 = { "John", "Paul", "Mary" };
            turing.Input();
            turing.Run();
        }
    }
}
