using WebScraping.Core.Application.Heplers;

namespace WebScraping.Core.Application.Utils
{
    public class LogFile
    {
        private static string path = $"{Helper.basePath}/Logs";
        public static void Write<T>(string message) where T : class
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string className = typeof(T).Name;
            string logPath = $"{path}/{className}.txt";

            EventWaitHandle waitHandle = new EventWaitHandle(true, EventResetMode.AutoReset, "SHARED_BY_ALL_PROCESSES");
            waitHandle.WaitOne();
            var sw = new StreamWriter(logPath, true);
            sw.WriteLine(message);
            sw.Close();
            waitHandle.Set();
        }

    }
}
