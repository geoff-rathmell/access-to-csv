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
        public static bool GenerateLogFile { get; set; } = true;
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

        #endregion


        /// <summary>
        /// Initialize Global Variables
        /// </summary>
        public static void InitApplicationVariables()
        {
            ExitCodeStatus = 0;
            FileToProcess = string.Empty;
            GenerateLogFile = true;
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

        //TODO: Add ability to load these options from a file if it is present in the app directory


    }
}
