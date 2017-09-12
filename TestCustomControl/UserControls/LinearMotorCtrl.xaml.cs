using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236


namespace TestCustomControl.UserControls
{
    public sealed partial class LinearMotorCtrl : UserControl
    {
        public LinearMotorCtrl()
        {
            this.InitializeComponent();
        }




        public string MotorLabel
        {
            get { return (string)GetValue(MotorLabelProperty); }
            set { SetValue(MotorLabelProperty, value); }
        }
        public static readonly DependencyProperty MotorLabelProperty =
            DependencyProperty.Register("MotorLabel", typeof(string), typeof(LinearMotorCtrl),
                new PropertyMetadata(null));



        public double CurrentPosition
        {
            get { return (double)GetValue(CurrentPositionProperty); }
            set { SetValue(CurrentPositionProperty, value); }
        }
        public static readonly DependencyProperty CurrentPositionProperty =
            DependencyProperty.Register("CurrentPosition", typeof(double), typeof(LinearMotorCtrl),
                new PropertyMetadata(0.0, OnNewPosition));

        private static void OnNewPosition(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            double newPosition = (double)d.GetValue(SliderPositionProperty);
            if (newPosition != (double)e.NewValue)
                d.SetValue(SliderPositionProperty, e.NewValue);
        }



        public double SliderPosition
        {
            get { return (double)GetValue(SliderPositionProperty); }
            set { SetValue(SliderPositionProperty, value); }
        }
        public static readonly DependencyProperty SliderPositionProperty =
            DependencyProperty.Register("SliderPosition", typeof(double), typeof(LinearMotorCtrl), 
                new PropertyMetadata(0.0, SliderChanged));

        private static void SliderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            double newPosition = (double)d.GetValue(CurrentPositionProperty);
            if (newPosition != (double)e.NewValue)
                d.SetValue(CurrentPositionProperty, e.NewValue);
        }



        public int MoveXAxisToLeft
        {
            get { return (int)GetValue(MoveXAxisToLeftProperty); }
            set { SetValue(MoveXAxisToLeftProperty, value); }
        }
        public static readonly DependencyProperty MoveXAxisToLeftProperty =
            DependencyProperty.Register("MoveXAxisToLeft", typeof(int), typeof(LinearMotorCtrl), new PropertyMetadata(0));



        public int MoveXAxisToRight
        {
            get { return (int)GetValue(MoveXAxisToRightProperty); }
            set { SetValue(MoveXAxisToRightProperty, value); }
        }
        public static readonly DependencyProperty MoveXAxisToRightProperty =
            DependencyProperty.Register("MoveXAxisToRight", typeof(int), typeof(LinearMotorCtrl), new PropertyMetadata(0));




        private void HomeEvent(object sender, RoutedEventArgs e)
        {
            CurrentPosition = 0.0;
            SliderPosition = 0.0;
        }

        private void MotorPos_LostFocus(object sender, RoutedEventArgs e)
        {
            if (SliderPosition != CurrentPosition)
                SliderPosition = CurrentPosition;
        }
    }
}
