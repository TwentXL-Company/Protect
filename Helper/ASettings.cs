using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Helper
{
    public class ASettings
    {
        private readonly static string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        protected readonly static string filePath = Path.Combine(localAppData, "TwentXL Protect", "user_credentials.dat");
        protected readonly static string filePathIV = Path.Combine(localAppData, "TwentXL Protect", "iv.dat");
        protected readonly static string filePathSettings = Path.Combine(localAppData, "TwentXL Protect", "settings.json");

        protected const string LightThemePath = "pack://application:,,,/Styles.xaml";
        protected const string DarkThemePath = "pack://application:,,,/Styles_Dark.xaml";

        public readonly static string public_filePathSettings = Path.Combine(localAppData, "TwentXL Protect", "settings.json");
    }
}
