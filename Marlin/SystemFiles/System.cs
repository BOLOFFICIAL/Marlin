using System.Threading.Tasks;
using System.Windows.Controls;

namespace Marlin.SystemFiles
{
    public class System
    {
        public static async Task SetPage(Page page)
        {
            for (double i = 1; i > 0; i -= 0.1)
            {
                Context.MainWindow.Opacity = i;
                await Task.Delay(1);
            }
            Context.MainWindow.Content = page;
            for (double i = 0.1; i <= 1; i += 0.1)
            {
                Context.MainWindow.Opacity = i;
                await Task.Delay(1);
            }
        }
    }
}
