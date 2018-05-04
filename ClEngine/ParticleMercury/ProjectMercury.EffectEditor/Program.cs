/*  
 Copyright © 2010 Project Mercury Team Members (http://mpe.codeplex.com/People/ProjectPeople.aspx)

 This program is licensed under the Microsoft Permissive License (Ms-PL).  You should 
 have received a copy of the license along with the source code.  If not, an online copy
 of the license can be found at http://mpe.codeplex.com/license.
*/

namespace ProjectMercury.EffectEditor
{
    using System;
    using System.Diagnostics;
    using System.Windows.Forms;

    static public class Program
    {
        static internal Core Core { get; private set; }

        [STAThread]
        static public void Main(string[] args)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;

            Trace.WriteLine("CLEngine 粒子编辑器", "APP");
            
            using (new TraceIndenter())
            {
                Trace.WriteLine("日期: " + DateTime.Now.ToString());
                Trace.WriteLine("版本: " + Application.ProductVersion);
                Trace.WriteLine("区域: " + Application.CurrentCulture);
                Trace.WriteLine("路径: " + Application.StartupPath);
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using (Core = new Core())
            {
                Application.Run(Core);
            }
        }
    }
}