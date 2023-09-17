using Marlin.Commands;
using Marlin.Models.Main;
using Marlin.SystemFiles;
using Marlin.ViewModels.Base;
using Marlin.Views.Main;
using Newtonsoft.Json;
using System.Windows.Input;
using System.Windows.Media;

namespace Marlin.ViewModels.Main
{
    public class ScriptPageViewModel : ViewModel
    {
        public ICommand ToMainCommand { get; }
        public ICommand ButtonActionCommand { get; }

        private string _pagetitle = "Новый скрипт";

        public ScriptPageViewModel()
        {
            ToMainCommand = new LambdaCommand(OnToMainCommandExecuted);
            ButtonActionCommand = new LambdaCommand(OnButtonActionCommandExecuted, CanButtonActionCommandExecute);

            Context.Script = new Models.Main.Script();
            Context.CopyScript = JsonConvert.DeserializeObject<Script>(JsonConvert.SerializeObject(Context.Script));

            if (Context.SelectedId > -1)
            {
                Context.Script = JsonConvert.DeserializeObject<Script>(JsonConvert.SerializeObject(Script.GetScript(Context.SelectedId)));
                PageTitle = Context.Script.Title;
                Context.CopyScript = JsonConvert.DeserializeObject<Script>(JsonConvert.SerializeObject(Script.GetScript(Context.SelectedId)));
            }

            else
            {
                if (Context.ProgramData.Scripts.Count > 0)
                {
                    Title = "Скипт" + (Context.ProgramData.Scripts[Context.ProgramData.Scripts.Count - 1].id + 1).ToString(); 
                }
                else
                {
                    Title = "Скипт" + Context.Script.id.ToString();
                }
            }
        }

        public string PageColor
        {
            get => Context.Settings.Theme.PageColor;
        }

        public string FontColor
        {
            get => Context.Settings.Theme.FontColor;
        }

        public string ExternalBackgroundColor
        {
            get => Context.Settings.Theme.ExternalBackgroundColor;
        }

        public string InternalBackgroundColor
        {
            get => Context.Settings.Theme.InternalBackgroundColor;
        }

        public string BackgraundImagePath
        {
            get => Context.Settings.BackgraundImagePath;
        }

        public string ImageViewport
        {
            get => Context.Settings.ImageViewport;
        }

        public string ImageScail
        {
            get => Context.Settings.ImageScail;
        }

        public Stretch Stretch
        {
            get => Context.Settings.Stretch;
        }

        public TileMode TileMode
        {
            get => Context.Settings.TileMode;
        }

        public BrushMappingMode ViewportUnits
        {
            get => Context.Settings.ViewportUnits;
        }



        public string Title 
        {
            get => Context.Script.Title;
            set => Set(ref Context.Script.Title, value);
        }

        public bool Checkpuss
        {
            get => Context.Script.Checkpuss;
            set => Set(ref Context.Script.Checkpuss, value);
        }

        public string Comment
        {
            get => Context.Script.Comment;
            set => Set(ref Context.Script.Comment, value);
        }

        public string PageTitle
        {
            get => _pagetitle;
            set => Set(ref _pagetitle, value);
        }

        public string ButtonContent
        {
            get
            {
                if (Context.SelectedId > -1)
                {

                    return "Сохранить";
                }
                else
                {
                    return "Добавить";
                }
            }
        }

        private void OnButtonActionCommandExecuted(object p)
        {
            if (ValidationCommand())
            {
                if (Context.SelectedId > -1)
                {
                    Script.SetScript(Context.SelectedId, Context.Script);
                }

                else
                {
                    Script.AddScript(Context.Script);
                }

                ProgramData.SaveData();

                Program.SetPage(new ActionsPage());
            }
        }

        private bool CanButtonActionCommandExecute(object p)
        {
            return !Context.Script.Equals(Context.CopyScript);
        }

        private void OnToMainCommandExecuted(object p)
        {
            Program.SetPage(new ActionsPage());
        }

        private bool ValidationCommand() 
        {
            return true;
        }
    }
}
