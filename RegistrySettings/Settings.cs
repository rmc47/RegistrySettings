using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RegistrySettings
{
    public sealed class Settings
    {
        private readonly string s_RegistryRoot;

        public Settings()
            : this (Assembly.GetCallingAssembly().GetName().Name)
        {
        }

        public Settings(string appName)
        {
            s_RegistryRoot = "SOFTWARE\\" + appName;
        }

        public string this[string key]
        {
            get
            {
                using (RegistryKey regKey = Registry.CurrentUser.OpenSubKey(s_RegistryRoot, false))
                {
                    if (regKey == null)
                        return null;
                    return (string)regKey.GetValue(key);
                }
            }
            set
            {
                using (RegistryKey regKey = Registry.CurrentUser.CreateSubKey(s_RegistryRoot))
                {
                    regKey.SetValue(key, value);
                }
            }
        }

        public int GetInt(string key, int defaultVal)
        {
            int i;
            if (int.TryParse(this[key], out i))
                return i;
            else
                return defaultVal;
        }

        public string GetString(string key, string defaultVal)
        {
            return this[key] ?? defaultVal;
        }
    }
}
