using SubtitleBurner.Models;
using System.Globalization;
using System.Threading;

namespace SubtitleBurner
{

  public class AppCulture
  {
    private static AppCulture _appCulture = null;
    
    private AppCulture()
    {
    }

    public static AppCulture Instance {
      get
      {
        if (_appCulture == null)
        {
          _appCulture = new AppCulture();
        }
        return _appCulture;
      }
    }

    public Culture Culture
    {
      get
      {
        AppSettings.ReadSettings();
        return AppSettings.Culture;
      }
    }

    public static void SetCulture(Culture culture)
    {
      AppSettings.Culture = culture;
      AppSettings.SaveSettings();
      ChangeAppCulture(culture);
    }

    public static void ChangeAppCulture(Culture culture)
    {
      if (culture == Culture.Fa)
      {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("fa-IR");
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("fa-IR");
      } else
      {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
      }
    }
  }
}
