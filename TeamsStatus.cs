using System.Text.RegularExpressions;

namespace TeamsStatusLight
{
    public class TeamsStatus : ITeamsStatus
    {
        Presence _presence;

        public TeamsStatus() { 
            _presence = new Presence();
        }
        public IPresence getPresence()
        {

            IEnumerable<string> logFiles = Directory.EnumerateFiles(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Packages", "MSTeams_8wekyb3d8bbwe", "LocalCache", "Microsoft", "MSTeams", "Logs"), "MSTeams_*.log", SearchOption.TopDirectoryOnly).OrderByDescending(file => file);
            string logFile = logFiles.First();

            // read an entire line (i.e., until a newline is detected)
            // reverse the read line
            // check that it matches the pattern 2024-11-27T20:18:06.744374-05:00 0x000047bc native_modules::UserDataCrossCloudModule: BroadcastGlobalState: New Global State Event: UserDataGlobalState total number of users: 1 {user id: , availability: , unread notification count: x}

            // There's probably a more efficient way of reading the log backwards.  For now let's load the whole file line by line.
            Regex regex = new Regex(@"^\d{4}\-\d{2}\-\d{2}T\d{2}:\d{2}:\d{2}\.\d{6}[+|\-]\d{2}:\d{2} 0x[0-9a-f]{8} <INFO> native_modules::UserDataCrossCloudModule: BroadcastGlobalState: New Global State Event: UserDataGlobalState total number of users: \d{1,2} { user id :[0-9a-f]{8}\-[0-9a-f]{4}\-[0-9a-f]{4}\-[0-9a-f]{4}\-[0-9a-f]{12}, availability: (.+?), unread notification count: \d{1,2} }$");

            string[] fs = File.ReadAllLines(logFile);
            for (int l = fs.Length - 1; l >= 0; l--)
            {
                string s = fs[l];
                if (regex.Match(s).Success)
                {
                    Console.WriteLine("Match found!");
                    this._presence.availability = regex.Matches(s)[0].Groups[1].Value;
                    break;
                };
            }
            return this._presence;
        }
    }
}
