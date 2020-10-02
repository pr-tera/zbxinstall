using System;
using System.Diagnostics.Eventing.Reader;

namespace zbxinstall
{
    class Program
    {
        static void Main(string[] args)
        {
            Start();
            ZabbixAPI.Createhost();
        }
        private static void Start()
        {
            if (IO.CheckDirExists(IOs.RootDir + IOs.ZabbixAgent) == true)
            {
                if (IO.CheckDirExists(IOs.RootDir + IOs.ZbxTempPAth) == true)
                {
                    if (Version.GetVersion() == true)
                    {
                        if (Update.Get() == true)
                        {
                            if (IO.GetFileInDir(IOs.RootDir + IOs.ZbxTempPAth) == true)
                            {
                                if (Service.Stop() == true)
                                {
                                    foreach (var file in IOs.TempFiles)
                                    {
                                        if (IO.MoveFile(file) == true)
                                        {
                                            Logs.Log += $"\n{DateTime.Now} Файл {file} перемещён из временной папки.\n";
                                        }
                                        else
                                        {
                                            Logs.Log += $"\n{DateTime.Now} Файл не {file} перемещён из временной папки.\n";
                                        }
                                    }
                                    if (Service.Start() == true)
                                    {                                       
                                        Logs.Log += $"\n{DateTime.Now} Обновление прошло успешно!.\n";
                                        if (IO.DelDir(IOs.RootDir + IOs.ZbxTempPAth) == true)
                                        {
                                            Logs.Log += $"\n{DateTime.Now} Удаление каталога временных файлов прощло успешно.\n";
                                            IO.DelDir(IOs.RootDir + IOs.ZbxTempPAth);
                                            Log.WriteAsync();
                                        }
                                        else
                                        {
                                            Logs.Log += $"\n{DateTime.Now} Не удалось удалить каталог временных файлов.\n";
                                            IO.DelDir(IOs.RootDir + IOs.ZbxTempPAth);
                                            Log.WriteAsync();
                                        }
                                    }
                                    else
                                    {
                                        Logs.Log += $"\n{DateTime.Now} Не удалось запустить сзлужбу.\n";
                                        IO.DelDir(IOs.RootDir + IOs.ZbxTempPAth);
                                        Log.WriteAsync();
                                    }
                                }
                                else
                                {
                                    Logs.Log += $"\n{DateTime.Now} Не удалось остановить службу.\n";
                                    IO.DelDir(IOs.RootDir + IOs.ZbxTempPAth);
                                    Log.WriteAsync();
                                }
                            }
                            else
                            {
                                Logs.Log += $"\n{DateTime.Now} Не удалось получить список файлов.\n";
                                IO.DelDir(IOs.RootDir + IOs.ZbxTempPAth);
                                Log.WriteAsync();
                            }
                        }
                        else
                        {
                            Logs.Log += $"\n{DateTime.Now} Обновление не требуется.\n";
                            IO.DelDir(IOs.RootDir + IOs.ZbxTempPAth);
                            Log.WriteAsync();
                        }
                    }
                    else
                    {
                        Logs.Log += $"\n{DateTime.Now} Ошибка чтения файла версии.\n";
                        IO.DelDir(IOs.RootDir + IOs.ZbxTempPAth);
                        Log.WriteAsync();
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(IO.CreatFolder(IOs.RootDir + IOs.ZbxTempPAth)))
                    {
                        Start();
                    }
                    else
                    {
                        Logs.Log += $"\n{DateTime.Now} Не удалось создать каталог для временных файлов.\n";
                        //IO.DelDir(IOs.RootDir + IOs.ZbxTempPAth);
                        Log.WriteAsync();
                    }
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(IO.CreatFolder(IOs.RootDir + IOs.ZabbixAgent)))
                {
                    if (!string.IsNullOrEmpty(IO.CreatFolder(IOs.RootDir + IOs.ZbxTempPAth)))
                    {
                        FTP.GetListFolber();
                        if (FTP.Download() == true)
                        {
                            if (IO.GetFileInDir(IOs.RootDir + IOs.ZbxTempPAth) == true)
                            {
                                foreach (var file in IOs.TempFiles)
                                {
                                    if (IO.MoveFile(file) == true)
                                    {
                                        Logs.Log += $"\n{DateTime.Now} Файл {file} перемещён из временной папки.\n";
                                    }
                                    else
                                    {
                                        Logs.Log += $"\n{DateTime.Now} Файл не {file} перемещён из временной папки.\n";
                                    }
                                }
                                Install.Start();
                                if (Service.Status() == true)
                                {
                                    Logs.Log += $"\n{DateTime.Now} Установка прошла успешно.\n";
                                    Log.WriteAsync();
                                }
                                IO.DelDir(IOs.RootDir + IOs.ZbxTempPAth);
                            }
                            else
                            {
                                Logs.Log += $"\n{DateTime.Now} Не удалось получить список файлов.\n";
                                IO.DelDir(IOs.RootDir + IOs.ZbxTempPAth);
                                Log.WriteAsync();
                            }
                        }
                        else
                        {
                            Logs.Log += $"\n{DateTime.Now} Не удалось загрузить агента.\n";
                            IO.DelDir(IOs.RootDir + IOs.ZbxTempPAth);
                            Log.WriteAsync();
                        }
                    }
                    else
                    {
                        Logs.Log += $"\n{DateTime.Now} Не удалось создать каталог для временных файлов.\n";
                        IO.DelDir(IOs.RootDir + IOs.ZbxTempPAth);
                        Log.WriteAsync();
                    }
                }
                else
                {
                    Logs.Log += $"\n{DateTime.Now} Не удалось создать каталог размещения агента.\n";
                    IO.DelDir(IOs.RootDir + IOs.ZbxTempPAth);
                    Log.WriteAsync();
                }
            }
        }
    }
}
