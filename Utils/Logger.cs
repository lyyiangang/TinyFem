using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace TinyFem.Utils
{
        public class Logger
        {
            public static string FilePath = "Log\\SystemRuntime.log";
            /// <summary>
            /// 将信息写入日志中
            /// </summary>
            /// <param name="filepath"></param>
            /// <param name="messsage"></param>
            public static void WriteLogMessage(string message)
            {
                try
                {
                    using (FileStream logFileStreamd = new FileStream(FilePath, FileMode.Append, FileAccess.Write))
                    {
                        using (StreamWriter logStreamWriter = new StreamWriter(logFileStreamd))
                        {
                            logStreamWriter.WriteLine(string.Format("【{0}】:{1}", DateTime.Now.ToString(), message));
                        }
                    }
                }
                catch
                {
                }
            }
        }
}
