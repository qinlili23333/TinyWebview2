using System.IO;
using System.Net.Http;
using System.Windows;

namespace DynamicWebview
{
    /// <summary>
    /// Interaction logic for Install.xaml
    /// </summary>
    public partial class Install : Window
    {
        public Install(string Link)
        {
            Init(Link);
            
        }
        private async void Init(string Link)
        {
            var fileName = "TinyWebview2-" + System.Runtime.InteropServices.RuntimeInformation.ProcessArchitecture.ToString() + ".exe";
            if (File.Exists(fileName))
            {
            }
            else
            {
                InitializeComponent();
                this.Show();
                await DownloadFileAsync(fileName);
            }
            System.Diagnostics.Process.Start(fileName, Link);
            this.Close();
        }
        private async Task DownloadFileAsync(string filename)
        {
            //From https://gist.github.com/dalexsoto/9fd3c5bdbe9f61a717d47c5843384d11
            // for the sake of the example lets add a client definition here
            var client = new HttpClient();
            var docUrl = "https://github.com/qinlili23333/TinyWebview2/releases/download/Dist/" + filename;

            // Setup your progress reporter
            var progress = new Progress<float>();
            progress.ProgressChanged += (_, e) =>
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    PBar.Value += e;
                }));
            };

            // Use the provided extension method
            using (var file = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None))
                await client.DownloadDataAsync(docUrl, file, progress);
        }
    }

}
