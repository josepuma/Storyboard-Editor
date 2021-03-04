using System;
using Storyboarding.Animation;
using Storyboarding.Sound;

namespace Storyboarding
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            using (var game = new BaseGame())
            {
                game.Run();
            }
                
        }
    }
}
