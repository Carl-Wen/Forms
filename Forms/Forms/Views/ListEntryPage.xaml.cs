using Forms.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Forms.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListEntryPage : ContentPage
    {
        public ListEntryPage()
        {
            InitializeComponent();
        }

        private void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            var entry = sender as Entry;
            var entryBind = entry.BindingContext as string;
            var bindingContext = BindingContext as ListEntryPageViewModel;
            var index = bindingContext.Source.IndexOf(entryBind);
            bindingContext.Source.RemoveAt(index);
            bindingContext.Source.Insert(index, e.NewTextValue);
        }
    }
}