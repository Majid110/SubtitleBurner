using SubtitleBurner.Models;
using SubtitleBurner.Properties;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace SubtitleBurner
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application
  {
    App()
    {
      setCultureInfo();
    }

    private static void setCultureInfo()
    {
      AppSettings.ReadSettings();
      AppCulture.ChangeAppCulture(AppSettings.Culture);
    }
  }
}
