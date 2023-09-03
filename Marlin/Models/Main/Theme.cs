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
        public int PageColorInt = 0;
        public int FontColorInt = 0;
        public int InternalBackgroundColorInt = 16777215;
        public int ExternalBackgroundColorInt = 16777215;

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

        public static string NumberToHexColor(int number)
        {
            number = Math.Max(0, Math.Min(number, 16777215));
            string hexColor = number.ToString("X6");
            hexColor = "#" + hexColor;
            return hexColor;
        }

        public static int HexColorToNumber(string hexColor)
        {
            hexColor = hexColor.TrimStart('#');

            if (hexColor.Length == 8)
            {
                hexColor = hexColor.Substring(2);
            }

            if (hexColor.Length == 6)
            {
                if (int.TryParse(hexColor.Substring(0, 2), System.Globalization.NumberStyles.HexNumber, null, out int red) &&
                    int.TryParse(hexColor.Substring(2, 2), System.Globalization.NumberStyles.HexNumber, null, out int green) &&
                    int.TryParse(hexColor.Substring(4, 2), System.Globalization.NumberStyles.HexNumber, null, out int blue))
                {
                    int argb = (red << 16) | (green << 8) | blue;
                    return argb;
                }
            }

            return -1;
        }

        public static void SetColor()
        {

        }
    }
}
