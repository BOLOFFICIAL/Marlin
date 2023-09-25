using Marlin.Commands;
using Marlin.Models;
using Marlin.SystemFiles;
using Marlin.SystemFiles.Types;
using Marlin.ViewModels.Base;
using Marlin.Views.Main;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using Theme = Marlin.Models.Theme;

namespace Marlin.ViewModels.Main
{
    public class SettingsPageViewModel : ViewModel
    {
        public ICommand ToMainCommand { get; }
        public ICommand SaveSettingsCommand { get; }
        public ICommand ChoseCommand { get; }
        public ICommand DeleteImageCommand { get; }
        public ICommand SelectColorCommand { get; }

        public SettingsPageViewModel()
        {
            NewPassword = "";
            ToMainCommand = new LambdaCommand(OnToMainCommandExecuted);
            SaveSettingsCommand = new LambdaCommand(OnSaveSettingsCommandExecuted, CanSaveSettingsCommandExecute);
            ChoseCommand = new LambdaCommand(OnChoseCommandExecuted);
            DeleteImageCommand = new LambdaCommand(OnDeleteImageCommandExecuted, CanDeleteImageCommandExecute);
            SelectColorCommand = new LambdaCommand(OnSelectColorCommand);
        }

        public int TimeCheckPassword
        {
            get => Context.Settings.TimeCheckPassword;
            set => Set(ref Context.Settings.TimeCheckPassword, value);
        }

        public string PageColor
        {
            get => Context.Settings.Theme.PageColor;
            set
            {
                if (!value.StartsWith("#"))
                {
                    value = "#" + value;
                }
                if (value.Contains("#") && value.Length == 1)
                {
                    value += "000000";
                }
                if (value.Length < 8)
                {
                    Set(ref Context.Settings.Theme.PageColor, value);
                    return;
                }
            }
        }

        public bool Аutorun
        {
            get => Context.Settings.IsАutorun;
            set => Set(ref Context.Settings.IsАutorun, value);
        }

        public string FontColor
        {
            get => Context.Settings.Theme.FontColor;
            set
            {
                if (!value.StartsWith("#"))
                {
                    value = "#" + value;
                }
                if (value.Contains("#") && value.Length == 1)
                {
                    value += "000000";
                }
                if (value.Length < 8)
                {
                    Set(ref Context.Settings.Theme.FontColor, value);
                    return;
                }
            }
        }

        public string ExternalBackgroundColor
        {
            get => Context.Settings.Theme.ExternalBackgroundColor;
            set
            {
                if (!value.StartsWith("#"))
                {
                    value = "#" + value;
                }
                if (value.Contains("#") && value.Length == 1)
                {
                    value += "FFFFFF";
                }
                if (value.Length < 8)
                {
                    Set(ref Context.Settings.Theme.ExternalBackgroundColor, value);
                    return;
                }
            }
        }

        public string InternalBackgroundColor
        {
            get => Context.Settings.Theme.InternalBackgroundColor;
            set
            {
                if (!value.StartsWith("#"))
                {
                    value = "#" + value;
                }
                if (value.Contains("#") && value.Length == 1)
                {
                    value += "FFFFFF";
                }
                if (value.Length < 8)
                {
                    Set(ref Context.Settings.Theme.InternalBackgroundColor, value);
                    return;
                }
            }
        }

        public bool IsSay
        {
            get => Context.Settings.IsSay;
            set
            {
                Set(ref Context.Settings.IsSay, value);
                LengthSay = IsSay ? GridLength.Auto : new GridLength(0, GridUnitType.Pixel);
            }
        }

        public bool Seamless
        {
            get => Context.Settings.Seamless;
            set
            {
                if (BackgraundImagePath.Length > 0 && Seamless != true)
                {
                    Image image = Image.FromFile(BackgraundImagePath);
                    if (image.Width != image.Height)
                    {
                        Models.MessageBox.MakeMessage("Фоновое изображение не имеет квадратную форму.\nЭто может привести к искажению пропорций изображения.\nВключить режим безшовное заполнение?", MessageType.YesNoQuestion);
                        if (Context.MessageBox.Answer == "No")
                        {
                            return;
                        }
                    }
                }
                Set(ref Context.Settings.Seamless, value);
                LengthScale = Seamless && BackgraundImagePath.Length > 0 ? GridLength.Auto : new GridLength(0, GridUnitType.Pixel);
                if (Seamless)
                {
                    Stretch = Stretch.Fill;
                    ImageViewport = $"{ImageScail},{ImageScail},{ImageScail},{ImageScail}";
                    TileMode = TileMode.Tile;
                    ViewportUnits = BrushMappingMode.Absolute;
                }
                else
                {
                    Stretch = Stretch.UniformToFill;
                    TileMode = TileMode.None;
                    ViewportUnits = BrushMappingMode.RelativeToBoundingBox;
                    ImageViewport = "0,0,1,1";
                }
            }
        }

        public GridLength LengthSay
        {
            get => Context.Settings.LengthSay;
            set => Set(ref Context.Settings.LengthSay, value);
        }

