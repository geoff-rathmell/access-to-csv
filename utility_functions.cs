using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using logging;
using Microsoft.Win32;

namespace mdbtocsv_util
{

    public static class Util
    {
        /// <summary>
        /// Determine if given filename is locked by other processes
        /// </summary>
        /// <param name="filePath">file to evaluate</param>
        /// <returns>TRUE if file is locked</returns>
        public static bool IsFileLocked(string filePath)
        {
            try
            {
                using (File.Open(filePath, FileMode.Open)) { }
                Debug.Print("[IsFileLocked] File is NOT Locked.");
            }
            catch (IOException e)
            {
                var errorCode = Marshal.GetHRForException(e) & ((1 << 16) - 1);

                Debug.Print($"[IsFileLocked] File Is Locked!");
                System.Threading.Thread.Sleep(100);
                return errorCode == 32 || errorCode == 33;
            }

            return false;
        }

        /// <summary>
        /// Gets the name of an ODBC driver for Microsoft Access giving preference to the most recent one.
        /// </summary>
        /// <returns>the name of an ODBC driver for Microsoft Access, if one is present; null, otherwise.</returns>
        public static string GetOdbcAccessDriverName()
        {
            var driverName = string.Empty;

            try
            {

                List<string> driverPrecedence = new() { "Microsoft Access Driver (*.mdb, *.accdb)", "Microsoft Access Driver (*.mdb)" };
                string[]? availableOdbcDrivers = GetOdbcDriverNames();

                if (availableOdbcDrivers != null)
                {
                    driverName = driverPrecedence.Intersect(availableOdbcDrivers).FirstOrDefault();
                }

            }
            catch (Exception ex)
            {
                Log.WriteToLogFile($"[GetOdbcAccessDriverName] CAUGHT ERROR : {ex.Message}");

            }

            return driverName ?? string.Empty;
        }


        /// <summary>
        /// Gets the ODBC driver names from the registry.
        /// </summary>
        /// <returns>a string array containing the ODBC driver names, if the registry key is present; null, otherwise.</returns>
        [SupportedOSPlatform("windows")]
        public static string[]? GetOdbcDriverNames(bool debugmode = false)
        {
            string[]? returnResult = null;
            
            try
            {
                using RegistryKey localMachineHive = Registry.LocalMachine;
                using RegistryKey? odbcDriversKey = localMachineHive.OpenSubKey(@"SOFTWARE\ODBC\ODBCINST.INI\ODBC Drivers");
                if (odbcDriversKey != null)
                {
                    returnResult = odbcDriversKey.GetValueNames();

                    if (debugmode)
                    {
                        foreach (var d in returnResult)
                        {
                            Log.WriteToLogFile($"INFO:ODBC_DRIVERS:{d}");
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Log.WriteToLogFile($"[GetOdbcDriverNames] CAUGHT ERROR : {ex.Message}");
            }

            return returnResult;
        }


        /// <summary>
        /// Displays data from a data table to console. Used for Debuging.
        /// </summary>
        /// <param name="table"></param>
        /// <remarks></remarks>
        public static void DisplayDataTable(DataTable table, bool writeToLogFileOnly = false)
        {
            foreach (DataRow row in table.Rows)
            {
                foreach (DataColumn col in table.Columns)
                {
                    if (writeToLogFileOnly)
                    { Log.WriteToLogFile($"{col.ColumnName} = {row[col]}"); }
                    else
                    { Console.WriteLine("{0} = {1}", col.ColumnName, row[col]); }
                }
                Log.WriteToLogFile("============================");
            }
        }

        
        public static Point currentPosition;
        public static void WriteAt(string s, Point p)
        {
            try
            {
                Console.SetCursorPosition(currentPosition.X + p.X, currentPosition.Y + p.Y);
                Console.Write(s);
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.Clear();
                Console.WriteLine(e.Message);
            }
        }

        // <summary>
        /// Create a zip file for a single file. Will place in a \archive subfolder from original directory unless subfolder value is modified.
        /// The original file will be deleted after successfully archived.
        /// </summary>
        /// <param name="filename">The file to zip</param>
        /// <param name="subFolderName">The subfolder to save the zip file to. Pass in empty string to archive in same folder.</param>
        public static void ArchiveAndZipFile(string filename, string subFolderName = "archive")
        {
            if (File.Exists(filename))
            {
                FileInfo fi = new(filename);

                string zipFileName;
                if (string.IsNullOrEmpty(subFolderName))
                    zipFileName = $"{Path.GetDirectoryName(fi.FullName)}{Path.DirectorySeparatorChar}{Path.GetFileNameWithoutExtension(fi.Name)}_{DateTime.Now.ToString("yyyy-MM-dd HH-mm")}.zip";
                else
                    zipFileName = $"{Path.GetDirectoryName(fi.FullName)}{Path.DirectorySeparatorChar}{subFolderName}{Path.DirectorySeparatorChar}{Path.GetFileNameWithoutExtension(fi.Name)}_{DateTime.Now.ToString("yyyy-MM-dd HH-mm")}.zip";


                if (!Directory.Exists(Path.GetDirectoryName(zipFileName)))
                    Directory.CreateDirectory(Path.GetDirectoryName(zipFileName)!);

                if (File.Exists(zipFileName))
                    File.Delete(zipFileName);

                try
                {
                    using ZipArchive archive = ZipFile.Open(zipFileName, ZipArchiveMode.Create);

                    if (string.IsNullOrEmpty(subFolderName))
                        Log.WriteToLogFile($"INFO: ARCHIVED {Path.GetFileName(fi.Name)} to {Path.GetFileName(zipFileName)}", true);
                    else
                        Log.WriteToLogFile($"INFO: ARCHIVED {Path.GetFileName(fi.Name)} to ./{subFolderName}/{Path.GetFileName(zipFileName)}", true);

                    archive.CreateEntryFromFile(filename, Path.GetFileName(filename));
                }
                catch (Exception e)
                {
                    Log.WriteToLogFile($"ERROR! Caught error while archiving file: {filename}", true);
                    Log.WriteToLogFile($"ERROR:ArchiveAndZipFile: CAUGHT {e.Message}");
                }

                if (File.Exists(zipFileName)) // delete the original file now.
                {
                    File.Delete(filename);
                }
            }

        }
    }

}