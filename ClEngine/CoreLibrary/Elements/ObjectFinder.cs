using ClEngine.CoreLibrary.GuiDisplay.Facades;
using ClEngine.CoreLibrary.SaveClasses;

namespace ClEngine.CoreLibrary.Elements
{
    public class ObjectFinder : IObjectFinder
    {
        public static ObjectFinder Self { get; } = new ObjectFinder();

        public ProjectSave Project { get; set; }
    }
}