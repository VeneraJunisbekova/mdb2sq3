mdb2sq3
=
This is a simple tool to convert MS Access DB into Sqlite databases.
You can find the original work [here](http://mdb2sq3.codeplex.com/).
The tool also tries to port the views of MS Access by converting the
right joins into left joins with some string manipulations.

###Build 
I have tested it in Visual Studio 2010.
You will need the SQLite ADO.NET library in order to compile and use this project. You can find it [here](http://sourceforge.net/projects/sqlite-dotnet2/)

###How To Use
This is a console application, just type:
`msql2db3 -?`
to display the help

mdb2sq3 -s:sourcefile [-t:targetfile] [options]
-s:sourcefile   Sets the source MSAccess database
-t:targetfile   Sets the target SQLite database
-e              Forces deletion of target file if it exists
-v              Verbose mode.
-?              Prints this help and exits the program.

####Examples
`mdb2sq3 -s:"c:\db\accessdb.mdb"`
converts the database into sqlite and saves it on the file accessdb.db.

`mdb2sq3 -s:"c:\db\accessdb.mdb" -t:"c:\db\converted_acc.db"`
creates a file named "c:\db\converted_acc.db" to store the sqlite database.
