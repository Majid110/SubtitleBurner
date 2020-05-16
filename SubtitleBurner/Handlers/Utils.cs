using System;
using System.Runtime.InteropServices;
using System.Text;

namespace SubtitleBurner.Handlers
{
  public class Utils
  {
    const int MAX_PATH = 255;

    [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
    private static extern int GetShortPathName(
        [MarshalAs(UnmanagedType.LPTStr)]
         string path,
        [MarshalAs(UnmanagedType.LPTStr)]
         StringBuilder shortPath,
        int shortPathLength
        );

    public static string GetShortPath(string path)
    {
      var shortPath = new StringBuilder(MAX_PATH);
      GetShortPathName(path, shortPath, MAX_PATH);
      return shortPath.ToString();
    }

    public static void SetLanguage(string locale)
    {
      if (string.IsNullOrEmpty(locale)) locale = "en-US";
      TranslationSource.Instance.CurrentCulture = new System.Globalization.CultureInfo(locale);
    }

    public static string GetResourceText(string resourceKey)
    {
      return TranslationSource.Instance[$"SubtitleBurner.Properties.Resources.{resourceKey}"];
    }
  }
}
