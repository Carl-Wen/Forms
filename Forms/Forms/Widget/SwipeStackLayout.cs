using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace Forms.Widget
{
    public class SwipeStackLayout : StackLayout
    {
        public static readonly BindableProperty LeftCommandProperty = BindableProperty.Create(nameof(SwipeLeftCommand), typeof(ICommand), typeof(ICommand));
        public static readonly BindableProperty RightCommandProperty = BindableProperty.Create(nameof(SwipeRightCommand), typeof(ICommand), typeof(ICommand));

        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(object));
        public static readonly BindableProperty LeftCommandParameterProperty = BindableProperty.Create(nameof(SwipeLeftCommandParameter), typeof(object), typeof(object));
        public static readonly BindableProperty RightCommandParameterProperty = BindableProperty.Create(nameof(SwipeRightCommandParameter), typeof(object), typeof(object));

        public event EventHandler<object> SwipeLeft;
        public event EventHandler<object> SwipeRight;

        public ICommand SwipeLeftCommand
        {
            get { return (ICommand)GetValue(LeftCommandProperty); }
            set { SetValue(LeftCommandProperty, value); }
        }
        public ICommand SwipeRightCommand
        {
            get { return (ICommand)GetValue(RightCommandProperty); }
            set { SetValue(RightCommandProperty, value); }
        }
        public object CommandParameter
        {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }
        public object SwipeLeftCommandParameter
        {
            get { return GetValue(LeftCommandParameterProperty); }
            set { SetValue(LeftCommandParameterProperty, value); }
        }
        public object SwipeRightCommandParameter
        {
            get { return GetValue(RightCommandParameterProperty); }
            set { SetValue(RightCommandParameterProperty, value); }
        }

        public SwipeStackLayout()
        {
            if (Device.RuntimePlatform == Device.Android)
            {
            }
            else
            {
                var swipeLeftGesture = new SwipeGestureRecognizer
                {
                    Direction = SwipeDirection.Left,
                };
                swipeLeftGesture.Swiped += (s, e) =>
                {
                    SwipeLeftEvent();
                };
                var swipeRightGesture = new SwipeGestureRecognizer
                {
                    Direction = SwipeDirection.Right
                };
                swipeRightGesture.Swiped += (s, e) =>
                {
                    SwipeRightEvent();
                };
                GestureRecognizers.Add(swipeLeftGesture);
                GestureRecognizers.Add(swipeRightGesture);
            }
        }

        public void SwipeLeftEvent()
        {
            var para = SwipeLeftCommandParameter ?? CommandParameter;
            if (null != SwipeLeftCommand)
            {
                if (SwipeLeftCommand.CanExecute(para))
                {
                    SwipeLeftCommand.Execute(para);
                }
            }
            else
            {
                SwipeLeft?.Invoke(this, para);
            }
        }

        public void SwipeRightEvent()
        {
            var para = SwipeRightCommandParameter ?? CommandParameter;
            if (null != SwipeRightCommand)
            {
                if (SwipeRightCommand.CanExecute(para))
                {
                    SwipeRightCommand.Execute(para);
                }
            }
            else
            {
                SwipeRight?.Invoke(this, para);
            }
        }
    }
}
