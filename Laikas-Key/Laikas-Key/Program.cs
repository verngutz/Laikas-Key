using System;

namespace Laikas_Key
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (Laikas game = new Laikas())
            {
                game.Run();
            }
        }
    }
#endif
}

