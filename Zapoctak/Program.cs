﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Zapoctak
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try
            {
                Application.Run(new Dama());
            }
            catch (Exception)
            {
            }
            
        }
    }
}