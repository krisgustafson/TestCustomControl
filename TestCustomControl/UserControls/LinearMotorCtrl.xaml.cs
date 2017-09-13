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



        #region Dependency Properties
        /// <summary>
        /// The furthest motor position along the axis
        /// </summary>
        public double MaxPosition
        {
            get { return (double)GetValue(MaxPositionProperty); }
            set { SetValue(MaxPositionProperty, value); }
        }
        public static readonly DependencyProperty MaxPositionProperty =
            DependencyProperty.Register("MaxPosition", typeof(double), typeof(LinearMotorCtrl), new PropertyMetadata(2000.0));



        /// <summary>
        /// The amount by which clicking the increase/decrease buttons move the motor along the axis
        /// </summary>
        public double Step
        {
            get { return (double)GetValue(StepProperty); }
            set { SetValue(StepProperty, value); }
        }
        public static readonly DependencyProperty StepProperty =
            DependencyProperty.Register("Step", typeof(double), typeof(LinearMotorCtrl), new PropertyMetadata(10.0));




        public bool IsDecreaseEnableable
        {
            get { return (bool)GetValue(IsDecreaseEnableableProperty); }
            set { SetValue(IsDecreaseEnableableProperty, value); }
        }
        public static readonly DependencyProperty IsDecreaseEnableableProperty =
            DependencyProperty.Register("IsDecreaseEnableable", typeof(bool), typeof(LinearMotorCtrl), new PropertyMetadata(false));




        public bool IsIncreaseEnableable
        {
            get { return (bool)GetValue(IsIncreaseEnableableProperty); }
            set { SetValue(IsIncreaseEnableableProperty, value); }
        }
        public static readonly DependencyProperty IsIncreaseEnableableProperty =
            DependencyProperty.Register("IsIncreaseEnableable", typeof(bool), typeof(LinearMotorCtrl), new PropertyMetadata(false));




        public string MotorLabel
        {
            get { return (string)GetValue(MotorLabelProperty); }
            set { SetValue(MotorLabelProperty, value); }
        }
        public static readonly DependencyProperty MotorLabelProperty =
            DependencyProperty.Register("MotorLabel", typeof(string), typeof(LinearMotorCtrl),
                new PropertyMetadata(null));


        /// <summary>
        /// Motor's new or current position along the axis
        /// </summary>
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
            {
                d.SetValue(SliderPositionProperty, e.NewValue);
                EnableDisableIncreaseDecreaseButtons(d, (double)e.NewValue);
            }
        }

        /// <summary>
        /// Slider control, tied in with CurrentPosition
        /// </summary>
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
            {
                d.SetValue(CurrentPositionProperty, e.NewValue);
                EnableDisableIncreaseDecreaseButtons(d, (double)e.NewValue);
            }
        }


        /// <summary>
        /// Enable or disable the Increase button depending on the new/current motor position
        /// </summary>
        /// <param name="d">Dependency property for either CurrentPosition or SliderPosition</param>
        /// <param name="newPosition">New motor position relative to last position</param>
        private static void EnableDisableIncreaseDecreaseButtons(DependencyObject d, double newPosition)
        {
            double step = (double)d.GetValue(StepProperty);
            d.SetValue(IsDecreaseEnableableProperty, (newPosition >= step));
            double maxPosition = (double)d.GetValue(MaxPositionProperty);
            d.SetValue(IsIncreaseEnableableProperty, (newPosition <= maxPosition - step));
        }
        #endregion


        #region Event handlers
        private void HomeClicked(object sender, RoutedEventArgs e)
        {
            CurrentPosition = 0.0;
            SliderPosition = 0.0;
            IsDecreaseEnableable = false;
            IsIncreaseEnableable = true;
        }

        private void MotorPos_LostFocus(object sender, RoutedEventArgs e)
        {
            if (SliderPosition != CurrentPosition)
            {
                SliderPosition = CurrentPosition;
            }
        }

        private void DecreaseClicked(object sender, RoutedEventArgs e)
        {
            if (CurrentPosition >= Step)
            {
                CurrentPosition -= Step;
                IsDecreaseEnableable = (CurrentPosition >= Step);
            }
        }

        private void IncreaseClicked(object sender, RoutedEventArgs e)
        {
            if (CurrentPosition <= MaxPosition - Step)
            {
                CurrentPosition += Step;
                IsIncreaseEnableable = (CurrentPosition < MaxPosition - Step);
            }
        }
        #endregion
    }
}
