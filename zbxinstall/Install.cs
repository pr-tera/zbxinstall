using System;
using System.Diagnostics;

namespace zbxinstall
{
    class Install
    {
        internal static void Start()
        {
            Process Install = new Process();
            Install.StartInfo.FileName = $"{IOs.RootDir}{IOs.ZabbixAgent}\\{IOs.Zabbix_agent}";
            Install.StartInfo.Arguments = $"--config \"{IOs.RootDir}{IOs.ZabbixAgent}\\{IOs.Config}\" \" --install\"";
            Install.Start();
            Install.WaitForExit();
            Install.Close();
            Process Start = new Process();
            Start.StartInfo.FileName = $"{IOs.RootDir}{IOs.ZabbixAgent}\\{IOs.Zabbix_agent}";
            Start.StartInfo.Arguments = $"--config \"{IOs.RootDir}{IOs.ZabbixAgent}\\{IOs.Config}\" \"--start\"";
            Start.Start();
            Environment.Exit(0);
        }
    }
}
