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
            Int32 verID = 0;
            string pwd = "";
            string filename = "";
            string verName = "";
            string languageName = "en-US";
            bool upload = false;


            for (int i = 0; i < args.Length; i++)
            {
                if ((args[i][0] == '-') || (args[i][0] == '/'))
                {
                    switch (args[i][1])
                    {
                        case 'v':
                            verID = Int32.Parse(args[++i]);
                            break;
                        case 'p':
                            pwd = args[++i];
                            break;
                        case 'l':
                            languageName = args[++i];
                            break;
                        case 'n':
                            verName = args[++i];
                            break;
                        case 'u':
                            upload = true;
                            break;
                    }
                }
                else
                {
                    filename = args[i];
                }
            }

            if(filename.Length == 0)
            {
                return;
            }

            DatabaseReaderWriter dbrw = new DatabaseReaderWriter(false);
            dbrw.SetVersionDictionaryID(verID, verName, languageName);
            dbrw.ParseTestTrieFile(filename);
            if (upload) dbrw.UploadNewTableEntries();
        }
    }
}
