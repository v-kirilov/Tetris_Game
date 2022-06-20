using System;
using System.Threading;

namespace Tetris_OOP
{
    internal class MusicPlayer
    {
        public void Play()
        {
            new Thread(PlayMusic).Start();
        }

        private void PlayMusic()
        {
            while (true)
            {
                const int soundLength = 100;

                Console.Beep(1320, soundLength * 4);
                Console.Beep(990, soundLength * 2);
                Console.Beep(1056, soundLength * 2);
                Console.Beep(1188, soundLength * 2);
                Console.Beep(1320, soundLength);
                Console.Beep(1188, soundLength);
                Console.Beep(1056, soundLength * 2);
                Console.Beep(990, soundLength * 2);
                Console.Beep(880, soundLength * 4);
                Console.Beep(880, soundLength * 2);
                Console.Beep(1056, soundLength * 2);
                Console.Beep(1320, soundLength * 4);
                Console.Beep(1188, soundLength * 2);
                Console.Beep(1056, soundLength * 2);
                Console.Beep(990, soundLength * 6);
                Console.Beep(1056, soundLength * 2);
                Console.Beep(1188, soundLength * 4);
                Console.Beep(1320, soundLength * 4);
                Console.Beep(1056, soundLength * 4);
                Console.Beep(880, soundLength * 4);
                Console.Beep(880, soundLength * 4);
                Thread.Sleep(soundLength * 2);
                Console.Beep(1188, soundLength * 4);
                Console.Beep(1408, soundLength * 2);
                Console.Beep(1760, soundLength * 4);
                Console.Beep(1584, soundLength * 2);
                Console.Beep(1408, soundLength * 2);
                Console.Beep(1320, soundLength * 6);
                Console.Beep(1056, soundLength * 2);
                Console.Beep(1320, soundLength * 4);
                Console.Beep(1188, soundLength * 2);
                Console.Beep(1056, soundLength * 2);
                Console.Beep(990, soundLength * 4);
                Console.Beep(990, soundLength * 2);
                Console.Beep(1056, soundLength * 2);
                Console.Beep(1188, soundLength * 4);
                Console.Beep(1320, soundLength * 4);
                Console.Beep(1056, soundLength * 4);
                Console.Beep(880, soundLength * 4);
                Console.Beep(880, soundLength * 4);
                Thread.Sleep(soundLength * 4);
                Console.Beep(1320, soundLength * 4);
                Console.Beep(990, soundLength * 2);
                Console.Beep(1056, soundLength * 2);
                Console.Beep(1188, soundLength * 2);
                Console.Beep(1320, soundLength);
                Console.Beep(1188, soundLength);
                Console.Beep(1056, soundLength * 2);
                Console.Beep(990, soundLength * 2);
                Console.Beep(880, soundLength * 4);
                Console.Beep(880, soundLength * 2);
                Console.Beep(1056, soundLength * 2);
                Console.Beep(1320, soundLength * 4);
                Console.Beep(1188, soundLength * 2);
                Console.Beep(1056, soundLength * 2);
                Console.Beep(990, soundLength * 6);
                Console.Beep(1056, soundLength * 2);
                Console.Beep(1188, soundLength * 4);
                Console.Beep(1320, soundLength * 4);
                Console.Beep(1056, soundLength * 4);
                Console.Beep(880, soundLength * 4);
                Console.Beep(880, soundLength * 4);
                Thread.Sleep(soundLength * 2);
                Console.Beep(1188, soundLength * 4);
                Console.Beep(1408, soundLength * 2);
                Console.Beep(1760, soundLength * 4);
                Console.Beep(1584, soundLength * 2);
                Console.Beep(1408, soundLength * 2);
                Console.Beep(1320, soundLength * 6);
                Console.Beep(1056, soundLength * 2);
                Console.Beep(1320, soundLength * 4);
                Console.Beep(1188, soundLength * 2);
                Console.Beep(1056, soundLength * 2);
                Console.Beep(990, soundLength * 4);
                Console.Beep(990, soundLength * 2);
                Console.Beep(1056, soundLength * 2);
                Console.Beep(1188, soundLength * 4);
                Console.Beep(1320, soundLength * 4);
                Console.Beep(1056, soundLength * 4);
                Console.Beep(880, soundLength * 4);
                Console.Beep(880, soundLength * 4);
                Thread.Sleep(soundLength * 4);
                Console.Beep(660, soundLength * 8);
                Console.Beep(528, soundLength * 8);
                Console.Beep(594, soundLength * 8);
                Console.Beep(495, soundLength * 8);
                Console.Beep(528, soundLength * 8);
                Console.Beep(440, soundLength * 8);
                Console.Beep(419, soundLength * 8);
                Console.Beep(495, soundLength * 8);
                Console.Beep(660, soundLength * 8);
                Console.Beep(528, soundLength * 8);
                Console.Beep(594, soundLength * 8);
                Console.Beep(495, soundLength * 8);
                Console.Beep(528, soundLength * 4);
                Console.Beep(660, soundLength * 4);
                Console.Beep(880, soundLength * 8);
                Console.Beep(838, soundLength * 16);
                Console.Beep(660, soundLength * 8);
                Console.Beep(528, soundLength * 8);
                Console.Beep(594, soundLength * 8);
                Console.Beep(495, soundLength * 8);
                Console.Beep(528, soundLength * 8);
                Console.Beep(440, soundLength * 8);
                Console.Beep(419, soundLength * 8);
                Console.Beep(495, soundLength * 8);
                Console.Beep(660, soundLength * 8);
                Console.Beep(528, soundLength * 8);
                Console.Beep(594, soundLength * 8);
                Console.Beep(495, soundLength * 8);
                Console.Beep(528, soundLength * 4);
                Console.Beep(660, soundLength * 4);
                Console.Beep(880, soundLength * 8);
                Console.Beep(838, soundLength * 16);
            }
        }

    }
}
