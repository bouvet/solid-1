using System.Diagnostics;

namespace SRP_DI_Workshop
{
    public class LoggerService
    {
        public void WriteLine(string message, string category)
        {
            Trace.WriteLine(message, category);
        }
    }
}