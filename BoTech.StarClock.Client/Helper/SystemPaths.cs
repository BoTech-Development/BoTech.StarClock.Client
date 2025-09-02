using System;
using System.IO;

namespace BoTech.StarClock.Client.Helper;

public class SystemPaths
{
    public static string GetBaseAppDataPath(string extensionToPath = "")
    {
        if (OperatingSystem.IsWindows())
        {
            return ReturnPathAndCreate( Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData,
                Environment.SpecialFolderOption.Create) + "/botech/bot.scc/AppData/" + extensionToPath);
        }
        else if(OperatingSystem.IsLinux())
        {
            return ReturnPathAndCreate(Environment.GetFolderPath(Environment.SpecialFolder.Desktop,
                Environment.SpecialFolderOption.Create) + "/botech/bot.scc/AppData/" + extensionToPath);
        }else if (OperatingSystem.IsAndroid())
        {
            return ReturnPathAndCreate(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData,
                Environment.SpecialFolderOption.Create) + "/" + extensionToPath); 
        }
        return string.Empty;
    }
    /// <summary>
    /// Creates the given directory if it's not exists.
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    private static string ReturnPathAndCreate(string path)
    {
        if(!Directory.Exists(path)) Directory.CreateDirectory(path);
        return path;
    }
}