using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ToDoApp
{
    public partial class App : Application
    {
        public static string FolderPath { get; private set; }
        public App()
        {
            InitializeComponent();

            
            MainPage = new NavigationPage(new MainPage())
            {
                BarBackgroundColor = Color.Black ,
                BarTextColor = Color.White
            };
            
        }
    }
}
