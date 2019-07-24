using System.Collections.ObjectModel;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Forms.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SwipeListViewPage : ContentPage
    {
        public ObservableCollection<string> Items { get; set; }

        public SwipeListViewPage()
        {
            InitializeComponent();

            Items = new ObservableCollection<string>();
            for (int i = 0; i < 20; i++)
            {
                Items.Add(string.Format("Item {0}", i));
            }

            MyListView.ItemsSource = Items;
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            await DisplayAlert("Item Tapped", e.Item.ToString() + " was tapped.", "OK");

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }

        void ItemSwipeLeft(object sender, object parameter)
        {
            System.Console.WriteLine(parameter);
            Items.Insert(0, "Add " + Items.Count);
        }

        void ItemSwipeRight(object sender, object parameter)
        {
            System.Console.WriteLine(parameter);
            Items.Remove(parameter.ToString());
        }
    }
}
