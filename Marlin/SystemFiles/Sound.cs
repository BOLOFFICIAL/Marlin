using Marlin.SystemFiles.Types;
using System.Media;
using System.Threading;

namespace Marlin.SystemFiles
{
    public class Sound
    {
        public static void PlaySound(MessageType messagetype)
        {
            var soundplayer = new SoundPlayer();
            switch (messagetype)
            {
                case MessageType.Info: soundplayer = new SoundPlayer(Properties.Resources.OkInfo); break;
                case MessageType.TextQuestion: soundplayer = new SoundPlayer(Properties.Resources.YesNo); break;
                case MessageType.YesNoQuestion: soundplayer = new SoundPlayer(Properties.Resources.YesNo); break;
                case MessageType.Error: soundplayer = new SoundPlayer(Properties.Resources.Error); break;
            }
            soundplayer.Play();
            Thread.Sleep(300);
        }
    }
}
