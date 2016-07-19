namespace DictionaryLookup.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using DictionaryLookup.Models;
    using System.IO;
    using System.Collections;
    using System.Collections.Generic;

    internal sealed class Configuration : DbMigrationsConfiguration<DictionaryLookup.Models.DictionaryLookupContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DictionaryLookup.Models.DictionaryLookupContext context)
        {
            // Deletes all data, from all tables, except for __MigrationHistory
            context.Database.ExecuteSqlCommand("sp_MSForEachTable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL'");
            context.Database.ExecuteSqlCommand("sp_MSForEachTable 'IF OBJECT_ID(''?'') NOT IN (ISNULL(OBJECT_ID(''[dbo].[__MigrationHistory]''),0)) DELETE FROM ?'");
            context.Database.ExecuteSqlCommand("EXEC sp_MSForEachTable 'ALTER TABLE ? CHECK CONSTRAINT ALL'");
            
            List<DictionaryWord> wordsToAdd = new List<DictionaryWord>();

            int count = 0;
            using (FileStream fs = File.OpenRead(@"S:\RS\dev\base\win32\winnls\ELS\AdvancedServices\Spelling\nlg7\spellers\dictionaries\english_united_states\dump.txt"))
            using (TextReader tr = new StreamReader(fs))
            {
                List<string> continueStrings = new List<string>();
                List<Int32> continueCosts = new List<int>();
                while (tr.Peek() > -1)
                {
                    string line = tr.ReadLine();

                    // Unwrap the continue costs back so that the last one matches the current label
                    while ((continueStrings.Count > 0) && (!line.StartsWith(continueStrings.Last())))
                    {
                        continueStrings.Remove(continueStrings.Last());
                        continueCosts.Remove(continueCosts.Last());
                    }

                    if (!line.Contains("\t#"))
                    {
                        // Valid word. Process and write to the console.
                        if (continueStrings.Count > 0)
                        {
                            if (line.Contains("\t1=0x") || line.Contains("|1=0x"))
                            {
                                // Extract the stop cost and add to the current continue cost
                                string tag1ValueString = line.Substring(line.IndexOf("1=0x"), 12);
                                Int32 stopCost = continueCosts.Last() + Int32.Parse(tag1ValueString.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
                                // Rewrite the total back to the stop cost
                                line = line.Replace(tag1ValueString,
                                    tag1ValueString.Substring(0, 6)
                                    + Convert.ToString(stopCost, 16).PadLeft(2, '0')
                                    + tag1ValueString.Substring(8, 4));
                            }
                            else
                            {
                                if (!line.Contains("\t"))
                                {
                                    line = line + "\t|1=0x00" + Convert.ToString(continueCosts.Last(), 16).PadLeft(2, '0') + "0000";
                                }
                                else
                                {
                                    line = line + "\t1=0x00" + Convert.ToString(continueCosts.Last(), 16).PadLeft(2, '0') + "0000";

                                }
                            }
                        }
                        // Dialect filter
                        // US English only for now
                        if (line.Contains("\t0=0x") | line.Contains("\t|0=0x"))
                        {
                            string tags = line.Substring(line.IndexOf("|") + 1);
                            Int32 dialectTag = Int32.Parse(tags.Substring(tags.IndexOf("0=0x") + 10, 2), System.Globalization.NumberStyles.HexNumber);
                            if ((dialectTag & 1) != 1)
                            {
                                // Has a dialect tag and it does not have bit 1 set.
                                continue;
                            }
                        }

                        wordsToAdd.Add(new DictionaryWord(line));
                        if (count++ > 400000)
                        {
                            break;
                        }

                    }

                    // Update the continue node state
                    if (line.Contains("\t1=") || line.Contains("|1=") | line.Contains("#1="))
                    {
                        Int32 continueCost = Int32.Parse(line.Substring(line.IndexOf("1=0x") + 10, 2), System.Globalization.NumberStyles.HexNumber);
                        if (continueCosts.Count > 0)
                            continueCost += continueCosts.Last();
                        continueStrings.Add(line.Substring(0, line.IndexOf('\t')));
                        continueCosts.Add(continueCost);
                    }
                }
            }
            context.DictionaryWords.AddRange(wordsToAdd);
        }
    }
}
