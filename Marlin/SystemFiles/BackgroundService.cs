using System.Threading.Tasks;

namespace Marlin.SystemFiles
{
    public class BackgroundService
    {
        public static void StartServise()
        {
            Task.Run(() => { Servise(); });
        }

        private static void Servise()
        {
            while (true)
            {

            }
        }
    }
}
