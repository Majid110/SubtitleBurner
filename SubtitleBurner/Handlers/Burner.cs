using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;

namespace SubtitleBurner.Handlers
{
  public class Burner
  {
    public bool StartBurning(string video,
                             string subtitle,
                             string scale,
                             out string output)
    {
      output = generateOutputFileName(video, subtitle, scale);
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
      var workingDir = Path.GetDirectoryName(video);
      var subtitleShortName = string.IsNullOrEmpty(subtitle) ? "" : Utils.GetShortPath(subtitle);
      var subtitleFileName = string.IsNullOrEmpty(subtitle) ? "" : Path.GetFileName(subtitleShortName);
      var videoShortName = Utils.GetShortPath(video);
      var arguments = generateArguments(videoShortName, subtitleFileName, scale, output);
      var p = new Process();
      p.StartInfo.WorkingDirectory = workingDir;
      p.StartInfo.FileName = ffmpeg;
      p.StartInfo.Arguments = arguments;
      p.Start();
      p.WaitForExit(); // or p.WaitForExit(Timeout-Period-In-Milliseconds);
      return p.ExitCode == 0;
    }

    private string generateArguments(string videoShortName, string subtitleFileName, string scale, string output)
    {
      return string.IsNullOrEmpty(scale)
        ? $"-i \"{videoShortName}\" -vf subtitles=\"{subtitleFileName}\" \"{output}\""
        : string.IsNullOrEmpty(subtitleFileName)
          ? $"-i \"{videoShortName}\" -vf scale={scale} \"{output}\""
          : $"-i \"{videoShortName}\" -vf \"subtitles={subtitleFileName}, scale={scale}\" \"{output}\"";
    }

    private string generateOutputFileName(string video, string subtitle, string scale)
    {
      var videoFileName = Path.GetFileNameWithoutExtension(video);
      var videoExt = Path.GetExtension(video);
      var subtitleDir = Path.GetDirectoryName(video);
      var outputFileName = string.IsNullOrEmpty(scale)
        ? $"{videoFileName}_output{videoExt}"
        : $"{videoFileName}_output_{scale.Replace(":", "x")}{videoExt}";
      return Path.Combine(subtitleDir, outputFileName);
    }
  }
}
