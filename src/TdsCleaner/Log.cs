﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TdsCleaner
{
    public static class Log
    {
        public static void Info(string category, string message, params object[] args)
        {
            Console.Write("[{0}] ", category);
            Console.WriteLine(message, args);
        }
    }
}
