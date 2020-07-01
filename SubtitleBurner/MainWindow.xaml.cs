using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using SubtitleBurner.Handlers;
using SubtitleBurner.Models;

namespace SubtitleBurner
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      setFlowDirection();
      InitializeComponent();
    }

    private void setFlowDirection()
    {
      this.FlowDirection = CultureInfo.CurrentUICulture.Name == "fa-IR"
        ? FlowDirection.RightToLeft
        : FlowDirection.LeftToRight;
    }

    private void BtnSelectVideo_Click(object sender, RoutedEventArgs e)
    {
      var openFileDialog = new OpenFileDialog()
      {
        Filter = "mp4 files (*.mp4)|*.mp4|All files (*.*)|*.*"
      };
      if (openFileDialog.ShowDialog() == false) return;
      txtVideo.Text = openFileDialog.FileName;
    }

    private void BtnSelectSubtitle_Click(object sender, RoutedEventArgs e)
    {
      var openFileDialog = new OpenFileDialog()
      {
        Filter = "ass files (*.ass)|*.ass"
      };
      if (openFileDialog.ShowDialog() == false) return;
      txtSubtitle.Text = openFileDialog.FileName;
    }

    private void BtnMakeOutput_Click(object sender, RoutedEventArgs e)
    {
      if (string.IsNullOrEmpty(txtVideo.Text))
      {
        MessageBox.Show(Utils.GetResourceText("NoVideoWarning"), Utils.GetResourceText("WarningTitle"), MessageBoxButton.OK, MessageBoxImage.Warning);
        return;
      }

      if (string.IsNullOrEmpty(txtSubtitle.Text))
      {
        if (comboScale.SelectedIndex == 0)
        {
          MessageBox.Show(Utils.GetResourceText("NoSubtitleWarning"), Utils.GetResourceText("WarningTitle"), MessageBoxButton.OK, MessageBoxImage.Warning);
          return;
        }
      }

      var video = txtVideo.Text;
      var subtitle = txtSubtitle.Text;
      var scale = extractScale(comboScale.Text);
      string output;
      var result = new Burner().StartBurning(video, subtitle, scale, out output);
      if (result)
      {
        MessageBox.Show(Utils.GetResourceText("CompletedMessage"), Utils.GetResourceText("InformationTitle"), MessageBoxButton.OK, MessageBoxImage.Information);
        Process.Start(Path.GetDirectoryName(output));
        return;
      }
      MessageBox.Show(Utils.GetResourceText("FailedMessage"), Utils.GetResourceText("ErrorTitle"), MessageBoxButton.OK, MessageBoxImage.Error);
    }

    private void Txt_PreviewDragOver(object sender, DragEventArgs e)
    {
      e.Handled = true;
    }

    private void Txt_PreviewDrop(object sender, DragEventArgs e)
    {
      var fileNames = (string[])e.Data.GetData(DataFormats.FileDrop);
      if (fileNames == null) return;
      var fileName = fileNames.FirstOrDefault();
      if (fileName == null) return;
      (sender as TextBox).Text = fileName;
    }

    private void BtnFaCulture_Click(object sender, RoutedEventArgs e)
    {
      AppCulture.SetCulture(Culture.Fa);
      restartApplication();
    }

    private void BtnEnCulture_Click(object sender, RoutedEventArgs e)
    {
      AppCulture.SetCulture(Culture.En);
      restartApplication();
    }

    private void restartApplication()
    {
      Process.Start(Application.ResourceAssembly.Location);
      Application.Current.Shutdown();
    }

    private string extractScale(string text)
    {
      var match = Regex.Match(text, @"^\d+:\d+", RegexOptions.IgnoreCase);
      return match.Success ? match.Value : "";
    }
  }
}
