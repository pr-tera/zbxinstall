using System;
using System.ServiceProcess;

namespace zbxinstall
{
    class Service
    {
        internal static bool Stop()
        {
            bool _check;
            TimeSpan timeSpan = TimeSpan.FromMinutes(1);
            ServiceController service = new ServiceController("Zabbix Agent");
            if (service.Status != ServiceControllerStatus.Stopped)
            {
                try
                {
                    service.Stop();
                }
                catch (Exception ex)
                {
                    Logs.Log += $"\n{ex}\n";
                }
                service.WaitForStatus(ServiceControllerStatus.Stopped, timeSpan);
            }
            if (service.Status == ServiceControllerStatus.Stopped)
            {
                _check = true;
            }
            else
            {
                _check = false;
            }
            return _check;
        }
        internal static bool Start()
        {
            bool _check;
            TimeSpan timeSpan = TimeSpan.FromMinutes(1);
            ServiceController service = new ServiceController("Zabbix Agent");
            if (service.Status != ServiceControllerStatus.Running)
            {
                try
                {
                    service.Start();
                }
                catch (Exception ex)
                {
                    Logs.Log += $"\n{ex}\n";
                }
                service.WaitForStatus(ServiceControllerStatus.Stopped, timeSpan);
            }
            if (service.Status == ServiceControllerStatus.Running)
            {
                _check = true;
            }
            else
            {
                _check = false;
            }
            return _check;
        }
        internal static bool Status()
        {
            bool _check = false;
            ServiceController service = new ServiceController("Zabbix Agent");
            switch (service.Status)
            {
                case ServiceControllerStatus.Stopped:
                    _check = false;
                    break;
                case ServiceControllerStatus.Running:
                    _check = true;
                    break;
                case ServiceControllerStatus.StopPending:
                    _check = false;
                    break;
                case ServiceControllerStatus.ContinuePending:
                    _check = false;
                    break;
                case ServiceControllerStatus.Paused:
                    _check = false;
                    break;
                case ServiceControllerStatus.PausePending:
                    _check = false;
                    break;
                case ServiceControllerStatus.StartPending:
                    _check = true;
                    break;
            }
            return _check;
        }
    }
}
