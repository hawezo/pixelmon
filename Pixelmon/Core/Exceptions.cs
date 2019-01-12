using System;

namespace Pixelmon.Core
{
    public class Exceptions
    {

        public class MinecraftNotFoundException : Exception
        {
            public string GivenPath { get; private set; }

            public MinecraftNotFoundException(string givenPath) : base()
            {
                this.GivenPath = givenPath;
            }
        }

        public class PixelmonNotFoundException : Exception
        {
            public string GivenPath { get; private set; }

            public PixelmonNotFoundException(string givenPath) : base()
            {
                this.GivenPath = givenPath;
            }
        }

        public class AppdataNotFoundException : Exception
        {
            public string GivenPath { get; private set; }

            public AppdataNotFoundException(string givenPath) : base()
            {
                this.GivenPath = givenPath;
            }
        }
        
        public class ForgeTimeoutException : Exception
        {
            public ForgeTimeoutException() : base()
            {
            }
        }
    }
}
