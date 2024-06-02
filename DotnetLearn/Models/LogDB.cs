namespace DotnetLearn.Models
{
    public class LogDB:IMLogger
    {

        public void log(string message)
        {
            Console.WriteLine(message);
            Console.WriteLine("LogtoDB"); 
        }
    }
}
