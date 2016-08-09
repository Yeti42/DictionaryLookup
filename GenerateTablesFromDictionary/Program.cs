using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;

namespace GenerateTablesFromDictionary
{
    class Program
    {
        static void Main(string[] args)
        {
            DatabaseReaderWriter dbrw = new DatabaseReaderWriter();

            dbrw.ParseTestTrieFile(args[0], 1);

        }
    }
}
