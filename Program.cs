// Convert MS Access to Sqlite 
// Copyright (C) 2015  Andi Palo
// 
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation; either version 2
// of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.

﻿using System;
using mdb2sq3.Tests;

namespace mdb2sq3
{
    class Program
    {
        static int Main(string[] args)
        {
            /// TESTs
            /// 
            bool enableTest = false;
            if (enableTest)
            {
                UtilsTests.RunTableFindTest();
                UtilsTests.RunSwapRightJoinTest();
                return 0;
            }

            if (!CommandLineParametersHelper.ParseArguments(args))
            {
                return 1;
            }

            MSAccessBackend msbackend = new MSAccessBackend(CommandLineParametersHelper.databaseSource);
            string TargetFile = CommandLineParametersHelper.databaseTarget;
            if (null == TargetFile)
                TargetFile = System.IO.Path.ChangeExtension(CommandLineParametersHelper.databaseSource, "db");

            if (System.IO.File.Exists(TargetFile) && CommandLineParametersHelper.erase)
                System.IO.File.Delete(TargetFile);

            SQLiteBackend backend = new SQLiteBackend(TargetFile);

            // Explore the source database to get metadata.
            SchemaTablesMetaData schemaInfo = msbackend.QuerySchemaDefinition(null);
            foreach (TableMetaData t in schemaInfo.tables)
            {
                msbackend.QueryTableDefinition(t);
            }
            if (CommandLineParametersHelper.verbose)
            {
                Console.Out.WriteLine(schemaInfo.ToString());
                Console.Out.WriteLine("There are {0} tables to convert.", schemaInfo.tables.Count);
            }

            schemaInfo.SortTablesByDependencies();

            if (CommandLineParametersHelper.verbose)
            {
                Console.Out.WriteLine("Tables will be created in this order:");
                foreach (TableMetaData t in schemaInfo.tables)
                {
                    Console.Out.Write(t.tableName + " , ");
                }
                Console.Out.WriteLine();
            }

            // Clone source db schema into target db schema (no data)
            backend.CloneSchema(schemaInfo);

            // Copy data from source db to target db
            DateTime dt1 = DateTime.Now;
            foreach (TableMetaData table in schemaInfo.tables)
            {
                Console.Out.Write("Dumping table {0}", table.tableName);
                msbackend.DumpTableContents(table, backend);
            }
            TimeSpan sp = DateTime.Now - dt1;

            Console.Out.WriteLine("Complete dump performed in {0} ", sp);
            return 0;
        }
    }
}
