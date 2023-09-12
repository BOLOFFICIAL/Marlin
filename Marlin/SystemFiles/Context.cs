using Marlin.Models;
using Marlin.Models.Main;
using Marlin.SystemFiles.Types;
using System;
using System.Windows;

namespace Marlin.SystemFiles
{
    public class Context
    {
        public static Models.MessageBox MessageBox = new();

        public static Settings Settings = new();
        public static Settings CopySettings = new();

        public static Command CopyCommand = new();
        public static Command Command = new();

        public static Window Window = new();
        public static Window MainWindow = new();

        public static ActionType Action;
        public static int SelectedId = -1;

        public static DateTime LastCheckPassword = DateTime.MinValue;
    }
}
