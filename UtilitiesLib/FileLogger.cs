using System.Text;

namespace CardGameLib
{
    public class FileLogger
    {

        private string _filePath;

        public FileLogger(string filePath)

        {
            Logger.WriteMessage += WriteToFile;
            Logger.WriteError += WriteToFile;
            _filePath = filePath;

        }

        private void WriteToFile(string log)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(log + "\n");
            File.AppendAllText(_filePath, sb.ToString());
            sb.Clear();
        }
    }
}
