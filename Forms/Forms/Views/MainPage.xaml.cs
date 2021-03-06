﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Forms.Models;
using System.Diagnostics;

namespace Forms.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : MasterDetailPage
    {
        Dictionary<int, NavigationPage> MenuPages = new Dictionary<int, NavigationPage>();
        public MainPage()
        {
            InitializeComponent();

            MasterBehavior = MasterBehavior.Popover;

            MenuPages.Add((int)MenuItemType.ImageViewer, (NavigationPage)Detail);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            try
            {
                var x = Application.Current.MainPage as MasterDetailPage;
                var y = x.Detail as NavigationPage;
                y.BarBackgroundColor = Color.White;
                y.BarTextColor = Color.Black;
            }catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        public async Task NavigateFromMenu(int id)
        {
            if (!MenuPages.ContainsKey(id))
            {
                switch (id)
                {
                    case (int)MenuItemType.Circle:
                        MenuPages.Add(id, new NavigationPage(new CircleImagePage()));
                        break;
                    case (int)MenuItemType.Browse:
                        MenuPages.Add(id, new NavigationPage(new ItemsPage()));
                        break;
                    case (int)MenuItemType.About:
                        MenuPages.Add(id, new NavigationPage(new AboutPage()));
                        break;
                    case (int)MenuItemType.SwipeListView:
                        MenuPages.Add(id, new NavigationPage(new SwipeListViewPage()));
                        break;
                    case (int)MenuItemType.Animation:
                        MenuPages.Add(id, new NavigationPage(new ColorAnimationPage()));
                        break;
                    case (int)MenuItemType.ModalPage:
                        MenuPages.Add(id, new NavigationPage(new ModalPage()));
                        break;
                    case (int)MenuItemType.Scroll:
                        MenuPages.Add(id, new NavigationPage(new ScrollPage()));
                        break;
                    case (int)MenuItemType.ListEntry:
                        MenuPages.Add(id, new NavigationPage(new ListEntryPage()));
                        break;
                    case (int)MenuItemType.ImageViewer:
                        MenuPages.Add(id, new NavigationPage(new ImageViewerPage()));
                        break;
                }
            }

            var newPage = MenuPages[id];

            if (newPage != null && Detail != newPage)
            {
                Detail = newPage;

                if (Device.RuntimePlatform == Device.Android)
                    await Task.Delay(100);

                IsPresented = false;
            }
        }
    }
}
