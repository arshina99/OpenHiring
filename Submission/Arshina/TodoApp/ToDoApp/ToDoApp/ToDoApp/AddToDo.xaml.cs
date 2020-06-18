using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApp.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Net.Http;
using Newtonsoft.Json;

namespace ToDoApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddToDo : ContentPage
    {
        HttpClient client;
        private const string RestUrl = "https://fctodo.azurewebsites.net/todo";
        private bool isNewItem;
        public AddToDo(bool isNew)
        {
            isNewItem = isNew;
            client = new HttpClient();
            InitializeComponent();
        }
        public async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            Uri uri = new Uri(string.Format(RestUrl, string.Empty));

            var todo = (Todo)BindingContext;

            if (!string.IsNullOrWhiteSpace(todo.name))
            {
                try
                {
                    string json = JsonConvert.SerializeObject(todo);
                    StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = null;
                    if (isNewItem)
                    {
                        response = await client.PostAsync(uri, content);
                    }
                    else
                    {
                        response = await client.PutAsync(uri, content);
                    }

                    if (response.IsSuccessStatusCode)
                    {
                        Debug.WriteLine(@"\tTodoItem successfully saved.");
                    }

                }
                catch (Exception ex)
                {
                    Debug.WriteLine(@"\tERROR {0}", ex.Message);
                }

                await DisplayAlert("Alert", "Successfully Added Todo Item.", "OK");
            }
            else
            {
                await DisplayAlert("Alert", "Please Input TODO Text.", "OK");

            }

            await Navigation.PopAsync();
        }

        public async void OnDeleteButtonClicked(object sender, EventArgs e)
        {
            var item = (Todo)BindingContext;

            Uri uri = new Uri(string.Format(RestUrl, item.id.ToString()));

            try
            {
                HttpResponseMessage response = await client.DeleteAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine(@"\tTodoItem successfully deleted.");
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }
        }
    }
}
