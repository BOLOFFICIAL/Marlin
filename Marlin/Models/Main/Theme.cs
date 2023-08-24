using Marlin.SystemFiles.Types;
using System;

namespace Marlin.Models
{
    public class Theme
    {
        public string PageColor = "#000000";
        public string FontColor = "#000000";
        public string InternalBackgroundColor = "#FFFFFF";
        public string ExternalBackgroundColor = "#FFFFFF";

        public Theme() { }

        public Theme
            (
            string pagecolor,
            string fontcolor,
            string externalbackgroundcolor,
            string internalbackgroundcolor
            )
        {
            PageColor = pagecolor;
            FontColor = fontcolor;
            ExternalBackgroundColor = externalbackgroundcolor;
            InternalBackgroundColor = internalbackgroundcolor;
        }

        public static (byte A, byte R, byte G, byte B) ConvertHexToArgb(string hexColor)
        {
            if (hexColor.StartsWith("#"))
                hexColor = hexColor.Substring(1);

            if (hexColor.Length != 8)
            {
                if (hexColor.Length == 6)
                {
                    hexColor = "FF" + hexColor;
                }
                else
                {
                    MessageBox.MakeMessage("Ошибка загрузки цветов программы", MessageType.Error);
                }
            }

            string alphaHex = hexColor.Substring(0, 2);
            string redHex = hexColor.Substring(2, 2);
            string greenHex = hexColor.Substring(4, 2);
            string blueHex = hexColor.Substring(6, 2);

            byte alpha = (byte)Convert.ToInt32(alphaHex, 16);
            byte red = (byte)Convert.ToInt32(redHex, 16);
            byte green = (byte)Convert.ToInt32(greenHex, 16);
            byte blue = (byte)Convert.ToInt32(blueHex, 16);

            return (alpha, red, green, blue);
        }
    }
}
