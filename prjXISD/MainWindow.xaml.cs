using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace prjXISD
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static Database db = new Database();
        private static Employee LoggedIn = null;
        private static string EncryptionKey = "eSgVkYp3s6v9y$B&E)H@McQfTjWmZq4t";

        public MainWindow()
        {
            InitializeComponent();
            gridLoginRegister.Visibility = Visibility.Visible;
            dpnlMenu.IsEnabled = false;
        }

        

        public void ConnectDB()
        {
            try
            {
                db.cnn.Open();
                MessageBox.Show("Connection Open!");
                db.cnn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Can not open connection!");
            }
        }

        public static string EncryptString(string key, string plainText)
        {
            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(array);
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string empNum = edtEmpID.Text;
            string empPassword = EncryptString(EncryptionKey, edtPassword.Password);

            db.cnn.Close();
            Employee attempt = new Employee();
            attempt = attempt.Login(empNum, empPassword);

            switch(attempt)
            {
                case null:
                    MessageBox.Show("Invalid Login Details! Please try again.");
                    break;
                default:
                    MessageBox.Show("You are now logged in!");
                    LoggedIn = attempt;

                    gridLoginRegister.Visibility = Visibility.Hidden;
                    gridLoginRegister.IsEnabled = false;

                    dpnlMenu.IsEnabled = true;

                    break;
            }
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            string empNum = edtEmpID.Text;
            string empPassword = edtPassword.Password;
            string encPassword = EncryptString(EncryptionKey, empPassword);

            db.cnn.Close();
            Employee attempt = new Employee();
            attempt = attempt.Register(empNum, encPassword);

            switch (attempt)
            {
                case null:
                    MessageBox.Show("Invalid Input. Please choose different details.");
                    break;
                default:
                    MessageBox.Show("You are now registered and logged in!");
                    LoggedIn = attempt;

                    gridLoginRegister.Visibility = Visibility.Hidden;
                    gridLoginRegister.IsEnabled = false;

                    break;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            gridOrders.Visibility = Visibility.Visible;
        }
    }
}
