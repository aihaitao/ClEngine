﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using FlatRedBall.Content;
using FlatRedBall.IO;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;

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

        public int Count => List.Count;

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
            if (destType == typeof(string) && value is ProjectSpecificFile)
            {
                var emp = (ProjectSpecificFile)value;
                return emp.FilePath;
            }
            return base.ConvertTo(context, culture, value, destType);
        }
    }
    #endregion

    [TypeConverter(typeof(ProjectSpecificFileConverter))]
    public class ProjectSpecificFile
    {
        [Obsolete("This was sometimes used to mean the ID like Android, and sometimes name. ID can be misleading because multiple projects can have the same ID, so we shouldn't use this. Use ProjectName instead")]
        public string ProjectId
        {
            get { return ProjectName; }
            set { ProjectName = value; }
        }

        public string ProjectName
        {
            get; set;
        }

        public string FilePath { get; set; }

        public string Display
        {
            get
            {
                return FilePath + " (" + ProjectName + ")";
            }
        }

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



        string mName;
        private ProjectSpecificFileCollection _projectSpecificFiles = new ProjectSpecificFileCollection();

        // Vic says: Eventually we'll want to add this:
        //private string mLayerOn;


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
            get { return mName; }
            set
            {
                if (!String.IsNullOrEmpty(value) && value.ToLower().Replace("\\", "/").StartsWith("content/"))
                    value = value.Substring("content/".Length);

                mName = value;


            }
        }

        [Category("Memory and Performance"),
        Description("Whether the object created by this file will be a manually-updated object.  For example, Scenes which do not move can be made manually updated to improve runtime performance."),
        DefaultValue(false)]
        public bool IsManuallyUpdated
        {
            get;
            set;
        }

        [Category("Memory and Performance")]
        public bool IsSharedStatic
        {
            get;
            set;
        }

        [CategoryAttribute("Access"), DefaultValue(false)]
        public bool IncludeDirectoryRelativeToContainer
        {
            get;
            set;
        }

        [Category("Memory and Performance"), DefaultValue(false)]
        public bool LoadedOnlyWhenReferenced
        {
            get;
            set;
        }

        [Category("Destroy"), DefaultValue(true)]
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

        // December 21, 2011
        // Why do we need this property?
        // I don't think it's used anywhere.
        //[Browsable(false)]
        //public string ContentProjectParent
        //{
        //    get;
        //    set;
        //}

        [CategoryAttribute("Access"), DefaultValue(false)]
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

        [CategoryAttribute("CSV"), DefaultValue(false)]
        public bool CreatesDictionary
        {
            get;
            set;
        }

        [CategoryAttribute("CSV"), DefaultValue(AvailableDelimiters.Comma)]
        public AvailableDelimiters CsvDelimiter
        {
            get;
            set;
        }

        [CategoryAttribute("CSV"), DefaultValue(false)]
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
            // Setter is made public so extension methods can access it
            set;
        }

        [Category("Build")]
        public string SourceFile
        {
            get;
            set;
        }
        public bool ShouldSerializeSourceFile()
        {
            return !string.IsNullOrEmpty(SourceFile);
        }


        [Category("Build")]
        public string BuildTool
        {
            get;
            set;
        }
        public bool ShouldSerializeBuildTool()
        {
            return !string.IsNullOrEmpty(BuildTool);
        }

        [Category("Build")]
        public string AdditionalArguments
        {
            get;
            set;
        }
        public bool ShouldSerializeAdditionalArguments()
        {
            return !string.IsNullOrEmpty(AdditionalArguments);
        }

        //[Category("Build")]
        //public string BuildTool
        //{
        //    get;
        //    set;
        //}

        [XmlIgnore]
        public bool IsDatabaseForLocalizing
        {
            get
            {
                return Properties.ContainsValue("IsDatabaseForLocalizing") && ((bool)Properties.GetValue("IsDatabaseForLocalizing"));
            }
            set
            {
                Properties.SetValue("IsDatabaseForLocalizing", value);
            }
        }

        [XmlIgnore]
        public bool UseContentPipeline
        {
            get
            {
                return Properties.ContainsValue("UseContentPipeline") && ((bool)Properties.GetValue("UseContentPipeline"));
            }
            set
            {
                Properties.SetValue("UseContentPipeline", value);
            }
        }

        [XmlIgnore]
        public TextureProcessorOutputFormat TextureFormat
        {
            get => Properties.GetValue<TextureProcessorOutputFormat>("TextureFormat");
            set => Properties.SetValue("TextureFormat", value);
        }

        //[XmlIgnore]
        //public bool GenerateMipmaps

        [XmlIgnore]
        [CategoryAttribute("CSV")]
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

        public ProjectSpecificFileCollection ProjectSpecificFiles
        {
            get => _projectSpecificFiles;
            set => _projectSpecificFiles = value;
        }
        public bool ShouldSerializeProjectSpecificFiles()
        {
            return ProjectSpecificFiles != null && ProjectSpecificFiles.Count != 0;
        }

        [CategoryAttribute("Build")]
        public string ConditionalCompilationSymbols
        {
            get;
            set;
        }
        public bool ShouldSerializeConditionalCompilationSymbols()
        {
            return !string.IsNullOrEmpty(ConditionalCompilationSymbols);
        }

        [CategoryAttribute("Code")]
        public string RuntimeType
        {
            get;
            set;
        }

        [CategoryAttribute("Code"), DefaultValue(true)]
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

            // September 12, 2012
            // IsSharedStatic used 
            // to default to false.
            // It would be set to true
            // on Entities and false on
            // Screen, but I think it was
            // set to false on Screens just
            // as a holdover from the very early
            // days.  Files need to be static for
            // Entities and GlobalContent.  They could
            // go either way on Screens, but we're going
            // to default them to static for consistency and
            // for the generated GetFile method.
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
            mName = newName;
        }

        public override string ToString()
        {
            if (ToStringDelegate != null)
            {
                return ToStringDelegate(this);
            }
            else
            {
                return mName;
            }
        }

        #endregion

        #endregion
    }
}