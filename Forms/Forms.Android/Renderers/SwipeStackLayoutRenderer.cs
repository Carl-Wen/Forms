using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Forms.Droid.Renderers;
using Forms.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(SwipeStackLayout), typeof(SwipeStackLayoutRenderer))]
namespace Forms.Droid.Renderers
{
    public class SwipeStackLayoutRenderer : ViewRenderer
    {
        const int MinSwipeLength = 200;

        int RealMinLength = MinSwipeLength;

        public SwipeStackLayoutRenderer(Context context) : base(context)
        {

        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            System.Console.WriteLine(string.Format("TouchEvent: Action:{0} X:{1} Y:{2}", e.Action, e.GetX(), e.GetY()));
            return base.OnTouchEvent(e);
        }

        public override bool OnInterceptTouchEvent(MotionEvent e)
        {
            System.Console.WriteLine(string.Format("InterceptTouchEvent: Action:{0} X:{1} Y:{2}", e.Action, e.GetX(), e.GetY()));
            return base.OnInterceptTouchEvent(e);
        }

        public override bool DispatchTouchEvent(MotionEvent e)
        {
            System.Console.WriteLine(string.Format("DispatchTouchEvent: Action:{0} X:{1} Y:{2}", e.Action, e.GetX(), e.GetY()));

            if (TouchDispatcher.TouchingView == null && e.ActionMasked == MotionEventActions.Down)
            {
                TouchDispatcher.TouchingView = this.Element as SwipeStackLayout;
                TouchDispatcher.StartingBiasX = e.GetX();
                TouchDispatcher.StartingBiasY = e.GetY();
                TouchDispatcher.InitialTouch = DateTime.Now;
            }

            switch (e.ActionMasked)
            {
                case MotionEventActions.Down:
                    if (null == TouchDispatcher.TouchingView)
                    {
                        TouchDispatcher.TouchingView = this.Element as SwipeStackLayout;
                        TouchDispatcher.StartingBiasX = e.GetX();
                        TouchDispatcher.StartingBiasY = e.GetY();
                        TouchDispatcher.InitialTouch = DateTime.Now;
                    }
                    break;
                case MotionEventActions.Up:

                    TouchDispatcher.TouchingView = null;
                    break;
                case MotionEventActions.Move:

                    break;
                default:
                    break;
            }

            var result = IsSwipeUpOrDown(e);
            if (result)
            {
                return base.DispatchTouchEvent(e);
            }
            else
                return true;
            //return base.DispatchTouchEvent(e);
        }

        private bool IsSwipeLeft(MotionEvent e)
        {
            if (null != TouchDispatcher.TouchingView)
            {
                var length = e.GetX() - TouchDispatcher.StartingBiasX;
                if (length > RealMinLength)
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsSwipeRight(MotionEvent e)
        {
            if (null != TouchDispatcher.TouchingView)
            {
                var length = TouchDispatcher.StartingBiasX - e.GetX();
                if (length > RealMinLength)
                {
                    return true;
                }
                else
                {
                }
            }

            return false;
        }

        private bool IsSwipeUpOrDown(MotionEvent e)
        {
            if (null != TouchDispatcher.TouchingView)
            {
                var length = Math.Abs(TouchDispatcher.StartingBiasY - e.GetY());
                return length > RealMinLength;
            }

            return false;
        }
    }

    public static class TouchDispatcher
    {
        public static SwipeStackLayout TouchingView { get; internal set; }
        public static float StartingBiasX { get; internal set; }
        public static float StartingBiasY { get; internal set; }
        public static DateTime InitialTouch { get; internal set; }
        static TouchDispatcher()
        {
            TouchingView = null;
            StartingBiasX = 0;
            StartingBiasY = 0;
        }
    }
}