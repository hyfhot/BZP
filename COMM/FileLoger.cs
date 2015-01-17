using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COMM
{
    public class FileLoger
    {
        private const int MAXLINES = 1000;
        public static FileLoger ErrorLoger = new FileLoger("Err_");
        public static FileLoger AccessLoger = new FileLoger("Acc_");

        private string fileflag = "_", fileext = "log";
        private string path;
        private IList<string> LogList = new List<string>();

        public FileLoger(string fileflag = "_", string fileext = "log")
        {
            this.path = string.Format("{0}\\log", Environment.CurrentDirectory);
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }

            this.fileflag = fileflag;
            this.fileext = fileext;
        }

        ~FileLoger()
        {
            WriteToFile();
        }
        
        public void AddLog(string logmsg)
        {
            LogList.Add(logmsg);

            if(LogList.Count >= MAXLINES)
            {
                WriteToFile();
            }
        }

        public void WriteToFile()
        {
            try
            {
                string filename = string.Format("{0}\\{1}{2}.{3}", path, fileflag, DateTime.Now.ToString("yyyMMdd"),fileext);
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(filename, true))
                {
                    while (LogList.Count > 0)
                    {
                        string strlog = LogList[0];
                        file.WriteLine(strlog);
                        LogList.RemoveAt(0);
                    }
                    file.Close();
                }
            }
            catch
            {
            }
        }
    }
}