        public int[] Speeds
        {
            get => Program.Speeds;
        }

        public int Speed
        {
            get => Context.Settings.Speed;
            set
            {
                Set(ref Context.Settings.Speed, value);
                Voix.SpeakAsync($"Установлена скорость озвучивания {value}");
            }
        }

        public string[] Genders
        {
            get => Program.Genders;
        }

        public string NewGender
        {
            get => Context.Settings.Gender;
            set => Set(ref Context.Settings.Gender, value);
        }

        public string NewMainFolder
        {
            get => Context.Settings.MainFolder;
            set => Set(ref Context.Settings.MainFolder, value);
        }

        public string NewMainFolderPath
        {
            get => Context.Settings.MainFolderPath;
            set => Set(ref Context.Settings.MainFolderPath, value);
        }

        public string NewPassword
        {
            get => Context.Settings.NewPassword;
            set => Set(ref Context.Settings.NewPassword, value);
        }

        public string NewLogin
        {
            get => Context.Settings.Login;
            set => Set(ref Context.Settings.Login, value);
        }

        public string BackgraundImage
        {
            get => Context.Settings.BackgraundImage;
            set => Set(ref Context.Settings.BackgraundImage, value);
        }

        public GridLength LengthImage
        {
            get => Context.Settings.LengthImage;
            set => Set(ref Context.Settings.LengthImage, value);
        }

        public GridLength LengthScale
        {
            get => Context.Settings.LengthScale;
            set => Set(ref Context.Settings.LengthScale, value);
        }

        public string BackgraundImagePath
        {
            get => Context.Settings.BackgraundImagePath;
            set => Set(ref Context.Settings.BackgraundImagePath, value);
        }

        public string ImageViewport
        {
            get => Context.Settings.ImageViewport;
            set => Set(ref Context.Settings.ImageViewport, value);
        }

        public string ImageScail
        {
            get => Context.Settings.ImageScail;
            set
            {
                ImageViewport = $"{value},{value},{value},{value}";
                Set(ref Context.Settings.ImageScail, value);
            }
        }

        public Stretch Stretch
        {
            get => Context.Settings.Stretch;
            set => Set(ref Context.Settings.Stretch, value);
        }

        public TileMode TileMode
        {
            get => Context.Settings.TileMode;
            set => Set(ref Context.Settings.TileMode, value);
        }

        public BrushMappingMode ViewportUnits
        {
            get => Context.Settings.ViewportUnits;
            set => Set(ref Context.Settings.ViewportUnits, value);
        }

        private void OnToMainCommandExecuted(object p)
        {
            Context.Settings = Context.CopySettings;
            Program.SetPage(new MainPage());
        }

        private void OnSelectColorCommand(object p)
        {
            var parameter = p.ToString();
            using (ColorDialog colorDialog = new ColorDialog())
            {
                int alpha = 0;
                int red = 0;
                int green = 0;
                int blue = 0;

                string currentcolor = "";

                switch (parameter)
                {
                    case "ExternalBackgroundColor": currentcolor = ExternalBackgroundColor; break;
                    case "InternalBackgroundColor": currentcolor = InternalBackgroundColor; break;
                    case "FontColor": currentcolor = FontColor; break;
                    case "PageColor": currentcolor = PageColor; break;
                }
                if (currentcolor.Length == 7)
                {
                    (alpha, red, green, blue) = Theme.ConvertHexToArgb(currentcolor);
                }

                colorDialog.Color = System.Drawing.Color.FromArgb(alpha, red, green, blue);

                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectcolor = colorDialog.Color.ToArgb().ToString("X6").Substring(2);
                    switch (parameter)
                    {
                        case "ExternalBackgroundColor": ExternalBackgroundColor = "#" + selectcolor; break;
                        case "InternalBackgroundColor": InternalBackgroundColor = "#" + selectcolor; break;
                        case "FontColor": FontColor = "#" + selectcolor; break;
                        case "PageColor": PageColor = "#" + selectcolor; break;
                    }
                }
            }
        }

