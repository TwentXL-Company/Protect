using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using PasswordManager.Components;
using PasswordManager.Helper;
using PasswordManager.Helper.Interfaces;
using PasswordManager.Models;
using PasswordManager.Services;

namespace PasswordManager.Pages
{
    public partial class SettingsPage : UserControl
    {
        private SettingsModel settingsModel = GlobalSettings.settingsModel;
        public static SettingsPage? SettingsPageInstance { get; private set; }
        private IGlobalSettings _globalSettings;

        public SettingsPage(IGlobalSettings globalSettings)
        {
            SettingsPageInstance = this;
            _globalSettings = globalSettings;

            InitializeComponent();
            UpdateSettings();
        }

        private void BackupPathEdit_Click(object sender, RoutedEventArgs e)
        {
            string path = Utils.GetPathDir();
            settingsModel.BackupPath = path;
            UpdateSettings();
        }

        private void DarkTheme_Click(object sender, RoutedEventArgs e)
        {
            if (settingsModel.DarkTheme)
                settingsModel.DarkTheme = false;
            else
                settingsModel.DarkTheme = true;

            UpdateSettings();
            _globalSettings.ApplyTheme(settingsModel.DarkTheme);
        }

        public void UpdateSettings()
        {
            if (settingsModel.DarkTheme)
                DarkThemeButton.Content = "Off";
            else
                DarkThemeButton.Content = "On";

            BackupPath.Content = settingsModel.BackupPath;
        }
    }
}
