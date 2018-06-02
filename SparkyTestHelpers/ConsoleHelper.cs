using System;
using System.IO;

namespace SparkyTestHelpers
{
    public static class ConsoleHelper
    {
        public static int GetWidth()
        {
            int width = 80;

            try
            {
                if (!Console.IsOutputRedirected)
                {
                    width = Console.BufferWidth;
                }
            }
            catch (IOException)
            {
            }

            return width;
        }
    }
}
