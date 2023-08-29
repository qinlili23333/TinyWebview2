using Microsoft.Web.WebView2.Core;

namespace TinyWebview2
{
    public partial class Form1 : Form
    {
        private string lastIcon;
        public Form1()
        {
                InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            var WebviewArgu = "--disable-features=msSmartScreenProtection --in-process-gpu --disable-web-security --no-sandbox --renderer-process-limit=1 --single-process";
            CoreWebView2EnvironmentOptions options = new CoreWebView2EnvironmentOptions()
            {
                AdditionalBrowserArguments = WebviewArgu
            };
            Directory.CreateDirectory(System.Environment.CurrentDirectory + @"\QinliliWebview2\");
            var webView2Environment = await CoreWebView2Environment.CreateAsync(null, System.Environment.CurrentDirectory + @"\QinliliWebview2\", options);
            await WebView.EnsureCoreWebView2Async(webView2Environment);
            WebView.Enabled = true;
            WebView.CoreWebView2.DocumentTitleChanged += (a,b)=> {

                this.Text = WebView.CoreWebView2.DocumentTitle;
            };
            WebView.CoreWebView2.FaviconChanged += async(a, b) =>
            {
                if (string.IsNullOrEmpty(WebView.CoreWebView2.FaviconUri))
                {
                    return;
                }
                else
                {
                    if (WebView.CoreWebView2.FaviconUri == lastIcon)
                    {
                        return;
                    }
                    this.Icon = await GetIcon(WebView.CoreWebView2.FaviconUri);
                    async Task<Icon> GetIcon(string url)
                    {
                        var httpClient = new HttpClient();
                        using (var stream = await httpClient.GetStreamAsync(url))
                        using (var ms = new MemoryStream())
                        {
                            stream.CopyTo(ms);
                            ms.Seek(0, SeekOrigin.Begin); // See https://stackoverflow.com/a/72205381/640195
                            var icon = (Icon)null;
                            try
                            {
                                icon = Icon.FromHandle(new Bitmap(ms).GetHicon());
                            }
                            catch (Exception)
                            {
                                icon = (Icon)new System.ComponentModel.ComponentResourceManager(typeof(Form1)).GetObject("$this.Icon");
                            }
                            return icon;
                        }
                    }
                    lastIcon = WebView.CoreWebView2.FaviconUri;
                }
            };
            WebView.CoreWebView2.Settings.IsStatusBarEnabled = false;
            WebView.CoreWebView2.Settings.IsBuiltInErrorPageEnabled = false;
            WebView.CoreWebView2.Navigate(Environment.GetCommandLineArgs()[1]);

        }

    }
}
