using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Runtime.CompilerServices;
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
using System.Windows.Threading;
using Microsoft.Win32;
using Wpf.Ui.Controls;

namespace Copy2Note
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private bool _isWatching = false;
        private DispatcherTimer _timer;
        private string _filePath = string.Empty;
        private string Oldtext = string.Empty;

        public MainWindow()
        {
            InitializeComponent();

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += CheckClipboardContent;
            
        }


        private void MonitorButton_Click(object sender, RoutedEventArgs e)
        {
            _isWatching = !_isWatching;
            if (_isWatching)
            {
                MonitorButton.Appearance = Wpf.Ui.Controls.ControlAppearance.Success;
                _timer.Start();
            } else
            {
                MonitorButton.Appearance = Wpf.Ui.Controls.ControlAppearance.Primary;
                _timer.Stop();
            }
        }

        private void CheckClipboardContent(object sender, EventArgs e)
        {
            if (Clipboard.ContainsText())
            {
                String text = Clipboard.GetText();

                if (text != Oldtext)
                {
                    if (string.IsNullOrEmpty(_filePath))
                    {

                        _filePath = $"C:\\Users\\liuliu勋娅\\Desktop\\Copy2Note_{DateTime.Now:yyyyMMddHHmm}.md";
                    }
                    File.AppendAllText(_filePath, text + "\n\n" + "---" + "\n");
                    Oldtext = text;
                }
            }
        }

        private void SelectFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Markdown Files (*.md)|*.md";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            if (openFileDialog.ShowDialog() == true)
            {
                _filePath = openFileDialog.FileName;
            }

        }


    }

}