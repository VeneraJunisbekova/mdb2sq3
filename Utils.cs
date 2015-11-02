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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace mdb2sq3
{
    class Utils
    {
        public static string SwapTablesOfRightJoin(string query)
        {
            string keyword = "right join";
            int index = query.IndexOf(keyword, StringComparison.OrdinalIgnoreCase);
            //Console.Out.WriteLine("Index is: " + index);
            if (index > 0)
            {
                Tuple<int, int> prev = FindFullTable(query, index, true);
                Tuple<int, int> next = FindFullTable(query, index + keyword.Length, false);

                string firstTable = query.Substring(prev.Item2, prev.Item1 - prev.Item2);
                string secondTable = query.Substring(next.Item1, next.Item2 - next.Item1);

                //Console.Out.WriteLine("Prev: " + prev.Item1 + "-" + prev.Item2 + " Next: " + next.Item1 + "-" + next.Item2);
                //Console.Out.WriteLine(firstTable + "|");
                //Console.Out.WriteLine(secondTable + "|");

                StringBuilder strBuilder = new StringBuilder(query);

                string newJoin = secondTable + " LEFT JOIN " + firstTable;

                strBuilder.Remove(prev.Item2, next.Item2 - prev.Item2);
                //Console.WriteLine("After remove: " + strBuilder.ToString());
                strBuilder.Insert(prev.Item2, newJoin);
                return SwapTablesOfRightJoin(strBuilder.ToString());
            }
            return query;
        }


        public static Tuple<int, int> FindFullTable(string query, int index, bool isBefore)
        {
            char c;
            bool isInside = false;
            bool isComposite = false;
            int counter = 0;

            int beginIndex = index;
            int endIndex = beginIndex;

            char beginChar = (isBefore) ? ')' : '(';
            char endChar = (isBefore) ? '(' : ')';

            while (true)
            {
                c = (isBefore) ? query[--endIndex] : query[++endIndex];
                if (c == beginChar) 
                { 
                    isComposite = true;
                    counter++;
                }

                if (c == endChar)
                {
                    counter--;
                    if (counter <= 0)
                    {
                        if (!isBefore && isComposite)
                            endIndex++;
                        break;
                    }
                }
                if (c != ' ')
                {
                    if (!isInside)
                    {
                        isInside = true;
                        beginIndex = endIndex + (isBefore ? + 1 : 0);
                    }
                }
                else
                {
                    if (isInside && !isComposite)
                    {
                        if (isBefore)
                            endIndex++;
                        break;
                    }
                }
            }
            
            return Tuple.Create(beginIndex,endIndex);
        }
    }
}
