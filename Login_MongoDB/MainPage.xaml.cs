using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using MongoDB.Driver.Linq;
using NSubstitute.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using System.Net.Mail;
using Windows.ApplicationModel.Contacts;
using Windows.ApplicationModel.Email;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using Windows.Media.MediaProperties;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Login_MongoDB
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    /// 

    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void Login(object sender, RoutedEventArgs e)
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("test");
            var collection = database.GetCollection<BsonDocument>("person");

            var list = await collection.Find(new BsonDocument("username", username.Text.ToLower())).ToListAsync();
            var n = list.Count();


            var kokoko = password.Password;

            if (n == 0 || list[0]["password"] != password.Password)
            {
                thongbao.Text = "Đăng nhập không thành công";
            }
            else
            {
                if(list[0]["role"] == "Admin")
                {
                    thongbao.Text = "Đăng nhập thành công với quyền admin";
                }
                else if(list[0]["role"] == "Sale")
                    thongbao.Text = "Đăng nhập thành công với quyền sale";
                else
                    thongbao.Text = "Đăng nhập thành công với quyền shipper";

                logOut.IsEnabled = true;
            }
        }

        private void SignUp_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(SignUpForStaff));
        }

        Human temp = new Human();
        
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != "" && e.Parameter != null)
            {
                temp = (Human)e.Parameter;
                base.OnNavigatedTo(e);
                username.Text = temp.username.ToLower();
                password.Password = temp.password;
            }
        }


        private void logOut_Click(object sender, RoutedEventArgs e)
        {
            SignUp.IsEnabled = false;
            logOut.IsEnabled = false;
        }

        private void LostPass_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ForgotEmail));
        }
    }

}
