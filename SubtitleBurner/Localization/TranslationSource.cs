using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Resources;

namespace SubtitleBurner
{
  public class TranslationSource : INotifyPropertyChanged
  {
    public static TranslationSource Instance { get; } = new TranslationSource();

    private readonly Dictionary<string, ResourceManager> resourceManagerDictionary = new Dictionary<string, ResourceManager>();

    public string this[string key]
    {
      get
      {
        string baseName, stringName;
        SplitName(key, out baseName, out stringName);
        string translation = null;
        if (resourceManagerDictionary.ContainsKey(baseName))
          translation = resourceManagerDictionary[baseName].GetString(stringName, currentCulture);
        return translation ?? key;
      }
    }

    private CultureInfo currentCulture = CultureInfo.CurrentUICulture;
    public CultureInfo CurrentCulture
    {
      get { return currentCulture; }
      set
      {
        if (currentCulture != value)
        {
          currentCulture = value;
          // string.Empty/null indicates that all properties have changed
          PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(string.Empty));
        }
      }
    }

    // WPF bindings register PropertyChanged event if the object supports it and update themselves when it is raised
    public event PropertyChangedEventHandler PropertyChanged;

    public void AddResourceManager(ResourceManager resourceManager)
    {
      if (!resourceManagerDictionary.ContainsKey(resourceManager.BaseName))
      {
        resourceManagerDictionary.Add(resourceManager.BaseName, resourceManager);
      }
    }

    public void SplitName(string name, out string baseName, out string stringName)
    {
      int idx = name.LastIndexOf('.');
      baseName = name.Substring(0, idx);
      stringName = name.Substring(idx + 1);
    }
  }
}