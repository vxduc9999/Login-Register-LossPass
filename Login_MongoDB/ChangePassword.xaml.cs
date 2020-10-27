using Caliburn.Micro;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Driver;
using NSubstitute.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
    public sealed partial class ChangePassword : Page
    { 
        public ChangePassword()
        {
            this.InitializeComponent();
        }

        BsonDocument temp = new BsonDocument();
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            temp = (BsonDocument)e.Parameter;
            base.OnNavigatedTo(e);
        }

        private void ChangePass_Click(object sender, RoutedEventArgs e)
        {
            if (confirmPassword1.Password.Trim() == confirmPassword2.Password.Trim() && confirmPassword1.Password.Trim() != "" && confirmPassword2.Password.Trim() != "")
            {
                BsonDocument k = temp;
                temp["password"] = confirmPassword1.Password.ToString();
                var client = new MongoClient("mongodb://localhost:27017");
                var database = client.GetDatabase("test");
                var collection = database.GetCollection<BsonDocument>("person");
                FilterDefinition<BsonDocument> filter = new BsonDocument("email", k["email"]);
                var update = Builders<BsonDocument>.Update.Set("password", temp["password"]);
                try
                {
                    collection.UpdateOne(filter, update);
                    Frame.Navigate(typeof(MainPage));
                }
                catch
                {

                }
            }
        }
    }
}
