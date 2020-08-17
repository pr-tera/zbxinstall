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
                FTPs.FileList.Add(IOs.Zabbix_agent);
            }
            FTPs.FileList.Add(IOs.Zabbix_agent);
            if (Versions.Config > VersionsLocal.Config)
            {
                FTPs.FileList.Add(IOs.Config);
            }
            if (Versions.Userparams > VersionsLocal.Userparams)
            {
                FTPs.FileList.Add(IOs.Userparams);
            }
            if (Versions.Zabbixscr > VersionsLocal.Zabbixscr)
            {
                FTPs.FileList.Add(IOs.Zabbixscr);
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
