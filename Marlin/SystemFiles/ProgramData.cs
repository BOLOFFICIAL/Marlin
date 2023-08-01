using System.Windows.Media;
using Marlin.Models;

namespace Marlin.SystemFiles
{
    public class ProgramData
    {
        public static ProgramColor Theme = new ProgramColor
            (
            pagecolor: Color.FromRgb(255, 255, 255).ToString(), 
            fontcolor: Color.FromRgb(255, 255, 255).ToString(), 
            backgroundcolor: Color.FromRgb(255, 0, 102).ToString(), 
            buttonfontcolor: Color.FromRgb(255, 0, 102).ToString()
            );
    }
}
