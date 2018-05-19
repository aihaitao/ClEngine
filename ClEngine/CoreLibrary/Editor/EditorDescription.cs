using System.ComponentModel;
using ClEngine.CoreLibrary.Asset;

namespace ClEngine.CoreLibrary.Editor
{
    public class EditorDescription : DescriptionAttribute
    {
        public string Des { get; }

        public override string Description => Des.GetTranslateName();

        public EditorDescription(string description)
        {
            Des = description;
        }
    }
}