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

namespace CodeGenHero.ProjectTemplate.Blazor6.Wizard
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string DEFAULT_APP_CONNECTIONSTRING = @"Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ArtistSite;Integrated Security=True;";

        private const string DEFAULT_IDP_CONNECTIONSTRING = @"Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MyIDP;Integrated Security=True;";

        private const string DEFAULT_ADMIN_USERNAME = "myAdmin";

        private const string DEFAULT_ADMIN_EMAIL = "myAdmin@example.com";

        public MainWindow()
        {
            InitializeComponent();

            textAppConnectionString.Text = DEFAULT_APP_CONNECTIONSTRING;
            textIDPConnectionString.Text = DEFAULT_IDP_CONNECTIONSTRING;
            textAdminUsername.Text = DEFAULT_ADMIN_USERNAME;
            textAdminEmail.Text = DEFAULT_ADMIN_EMAIL;
        }

        public event EventHandler<TemplateParametersSetEventArgs> TemplateParametersSet;

        public void OnTemplateParametersSet(string connectionString, string idpConnectionString, string adminUsername, string adminEmail)
        {
            var templateParameters = new TemplateParametersSetEventArgs(
                connectionString: connectionString, 
                idpConnectionString: idpConnectionString, 
                adminUsername: adminUsername, 
                adminEmail: adminEmail);

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

            if (string.IsNullOrWhiteSpace(textIDPConnectionString.Text))
            {
                sb.AppendLine("Enter a connection string for use with a NEW Identity Server database.");
            }

            if (string.IsNullOrWhiteSpace(textAdminUsername.Text))
            {
                sb.AppendLine("Enter a username for the Identity Server administrator.");
            }

            if (string.IsNullOrWhiteSpace(textAdminEmail.Text))
            {
                sb.AppendLine("Enter the email address for the Identity Server administrator.");
            }

            if (sb.Length > 0)
            {
                MessageBox.Show(sb.ToString());
            }
            else
            {
                DialogResult = true;
                OnTemplateParametersSet(
                    connectionString: textAppConnectionString.Text, 
                    idpConnectionString: textIDPConnectionString.Text, 
                    adminUsername: textAdminUsername.Text, 
                    adminEmail: textAdminEmail.Text);
            }
        }

        public class TemplateParametersSetEventArgs : EventArgs
        {
            public TemplateParametersSetEventArgs(string connectionString, string idpConnectionString, string adminUsername, string adminEmail)
            {
                ConnectionString = connectionString;
                IDPConnectionString = idpConnectionString;
                AdminUsername = adminUsername;
                AdminEmail = adminEmail;
            }

            private TemplateParametersSetEventArgs()
            {
            }

            public string ConnectionString { get; set; }
            public string IDPConnectionString { get; set; }
            public string AdminUsername { get; set; }
            public string AdminEmail { get; set; }
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
