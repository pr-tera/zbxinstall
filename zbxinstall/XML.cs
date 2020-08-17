using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace zbxinstall
{
    class XML
    {
        internal static bool Open(string File, bool local)
        {
            if (IO.CheckFileExists(File) == true)
            {
                if (local == true)
                {
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.Load(File);
                    VersionsLocal.Config = Convert.ToInt32(xmlDocument.SelectSingleNode("version/Config").InnerText);
                    VersionsLocal.Userparams = Convert.ToInt32(xmlDocument.SelectSingleNode("version/Userparams").InnerText);
                    VersionsLocal.Zabbixscr = Convert.ToInt32(xmlDocument.SelectSingleNode("version/Zabbixscr").InnerText);
                    VersionsLocal.Zabbix_agent = Convert.ToInt32(xmlDocument.SelectSingleNode("version/Zabbix_agent").InnerText);
                    return true;
                }
                else
                {
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.Load(File);
                    Versions.Config = Convert.ToInt32(xmlDocument.SelectSingleNode("version/Config").InnerText);
                    Versions.Userparams = Convert.ToInt32(xmlDocument.SelectSingleNode("version/Userparams").InnerText);
                    Versions.Zabbixscr = Convert.ToInt32(xmlDocument.SelectSingleNode("version/Zabbixscr").InnerText);
                    Versions.Zabbix_agent = Convert.ToInt32(xmlDocument.SelectSingleNode("version/Zabbix_agent").InnerText);
                    return true;
                }
            }
            else
            {
                Logs.Log += $"{DateTime.Now} Не удалось открыть файл версии.\n";
                return false;
            }
        }
    }
}
