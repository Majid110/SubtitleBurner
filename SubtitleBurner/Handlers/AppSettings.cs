using System;

namespace SubtitleBurner.Models
{
  public class AppSettings
  {
    public static Culture Culture = Culture.En;

    public static void ReadSettings()
    {
      Enum.TryParse(Properties.Settings.Default.Culture, out Culture);
    }

    public static void SaveSettings()
    {
      Properties.Settings.Default.Culture = Culture.ToString("G");
      Properties.Settings.Default.Save();
    }
  }
}