using System;

namespace Marlin.Models
{
    public class ProgramColor
    {
        public string PageColor;
        public string FontColor;
        public string BackgroundColor;
        public string ButtonfontColor;

        public ProgramColor
            (
            string pagecolor,
            string fontcolor,
            string backgroundcolor,
            string buttonfontcolor
            )
        {
            PageColor = pagecolor;
            FontColor = fontcolor;
            BackgroundColor = backgroundcolor;
            ButtonfontColor = buttonfontcolor;
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
                    throw new ArgumentException("Invalid HEX color format. The color should be represented as #AARRGGBB.", nameof(hexColor));
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
