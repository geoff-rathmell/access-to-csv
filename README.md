# mdbtocsv

### Description

**mdbtocsv** is a Windows console app that reads an Access file (mdb|accdb) and exports all *user tables* to a csv file.

Code published from a Visual Studio C# project.

#### mdbtocsv is still work in progress.

### Prerequisites 
- 64 bit ODBC Access Driver installed.

### Usage

**mdbtocsv** -s:"sourceFileName" -noprompt -lower

The above example will extract all tables in source file as separate CSVs using ',' as default delimiter.

**PARAMETERS**:

|OPTION|DESCRIPTION|
|----- | ----- |
|-s:\<sourceFileName\> | (required) The Access file to process. Wrap in quotes if you have spaces in the path.|
|-nooverwrite | \(optional\) Disables file over-write of output file. (Not Implemented)|
|-o:\<directory\> | \(optional\) user specified output directory. Default = same path as source file.|
|-filter:\<table_filter\> | \(optional\) Filters output tables to match provided string. (case insensitive)|
|-nolog | \(optional\) disables the runtime log. Set this as very first parameter to fully disable log. (Not Implemented)|
|-lower | \(optional\) force output filenames to be lowercase.|
|-upper | \(optional\) force output filenames to be UPPERCASE.|
|--add-date | \(optional\) Appends source file create date to output file(s).|
|--add-filename | \(optional\) Adds source filename as last field in output file(s).|
|-p | \(optional\) use PIPE '\|' Delimiter in output file.|
|-t | \(optional\) use TAB Delimiter in output file.|
|-clean | \(optional\) Clean Fieldnames. Replaces all symbol chars with '_' and convert FieldName to UPPER case|
|-debug | \(optional\) enables debug 'verbose' mode.|


**Example**

mdbtocsv.exe "-s:c:\data\receipts.mdb" -p -c -lower

This parses all tables found in the receipts.mdb file, using the PIPE delimiter, cleaning the fieldnames, and writing the output filenames in lowercase.


### License and Contact
[MIT License](https://mit-license.org/)

mdbtocsv also uses the [csvhelper](https://joshclose.github.io/csvhelper/) library with its own license.

[Contact me](mailto:geoff@bentonvillebase.com) for any questions or concerns.
