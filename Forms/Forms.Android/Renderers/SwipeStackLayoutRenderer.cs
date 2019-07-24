using Android.Content;
using Android.Views;
using Forms.Droid.Renderers;
using Forms.Widget;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(SwipeStackLayout), typeof(SwipeStackLayoutRenderer))]
namespace Forms.Droid.Renderers
{
    public class SwipeStackLayoutRenderer : ViewRenderer
    {
        private float startX, startY;

        public SwipeStackLayoutRenderer(Context context) : base(context)
        {

        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            var result = base.OnTouchEvent(e);

            switch (e.Action)
            {
                case MotionEventActions.Down:
                    startX = e.GetX();
                    startY = e.GetY();
                    Parent.RequestDisallowInterceptTouchEvent(true);
                    break;
                case MotionEventActions.Cancel:
                case MotionEventActions.Up:
                    Parent.RequestDisallowInterceptTouchEvent(false);
                    var isLeft = false;
                    var isSwipe = IsSwipeHorizontal(e, out isLeft);
                    if (isSwipe)
                    {
                        var layout = this.Element as SwipeStackLayout;
                        if (isLeft)
                        {
                            layout?.SwipeLeftEvent();
                        }
                        else
                        {
                            layout?.SwipeRightEvent();
                        }
                    }
                    break;
                case MotionEventActions.Move:
                    var left = false;
                    result = IsSwipeHorizontal(e, out left);
                    Parent.RequestDisallowInterceptTouchEvent(result);
                    break;
                default:
                    break;
            }


            System.Console.WriteLine(string.Format("TouchEvent: Action:{0} X:{1} Y:{2} Result:{3}", e.Action, e.GetX(), e.GetY(), result));
            return true;
        }

        private bool IsSwipeHorizontal(MotionEvent e, out bool isLeft)
        {
            var h = e.GetX() - startX;
            var v = e.GetY() - startY;
            isLeft = h < 0;
            return Math.Abs(h) > Math.Abs(v);
        }

        /*
        public override bool OnInterceptTouchEvent(MotionEvent e)
        {
            var result = base.OnInterceptTouchEvent(e);
            System.Console.WriteLine(string.Format("InterceptTouchEvent: Action:{0} X:{1} Y:{2} Result:{3}", e.Action, e.GetX(), e.GetY(), result));
            return result;
        }

        public override bool DispatchTouchEvent(MotionEvent e)
        {
            switch (e.ActionMasked)
            {
                case MotionEventActions.Down:
                    break;
                case MotionEventActions.Cancel:
                case MotionEventActions.Up:
                    break;
                case MotionEventActions.Move:
                    break;
                default:
                    break;
            }

            var result = base.DispatchTouchEvent(e);
            System.Console.WriteLine(string.Format("DispatchTouchEvent: Action:{0} X:{1} Y:{2} Result:{3}", e.Action, e.GetX(), e.GetY(), result));
            return result;
        }
        */

    }
}