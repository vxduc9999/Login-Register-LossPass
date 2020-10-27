using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
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
    public sealed partial class SignUpForStaff : Page
    {
        public SignUpForStaff()
        {
            this.InitializeComponent();
        }

        bool stringAttribute(string s)
        {
            if (s == "" || s.Trim() == "")
                return false;
            return true;
        }

        Regex regexItem = new Regex("^[a-zA-Z0-9 ]*$");
        private void signUp_Click(object sender, RoutedEventArgs e)
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("test");
            var collection = database.GetCollection<Human>("person");

            var checkIfExistUsername = collection.Find(x => x.username == username.Text.ToLower());
            var checkIfExistEmail = collection.Find(x => x.username == Email.Text.ToLower());
            if (checkIfExistUsername.Count() == 0)
            {
                var IsValidEmail = new EmailAddressAttribute();
                var checkRole = roles.SelectedItem;
                if (!(bool)IsValidEmail.IsValid(Email.Text.ToString().ToLower()))
                {
                    thongbao.Text = "Email is invalid";
                }
                else if (checkIfExistEmail.Count() != 0)
                {
                    thongbao.Text = "That email is taken. Try another";
                }
                else if (!regexItem.IsMatch(username.Text) || username.Text.Length < 6)
                {
                    thongbao.Text = "Sorry, only letters (a-z), numbers (0-9), and your username must be greater than 6 characters long";
                }
                else if (!stringAttribute(firstName.Text.ToString()))
                {
                    thongbao.Text = "Enter first name";
                }
                else if (!stringAttribute(lastName.Text.ToString()))
                {
                    thongbao.Text = "Enter last name";
                }
                else if (!stringAttribute(password.Password.ToString()))
                {
                    thongbao.Text = "Use 8 characters or more for your password";
                }
                else if (checkRole == null)
                {
                    thongbao.Text = "Not yet selected a position for an employee";
                }
                else
                {
                    Human t = new Human();
                    t.firstName = firstName.Text;
                    t.lastName = lastName.Text;
                    t.username = username.Text.ToLower();
                    t.email = Email.Text.ToLower();
                    t.password = password.Password.ToString();
                    t.role = roles.SelectedItem.ToString();
                    thongbao.Text = "Sign Up Successfully";
                    collection.InsertOneAsync(t);
                    this.Frame.Navigate(typeof(MainPage), t);
                }
            }
            else
                thongbao.Text = "That username is taken. Try another";
        }
    }
}
