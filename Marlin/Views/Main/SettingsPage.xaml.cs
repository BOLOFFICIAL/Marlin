using Marlin.Models;
using Marlin.SystemFiles;
using Newtonsoft.Json;
using System.Windows.Controls;

namespace Marlin.Views.Main
{
    public partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            InitializeComponent();
            Context.CopySettings = JsonConvert.DeserializeObject<Settings>(JsonConvert.SerializeObject(Context.Settings));
            Context.CopySettings.NewPassword = "";
        }
    }
}