        private void OnChoseCommandExecuted(object p)
        {
            switch (p.ToString())
            {
                case "Фоновое изображение":
                    SelectImage();
                    break;
                case "Папка для данных":
                    if (Program.Authentication("Для смены папки для данных подтвердите пароль"))
                    {
                        SelectFolder();
                    }
                    break;
            }

            void SelectImage()
            {
                Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
                openFileDialog.Title = "Выбор фоновового изображения";
                openFileDialog.Filter = "Изображения (*.jpeg;*.jpg;*.png)|*.jpeg;*.jpg;*.png";

                bool? result = openFileDialog.ShowDialog();

                if (result == true)
                {
                    string path = openFileDialog.FileName;
                    Image image = Image.FromFile(path);
                    if (image.Width != image.Height && Seamless)
                    {
                        Models.MessageBox.MakeMessage("Выбранное изображение не имеет квадратную форму.\nЭто может привести к искажению пропорций изображения.\nВыбрать другое изображение?", MessageType.YesNoQuestion);
                        if (Context.MessageBox.Answer == "Yes")
                        {
                            SelectImage();
                            return;
                        }
                    }
                    BackgraundImagePath = path;
                    BackgraundImage = System.IO.Path.GetFileName(path);
                    LengthImage = GridLength.Auto;
                    LengthScale = Seamless ? GridLength.Auto : new GridLength(0, GridUnitType.Pixel);
                }
            }

            void SelectFolder()
            {
                CommonOpenFileDialog folderPicker = new CommonOpenFileDialog();

                folderPicker.IsFolderPicker = true;
                folderPicker.Title = "Выбор папки для хранения данных";

                CommonFileDialogResult dialogResult = folderPicker.ShowDialog();

                if (dialogResult == CommonFileDialogResult.Ok)
                {
                    string selectedFolderPath = folderPicker.FileName;
                    Context.Settings.MainFolderPath = selectedFolderPath;
                    NewMainFolder = System.IO.Path.GetFileName(selectedFolderPath);
                }
            }
        }

        private bool CanSaveSettingsCommandExecute(object p)
        {
            return !Program.Equals(Context.CopySettings, Context.Settings);
        }

        private void OnSaveSettingsCommandExecuted(object p)
        {
            if (Program.Equals(Context.CopySettings, Context.Settings))
            {
                Models.MessageBox.MakeMessage("Не обнаружено  изменений в настройках");
                return;
            }

            if (Context.Settings.Theme.PageColor.Length < 7 ||
                    Context.Settings.Theme.FontColor.Length < 7 ||
                    Context.Settings.Theme.ExternalBackgroundColor.Length < 7 ||
                    Context.Settings.Theme.InternalBackgroundColor.Length < 7)
            {
                Models.MessageBox.MakeMessage("Значение цвета должно иметь длину 7 символов", MessageType.Error);
                return;
            }
            var editadmin =
                (Context.Settings.NewPassword != Context.Settings.Password && Context.Settings.NewPassword.Length > 0) ||
                Context.CopySettings.Login != Context.Settings.Login ||
                Context.CopySettings.Gender != Context.Settings.Gender ||
                Context.CopySettings.TimeCheckPassword != Context.Settings.TimeCheckPassword ||
                Context.CopySettings.MainFolder != Context.Settings.MainFolder ||
                Context.CopySettings.IsАutorun != Context.Settings.IsАutorun;
            if (editadmin)
            {
                if (Context.Settings.Login.Length < 1 ||
                    Context.Settings.MainFolder.Length < 1 ||
                    Context.Settings.Gender.Length < 1)
                {
                    Models.MessageBox.MakeMessage("Блок администрирования должен быть заполнен", MessageType.Error);
                    return;
                }
                if (Context.Settings.Password.Length > 0)
                {
                    string oldpass = Context.Settings.NewPassword.Length > 0 ? " старый" : "";

                    if (Program.Authentication($"Были изменены настройки администрирования.\nДля сохранения введите{oldpass} пароль администпратора.",
                        check: (Context.Settings.NewPassword.Length > 0 || Context.Settings.TimeCheckPassword != Context.CopySettings.TimeCheckPassword)))
                    {
                        if (Context.Settings.NewPassword.Length > 0)
                        {
                            Program.ChangePassword();
                        }

                        if (Context.Settings.IsАutorun)
                        {
                            Program.AddToStartup();
                        }
                        else
                        {
                            Program.RemoveFromStartup();
                        }

                        if (Context.CopySettings.MainFolderPath != Context.Settings.MainFolderPath)
                        {
                            ProgramData.MoveData(Context.CopySettings.MainFolderPath, Context.Settings.MainFolderPath);
                        }

                        Settings.SaveSettings(Context.CopySettings.Theme.PageColor != Context.Settings.Theme.PageColor);

                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    Program.ChangePassword();
                    Settings.SaveSettings(Context.CopySettings.Theme.PageColor != Context.Settings.Theme.PageColor);
                }
            }
            else
            {
                if (Context.Settings.Password.Length > 0 &&
                    Context.Settings.Login.Length > 0 &&
                    Context.Settings.MainFolder.Length > 0 &&
                    Context.Settings.Gender.Length > 0)
                {
                    Settings.SaveSettings(Context.CopySettings.Theme.PageColor != Context.Settings.Theme.PageColor);
                }
                else
                {
                    Models.MessageBox.MakeMessage("Блок администрирования должен быть заполнен", MessageType.Error);
                }
            }
            NewPassword = "";
        }

        private void OnDeleteImageCommandExecuted(object p)
        {
            BackgraundImagePath = "";
            BackgraundImage = "";
            Seamless = false;
            LengthImage = new GridLength(0, GridUnitType.Pixel);
            LengthScale = new GridLength(0, GridUnitType.Pixel);
        }

        private bool CanDeleteImageCommandExecute(object p)
        {
            return BackgraundImagePath.Length > 0;
        }


    }
}
