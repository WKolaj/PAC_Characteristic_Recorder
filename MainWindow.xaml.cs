using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PAC_Characterstic_Recorder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CharacteristicRecorder _recorder = new CharacteristicRecorder();

        public MainWindow()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            this.DataContext = this._recorder;
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            this._recorder.StartRecording();
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            this._recorder.StopRecording();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
                dlg.FileName = DateTime.Now.ToString("MM_dd_yyyy_HH_mm_ss") + ".csv";
                dlg.DefaultExt = ".csv";
                dlg.Filter = "CSV file (.csv)|*.csv";

                Nullable<bool> result = dlg.ShowDialog();

                if (result == true)
                {
                    String content = _recorder.GetCSVData();
                    File.WriteAllText(dlg.FileName, content,Encoding.UTF8);
                    System.Diagnostics.Process.Start(dlg.FileName);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Błąd podczas zapisu", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }
    }
}
