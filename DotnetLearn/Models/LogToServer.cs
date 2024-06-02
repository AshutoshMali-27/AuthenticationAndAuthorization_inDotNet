namespace DotnetLearn.Models
{
    public class LogToServer: IMLogger()
    {
        public void log(string message)
        {
            Console.WriteLine(message);
            Console.WriteLine("LogtoServer");
        }
    }
}
