using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace zbxinstall
{
    class Version
    {
        internal static bool GetVersion()
        {
            FTPs.FileList.Clear();
            FTPs.FileList.Add($"-rw - r--r--    1 1015     1015                   166 Aug {FTPs.Version}");
            
            if (FTP.Download() == true)
            {
                if (XML.Open(IOs.RootDir + IOs.ZbxTempPAth + @"\" + FTPs.Version, false) == true)
                {               
                    if (XML.Open(IOs.RootDir + IOs.ZabbixAgent + @"\" + FTPs.Version , true) == true)
                    {
                        return true;
                    }
                    else
                    {
                        Logs.Log += $"{DateTime.Now} Локальный файл версии не существует!\n";
                        return true;
                    }
                }
                else
                {
                    Logs.Log += $"{DateTime.Now} Проверка версии не удалась, не удалось открыть файл версии.\n";
                    return false;
                }
            }
            else
            {
                Logs.Log += $"{DateTime.Now} Проверка версии не удалась.\n";
                return false;
            }
        }
    }
    struct Versions
    {
        internal static int Zabbix_agent { get; set; }
        internal static int Config { get; set; }
        internal static int Userparams { get; set; }
        internal static int Zabbixscr { get; set; }
    }
    struct VersionsLocal
    {
        internal static int Zabbix_agent { get; set; }
        internal static int Config { get; set; }
        internal static int Userparams { get; set; }
        internal static int Zabbixscr { get; set; }
    }
}
