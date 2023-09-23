using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System;
using mdbtocsv_util;

namespace logging
{

    public static class Log
    {

        public static string LogFileName { get; set; } = string.Empty;
        private static readonly int MAXLOGSIZEKB = 128;

        public static void Init(string log_file_name)
        {
            LogFileName = log_file_name;
        }

        /// <summary>
        /// Write text line to application log file
        /// </summary>
        /// <param name="line">the text to write to file</param>
        /// <remarks></remarks>
        public static void WriteToLogFile(string line, bool echoToConsole = false)
        {
            if (echoToConsole)
                Console.WriteLine(line);

            try
            {
                while (Util.IsFileLocked(LogFileName))
                {
                    System.Threading.Thread.Sleep(100);
                    Debug.Print("[WriteToLogFile] waiting for file to unlock!");
                }

                using StreamWriter sr = new(LogFileName, true);
                Debug.Print(System.DateTime.Now.ToString() + "|" + line);
                sr.WriteLine(System.DateTime.Now.ToString() + "|" + line);
            }
            catch (System.IO.IOException ioex)
            {
                Debug.Print("ERROR: Caught IOException : " + ioex.Message);
                var errorCode = Marshal.GetHRForException(ioex) & ((1 << 16) - 1);
                Debug.Print("IOException: HR CODE: " + errorCode.ToString());
            }
            catch (Exception ex)
            {
                Debug.Print("ERROR: Caught " + ex.Message);
                Console.WriteLine("ERROR: Unable to write to log file. Message = " + ex.Message);
            }
        }


        /// <summary>
        /// Trims Older Records From Log File
        /// </summary>
        public static void TrimLogFile()
        {
            if (File.Exists(LogFileName))
            {
                System.IO.FileInfo file = new(LogFileName);
                if (file.Length > MAXLOGSIZEKB * 1024)
                {
                    try
                    {
                        char[] cBuffer = new char[(int)file.Length / 2];
                        using (StreamReader sr = new(LogFileName))
                        {
                            // read first half of stream into buffer
                            sr.Read(cBuffer, 0, cBuffer.Length - 1);
                            // clear buffer
                            System.Array.Clear(cBuffer, 0, cBuffer.Length);
                            // read 2nd half of stream into buffer
                            sr.Read(cBuffer, 0, cBuffer.Length - 1);
                        }

                        using StreamWriter wr = new(LogFileName);
                        // write 2nd half of buffer to file
                        wr.Write(cBuffer, 0, cBuffer.Length - 1);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("[TrimLogFile] ERROR : " + ex.Message);
                    }
                }
            }

        }
    }

}