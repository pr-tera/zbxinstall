using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace zbxinstall
{
    class IO
    {
        internal static string CreatFolder(string dir)
        {
            if (CheckDirExists(dir) == true)
            {
                if (DelDir(dir) == true)
                {
                    try
                    {
                        DirectoryInfo directoryInfo = new DirectoryInfo(dir);
                        directoryInfo.Create();
                        Logs.Log += $"{DateTime.Now} Директория {directoryInfo.Name} создана.\n";
                    }
                    catch (Exception ex)
                    {
                        Logs.Log += $"{DateTime.Now} Директория не создана.\n {ex}\n";
                        dir = string.Empty;                    }

                }
                else
                {
                    Logs.Log += "Ошибка при удалении файла\n";
                    dir = string.Empty;
                }
            }
            else
            {
                try
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(dir);
                    directoryInfo.Create();
                    Logs.Log += $"{DateTime.Now} Директория {directoryInfo.Name} создана.\n";
                    return dir;
                }
                catch (Exception ex)
                {
                    Logs.Log += $"{DateTime.Now} Директория не создана.\n {ex}\n";
                    dir = string.Empty;
                }
            }
            return dir;
        }
        internal static bool CheckDirExists(string dir)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(dir);
            if (directoryInfo.Exists)
            {
                Logs.Log += $"{DateTime.Now} Директория {directoryInfo.Name} существует.\n";
                return true;
            }
            else
            {
                Logs.Log += $"{DateTime.Now} Директория {directoryInfo.Name} не существует.\n";
                return false;
            }
        }
        internal static bool DelDir(string dir)
        {
            try
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(dir);
                directoryInfo.Delete(true);
                Logs.Log += $"{DateTime.Now} Директория {directoryInfo.Name} удалена!\n";
                return true;
            }
            catch (Exception ex)
            {
                Logs.Log += $"{DateTime.Now} {ex}\n";
                return false;
            }
        }
        internal static bool CheckFileExists(string File)
        {
            FileInfo fileInfo = new FileInfo(File);
            if (fileInfo.Exists)
            {
                Logs.Log += $"{DateTime.Now} Файл {fileInfo.Name} существует!\n";
                return true;
            }
            else
            {
                Logs.Log += $"{DateTime.Now} Файл {fileInfo.Name} не существует!\n";
                return false;
            }
        }
        internal static bool DelFile(string File)
        {
            FileInfo fileInfo = new FileInfo(File);
            if (fileInfo.Exists)
            {
                try
                {
                    fileInfo.Delete();
                    Logs.Log += $"{DateTime.Now} Файл {fileInfo.Name} удален!\n";
                    return true;
                }
                catch (Exception ex)
                {
                    Logs.Log += $"{DateTime.Now} {ex}\n";
                    return false;
                }
            }
            else
            {
                Logs.Log += $"{DateTime.Now} Файл {fileInfo.Name} не существует!\n";
                return false;
            }
        }
        internal static bool MoveFile(string File)
        {
            FileInfo fileInfo = new FileInfo(File);
            if (fileInfo.Exists)
            {
                try
                {
                    if (CheckFileExists(IOs.RootDir + IOs.ZabbixAgent + @"\" + fileInfo.Name) == true)
                    {
                        if (DelFile(IOs.RootDir + IOs.ZabbixAgent + @"\" + fileInfo.Name) == true)
                        {
                            fileInfo.MoveTo(IOs.RootDir + IOs.ZabbixAgent + @"\" + fileInfo.Name);
                            if (CheckFileExists(IOs.RootDir + IOs.ZabbixAgent + @"\" + fileInfo.Name) == true)
                            {
                                Logs.Log += $"{DateTime.Now} Файл {fileInfo.Name} перемещён.\n";
                                return true;
                            }
                            else
                            {
                                Logs.Log += $"{DateTime.Now} Файл {fileInfo.Name} не был перемещён.\n";
                                return false;
                            }
                        }
                        else
                        {
                            Logs.Log += $"{DateTime.Now} Файл {fileInfo.Name} не удалось удалить.\n";
                            return false;
                        }
                    }
                    else
                    {
                        fileInfo.MoveTo(IOs.RootDir + IOs.ZabbixAgent + @"\" + fileInfo.Name);
                        if (CheckFileExists(IOs.RootDir + IOs.ZabbixAgent + @"\" + fileInfo.Name) == true)
                        {
                            Logs.Log += $"{DateTime.Now} Файл {fileInfo.Name} перемещён.\n";
                            return true;
                        }
                        else
                        {
                            Logs.Log += $"{DateTime.Now} Файл {fileInfo.Name} не был перемещён.\n";
                            return false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logs.Log += $"{DateTime.Now} {ex}\n";
                    return false;
                }
            }
            else
            {
                Logs.Log += $"{DateTime.Now} Файл {fileInfo.Name} не существует!\n";
                return false;
            }
        }
        internal static bool GetFileInDir(string Dir)
        {
            try
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(Dir);
                foreach (var file in directoryInfo.GetFiles())
                {
                    IOs.TempFiles.Add(file.FullName);
                }
                return true;
            }
            catch (Exception ex)
            {
                Logs.Log += $"{DateTime.Now} Исключение при получении списка файлов\n {ex}\n";
                return false;
            }
        }
    }
    struct IOs
    {
        internal static string RootDir { get; } = Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.System));
        //internal static string RootDir { get; } = @"C:\";
        internal static string CurrDir { get; } = Environment.CurrentDirectory;
        internal static string ZbxTempPAth { get; } = "Zbxtemp";
        internal static string ZabbixAgent { get; } = "Zabbix agent";
        internal static string Zabbix_agent { get; set; } = "zabbix_agentd.exe";
        internal static string Zabbixscr { get; set; } = "zabbixscr.exe";
        internal static string Userparams { get; set; } = "zabbix_agentd.userparams.conf";
        internal static string Config { get; set; } = "zabbix_agentd.conf";
        internal static List<string> TempFiles = new List<string>();
        internal static string LogPath { get; } = "logupdate.log";
    }
}
