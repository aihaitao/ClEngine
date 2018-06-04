namespace ClEngine.Core.ProjectCreator
{
    public class PlatformProjectInfo
    {
        public string FriendlyName;
        public string Namespace;
        public string ZipName;
        public string Url;
        public bool SupportedInGlue;

        public override string ToString()
        {
            return FriendlyName;
        }
    }
}