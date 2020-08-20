using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace zbxinstall
{
    class Update
    {
        private static bool Check()
        {
            bool check = false;
            if (Versions.Zabbix_agent > VersionsLocal.Zabbix_agent)
            {
                FTPs.FileList.Add($"-rw - r--r--    1 1015     1015                   166 Aug {IOs.Zabbix_agent}");
                check = true;
            }
            if (Versions.Config > VersionsLocal.Config)
            {
                FTPs.FileList.Add($"-rw - r--r--    1 1015     1015                   166 Aug {IOs.Config}");
                check = true;
            }
            if (Versions.Userparams > VersionsLocal.Userparams)
            {
                FTPs.FileList.Add($"-rw - r--r--    1 1015     1015                   166 Aug {IOs.Userparams}");
                check = true;
            }
            if (Versions.Zabbixscr > VersionsLocal.Zabbixscr)
            {
                FTPs.FileList.Add($"-rw - r--r--    1 1015     1015                   166 Aug {IOs.Zabbixscr}");
                check = true;
            }
            return check;
        }
        internal static bool Get()
        {
            bool check = false;
            if (Check() == true)
            {
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
            }
            else
            {
                check = false;
            }
            return check;
        }
    }
}
