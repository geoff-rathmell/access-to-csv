using logging;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;

namespace mdbtocsv
{
    internal static class App_config
    {

        #region CUSTOM_ENUMS
        public enum QuoteIdentifier
        {
            None, DoubleQuote
        }

        public enum CSVDelimiter
        {
            comma, tab, pipe
        }

        public enum FileNameCase
        {
            none, lower, upper
        }
        #endregion

        #region CLASS_VARS

        public static string FileToProcess { get; set; } = string.Empty;
        public static bool EnableLogFile { get; set; } = true;
        public static bool DEBUGMODE { get; set; } = false;
        public static bool AllowOverWrite { get; set; } = true;
        public static string OutputDirectory { get; set; } = string.Empty;
        public static CSVDelimiter DelimiterToUse { get; set; }
        public static bool CleanFieldNames { get; set; } = false;
        public static FileNameCase FileNameCaseToUse { get; set; }
        public static int ExitCodeStatus { get; set; } = 0;
        public static bool AppendCreateDateToOutputFiles { get; set; } = false;
        public static bool AddFilenameAsOutputField { get; set; } = false;
        public static string TableFilterMask { get; set; } = string.Empty;
        public static string OptionsFilename {  get; set; } = string.Empty;

        private static bool IsConfigFileProcessed { get; set; } = false;

        #endregion


        /// <summary>
        /// Initialize Global Variables
        /// </summary>
        public static void InitApplicationVariables()
        {
            ExitCodeStatus = 0;
            FileToProcess = string.Empty;
            EnableLogFile = true;
            DEBUGMODE = false;
            AllowOverWrite = true;
            OutputDirectory = string.Empty;
            DelimiterToUse = CSVDelimiter.comma;
            CleanFieldNames = false;
            FileNameCaseToUse = FileNameCase.none;
            AppendCreateDateToOutputFiles = false;
            AddFilenameAsOutputField = false;
            TableFilterMask = string.Empty;
        }

        /// <summary>
        /// Loads given config file if present
        /// </summary>
        /// <param name="configFile">Full path and filename for config file to use.</param>
        public static void LoadConfigFile(string? configFile)
        {
            if (!string.IsNullOrWhiteSpace(configFile))
            {
                if (File.Exists(configFile) && !IsConfigFileProcessed)
                {
                    Log.WriteToLogFile($"{Environment.NewLine}# Using custom config file:{configFile.ToLower()}", true);

                    // TODO: add try catch in here

                    using StreamReader sr = new(configFile);
                    string line;
                    while ((line = sr.ReadLine()!) != null)
                    {
                        if (line.StartsWith('#') || line.Length == 0 || !line.Contains('='))
                            continue;

                        var lineDetails = line.Split('=');
                        bool boolResult;
                        Debug.WriteLine($"DEBUG: processing option:{lineDetails[0]}");

                        if (lineDetails[1] == string.Empty)
                            continue;

                        switch (lineDetails[0])
                        {

                            case "FILE_TO_PROCESS":
                                if (lineDetails[1].Length > 0)
                                {
                                    FileToProcess = lineDetails[1];
                                    Log.WriteToLogFile($"* config param: FILE_TO_PROCESS={lineDetails[1]}", true);
                                }
                                    
                                break;
                            case "OUTPUT_FOLDER":
                                if (lineDetails[1].Length > 0)
                                {
                                    OutputDirectory = lineDetails[1];
                                    Log.WriteToLogFile($"* config param: OUTPUT_FOLDER={lineDetails[1]}", true);
                                }
                                break;
                            case "ENABLE_LOG":
                                if (bool.TryParse(lineDetails[1], out boolResult))
                                {
                                    EnableLogFile = boolResult;
                                    Log.WriteToLogFile($"* config param: ENABLE_LOG={lineDetails[1]}", true);
                                }
                                break;
                            case "ENABLE_OVER_WRITE":
                                if (bool.TryParse(lineDetails[1], out boolResult))
                                {
                                    AllowOverWrite = boolResult;
                                    Log.WriteToLogFile($"* config param: ENABLE_OVER_WRITE={lineDetails[1]}", true);
                                }
                                break;
                            case "DELIMITER":
                                DelimiterToUse = lineDetails[1].ToLower() switch
                                {
                                    "comma" => CSVDelimiter.comma,
                                    "tab" => CSVDelimiter.tab,
                                    "pipe" => CSVDelimiter.pipe,
                                    _ => CSVDelimiter.comma,
                                };
                                Log.WriteToLogFile($"* config param: DELIMITER={lineDetails[1]}", true);
                                break;
                            case "ENABLE_CLEAN_HEADERS":
                                if (bool.TryParse(lineDetails[1], out boolResult))
                                {
                                    CleanFieldNames = boolResult;
                                    Log.WriteToLogFile($"* config param: ENABLE_CLEAN_HEADERS={lineDetails[1]}", true);
                                }
                                break;
                            case "OUTPUT_FILE_CASE":
                                FileNameCaseToUse = lineDetails[1].ToLower() switch
                                {
                                    "lower" => FileNameCase.lower,
                                    "upper" => FileNameCase.upper,
                                    _ => FileNameCase.none,
                                };
                                Log.WriteToLogFile($"* config param: OUTPUT_FILE_CASE={lineDetails[1]}", true);
                                break;
                            case "ENABLE_APPEND_DATE":
                                if (bool.TryParse(lineDetails[1], out boolResult))
                                {
                                    AppendCreateDateToOutputFiles = boolResult;
                                    Log.WriteToLogFile($"* config param: ENABLE_APPEND_DATE={lineDetails[1]}", true);
                                }
                                break;
                            case "ENABLE_ADD_FILENAME":
                                if (bool.TryParse(lineDetails[1], out boolResult))
                                {
                                    AddFilenameAsOutputField = boolResult;
                                    Log.WriteToLogFile($"* config param: ENABLE_ADD_FILENAME={lineDetails[1]}", true);
                                }
                                break;
                            case "TABLE_FILTER":
                                if (lineDetails[1].Length > 0)
                                {
                                    TableFilterMask = lineDetails[1];
                                    Log.WriteToLogFile($"* config param: TABLE_FILTER={lineDetails[1]}", true);
                                }
                                break;
                            default:
                                break;
                        }
                    }

                    IsConfigFileProcessed = true;
                }
            }
        }

    }
}
