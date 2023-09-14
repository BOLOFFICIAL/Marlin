using Marlin.SystemFiles.Types;
using Newtonsoft.Json;
using System.Windows;

namespace Marlin.Models.Main
{
    public class Trigger
    {
        public int id;
        public string value;
        public TriggerType triggertype;
        //public string SelectedTrigger = Program.Triggers[0];
        public GridLength LengthTextTrigger = GridLength.Auto;
        public GridLength LengthAppTrigger = new GridLength(0, GridUnitType.Pixel);

        public bool Equals(Trigger otherTrigger)
        {
            if (otherTrigger is null)
            {
                return false;
            }

            string thistrigger = JsonConvert.SerializeObject(this);
            string othertrigger = JsonConvert.SerializeObject(otherTrigger);

            if (thistrigger.Length != othertrigger.Length)
            {
                return false;
            }
            for (int i = 0; i < thistrigger.Length; i++)
            {
                if (thistrigger[i] != othertrigger[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
