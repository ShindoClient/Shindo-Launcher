using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Shindo_Launcher.Utils
{
    internal class INIFile
    {
        private string path;

        public INIFile(string iniPath)
        {
            path = iniPath;
        }

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern long WritePrivateProfileString(string section, string key, string value, string filePath);

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern int GetPrivateProfileString(string section, string key, string defaultValue,
            StringBuilder retVal, int size, string filePath);

        public void Write(string section, string key, string value)
        {
            WritePrivateProfileString(section, key, value, path);
        }

        public string Read(string section, string key, string defaultValue = "")
        {
            StringBuilder buffer = new StringBuilder(255);
            GetPrivateProfileString(section, key, defaultValue, buffer, 255, path);
            return buffer.ToString();
        }
    }
}
