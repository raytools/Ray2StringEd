using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using CsvHelper;

namespace Ray2StringEd
{
    public static class CsvUtils
    {
        public static void ImportCsv(IEnumerable<FixString> strings, string path)
        {
            Dictionary<long, FixString> dStrings = strings.ToDictionary(s => s.Offset);
            int errorCounter = 0;
            int importedCounter = 0;

            using (StreamReader reader = new StreamReader(path))
            using (CsvReader csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Configuration.RegisterClassMap<FixStringMap>();
                csv.Read();
                csv.ReadHeader();
                while (csv.Read())
                {
                    FixString imported = csv.GetRecord<FixString>();

                    try
                    {
                        if (!dStrings.TryGetValue(imported.Offset, out FixString current))
                            throw new Exception("Offset not found.");

                        if (imported.DwordLength != current.DwordLength)
                            throw new Exception(
                                $"DWORD length does not match.\nExpected {current.DwordLength}, got {imported.DwordLength}.");

                        if (imported.Text.Length > current.MaxTextLength)
                            throw new Exception(
                                $"String is too long.\nExpected max {current.MaxTextLength} chars, got {imported.Text.Length} chars.");

                        current.Text = imported.Text;
                        importedCounter++;
                    }
                    catch (Exception e)
                    {
                        errorCounter++;

                        string error = $"Offset 0x{imported.Offset:X}:\n" +
                                         e.Message +
                                         "\nOK - skip current string and continue importing" +
                                         "\nCancel - Stop importing";

                        MessageBoxResult result = MessageBox.Show(error, "CSV Import Error",
                            MessageBoxButton.OKCancel, MessageBoxImage.Error, MessageBoxResult.Cancel);

                        if (result == MessageBoxResult.Cancel) break;
                    }
                }
            }

            string message = $"Imported {importedCounter} strings.\n" +
                             $"Errors: {errorCounter}\n" +
                             $"Total strings in CSV file: {importedCounter + errorCounter}\n" +
                             $"Total strings in Fix: {dStrings.Count}";
            MessageBox.Show(message, "CSV Import", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        public static void ExportCsv(IEnumerable<FixString> strings, string path)
        {
            using (StreamWriter writer = new StreamWriter(path, false))
            using (CsvWriter csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.Configuration.RegisterClassMap<FixStringMap>();
                csv.WriteRecords(strings);
            }
        }
    }
}