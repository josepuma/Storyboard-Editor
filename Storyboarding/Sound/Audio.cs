using System;
using ManagedBass;

namespace Storyboarding.Sound
{
    public class Audio
    {

        public string AudioPath { get; set; }
        public int Stream { get; set; }

        public bool IsPlaying { get; set; }

        public Audio(string audioPath)
        {
            AudioPath = audioPath;
            if (Bass.Init())
            {
                Stream = Bass.CreateStream(AudioPath, 0, 0);
            }
        }

        public void Play(double position = 0)
        {
         
            if (Stream != 0)
            {
                Bass.ChannelSetPosition(Stream, Bass.ChannelSeconds2Bytes(Stream, position / 1000));
                Bass.ChannelPlay(Stream, false);
                IsPlaying = true;
            }
            
        }

        public void Pause()
        {
            if (IsPlaying)
            {
                Bass.ChannelPause(Stream);
                IsPlaying = false;
            }
            else{
                Bass.ChannelPlay(Stream, false);
                IsPlaying = true;
            }
        }


        public double GetPosition()
        {
            return Math.Round(Bass.ChannelBytes2Seconds(Stream, Bass.ChannelGetPosition(Stream)) * 1000);
        }

        public void SetPosition(double newPosition)
        {
            if(IsPlaying)
                Bass.ChannelSetPosition(Stream, Bass.ChannelSeconds2Bytes(Stream, newPosition / 1000));
        }

        public double GetLength()
        {
            var lengthBytes = Bass.ChannelGetLength(Stream, PositionFlags.Bytes);
            var time = Bass.ChannelBytes2Seconds(Stream, lengthBytes) * 1000;

            return Math.Round(time);

        }

    }
}
