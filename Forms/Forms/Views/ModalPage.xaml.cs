using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Forms.Views
{
    public partial class ModalPage : ContentPage
    {
        public ModalPage()
        {
            InitializeComponent();
        }

        public void Push(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new ModalPage());
        }

        public void Pop(object sender, EventArgs e)
        {
            this.Navigation.PopModalAsync();
        }
    }
}
