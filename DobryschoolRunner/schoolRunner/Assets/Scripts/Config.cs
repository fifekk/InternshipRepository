using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

public class Config : MonoBehaviour
{
    private static string path = Application.dataPath + "/ServerIP.ini";

    [DllImport("kernel32")]
    private static extern long WritePrivateProfileString(string section,
        string key, string val, string filePath);
    [DllImport("kernel32")]
    private static extern int GetPrivateProfileString(string section,
             string key, string def, StringBuilder retVal,
        int size, string filePath);

    public static void IniWriteValue(string Section, string Key, string Value)
    {
        WritePrivateProfileString(Section, Key, Value, path);
    }

    public static string IniReadValue(string Section, string Key)
    {
        StringBuilder temp = new StringBuilder(255);
        //int i = GetPrivateProfileString(Section, Key, "", temp, 255, this.path);
        GetPrivateProfileString(Section, Key, "", temp, 255, path);
        return temp.ToString();
    }
}