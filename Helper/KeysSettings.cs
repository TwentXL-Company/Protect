using PasswordManager.Helper.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace PasswordManager.Helper
{
    public class KeysSettings : IKeysSettings
    {
        public void SaveIV(string filePathIV)
        {
            List<byte[]> keysList = new List<byte[]>()
            {
                Crypto.iv
            };

            string json = JsonSerializer.Serialize(keysList);

            File.WriteAllText(filePathIV, json);
            File.SetAttributes(filePathIV, File.GetAttributes(filePathIV) | FileAttributes.Hidden);
        }

        public void LoadIV(string filePathIV)
        {
            try
            {
               string ivText = File.ReadAllText(filePathIV);

               if(!string.IsNullOrWhiteSpace(ivText) || !string.IsNullOrEmpty(ivText))
                {
                    List<byte[]> list = JsonSerializer.Deserialize<List<byte[]>>(ivText);
                    if (list != null)
                        Crypto.iv = list[0];
                }
            }
            catch
            {
                MessageBox.Show("Launch error: The keys were not found", "", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
    }
}
