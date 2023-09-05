using Marlin.SystemFiles.Types;
using System.Media;
using System.Threading;
using System.Threading.Tasks;

namespace Marlin.SystemFiles
{
    public class Sound
    {
        private static SoundPlayer soundPlayer = new SoundPlayer();

        public static void PlaySoundAsync(MessageType messagetype)
        {
            LoadSound(messagetype);
            soundPlayer.Play();
        }

        public static void PlaySound(MessageType messagetype)
        {
            LoadSound(messagetype);
            soundPlayer.Play();
            Thread.Sleep(300);
        }

        private static void LoadSound(MessageType messagetype)
        {
            switch (messagetype)
            {
                case MessageType.Info:
                    soundPlayer.Stream = Properties.Resources.OkInfo;
                    break;
                case MessageType.TextQuestion:
                case MessageType.YesNoQuestion:
                    soundPlayer.Stream = Properties.Resources.YesNo;
                    break;
                case MessageType.Error:
                    soundPlayer.Stream = Properties.Resources.Error;
                    break;
            }
        }
    }
}
