using System.Windows.Media;

namespace Marlin.SystemFiles
{
    public class ProgramData
    {
        public static Context Context = new Context();
        public static Theme Theme = new Theme(
            Color.FromRgb(255, 255, 255).ToString(), 
            Color.FromRgb(255, 255, 255).ToString(), 
            Color.FromRgb(255, 0, 102).ToString(), 
            Color.FromRgb(255, 0, 102).ToString());
    }
}
