using System;
using Xamarin.Forms;

namespace Forms.Widget
{
    public class SwipeStackLayout : StackLayout
    {
        public event EventHandler SwipeLeft;
        public event EventHandler SwipeRight;

        public SwipeStackLayout()
        {

        }
    }
}
