using Marlin.SystemFiles.Types;
using System.Speech.Synthesis;
using System.Threading;
using System.Threading.Tasks;

namespace Marlin.SystemFiles
{
    public class Voix
    {
        private static readonly SpeechSynthesizer Synthesizer = new SpeechSynthesizer();
        private static CancellationTokenSource CancellationTokenSource = new CancellationTokenSource();

        public static void Speak(string text)
        {
            if (!Context.Settings.IsSay)
                return;

            CancellationTokenSource.Cancel();
            CancellationTokenSource.Dispose();
            CancellationTokenSource = new CancellationTokenSource();

            try
            {
                Synthesizer.SelectVoice("Microsoft Irina Desktop");
                Synthesizer.Rate = Context.Settings.Speed;
                Synthesizer.Speak(text);
            }
            catch
            {
                Models.MessageBox.MakeMessage("Возникла ошибка озвучивания", MessageType.Error);
            }
        }

        public static async Task SpeakAsync(string text)
        {
            if (!Context.Settings.IsSay)
                return;

            CancellationTokenSource.Cancel();
            CancellationTokenSource.Dispose();
            CancellationTokenSource = new CancellationTokenSource();

            try
            {
                Synthesizer.SpeakAsyncCancelAll();
                Synthesizer.SelectVoice("Microsoft Irina Desktop");
                Synthesizer.Rate = Context.Settings.Speed;
                Synthesizer.SpeakAsync(text);
            }
            catch
            {
                Models.MessageBox.MakeMessage("Возникла ошибка озвучивания", MessageType.Error);
            }
        }
    }
}