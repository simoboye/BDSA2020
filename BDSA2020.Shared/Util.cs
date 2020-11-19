using System;

namespace BDSA2020.Shared
{
    public class Util
    {
        public static void LogError(Exception e, bool isTest = false)
        {   
            if (!isTest)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e); // Primitive logging
                Console.ResetColor();
            }
        }
    }
}
