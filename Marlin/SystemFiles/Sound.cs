using Marlin.SystemFiles.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
