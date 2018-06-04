using ClEngine.CoreLibrary.GuiDisplay.Facades;
using ClEngine.CoreLibrary.SaveClasses;
using FlatRedBall.Glue.SaveClasses;

namespace ClEngine.CoreLibrary.Elements
{
    public class ObjectFinder : IObjectFinder
    {
        public static ObjectFinder Self { get; } = new ObjectFinder();

        public GlueProjectSave Project { get; set; }
    }
}