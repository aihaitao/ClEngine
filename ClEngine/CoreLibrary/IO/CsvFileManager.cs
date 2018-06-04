using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ClEngine.CoreLibrary.IO.Csv;
using FlatRedBall;
using FlatRedBall.IO;

namespace ClEngine.CoreLibrary.IO
{
    public class CsvFileManager
    {
#if FRB_RAW
        public static string ContentManagerName = "Global";
#else
        public static string ContentManagerName = FlatRedBallServices.GlobalContentManager;
#endif
        public static char Delimiter = ',';

        public static List<object> CsvDeserializeList(Type typeOfElement, string fileName)
        {

            var listOfObjects = new List<object>();


            CsvDeserializeList(typeOfElement, fileName, listOfObjects);

            return listOfObjects;
        }

        public static void CsvDeserializeList(Type typeOfElement, string fileName, IList listToPopulate)
        {
            RuntimeCsvRepresentation rcr = CsvDeserializeToRuntime(fileName);

            rcr.CreateObjectList(typeOfElement, listToPopulate, ContentManagerName);
        }

        public static RuntimeCsvRepresentation CsvDeserializeToRuntime(string fileName)
        {
            return CsvDeserializeToRuntime<RuntimeCsvRepresentation>(fileName);
        }

        public static T CsvDeserializeToRuntime<T>(string fileName) where T : RuntimeCsvRepresentation, new()
        {
            if (FileManager.IsRelative(fileName))
            {
                fileName = FileManager.MakeAbsolute(fileName);
            }

#if ANDROID || IOS
			fileName = fileName.ToLowerInvariant();
#endif

            FileManager.ThrowExceptionIfFileDoesntExist(fileName);

            T runtimeCsvRepresentation = null;

            string extension = FileManager.GetExtension(fileName).ToLower();
            if (extension == "csv" || extension == "txt")
            {
#if SILVERLIGHT || XBOX360 || WINDOWS_PHONE || MONOGAME
                
                Stream stream = FileManager.GetStreamForFile(fileName);

#else
                FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
#endif
                runtimeCsvRepresentation = CsvDeserializeToRuntime<T>(stream);
                FileManager.Close(stream);
                stream.Dispose();


#if XBOX360
                if (FileManager.IsFileNameInUserFolder(fileName))
                {
                    FileManager.DisposeLastStorageContainer();
                }
#endif
            }
#if FRB_XNA
            else
            {
                if (extension != String.Empty)
                {
#if DEBUG
                    if (extension != "xnb")
                        throw new ArgumentException(string.Format("CSV files with extension '.{0}' are not supported", extension));
#endif
                    fileName = FileManager.RemoveExtension(fileName);
                }
                runtimeCsvRepresentation = FlatRedBallServices.Load<T>(fileName);
            }
#endif
            return runtimeCsvRepresentation;
        }

        public static T CsvDeserializeToRuntime<T>(Stream stream) where T : RuntimeCsvRepresentation, new()
        {
            T runtimeCsvRepresentation;

            using (var streamReader = new StreamReader(stream))
            using (var csv = new CsvReader(streamReader, true, Delimiter, CsvReader.DefaultQuote,
                CsvReader.DefaultEscape, CsvReader.DefaultComment, true, CsvReader.DefaultBufferSize))
            {
                runtimeCsvRepresentation = new T();

                var fileHeaders = csv.GetFieldHeaders();
                runtimeCsvRepresentation.Headers = new CsvHeader[fileHeaders.Length];

                for (var i = 0; i < fileHeaders.Length; i++)
                {
                    runtimeCsvRepresentation.Headers[i] = new CsvHeader(fileHeaders[i]);
                }

                var numberOfHeaders = runtimeCsvRepresentation.Headers.Length;

                runtimeCsvRepresentation.Records = new List<string[]>();

                var recordIndex = 0;
                var columnIndex = 0;
                string[] newRecord = null;
                try
                {
                    while (csv.ReadNextRecord())
                    {


                        newRecord = new string[numberOfHeaders];
                        if (recordIndex == 123)
                        {
                        }

                        var anyNonEmpty = false;
                        for (columnIndex = 0; columnIndex < numberOfHeaders; columnIndex++)
                        {
                            var record = csv[columnIndex];

                            newRecord[columnIndex] = record;
                            if (record != "")
                            {
                                anyNonEmpty = true;
                            }
                        }

                        if (anyNonEmpty)
                        {
                            runtimeCsvRepresentation.Records.Add(newRecord);
                        }

                        recordIndex++;
                    }
                }
                catch (Exception e)
                {
                    var message =
                        "Error reading record " + recordIndex + " at column " + columnIndex;

                    if (columnIndex != 0 && newRecord != null)
                    {
                        message = newRecord.Aggregate(message, (current, s) => current + ("\n" + s));
                    }

                    throw new Exception(message, e);

                }
            }

            return runtimeCsvRepresentation;
        }
    }
}