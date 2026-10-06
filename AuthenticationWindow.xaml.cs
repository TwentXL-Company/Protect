using Microsoft.Extensions.DependencyInjection;
using PasswordManager.Helper;
using PasswordManager.Helper.Interfaces;
using PasswordManager.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PasswordManager
{
    public partial class AuthenticationWindow : Window
    {
        private IDataSettings _dataSettings; 

        public AuthenticationWindow(IDataSettings dataSettings)
        {
            InitializeComponent();
            AuthCodeCheck();
            ErrorMessage.Visibility = Visibility.Collapsed;

            _dataSettings = dataSettings;
        }

        private void AuthCodeCheck()
        {
            if (GlobalSettings.hasCrypt)
                H1_Content.Content = "Enter a secret code";
            else
            {
                H1_Content.Content = "Come up with a secret code";
                CreateSecretCode_Message.Visibility = Visibility.Visible;
            }
        }

        private void Titlebar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void Close_Click(object sender, RoutedEventArgs e) 
            => this.Close();

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Code.Text.Length < 4)
                {
                    ErrorMessage.Visibility = Visibility.Visible;
                    ErrorMessage.Content = "The length must be at least 4 characters";
                    return;
                }

                byte[] key = SHA256.HashData(Encoding.UTF8.GetBytes(Code.Text));
                string result = Convert.ToBase64String(key);
                Crypto.key = key;

                if (GlobalSettings.hasCrypt)
                {
                    _dataSettings.LoadIV();

                    bool isValid = Utils.DataDecryptCheck();

                    if (!isValid)
                        ErrorMessage.Visibility = Visibility.Visible;
                        ErrorMessage.Content = "Wrong code";
                        return;
                }
                else
                {
                    using (Aes aes = Aes.Create())
                    {
                        aes.GenerateIV();
                        Crypto.iv = aes.IV;
                        _dataSettings.SaveIV();
                    }
                }

                MainWindowShow();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Authentification error", "", MessageBoxButton.OK, MessageBoxImage.Error);
                Debug.Write("Authentification error: " + ex.Message);
            }
        }

        private void MainWindowShow()
        {
            var mainWindow = App.Services?.GetRequiredService<MainWindow>();
            mainWindow?.Show();
            this.Close();
        }

        private void DestroyClick(object sender, RoutedEventArgs e)
        {
            var message = MessageBox.Show("\"Destroy all\" will lead to a complete cleanup of your data, including your passwords and authorization code. Are you sure you want to continue?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if(message == MessageBoxResult.Yes)
            {
                _dataSettings.DestroyAll();
                this.Close();
            }
        }
    }
}
