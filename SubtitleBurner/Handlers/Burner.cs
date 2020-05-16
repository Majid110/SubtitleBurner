using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;

namespace SubtitleBurner.Handlers
{
  public class Burner
  {
    public bool StartBurning(string video, string subtitle, out string output)
    {
      output = generateOutputFileName(video, subtitle);
      if (File.Exists(output))
      {
        if (MessageBox.Show(
          $"{Utils.GetResourceText("OverwriteMessage")}\n{output}",
          Utils.GetResourceText("QuestionTitle"),
          MessageBoxButton.YesNo,
          MessageBoxImage.Warning
          ) != MessageBoxResult.Yes)
        {
          return false;
        }
        File.Delete(output);
      }

      var appPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
      var ffmpeg = Path.Combine(appPath, "ffmpeg") + "\\ffmpeg.exe";
      var workingDir = Path.GetDirectoryName(subtitle);
      var subtitleShortName = Utils.GetShortPath(subtitle);
      var subtitleFileName = Path.GetFileName(subtitleShortName);
      var videoShortName = Utils.GetShortPath(video);

      var p = new Process();
      p.StartInfo.WorkingDirectory = workingDir;
      p.StartInfo.FileName = ffmpeg;
      p.StartInfo.Arguments = $"-i \"{videoShortName}\" -vf \"ass={subtitleFileName}\" \"{output}\"";
      p.Start();
      p.WaitForExit(); // or p.WaitForExit(Timeout-Period-In-Milliseconds);
      return p.ExitCode == 0;
    }

    private string generateOutputFileName(string video, string subtitle)
    {
      var videoFileName = Path.GetFileNameWithoutExtension(video);
      var videoExt = Path.GetExtension(video);
      var subtitleDir = Path.GetDirectoryName(subtitle);
      var outputFileName = $"{videoFileName}_output{videoExt}";
      return Path.Combine(subtitleDir, outputFileName);
    }
  }
}
