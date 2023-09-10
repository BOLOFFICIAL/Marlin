using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Marlin.Models.Main
{
    public class Command
    {
        public static StackPanel panel = new StackPanel();

        public int id = 0;
        public string title = "";
        public string fileputh = "";
        public string appputh = "";
        public bool checkpuss = false;
        public List<Trigger> triggers = new List<Trigger>();
    }
}
