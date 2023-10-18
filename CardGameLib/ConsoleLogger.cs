using System.Diagnostics;

namespace CardGameLib
{
    public class ConsoleLogger
    {
        public ConsoleLogger()
        {

            Logger.WriteMessage += WriteToConsole;
            Logger.WriteError += WriteToConsole;

        }

        private void WriteToConsole(string log)
        {
            Debug.WriteLine(log);
        }
    }
}
