using Marlin.Commands;
using Marlin.Models;
using Marlin.SystemFiles;
using Marlin.SystemFiles.Types;
using Marlin.ViewModels.Base;
using Marlin.Views.Main;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Marlin.ViewModels.Main
{
    public class SettingsPageViewModel : ViewModel
    {
        public ICommand ToMainCommand { get; }
        public ICommand SaveSettingsCommand { get; }
        public ICommand ChoseCommand { get; }
        public ICommand DeleteImageCommand { get; }

        public SettingsPageViewModel()
        {
            ToMainCommand = new LambdaCommand(OnToMainCommandExecuted);
            SaveSettingsCommand = new LambdaCommand(OnSaveSettingsCommandExecuted, CanSaveSettingsCommandExecute);
            ChoseCommand = new LambdaCommand(OnChoseCommandExecuted);
            DeleteImageCommand = new LambdaCommand(OnDeleteImageCommandExecuted, CanDeleteImageCommandExecute);
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
                if (value.Length < 7)
                {
                    Set(ref Context.Settings.Theme.PageColor, value);
                    return;
                }
                if (value.Contains("#") && value.Length == 1)
                {
                    value += "FFFFFF";
                }
                if (Set(ref Context.Settings.Theme.PageColor, value))
                {
                    PageColorInt = Theme.HexColorToNumber(value);
                }
            }
        }

        public int PageColorInt
        {
            get => Context.Settings.Theme.PageColorInt;
            set
            {
                Set(ref Context.Settings.Theme.PageColorInt, value);
                PageColor = Theme.NumberToHexColor(PageColorInt);
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
                if (value.Length < 7)
                {
                    Set(ref Context.Settings.Theme.FontColor, value);
                    return;
                }
                if (value.Contains("#") && value.Length == 1)
                {
                    value += "FFFFFF";
                }
                if (Set(ref Context.Settings.Theme.FontColor, value))
                {
                    FontColorInt = Theme.HexColorToNumber(value);
                }
            }
        }

        public int FontColorInt
        {
            get => Context.Settings.Theme.FontColorInt;
            set
            {
                Set(ref Context.Settings.Theme.FontColorInt, value);
                FontColor = Theme.NumberToHexColor(FontColorInt);
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
                if (value.Length < 7)
                {
                    Set(ref Context.Settings.Theme.ExternalBackgroundColor, value);
                    return;
                }
                if (value.Contains("#") && value.Length == 1)
                {
                    value += "FFFFFF";
                }
                if (Set(ref Context.Settings.Theme.ExternalBackgroundColor, value))
                {
                    ExternalBackgroundColorInt = Theme.HexColorToNumber(value);
                }
            }
        }

        public int ExternalBackgroundColorInt
        {
            get => Context.Settings.Theme.ExternalBackgroundColorInt;
            set
            {
                Set(ref Context.Settings.Theme.ExternalBackgroundColorInt, value);
                ExternalBackgroundColor = Theme.NumberToHexColor(ExternalBackgroundColorInt);
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
                if (value.Length < 7)
                {
                    Set(ref Context.Settings.Theme.InternalBackgroundColor, value);
                    return;
                }
                if (value.Contains("#") && value.Length == 1)
                {
                    value += "FFFFFF";
                }
                if (Set(ref Context.Settings.Theme.InternalBackgroundColor, value))
                {
                    InternalBackgroundColorInt = Theme.HexColorToNumber(value);
                }
            }
        }

        public int InternalBackgroundColorInt
        {
            get => Context.Settings.Theme.InternalBackgroundColorInt;
            set
            {
                Set(ref Context.Settings.Theme.InternalBackgroundColorInt, value);
                InternalBackgroundColor = Theme.NumberToHexColor(InternalBackgroundColorInt);
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
                if (BackgraundImagePath.Length>0&& Seamless!=true) 
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
            get => Context.Settings.Speeds;
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
            get => Context.Settings.Genders;
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

        private void OnChoseCommandExecuted(object p)
        {
            switch (p.ToString())
            {
                case "Фоновое изображение":
                    //Models.MessageBox.MakeMessage("Рекомендую выбирать бесшовные изображения");
                    SelectImage();
                    break;
                case "Папка для хранения данных":
                    SelectFolder();
                    break;
            }

            void SelectImage()
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
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
            return !Context.CopySettings.Equals(Context.Settings);
        }

        private void OnSaveSettingsCommandExecuted(object p)
        {
            if (Context.CopySettings.Equals(Context.Settings))
            {
                Models.MessageBox.MakeMessage("Не обнаружено  изменений в настройках");
                return;
            }

            if (Context.Settings.Theme.PageColor.Length < 7 ||
                    Context.Settings.Theme.FontColor.Length < 7 ||
                    Context.Settings.Theme.ExternalBackgroundColor.Length < 7 ||
                    Context.Settings.Theme.PageColor.Length == 8 ||
                    Context.Settings.Theme.FontColor.Length == 8 ||
                    Context.Settings.Theme.ExternalBackgroundColor.Length == 8)
            {
                Models.MessageBox.MakeMessage("Значение цвета должно иметь длину 7 символов", MessageType.Error);
                return;
            }
            var editadmin =
                (Context.Settings.NewPassword != Context.Settings.Password && Context.Settings.NewPassword.Length > 0) ||
                Context.CopySettings.Login != Context.Settings.Login ||
                Context.CopySettings.Gender != Context.Settings.Gender ||
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
                    string oldpass = (Context.Settings.NewPassword != Context.Settings.Password && Context.Settings.NewPassword.Length > 0) ? "старый" : "";
                    Models.MessageBox.MakeMessage($"Были изменены настройки администрирования.\nДля сохранения введите {oldpass} пароль администпратора.", MessageType.TextQuestion);
                    if (Context.MessageBox.Answer == Context.Settings.Password)
                    {
                        if (Context.Settings.NewPassword.Length > 0)
                        {
                            Context.Settings.Password = Context.Settings.NewPassword;
                        }
                        if (Context.Settings.IsАutorun)
                        {
                            Program.AddAutorun();
                        }
                        else
                        {
                            Program.RemoveAutorun();
                        }
                        Settings.SaveSettings(Context.CopySettings.Theme.PageColor != Context.Settings.Theme.PageColor);
                    }
                    else
                    {
                        Models.MessageBox.MakeMessage($"Введен неправильный пароль", MessageType.Error);
                    }
                }
                else
                {
                    Context.Settings.Password = Context.Settings.NewPassword;
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
        }

        private void OnDeleteImageCommandExecuted(object p)
        {
            BackgraundImagePath = "";
            BackgraundImage = "";
            LengthImage = new GridLength(0, GridUnitType.Pixel);
            LengthScale = new GridLength(0, GridUnitType.Pixel);
        }

        private bool CanDeleteImageCommandExecute(object p)
        {
            return BackgraundImagePath.Length > 0;
        }
    }
}
