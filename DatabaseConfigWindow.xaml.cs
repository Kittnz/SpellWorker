using System;
using System.Windows;

namespace SpellWorker
{
    public partial class DatabaseConfigWindow : Window
    {
        public string Server { get; private set; }
        public string Database { get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }
        public int Port { get; private set; }

        public bool ConnectionSuccessful { get; private set; }

        public DatabaseConfigWindow()
        {
            InitializeComponent();

            // Load saved settings if available
            LoadSettings();
        }

        private void LoadSettings()
        {
            // Load from app settings if available
            txtServer.Text = SpellEditor.Settings.Default.DbServer;
            txtDatabase.Text = SpellEditor.Settings.Default.DbName;
            txtUsername.Text = SpellEditor.Settings.Default.DbUsername;
            txtPassword.Password = SpellEditor.Settings.Default.DbPassword;
            txtPort.Text = SpellEditor.Settings.Default.DbPort.ToString();
        }

        private void SaveSettings()
        {
            // Save to app settings
            SpellEditor.Settings.Default.DbServer = Server;
            SpellEditor.Settings.Default.DbName = Database;
            SpellEditor.Settings.Default.DbUsername = Username;
            SpellEditor.Settings.Default.DbPassword = Password;
            SpellEditor.Settings.Default.DbPort = Port;
            SpellEditor.Settings.Default.Save();
        }

        private async void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            // Get values from UI
            Server = txtServer.Text.Trim();
            Database = txtDatabase.Text.Trim();
            Username = txtUsername.Text.Trim();
            Password = txtPassword.Password;

            // Validate port
            if (!int.TryParse(txtPort.Text, out int port))
            {
                MessageBox.Show("Please enter a valid port number.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            Port = port;

            // Validate required fields
            if (string.IsNullOrEmpty(Server) || string.IsNullOrEmpty(Database) ||
                string.IsNullOrEmpty(Username))
            {
                MessageBox.Show("Please fill in all required fields.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Test connection
            var dbConnector = new DatabaseConnector(Server, Database, Username, Password, Port);

            // Show a loading indicator or disable the button to prevent multiple clicks
            btnConnect.IsEnabled = false;

            // Use await with the async method
            bool connectionSuccessful = await dbConnector.TestConnectionAsync();

            // Re-enable the button
            btnConnect.IsEnabled = true;

            if (connectionSuccessful)
            {
                // Connection successful
                SaveSettings();
                ConnectionSuccessful = true;
                DialogResult = true;
                Close();
            }
            else
            {
                // Connection failed
                MessageBox.Show("Failed to connect to the database. Please check your connection settings and try again.",
                    "Connection Error", MessageBoxButton.OK, MessageBoxImage.Error);
                ConnectionSuccessful = false;
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}