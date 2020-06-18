using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApp.Model;
using Xamarin.Forms;
using System.Net.Http;
using System.Diagnostics;
using Newtonsoft.Json;

namespace ToDoApp
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        
        HttpClient client;

        private const string RestUrl = "https://fctodo.azurewebsites.net/todo";
        private bool isNewItem;
        public List<Todo> Items { get; private set; }
        public MainPage(bool isNew = true)
        {
            client = new HttpClient();
            InitializeComponent();
            isNewItem = isNew;
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            Items = new List<Todo>();

            Uri uri = new Uri(string.Format(RestUrl, string.Empty));

            try
            {
                HttpResponseMessage response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    Items = JsonConvert.DeserializeObject<List<Todo>>(content);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }

            listView.ItemsSource = Items;
        }

        async void OnNoteAddedClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddToDo(isNewItem)
            {
                BindingContext = new Todo { id = Items.Last().id + 1 }
            });
        }
        async void OnListViewItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                await Navigation.PushAsync(new AddToDo(false)
                {
                    BindingContext = e.SelectedItem as Todo
                });
            }
        }
    }
}