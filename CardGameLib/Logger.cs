namespace CardGameLib
{

    public class Logger
    {
        public static Action<string> WriteMessage;
        public static Action<string> WriteError;

        public static void LogMessage(string message)
        {
            if (WriteMessage != null)
            {
                WriteMessage(message);
            }

        }
        public static void LogError(string message)
        {
            if (WriteError != null)
            {
                WriteError(message);
            }
        }
    }
}
