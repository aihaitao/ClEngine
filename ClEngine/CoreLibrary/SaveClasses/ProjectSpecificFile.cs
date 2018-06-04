using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using ClEngine.CoreLibrary.Content;
using ClEngine.CoreLibrary.Editor;
using ClEngine.CoreLibrary.IO;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
// ReSharper disable UnusedMember.Global

namespace ClEngine.CoreLibrary.SaveClasses
{
    #region ProjectSpecificFileCollection Class
    public class ProjectSpecificFileCollection : CollectionBase
    {
        public void Add(ProjectSpecificFile projectSpecificFile)
        {
            List.Add(projectSpecificFile);
        }

        public void Remove(ProjectSpecificFile projectSpecificFile)
        {
            List.Remove(projectSpecificFile);
        }

        public new int Count => List.Count;

        public ProjectSpecificFile this[int index] => (ProjectSpecificFile)List[index];
    }

    #endregion



    #region ProjectSpecificFileConverter
    internal class ProjectSpecificFileConverter : TypeConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context,
            System.Globalization.CultureInfo culture,
            object value, Type destType)
        {
            if (destType == typeof(string) && value is ProjectSpecificFile emp)
            {
                return emp.FilePath;
            }
            return base.ConvertTo(context, culture, value, destType);
        }
    }
    #endregion

    [TypeConverter(typeof(ProjectSpecificFileConverter))]
    public class ProjectSpecificFile
    {
        [Obsolete("这有时候被用来表示像Android这样的ID，有时也用于名称。 ID可能会引起误解，因为多个项目可能具有相同的ID，所以我们不应该使用它。 改用ProjectName")]
        public string ProjectId
        {
            get => ProjectName;
            set => ProjectName = value;
        }

        public string ProjectName
        {
            get; set;
        }

        public string FilePath { get; set; }

        public string Display => FilePath + " (" + ProjectName + ")";

        public override string ToString()
        {
            return FilePath + " (" + ProjectName + ")";
        }
    }

    #region Enums

    public enum AvailableDelimiters
    {
        Comma,
        Tab,
        Pipe
    }

    public static class AvailableDelimiterExtensions
    {
        public static char ToChar(this AvailableDelimiters delimiter)
        {
            switch (delimiter)
            {
                case AvailableDelimiters.Comma:
                    return ',';
                case AvailableDelimiters.Tab:
                    return '\t';
                case AvailableDelimiters.Pipe:
                    return '|';
            }
            throw new ArgumentException();
        }
    }


    #endregion

    public delegate string ReferencedFileSaveToString(ReferencedFileSave rfs);

    public class ReferencedFileSave
    {
        #region Fields

        [XmlIgnore]
        public static ReferencedFileSaveToString ToStringDelegate;

        [XmlIgnore]
        public static char[] InvalidFileNameCharacters = new char[]{
            '\\',
            '/',
            ':',
            '*',
            '?',
            '"',
            '<',
            '>',
            '|'};



        internal string MName;

        #endregion

        #region Properties

        public List<PropertySave> Properties
        {
            get;
            set;
        }
        public bool ShouldSerializeProperties()
        {
            return Properties != null && Properties.Count != 0;
        }

        //[ReadOnlyAttribute(true)]
        /// <summary>
        /// The name of the file name, relative to the Content folder.
        /// </summary>
        public string Name
        {
            get => MName;
            set
            {
                if (!String.IsNullOrEmpty(value) && value.ToLower().Replace("\\", "/").StartsWith("content/"))
                    value = value.Substring("content/".Length);

                MName = value;


            }
        }

        [EditorCategory("MemoryAndPerformance"),
        EditorDescription("ObejctCreatedWillBeManuaUpdate"),
        DefaultValue(false)]
        public bool IsManuallyUpdated
        {
            get;
            set;
        }

        [EditorCategory("MemoryAndPerformance")]
        public bool IsSharedStatic
        {
            get;
            set;
        }

        [EditorCategory("Access"), DefaultValue(false)]
        public bool IncludeDirectoryRelativeToContainer
        {
            get;
            set;
        }

        [EditorCategory("MemoryAndPerformance"), DefaultValue(false)]
        public bool LoadedOnlyWhenReferenced
        {
            get;
            set;
        }

        [EditorCategory("Destroy"), DefaultValue(true)]
        public bool DestroyOnUnload
        {
            get;
            set;
        }

        public string Summary
        {
            get;
            set;
        }

        [EditorCategory("Access"), DefaultValue(false)]
        public bool HasPublicProperty
        {
            get;
            set;
        }



        [DefaultValue("<DEFAULT>")]
        public string OpensWith
        {
            get;
            set;
        }

        [CategoryAttribute("Code"), DefaultValue(true)]
        public bool LoadedAtRuntime
        {
            get;
            set;
        }

        [EditorCategory("CSV"), DefaultValue(false)]
        public bool CreatesDictionary
        {
            get;
            set;
        }

        [EditorCategory("CSV"), DefaultValue(AvailableDelimiters.Comma)]
        public AvailableDelimiters CsvDelimiter
        {
            get;
            set;
        }

        [EditorCategory("CSV"), DefaultValue(false)]
        public bool TreatAsCsv
        {
            get;
            set;
        }

        [XmlIgnore]
        [Browsable(false)]
        public List<SourceReferencingFile> SourceFileCache
        {
            get;
            set;
        }

        [EditorCategory("Build")]
        public string SourceFile
        {
            get;
            set;
        }
        public bool ShouldSerializeSourceFile()
        {
            return !string.IsNullOrEmpty(SourceFile);
        }


        [EditorCategory("Build")]
        public string BuildTool
        {
            get;
            set;
        }
        public bool ShouldSerializeBuildTool()
        {
            return !string.IsNullOrEmpty(BuildTool);
        }

        [EditorCategory("Build")]
        public string AdditionalArguments
        {
            get;
            set;
        }
        public bool ShouldSerializeAdditionalArguments()
        {
            return !string.IsNullOrEmpty(AdditionalArguments);
        }

        [XmlIgnore]
        public bool IsDatabaseForLocalizing
        {
            get => Properties.ContainsValue("IsDatabaseForLocalizing") && ((bool)Properties.GetValue("IsDatabaseForLocalizing"));
            set => Properties.SetValue("IsDatabaseForLocalizing", value);
        }

        [XmlIgnore]
        public bool UseContentPipeline
        {
            get => Properties.ContainsValue("UseContentPipeline") && ((bool)Properties.GetValue("UseContentPipeline"));
            set => Properties.SetValue("UseContentPipeline", value);
        }

        [XmlIgnore]
        public TextureProcessorOutputFormat TextureFormat
        {
            get => Properties.GetValue<TextureProcessorOutputFormat>("TextureFormat");
            set => Properties.SetValue("TextureFormat", value);
        }

        [XmlIgnore]
        [EditorCategory("CSV")]
        public string UniformRowType
        {
            get
            {
                if (Properties.ContainsValue("UniformRowType"))
                {
                    return (string)Properties.GetValue("UniformRowType");
                }
                else
                {
                    return null;
                }
            }

            set
            {
                string valueToSet = value;
                if (valueToSet == "<NONE>")
                {
                    valueToSet = null;
                }
                Properties.SetValue("UniformRowType", valueToSet);
            }
        }

        [XmlIgnore]
        [Browsable(false)]
        public bool IsCsvOrTreatedAsCsv => TreatAsCsv || FileManager.GetExtension(Name) == "csv";

        public ProjectSpecificFileCollection ProjectSpecificFiles { get; set; } = new ProjectSpecificFileCollection();

        public bool ShouldSerializeProjectSpecificFiles()
        {
            return ProjectSpecificFiles != null && ProjectSpecificFiles.Count != 0;
        }

        [EditorCategory("Build")]
        public string ConditionalCompilationSymbols
        {
            get;
            set;
        }
        public bool ShouldSerializeConditionalCompilationSymbols()
        {
            return !string.IsNullOrEmpty(ConditionalCompilationSymbols);
        }

        [EditorCategory("Code")]
        public string RuntimeType
        {
            get;
            set;
        }

        [EditorCategory("Code"), DefaultValue(true)]
        public bool AddToManagers
        {
            get;
            set;
        }

        public List<string> ProjectsToExcludeFrom
        {
            get;
            set;
        }

        #endregion

        #region Methods

        #region Constructor

        public ReferencedFileSave()
        {
            ProjectsToExcludeFrom = new List<string>();
            AddToManagers = true;
            DestroyOnUnload = true;
            Properties = new List<PropertySave>();
            CsvDelimiter = AvailableDelimiters.Comma;
            SourceFileCache = new List<SourceReferencingFile>();
            LoadedAtRuntime = true;
            OpensWith = "<DEFAULT>";
            IsSharedStatic = true;
        }

        #endregion

        #region Public Methods

        public ReferencedFileSave Clone()
        {
            ReferencedFileSave clonedRfs = (ReferencedFileSave)MemberwiseClone();

            clonedRfs.ProjectsToExcludeFrom = new List<string>();
            clonedRfs.ProjectsToExcludeFrom.AddRange(ProjectsToExcludeFrom);

            return clonedRfs;
        }

        public void SetNameNoCall(string newName)
        {
            MName = newName;
        }

        public override string ToString()
        {
            return ToStringDelegate != null ? ToStringDelegate(this) : MName;
        }

        #endregion

        #endregion
    }
}