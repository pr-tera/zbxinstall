using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace zbxinstall
{
    class Update
    {
        private static void Check()
        {
            if (Versions.Zabbix_agent > VersionsLocal.Zabbix_agent)
            {
                FTPs.FileList.Add($"-rw - r--r--    1 1015     1015                   166 Aug {IOs.Zabbix_agent}");
            }
            if (Versions.Config > VersionsLocal.Config)
            {
                FTPs.FileList.Add($"-rw - r--r--    1 1015     1015                   166 Aug {IOs.Config}");
            }
            if (Versions.Userparams > VersionsLocal.Userparams)
            {
                FTPs.FileList.Add($"-rw - r--r--    1 1015     1015                   166 Aug {IOs.Userparams}");
            }
            if (Versions.Zabbixscr > VersionsLocal.Zabbixscr)
            {
                FTPs.FileList.Add($"-rw - r--r--    1 1015     1015                   166 Aug {IOs.Zabbixscr}");
            }
        }
        internal static bool Get()
        {
            bool check = false;
            Check();
            foreach (var file in FTPs.FileList)
            {
                if (FTP.Download() == true)
                {
                    check = true;
                }
                else
                {
                    check = false;
                }
            }
            return check;
        }
    }
}
