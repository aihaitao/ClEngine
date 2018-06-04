using System.Collections.Generic;
using System.Linq;
using ClEngine.Core.Properties;

namespace ClEngine.Core.ProjectCreator
{
    public class ProjectCreationHelper
    {
        internal static char[] InvalidCharacters = new char[]
        {
            '`', '~', '!', '@', '#', '$', '%', '^',
            '&', '*', '(', ')', '-', '+', '=', ',',
            '[', '{', ']', '}', '\\', '|', '.', '<',
            '>', '/', '?'
        };

        internal static List<string> MReservedProjectNames = new List<string>()
        {
            "Game1",
            "FlatRedBall",
            "Microsoft",
            "System",
            "Microsoft.Xna",
            "SpriteEditor",
            "Camera",
            "SpriteManager",
            "ModelManager",
            "ShapeManager",
            "Sprite",
            "Scene"
        };

        public static string GetWhyProjectNameIsntValid(string projectName)
        {
            if (string.IsNullOrEmpty(projectName))
                return Resources.ProjectNotBlank;

            if (char.IsDigit(projectName[0]))
                return Resources.ProjectNotNumber;

            if (projectName.Contains(' '))
                return Resources.ProjectNotSpace;

            var indexOfInvalid = projectName.IndexOfAny(InvalidCharacters);
            if (indexOfInvalid != -1)
                return Resources.ProjectNotChar + projectName[indexOfInvalid];

            if (MReservedProjectNames.Contains(projectName))
                return Resources.Name + projectName + Resources.ReserverdPickAnother;

            return null;
        }
    }
}