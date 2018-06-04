using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using EditorObjects.Collections;
using EditorObjects.SaveClasses;
using FlatRedBall.IO;

namespace ClEngine.CoreLibrary.SaveClasses
{
    public class FileProgramAssociations
    {
        public string Extension;
        public string DefaultProgram;
    }

    public class SettingsSave
    {
        public string LastProjectFile { get; set; }

        public static string SettingsFileName
        {
            get
            {
                if (StopSavesAndLoads)
                    return "";

                return FileManager.UserApplicationDataForThisApplication +
                       "settings.xml";
            }
        }

        [XmlElement("Association")]
        public ExternalSeparatingList<FileProgramAssociations> Associations = new ExternalSeparatingList<FileProgramAssociations>();

        public ExternalSeparatingList<BuildToolAssociation> BuildToolAssociations = new ExternalSeparatingList<BuildToolAssociation>();

        public List<ProjectFileFilePair> LocationSpecificLastProjectFiles
        {
            get;
            set;
        } = new List<ProjectFileFilePair>();

        [XmlIgnore]
        public static bool StopSavesAndLoads { get; set; }

        public void Save()
        {
            if (StopSavesAndLoads) return;

            string directory = FileManager.GetDirectory(SettingsFileName);

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            Associations.RemoveExternals();
            BuildToolAssociations.RemoveExternals();

            FileManager.XmlSerialize(this, SettingsFileName);

            Associations.ReAddExternals();
            BuildToolAssociations.ReAddExternals();
        }
    }
}