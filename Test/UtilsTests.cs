using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace mdb2sq3.Tests
{
    class UtilsTests
    {
        public static void RunTableFindTest()
        {
            PrintBeginTest("Find Full Table");
            string testQuery = "I am (leg (asdf) end) RIGHT JOIN some other text";

            Tuple<int,int> result = Utils.FindFullTable(testQuery, 4, false);
            string table = testQuery.Substring(result.Item1, result.Item2 - result.Item1);
            Console.WriteLine(table);
            Debug.Assert(table.Equals("(leg (asdf) end)"),table);

            result = Utils.FindFullTable(testQuery, 22, true);
            table = testQuery.Substring(result.Item2, result.Item1 - result.Item2);
            Console.WriteLine(table);
            Debug.Assert(table.Equals("(leg (asdf) end)"), table);

            result = Utils.FindFullTable(testQuery, 21, false);
            table = testQuery.Substring(result.Item1, result.Item2 - result.Item1);
            Console.WriteLine(table);
            Debug.Assert(table.Equals("RIGHT"), table);

            result = Utils.FindFullTable(testQuery, 28, true);
            table = testQuery.Substring(result.Item2, result.Item1 - result.Item2);
            Console.WriteLine(table);
            Debug.Assert(table.Equals("RIGHT"), table);

            PrintEndTest("Find Composite");
        }

        public static void RunSwapRightJoinTest()
        {
            PrintBeginTest("Swap");
            string input = "The following test will (do a now RIGHT JOIN back) RIGHT JOIN outer table";
            string test = "The following test will outer LEFT JOIN (do a back LEFT JOIN now) table";
            string output = Utils.SwapTablesOfRightJoin(input);
            Console.Out.WriteLine(input);
            Console.Out.WriteLine(output);

            Debug.Assert(output.Equals(test));

            PrintEndTest("Swap");
        }

        private static void PrintBeginTest(string name)
        {
            Console.WriteLine("---------------  Begin Test " + name + "-------------------");
        }
        private static void PrintEndTest(string name)
        {
            Console.WriteLine("_______________  End Test " + name + " ____________________\n\n");
        }
    }
}
