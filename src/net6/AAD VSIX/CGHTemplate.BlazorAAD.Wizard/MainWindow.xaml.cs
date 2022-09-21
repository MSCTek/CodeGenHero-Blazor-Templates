using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CGHTemplate.BlazorAAD.Wizard
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string DEFAULT_API_CONNECTIONSTRING = @"Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ArtistSite;Integrated Security=True;";
        private const string DEFAULT_APP_DOMAIN = @"YourSite.Domain";
        private const string DEFAULT_API_TENANTID = @"";
        private const string DEFAULT_API_CLIENTID = @"";
        private const string DEFAULT_APP_CLIENTID = @"";
        private const string DEFAULT_HOST_CLIENTID = @"";

        public MainWindow()
        {
            InitializeComponent();

            textAppConnectionString.Text = DEFAULT_API_CONNECTIONSTRING;
            textAadSiteDomain.Text = DEFAULT_APP_DOMAIN;
            textAadTenantId.Text = DEFAULT_API_TENANTID;
            textAadApiClientId.Text = DEFAULT_API_CLIENTID;
            textAadAppClientId.Text = DEFAULT_APP_CLIENTID;
            textAadHostClientId.Text = DEFAULT_HOST_CLIENTID;

        }

        public event EventHandler<TemplateParametersSetEventArgs> TemplateParametersSet;

        public void OnTemplateParametersSet(string connectionString, string siteDomain, string siteTenantId, string apiClientId, string appClientId, string hostClientId)
        {
            var templateParameters = new TemplateParametersSetEventArgs(connectionString, siteDomain, siteTenantId, apiClientId, appClientId, hostClientId);

            EventHandler<TemplateParametersSetEventArgs> handler = TemplateParametersSet;
            handler?.Invoke(this, templateParameters);
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            if (string.IsNullOrWhiteSpace(textAppConnectionString.Text))
            {
                sb.AppendLine("Enter a connection string to an EXISTING application database.");
            }

            if (sb.Length > 0) // TODO / Food for thought - Some sort of actual validation mechanism instead of this "Magic Length" check
            {
                MessageBox.Show(sb.ToString());
            }
            else
            {
                DialogResult = true;
                OnTemplateParametersSet(
                    textAppConnectionString.Text, textAadSiteDomain.Text, textAadTenantId.Text,
                    textAadApiClientId.Text, textAadAppClientId.Text, textAadHostClientId.Text);
            }
        }

        public class TemplateParametersSetEventArgs : EventArgs
        {
            public TemplateParametersSetEventArgs(string connectionString, string siteDomain, string siteTenantId, string apiClientId, string appClientId, string hostClientId)
            {
                ConnectionString = connectionString;
                AadSiteDomain = siteDomain;
                AadApiTenantId = siteTenantId;
                AadApiClientId = apiClientId;
                AadAppClientId = appClientId;
                AadHostClientId = hostClientId;
            }

            private TemplateParametersSetEventArgs()
            {
            }

            public string ConnectionString { get; set; }
            public string AadSiteDomain { get; set; }
            public string AadApiTenantId { get; set; }
            public string AadApiClientId { get; set; }
            public string AadAppClientId { get; set; }
            public string AadHostClientId { get; set; }
        }

        /// Some Windows Black-Magic that I'd rather hide-away in a region where I can pretend this isn't part of the Class
        #region Arcane C Interop Wizardry
        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var hwnd = new WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
        }

        #endregion
    }
}
