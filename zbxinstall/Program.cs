using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace zbxinstall
{
    class Program
    {
        static void Main(string[] args)
        {
            if (IO.CheckDirExists(IOs.RootDir + IOs.ZabbixAgent) == true)
            {
                if (Version.GetVersion() == true)
                {
                    Update.
                }
            }
            else
            { 
                
            }
        }
    }
}
