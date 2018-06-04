using ClEngine.Properties;

namespace ClEngine.CoreLibrary.Content
{
    public class SourceReferencingFile
    {
        public string SourceFile;
        public string DestinationFile;


        public string ObjectName;
        public SourceReferencingFile(string sourceFile, string destinationFile)
        {
            SourceFile = sourceFile;
            DestinationFile = destinationFile;
        }

        public bool HasTheSameFilesAs(SourceReferencingFile otherReferencingFile)
        {
            return SourceFile == otherReferencingFile.SourceFile &&
                   DestinationFile == otherReferencingFile.DestinationFile;
        }

        public override string ToString()
        {
            return $"{Resources.Source}: {SourceFile}, {Resources.Destination}: {DestinationFile}";
        }
    }
}