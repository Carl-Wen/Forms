using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;

using Forms.Models;
using Forms.Views;
using Forms.ViewModels;
using Xamarin.Forms.Internals;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Xamarin.Forms.PlatformConfiguration;

namespace Forms.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class ItemsPage : ContentPage
    {
        ItemsViewModel viewModel;

        public ItemsPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new ItemsViewModel();
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as Item;
            if (item == null)
                return;

            await Navigation.PushAsync(new ItemDetailPage(new ItemDetailViewModel(item)));

            // Manually deselect item.
            ItemsListView.SelectedItem = null;
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new Xamarin.Forms.NavigationPage(new NewItemPage()));
        }

        void SelectAll_Clicked(object sender, EventArgs e)
        {
            viewModel.Items.ForEach((item) => {
                item.Selected = true;
            });
        }

        void Delete_Clicked(object sender, EventArgs e)
        {
            viewModel.Items = new ObservableCollection<Item>(viewModel.Items.ToList().FindAll(x => !x.Selected).ToList());
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);

            try
            {
                var x = Xamarin.Forms.Application.Current.MainPage as Xamarin.Forms.MasterDetailPage;
                var y = x.Detail as Xamarin.Forms.NavigationPage;
                y.BarBackgroundColor = Color.White;
                y.BarTextColor = Color.Black;
                y.On<iOS>().SetHideNavigationBarSeparator(true);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }
    }
}