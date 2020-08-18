using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Messaging;
using System.Text;

namespace zbxinstall
{
    class FTP
    {
        protected static string _URI(string File)
        {
            if (string.IsNullOrEmpty(File))
            {
                string _uri = FTPs.Head + FTPs.Server + FTPs.Port + FTPs.Folder;
                return _uri;
            }
            else
            {
                string _uri = FTPs.Head + FTPs.Server + FTPs.Port + FTPs.Folder + File;
                return _uri;
            }    
        }
        protected static bool _checkFTP()
        {
            bool _check = false;
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(_URI(null));
            request.Method = WebRequestMethods.Ftp.ListDirectory;
            request.Credentials = new NetworkCredential(FTPs.Login, FTPs.Password);
            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            switch (response.StatusCode)
            {
                case FtpStatusCode.AccountNeeded:
                    _check = false;
                    break;
                case FtpStatusCode.OpeningData:
                    _check = true;
                    break;
                case FtpStatusCode.CommandOK:
                    _check = true;
                    break;
            }
            response.Close();
            return _check;
        }
        internal static void GetListFolber()
        {
            if (_checkFTP() == true)
            {
                try
                {
                    FtpWebRequest request = (FtpWebRequest)WebRequest.Create(_URI(null));
                    request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                    request.Credentials = new NetworkCredential(FTPs.Login, FTPs.Password);
                    using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                    using (Stream listStream = response.GetResponseStream())
                    using (StreamReader listReader = new StreamReader(listStream))
                    {
                        while (!listReader.EndOfStream)
                        {
                            FTPs.FileList.Add(listReader.ReadLine());
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logs.Log += $"{DateTime.Now} {ex}\n";
                }
            }
            else
            {
                Logs.Log += $"{DateTime.Now} Не удалось получить список файлов с ftp.\n";
            }
        }
        protected static bool _download(string File)
        {
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(_URI(File));
                request.Method = WebRequestMethods.Ftp.DownloadFile;
                request.Credentials = new NetworkCredential(FTPs.Login, FTPs.Password);
                request.EnableSsl = false;
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                FileStream fs = new FileStream(File, FileMode.Create);
                FileStream dfs = new FileStream(IOs.RootDir + IOs.ZbxTempPAth + @"\" + File, FileMode.Create);
                byte[] buffer = new byte[64];
                int size = 0;
                while ((size = responseStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    //fs.Write(buffer, 0, size);
                    fs.CopyTo(dfs);
                    dfs.Write(buffer, 0, size);
                }
                fs.Close();
                dfs.Close();
                response.Close();
                IO.DelFile(IOs.CurrDir + @"\" + File);
                return true;
            }
            catch (Exception ex)
            {
                Logs.Log += $"{DateTime.Now} {ex}\n";
                return false;
            }
        }
        internal static bool Download()
        {
            bool check = false;
            if (_checkFTP() == true)
            {
                foreach (var file in FTPs.FileList)
                {
                    string[] tokens = file.Split(new[] { ' ' }, 9, StringSplitOptions.RemoveEmptyEntries);
                    string filename = tokens[8];
                    string filepath = IOs.RootDir + IOs.ZbxTempPAth + @"\" + filename;
                    if (IO.CheckFileExists(filepath) == true)
                    {
                        if (IO.DelFile(filepath) == true)
                        {
                            _download(filename);
                            Logs.Log += $"{DateTime.Now} Началась загрузка файла {filename}\n";
                            if (IO.CheckFileExists(filepath) == true)
                            {
                                Logs.Log += $"{DateTime.Now} Файл {filename} успешно загружен\n";
                                check = true;
                            }
                            else
                            {
                                Logs.Log += $"{DateTime.Now} Не удалось загрузить файл {filename}\n";
                                check = false;
                            }
                        }
                        else
                        {
                            Logs.Log += $"{DateTime.Now} Не удалось начать загрузку {filename}\n";
                            check = false;
                        }
                    }
                    else
                    {
                        _download(filename);
                        Logs.Log += $"{DateTime.Now} Началась загрузка файла {filename}\n";
                        if (IO.CheckFileExists(filepath) == true)
                        {
                            Logs.Log += $"{DateTime.Now} Файл {filename} успешно загружен\n";
                            check = true;
                        }
                        else
                        {
                            Logs.Log += $"{DateTime.Now} Не удалось загрузить файл {filename}\n";
                            check = false;
                        }
                    }
                }
            }
            else
            {
                Logs.Log += $"{DateTime.Now} не удалось начать загрузку файла т.к. FTP не доступен";
                check = false;
            }
            return check;
        }
    }
    struct FTPs
    {
        internal static string Head { get; } = "ftp://";
        internal static string Server { get; } = "1eskaftp.hldns.ru";
        internal static string Port { get; } = ":22526";
        internal static string Login { get; } = "test1eska";
        internal static string Password { get; } = "thvLaQ8dIv8zTPKdc7wf63hu5nLRVBAmov3Qx1lpKGbaW";
        internal static string Folder { get; } = "/test/zabbix/";
        internal static string Version { get; } = "version.ver";
        internal static List<string> FileList = new List<string>();
    }
}
