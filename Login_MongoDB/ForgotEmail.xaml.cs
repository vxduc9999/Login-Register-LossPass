using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Login_MongoDB
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ForgotEmail : Page
    {
        public ForgotEmail()
        {
            this.InitializeComponent();
        }

        string flagCode = "";

        private string CheckIsvalidEmail(string inputEmail)
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("test");
            var collection = database.GetCollection<BsonDocument>("person");

            var list = collection.Find(new BsonDocument("email", inputEmail)).ToList();
            var n = list.Count();
            if (n == 0)
                return "";
            return list[0]["username"].ToString();
        }

        private async void sendCode_Click(object sender, RoutedEventArgs e)
        {
            string temp = CheckIsvalidEmail(inputEmail.Text.ToLower());
            if(temp != "")
            {
                string text = @"<font color=#000000>Hi,{0}<br><br></font>

<font color=#000000>We received a request to access your MyShop Account through your email address.</font><br><br>

<font color=#000000>If you did not request this code, it is possible that someone else is trying to access the MyShop Account. Do not forward or give this code to anyone.</font><br><br>

<font color=#000000>You received this message because this email address is listed as the recovery email for the MyShop Account.</font><br><br>

<font color=#000000>Sincerely yours,</font><br><br>

<font color=#000000>The MyShop Accounts team</font><br><br>

<font color=#000000>Your MyShop verification code is:</font> <b>{1}</b>";

                try
                {
                    MailMessage mm = new MailMessage();
                    mm.To.Add(inputEmail.Text.ToLower());
                    mm.Subject = "MyShopUWP Verification Code";
                    mm.BodyEncoding = System.Text.Encoding.UTF8;
                    mm.SubjectEncoding = System.Text.Encoding.UTF8;
                    Random generator = new Random();
                    String r = generator.Next(0, 999999).ToString("D6");
                    mm.Body = String.Format(text, temp, r);
                    mm.From = new MailAddress(inputEmail.Text.ToLower());
                    mm.IsBodyHtml = true;
                    System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.gmail.com");
                    smtp.Port = 587;
                    smtp.UseDefaultCredentials = true;
                    smtp.EnableSsl = true;
                    smtp.Credentials = new System.Net.NetworkCredential("myshopuwp@gmail.com", "shop1234567890");
                    smtp.SendAsync(mm, mm);
                    flagCode = r;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    throw;
                }
            }
        }

        private void confirm_Click(object sender, RoutedEventArgs e)
        {
            if (inputCode.Text == flagCode && inputCode.Text.Trim() != "")
            {
                var client = new MongoClient("mongodb://localhost:27017");
                var database = client.GetDatabase("test");
                var collection = database.GetCollection<BsonDocument>("person");
                var list = collection.Find(new BsonDocument("email", inputEmail.Text.ToLower())).ToList();
                Frame.Navigate(typeof(ChangePassword), list[0]);
            }
        }
    }
}
