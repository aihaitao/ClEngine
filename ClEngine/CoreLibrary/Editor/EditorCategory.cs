using System.ComponentModel;
using ClEngine.CoreLibrary.Asset;

namespace ClEngine.CoreLibrary.Editor
{
    public class EditorCategory : CategoryAttribute
    {
        public EditorCategory(string category)
            : base((string)category.GetTranslateName())
        {
        }
    }
}