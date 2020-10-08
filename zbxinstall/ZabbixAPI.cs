using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Text;


namespace zbxinstall
{
    class ZabbixAPI
    {
        public static void Createhost()
        {
            HttpF.CreateHost();
            //доделать проверки
        }
    }
    class HttpF : ZabbixAPI
    {
        static string URI = Http_s.Protocol[0] + Http_s.ServerIP + Http_s.ZabbixURL + Http_s.APIURL;
        private static string Ayth()
        {
            string responseString;
            try
            {
                using (var WebClient = new WebClient())
                {
                    var request = WebRequest.Create(URI);
                    request.ContentType = Http_s.ContentType[0];
                    request.Method = Http_s.Method[0];
                    Auth_param_json auth_Param_Json = new Auth_param_json { user = "admin", password = "efiamors" };
                    Auth_json auth_Json = new Auth_json { jsonrpc = "2.0", method = "user.login", @params = auth_Param_Json, id = "1" };
                    using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                    {
                        streamWriter.Write(Json.Create(auth_Json));
                    }
                    HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                        responseString = reader.ReadToEnd();
                    }
                }
                return responseString;
            }
            catch
            {
                return null;
            }
        }
        internal static string CreateHost()
        {
            string responseString;
            try
            {
                using (var WebClient = new WebClient())
                {
                    var request = WebRequest.Create(URI);
                    request.ContentType = Http_s.ContentType[0];
                    request.Method = Http_s.Method[0];
                    Host_parametrs_templates_json host_Parametrs_Templates_Json = new Host_parametrs_templates_json { templateid = "10352" };
                    Host_parametrs_group_json host_Parametrs_Group_Json = new Host_parametrs_group_json { groupid = "20" };
                    Host_parametrs_interface_json host_Parametrs_Interface_Json = new Host_parametrs_interface_json { type = "1", main = "1", useip = "0", ip = Info.IP(), dns = Info.DNSName() + ".1eska.local", port = "10051" };
                    Host_parametrs_json host_Parametrs_Json = new Host_parametrs_json { host = Info.DNSName().ToUpper(), interfaces = host_Parametrs_Interface_Json, groups = host_Parametrs_Group_Json, templates = host_Parametrs_Templates_Json, inventory_mode = "0", proxy_hostid = "10417" };
                    Host_json host_Json = new Host_json { jsonrpc = "2.0", method = "host.create", @params = host_Parametrs_Json, auth = Json.Disassemble(HttpF.Ayth()), id = "1" };
                    using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                    {
                        streamWriter.Write(Json.Create(host_Json));
                    }
                    HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                        responseString = reader.ReadToEnd();
                    }
                }
                return responseString;
            }
            catch
            {
                return null;
            }
        }
    }
    class Info
    { 
        internal static string DNSName()
        {
            try
            {
                string host = Dns.GetHostName();
                return host;
            }
            catch
            {
                return null;
            }
        }
        internal static string IP()
        {
            try
            {
                IPAddress ip = Dns.GetHostByName(DNSName()).AddressList[0];
                return ip.ToString();
            }
            catch
            {
                return null;
            }
        }
    }
    class Json
    {
        internal static string Create(object _json)
        {
            return JsonConvert.SerializeObject(_json, Formatting.Indented);
        }
        internal static string Disassemble(string _json)
        {
            Response response = JsonConvert.DeserializeObject<Response>(_json);
            return response.result;
        }
    }
    class Response
    { 
        public string jsonrpc { get; set; }
        public string result { get; set; }
        public string id { get; set; }
    }
    class Auth_json
    { 
        public string jsonrpc { get; set; }
        public string method { get; set; }
        public Auth_param_json @params { get; set; }
        public string id { get; set; }
        public string auth { get; set; }
    }
    class Auth_param_json
    { 
        public string user { get; set; }
        public string password { get; set; }
    }
    class Host_json
    {
        public string jsonrpc { get; set; }
        public string method { get; set; }
        public Host_parametrs_json @params { get; set; }
        public string auth { get; set; }
        public string id { get; set; }
    }
    class Host_parametrs_json
    {
        public string host { get; set; }
        public Host_parametrs_interface_json interfaces { get; set; }
        public Host_parametrs_group_json groups { get; set; }
        public Host_parametrs_templates_json templates {get; set;}
        public string inventory_mode { get; set; }
        public string proxy_hostid { get; set; }
    }
    class Host_parametrs_interface_json
    { 
        public string type { get; set; }
        public string main { get; set; }
        public string useip { get; set; }
        public string ip { get; set; }
        public string dns { get; set; }
        public string port { get; set; }
    }
    class Host_parametrs_group_json
    { 
        public string groupid { get; set; }
    }
    class Host_parametrs_templates_json
    { 
        public string templateid { get; set; }
    }
    struct Http_s
    {
        internal static string[] Method { get; } = { "POST", "GET" };
        internal static string[] Protocol { get; } = { "http://", "https://" };
        internal static string[] ContentType { get; } = { "application/json-rpc", "application/json", "application/jsonrequest" };
        internal static string ServerIP { get; } = "13.0.1.196";
        internal static string ZabbixURL { get; } = "/zabbix";
        internal static string APIURL { get; } = "/api_jsonrpc.php";
    }
}
