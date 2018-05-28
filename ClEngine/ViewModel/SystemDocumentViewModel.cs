using System.Collections.Generic;
using ClEngine.CoreLibrary.Asset;
using GalaSoft.MvvmLight;

namespace ClEngine.ViewModel
{
    public class ParameterList
    {
        public string Name { get; set; }
        public string TypeValue { get; set; }
        public bool IsNecessary { get; set; }
        public string Description { get; set; }
    }

    public class SystemDocumentViewModel : ViewModelBase
    {
        public List<ParameterList> RandomParameterList { get; set; }
        public List<ParameterList> LoadParameterList { get; set; }
        public List<ParameterList> CreateParameterList { get; set; }

        public SystemDocumentViewModel()
        {
            RandomParameterList = new List<ParameterList>
            {
                new ParameterList
                {
                    Name = "MinValue".GetTranslateName(),
                    TypeValue = "Int".GetTranslateName(),
                    IsNecessary = true
                },
                new ParameterList
                {
                    Name = "MaxValue".GetTranslateName(),
                    TypeValue = "Int".GetTranslateName(),
                    IsNecessary = true
                }
            };

            LoadParameterList = new List<ParameterList>
            {
                new ParameterList
                {
                    Name = "AssetPath".GetTranslateName(),
                    TypeValue = "String".GetTranslateName(),
                    IsNecessary = true
                }
            };

            CreateParameterList = new List<ParameterList>
            {
                new ParameterList
                {
                    Name = "x",
                    TypeValue = "Int".GetTranslateName(),
                    IsNecessary = true,
                },
                new ParameterList
                {
                    Name = "y",
                    TypeValue = "Int".GetTranslateName(),
                    IsNecessary = true,
                },
            };
        }
    }
}