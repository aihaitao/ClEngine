using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using ClEngine.Properties;
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedParameter.Local

namespace ClEngine.CoreLibrary.IO
{
    public enum RelativeType
    {
        Relative,
        Absolute
    }

    public static class FileManager
    {
        internal static Dictionary<int, string> MRelativeDirectoryDictionary = new Dictionary<int, string>();

        public static string DefaultRelativeDirectory =
            (Application.StartupPath + "/").ToLowerInvariant().Replace("\\", "/");

        public static bool PreserveCase { get; set; }

        public static string GetExtension(string fileName)
        {
            if (fileName == null)
                return "";

            var i = fileName.LastIndexOf('.');
            if (i != -1)
            {
                var hasDotSlash = i < fileName.Length + 1 && (fileName[i + 1] == '/' || fileName[i + 1] == '\\');

                var hasSlashAfterDot = i < fileName.LastIndexOf("/", StringComparison.Ordinal) ||
                                       i < fileName.LastIndexOf(@"\", StringComparison.Ordinal);

                if (hasDotSlash || hasSlashAfterDot)
                    return "";

                return fileName.Substring(i + 1, fileName.Length - (i + 1)).ToLowerInvariant();
            }

            return "";
        }

        public static List<string> GetAllFilesInDirectory(string directory, string fileType)
        {
            return GetAllFilesInDirectory(directory, fileType, int.MaxValue);
        }

        public static List<string> GetAllFilesInDirectory(string directory, string fileType, int depthToSearch)
        {
            var arrayToReturn = new List<string>();

            GetAllFilesInDirectory(directory, fileType, depthToSearch, arrayToReturn);

            return arrayToReturn;
        }

        public static void GetAllFilesInDirectory(string directory, string fileType, int depthToSearch,
            List<string> arrayToReturn)
        {
#if SILVERLIGHT || WINDOWS_8
            throw new NotImplementedException();
#endif

            if (!Directory.Exists(directory))
                return;

            if (directory == "")
                directory = RelativeDirectory;

            if (directory.EndsWith(@"\") == false && directory.EndsWith("/") == false)
                directory += @"\";

            if (!string.IsNullOrEmpty(fileType) && fileType[0] == '.')
                fileType = fileType.Substring(1);

            var files = Directory.GetFiles(directory);
            var directories = Directory.GetDirectories(directory);

            if (string.IsNullOrEmpty(fileType))
                arrayToReturn.AddRange(files);
            else
            {
                var fileCount = files.Length;

                for (int i = 0; i < fileCount; i++)
                {
                    var file = files[i];
                    if (GetExtension(file) == fileType)
                    {
                        arrayToReturn.Add(file);
                    }
                }
            }

            if (depthToSearch > 0)
            {
                var directoryCount = directories.Length;
                for (int i = 0; i < directoryCount; i++)
                {
                    var directoryChecking = directories[i];

                    GetAllFilesInDirectory(directoryChecking, fileType, depthToSearch - 1, arrayToReturn);
                }
            }
        }

        public static void ThrowExceptionIfFileDoesntExist(string fileName)
        {
            var fileToCheck = fileName;

            if (FileExists(fileToCheck) == false)
            {


#if WINDOWS_8
                throw new FileNotFoundException("Could not find the file " + fileName);
#else
                
                var directory = GetDirectory(fileName);


                if (IsRelative(directory))
                {
                    directory = MakeAbsolute(directory);
                }

                if (Directory.Exists(directory))
                {
                    var fileNameAsXnb = RemoveExtension(fileName) + ".xnb";

                    if (File.Exists(fileNameAsXnb))
                    {
                        throw new FileNotFoundException(
                            $"{Resources.CantFindFile}\n{fileName}\n{Resources.ButFindXnbFile}\n{fileNameAsXnb}.\n{Resources.IsLoadFromPipelineIfTryLoadExtension}");
                    }
                    else
                    {
#if MONODROID

                        FileNotFoundException fnfe = new FileNotFoundException("Could not find the " +
                            "file " + fileName + " but found the directory " + directory +
                            "  Did you type in the name of the file wrong?");
#else

                        FileNotFoundException fnfe = new FileNotFoundException(
                            $"{Resources.CantFindFile} {fileName}{Resources.ButFindDirectory} {directory} {Resources.TypeNameWrong}?",
                            fileName);
#endif
                        throw fnfe;
                    }
                }
                else
                {
#if MONODROID

                    throw new FileNotFoundException("Could not find the " +
                        "file " + fileName + " or the directory " + directory);
#else
                    throw new FileNotFoundException($"{Resources.CantFindFile} {fileName} {Resources.OrDirectory} {directory}", fileName);
#endif
                }

#endif
            }
        }

        public static string RelativeDirectory
        {
            get
            {
#if WINDOWS_8 || UWP
                int threadID = Environment.CurrentManagedThreadId;
#else
                int threadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
#endif

                lock (MRelativeDirectoryDictionary)
                {
                    if (MRelativeDirectoryDictionary.ContainsKey(threadId))
                        return MRelativeDirectoryDictionary[threadId];
                    return DefaultRelativeDirectory;
                }
            }
            set
            {
                if (IsRelative(value))
                    throw new InvalidOperationException(Resources.RelativeMustAbsolute);

                var valueToSet = value;
#if USES_DOT_SLASH_ABOLUTE_FILES
// On the Xbox 360 the way to specify absolute is to put a '/' before
// a file name.
                if (value.Length > 1 && (value[0] != '.' || value[1] != '/'))
                {
                    valueToSet = Standardize("./" + value.Replace("\\", "/"));
                }
                else if (value.Length == 0)
                {
                    valueToSet = "./";
                }
                else
                {
                    valueToSet = Standardize(value.Replace("\\", "/"));
                }

#else
                ReplaceSlashes(ref valueToSet);
                valueToSet = Standardize(valueToSet, "", false);
#endif

                if (!string.IsNullOrEmpty(valueToSet) && !valueToSet.EndsWith("/"))
                    valueToSet += "/";

#if WINDOWS_8 || UWP
                int threadID = Environment.CurrentManagedThreadId;
#else

                int threadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
#endif

                lock (MRelativeDirectoryDictionary)
                {
                    if (valueToSet == DefaultRelativeDirectory)
                    {
                        if (MRelativeDirectoryDictionary.ContainsKey(threadId))
                            MRelativeDirectoryDictionary.Remove(threadId);
                    }
                    else
                    {
                        if (MRelativeDirectoryDictionary.ContainsKey(threadId))
                            MRelativeDirectoryDictionary[threadId] = valueToSet;
                        else
                            MRelativeDirectoryDictionary.Add(threadId, valueToSet);
                    }
                }
            }
        }

        public static string Standardize(string fileNameToFix, string relativePath, bool makeAbsolute)
        {
            if (fileNameToFix == null)
                return null;

            var isNetwork = fileNameToFix.StartsWith("\\\\");

            ReplaceSlashes(ref fileNameToFix);

            if (makeAbsolute && !isNetwork)
            {
                if (IsRelative(fileNameToFix))
                {
                    fileNameToFix = relativePath + fileNameToFix;
                    ReplaceSlashes(ref fileNameToFix);
                }
            }

#if !XBOX360
            fileNameToFix = RemoveDotDotSlash(fileNameToFix);

            if (fileNameToFix.StartsWith("..") && makeAbsolute)
            {
                throw new InvalidOperationException($"{Resources.TryRemoveAllButEnd}: {fileNameToFix}");
            }
#endif

            fileNameToFix = fileNameToFix.Replace("//", "/");

#if !MONODROID && !IOS
            if (!PreserveCase)
            {
                fileNameToFix = fileNameToFix.ToLowerInvariant();
            }
#endif

            return fileNameToFix;
        }

        public static string RemoveDotDotSlash(string fileNameToFix)
        {
            if (fileNameToFix.Contains(".."))
            {
                fileNameToFix = fileNameToFix.Replace("\\", "/");

                var indexOfNextDotDotSlash = GetDotDotSlashIndex(fileNameToFix);

                var shouldLoop = indexOfNextDotDotSlash > 0;

                while (shouldLoop)
                {
                    indexOfNextDotDotSlash++;

                    var indexOfPreviousDirectory =
                        fileNameToFix.LastIndexOf('/', indexOfNextDotDotSlash - 2, indexOfNextDotDotSlash - 2);

                    fileNameToFix = fileNameToFix.Remove(indexOfPreviousDirectory + 1,
                        indexOfNextDotDotSlash - indexOfPreviousDirectory + 2);

                    indexOfNextDotDotSlash = GetDotDotSlashIndex(fileNameToFix);

                    shouldLoop = indexOfNextDotDotSlash > 0;
                }
            }

            if (fileNameToFix.Contains("/./"))
                fileNameToFix = fileNameToFix.Replace("/./", "/");

            if (fileNameToFix.Contains("\\.\\"))
                fileNameToFix = fileNameToFix.Replace("\\.\\", "\\");

            if (fileNameToFix.Contains("/.\\"))
                fileNameToFix = fileNameToFix.Replace("/.\\", "/");

            if (fileNameToFix.Contains("\\./"))
                fileNameToFix = fileNameToFix.Replace("\\./", "\\");

            return fileNameToFix;
        }

        private static int GetDotDotSlashIndex(string fileNameToFix)
        {
            var indexOfNextDotDotSlash = fileNameToFix.LastIndexOf("/../", StringComparison.Ordinal);

            while (indexOfNextDotDotSlash > 0 && fileNameToFix[indexOfNextDotDotSlash - 1] == '.')
            {
                indexOfNextDotDotSlash = fileNameToFix.LastIndexOf("/../", indexOfNextDotDotSlash, StringComparison.Ordinal);
            }

            return indexOfNextDotDotSlash;
        }

        internal static void ReplaceSlashes(ref string stringToReplace)
        {
            var isNetwork = false;
            if (stringToReplace.StartsWith("\\\\"))
            {
                stringToReplace = stringToReplace.Substring(2);
                isNetwork = true;
            }

            stringToReplace = stringToReplace.Replace("\\", "/");

            if (isNetwork)
                stringToReplace = "\\\\" + stringToReplace;
        }

        public static bool IsRelative(string fileName)
        {
            if (fileName == null)
                throw new ArgumentException(Resources.CantCheckNameIsRelative);

#if USES_DOT_SLASH_ABOLUTE_FILES
            if (fileName.Length > 1 && fileName[0] == '.' && fileName[1] == '/')
            {
                return false;
            }
            // let the leading forward slash be treated as absolute:
            else if (fileName.Length > 0 && fileName[0] == '/')
            {
                return false;
            }
            // If it's isolated storage, then it's not relative:
            else if (fileName.Contains(IsolatedStoragePrefix))
            {
                return false;
            }
            else
            {
                return true;
            }
#else
            return !Path.IsPathRooted(fileName);
#endif
        }

        public static string RemoveExtension(string fileName)
        {
            var extensionLength = GetExtension(fileName).Length;

            if (extensionLength == 0)
                return fileName;

            if (fileName.Length > extensionLength && fileName[fileName.Length - (extensionLength + 1)] == '.')
                return fileName.Substring(0, fileName.Length - (extensionLength + 1));

            return fileName;
        }

        public static string RemovePath(string fileName)
        {
            RemovePath(ref fileName);

            return fileName;
        }

        public static void RemovePath(ref string fileName)
        {
            var indexOf1 = fileName.LastIndexOf('/', fileName.Length - 1, fileName.Length);
            if (indexOf1 == fileName.Length - 1 && fileName.Length > 1)
            {
                indexOf1 = fileName.LastIndexOf('/', fileName.Length - 2, fileName.Length - 1);
            }

            var indexOf2 = fileName.LastIndexOf('\\', fileName.Length - 1, fileName.Length);
            if (indexOf2 == fileName.Length - 1 && fileName.Length > 1)
            {
                indexOf2 = fileName.LastIndexOf('\\', fileName.Length - 2, fileName.Length - 1);
            }


            if (indexOf1 > indexOf2)
                fileName = fileName.Remove(0, indexOf1 + 1);
            else if (indexOf2 != -1)
                fileName = fileName.Remove(0, indexOf2 + 1);
        }

        public static string FromFileText(string fileName)
        {
#if SILVERLIGHT
            string containedText;

            Uri uri = new Uri(fileName, UriKind.Relative);

            StreamResourceInfo sri = Application.GetResourceStream(uri);
            Stream stream = sri.Stream;
            StreamReader reader = new StreamReader(stream);

            containedText = reader.ReadToEnd();

            stream.Close();
            reader.Close();
            
            return containedText;

#else

            if (IsRelative(fileName))
            {
                fileName = MakeAbsolute(fileName);
            }

            fileName = Standardize(fileName);

            string containedText;

            Stream fileStream = null;
            try
            {
#if WINDOWS
                fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
#else
                fileStream = GetStreamForFile(fileName);

#endif
                using (StreamReader sr = new StreamReader(fileStream))
                {
                    containedText = sr.ReadToEnd();
                    Close(sr);
                }

            }
            finally
            {
                Close(fileStream);

            }


            return containedText;
#endif
        }

        public static Stream GetStreamForFile(string fileName)
        {
            return GetStreamForFile(fileName, FileMode.Open);
        }

        public static Stream GetStreamForFile(string fileName, FileMode mode)
        {
            if (IsRelative(fileName))
            {
                fileName = RelativeDirectory + fileName;
            }

#if IOS || ANDROID
            fileName = fileName.ToLowerInvariant();
#endif


            if (fileName.StartsWith("./"))
            {
                fileName = fileName.Substring(2);
            }
#if USES_DOT_SLASH_ABOLUTE_FILES && !IOS
            if (fileName.Length > 1 && fileName[0] == '.' && fileName[1] == '/')
                fileName = fileName.Substring(2);



            if (fileName.Contains(IsolatedStoragePrefix) || fileName.Contains(IsolatedStoragePrefix.ToLowerInvariant()))
            {
                fileName = GetIsolatedStorageFileName(fileName);

#if WINDOWS_8 || UWP
                throw new NotImplementedException();
#else
                IsolatedStorageFileStream isfs = new IsolatedStorageFileStream(fileName, mode, mIsolatedStorageFile);

                stream = isfs;
#endif
            }
            else
            {


#if ANDROID || WINDOWS_8 || IOS || UWP
                stream = TitleContainer.OpenStream(fileName);
#else

                fileName = fileName.Replace("\\", "/");
                Uri uri = new Uri(fileName, UriKind.Relative);

                StreamResourceInfo sri = Application.GetResourceStream(uri);

                if (sri == null)
                {

                    throw new Exception("Could not find the file " +
                        fileName + ".  Did you add " + fileName + " to " +
                        "your project and set its 'Build Action' to 'Content'?");
                }

                stream = sri.Stream;
#endif
            }
#else

            Stream stream = File.OpenRead(fileName);
#endif

            return stream;
        }

        public static void Close(Stream stream)
        {
#if WINDOWS_8 || UWP
// Close was removed - no need to do anything
#else
            stream.Close();
#endif
        }

        public static void Close(StreamReader streamReader)
        {
#if WINDOWS_8 || UWP
// Close was removed - no need to do anything
#else
            streamReader.Close();
#endif
        }

        private static void Close(BinaryWriter writer)
        {
#if WINDOWS_8 || UWP
// Close was removed - no need to do anything
#else
            writer.Close();
#endif
        }

        private static void Close(TextWriter writer)
        {
#if WINDOWS_8 || UWP
// Close was removed - no need to do anything
#else
            writer.Close();
#endif
        }

        public static void Close(TextReader writer)
        {
#if WINDOWS_8 || UWP
// Close was removed - no need to do anything
#else
            writer.Close();
#endif
        }

        public static string MakeAbsolute(string pathToMakeAbsolute)
        {
            if (IsRelative(pathToMakeAbsolute) == false)
            {
                throw new ArgumentException($"{Resources.PathIsAbsolute}: {pathToMakeAbsolute}");
            }

            return Standardize(pathToMakeAbsolute);
        }

        public static string Standardize(string fileNameToFix)
        {
            return Standardize(fileNameToFix, RelativeDirectory);
        }

        public static string Standardize(string fileNameToFix, string relativePath)
        {
            return Standardize(fileNameToFix, relativePath, true);
        }

        public static void SaveText(string stringToSave, string fileName)
        {
            SaveText(stringToSave, fileName, System.Text.Encoding.UTF8);

        }

        private static void SaveText(string stringToSave, string fileName, System.Text.Encoding encoding)
        {
            fileName = fileName.Replace("/", "\\");
            
#if WINDOWS
            if (!string.IsNullOrEmpty(FileManager.GetDirectory(fileName)) &&
                !Directory.Exists(FileManager.GetDirectory(fileName)))
            {
                Directory.CreateDirectory(FileManager.GetDirectory(fileName));
            }

            System.IO.File.WriteAllText(fileName, stringToSave);
            return;
#endif

#if MONOGAME && !DESKTOP_GL


            if (!fileName.Contains(IsolatedStoragePrefix))
            {
                throw new ArgumentException("You must use isolated storage.  Use FileManager.GetUserFolder.");
            }

            fileName = FileManager.GetIsolatedStorageFileName(fileName);

#if WINDOWS_8 || IOS || UWP
            throw new NotImplementedException();
#else
            IsolatedStorageFileStream isfs = null;

            isfs = new IsolatedStorageFileStream(
                fileName, FileMode.Create, mIsolatedStorageFile);

            writer = new StreamWriter(isfs);
#endif

#else
            if (!string.IsNullOrEmpty(GetDirectory(fileName)) &&
                !Directory.Exists(GetDirectory(fileName)))
            {
                Directory.CreateDirectory(GetDirectory(fileName));
            }


            var fileInfo = new FileInfo(fileName);

            var writer = fileInfo.CreateText();



#endif

            using (writer)
            {
                writer.Write(stringToSave);

                Close(writer);
            }

#if MONODROID
            isfs.Close();
            isfs.Dispose();
#endif
        }

        public static string GetDirectory(string fileName)
        {
            return GetDirectory(fileName, RelativeType.Absolute);
        }

        public static string GetDirectory(string fileName, RelativeType relativeType)
        {
            var lastIndex = Math.Max(
                fileName.LastIndexOf('/'), fileName.LastIndexOf('\\'));

            string directoryToReturn;

            if (lastIndex == fileName.Length - 1)
            {
                lastIndex = Math.Max(
                    fileName.LastIndexOf('/', fileName.Length - 2),
                    fileName.LastIndexOf('\\', fileName.Length - 2));
            }

            if (lastIndex != -1)
            {
#if !MONOGAME
                var isFtp = FtpManager.IsFtp(fileName);
#endif

                if (IsUrl(fileName) || isFtp)
                {
                    directoryToReturn = fileName.Substring(0, lastIndex + 1);

                }
                else
                {
                    directoryToReturn = relativeType == RelativeType.Absolute
                        ? Standardize(fileName.Substring(0, lastIndex + 1))
                        : Standardize(fileName.Substring(0, lastIndex + 1), "", false);
                }
            }
            else
            {
                directoryToReturn = "";
            }

            return directoryToReturn;
        }

        public static bool IsUrl(string fileName)
        {
            return fileName.IndexOf("http:", StringComparison.Ordinal) == 0;
        }

        public static string UserApplicationDataForThisApplication
        {
            get
            {
                var assembly = Assembly.GetEntryAssembly();

                var applicationDataName = assembly == null ? "" : Assembly.GetEntryAssembly().FullName;
                applicationDataName = string.IsNullOrEmpty(applicationDataName)
                    ? @"FRBDefault"
                    : applicationDataName.Substring(0, applicationDataName.IndexOf(','));

#if IOS
                
                var documents = Environment.GetFolderPath (Environment.SpecialFolder.MyDocuments);
                string folder = Path.Combine (documents, "..", "Library");

                folder = FileManager.RemoveDotDotSlash(folder);

                //// Make it absolute:
                // actually, leading / is now absolute:
                //folder = "." + folder;
#else
                var folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
#endif
                folder = folder + @"\" + applicationDataName + @"\";

#if IOS
                folder = folder.Replace('\\', '/');
#endif
                return folder;

            }
        }

        public static bool FileExists(string fileName)
        {
            if (IsRelative(fileName))
            {
                return FileExists(MakeAbsolute(fileName));
            }
#if USE_ISOLATED_STORAGE
                bool isIsolatedStorageFile = IsInIsolatedStorage(fileName);

                if (isIsolatedStorageFile)
                {
                    return FileExistsInIsolatedStorage(fileName);
                }
                else
                {

                    if (fileName.Length > 1 && fileName[0] == '.' && fileName[1] == '/')
                        fileName = fileName.Substring(2);
                    fileName = fileName.Replace("\\", "/");


                    // I think we can make this to-lower on iOS and Android so we don't have to spread to-lowers everywhere else:
                    fileName = fileName.ToLowerInvariant();

#if ANDROID
                    // We may be checking for a file outside of the title container
                    if (System.IO.File.Exists(fileName))
                    {
                        return true;
                    }
#endif


                    Stream stream = null;
                    // This method tells us if a file exists.  I hate that we have 
                    // to do it this way - the TitleContainer should have a FileExists
                    // property to avoid having to do logic off of exceptions.  <sigh>
                    try
                    {
                        stream = TitleContainer.OpenStream(fileName);
                    }
#if MONODROID
                    catch (Java.IO.FileNotFoundException fnfe)
                    {
                        return false;
                    }
#else
                    catch (FileNotFoundException fnfe)
                    {
                        return false;
                    }
#endif

                    if (stream != null)
                    {
                        stream.Dispose();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
#else

            if (fileName.Length > 1 && fileName[0] == '.' && fileName[1] == '/')
                fileName = fileName.Substring(2);

            return File.Exists(fileName);
#endif
        }
    }
}