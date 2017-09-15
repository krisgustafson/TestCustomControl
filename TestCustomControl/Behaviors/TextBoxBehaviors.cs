using System;
using TestCustomControl.Utilities;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;


namespace TestCustomControl.Behaviors
{
    /// <summary>
    /// Extra text box behaviors: 
    /// (1) select all text on gaining focus
    /// (2) force digits (with decimal point) entry only
    /// (3) allow one decimal separator in the input number (for a double value)
    /// </summary>
    public static class TextBoxBehaviors
    {
        #region Private Properties - used only for supplying a upper limit to the allowed numeric entry
        private static Int32 MaxNumberValue
        {
            get;
            set;
        }

        //private static Int32 DefaultMaxValue
        //{
        //    get => 2000;
        //}

        private static bool NumericOnly
        {
            get;
            set;
        }
        #endregion


        #region Select All Text
        /// <summary>
        /// Gets whether the text box only allows numeric digits to be typed
        /// </summary>
        public static bool GetIsSelectAllText(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsSelectAllTextProperty);
        }

        /// <summary>
        /// Sets whether the text box only allows numeric digits to be typed
        /// </summary>
        public static void SetIsSelectAllText(DependencyObject obj, bool value)
        {
            obj.SetValue(IsSelectAllTextProperty, value);
        }

        public static readonly DependencyProperty IsSelectAllTextProperty =
            DependencyProperty.RegisterAttached(
                "IsSelectAllText",
                typeof(bool),
                typeof(TextBoxBehaviors),
                new PropertyMetadata(false, new PropertyChangedCallback(IsSelectAllTextPropertyChanged)));
        #endregion


        //#region MaxLimit
        //public static String GetMaxLimit(DependencyObject obj)
        //{
        //    return (String)obj.GetValue(MaxLimitProperty);
        //}

        ///// <summary>
        ///// Sets the highest number (numeric entry) allowed entered in the text box
        ///// </summary>
        //public static void SetMaxLimit(DependencyObject obj, String value)
        //{
        //    obj.SetValue(MaxLimitProperty, value);
        //}

        //public static readonly DependencyProperty MaxLimitProperty =
        //    DependencyProperty.RegisterAttached(
        //        "MaxLimit",
        //        typeof(String),
        //        typeof(TextBoxBehaviors),
        //        new PropertyMetadata(String.Empty, new PropertyChangedCallback(OnMaxLimitPropertyChanged)));
        //#endregion


        #region IsNumeric
        public static bool GetIsNumericOnly(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsNumericOnlyProperty);
        }

        /// <summary>
        /// Sets whether the text box allows only numeric entry
        /// </summary>
        public static void SetIsNumericOnly(DependencyObject obj, bool value)
        {
            obj.SetValue(IsNumericOnlyProperty, value);
        }

        public static readonly DependencyProperty IsNumericOnlyProperty =
            DependencyProperty.RegisterAttached(
                "IsNumericOnly",
                typeof(bool),
                typeof(TextBoxBehaviors),
                new PropertyMetadata(false, new PropertyChangedCallback(OnIsNumericOnlyPropertyChanged)));
        #endregion


        #region Connect event handlers
        private static void IsSelectAllTextPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                if ((bool)e.NewValue)
                {
                    if (NumericOnly)
                    {
                        textBox.KeyDown += TextBox_KeyDown;
                    }
                    textBox.GotFocus += SelectAllText;
                }
                else
                {
                    textBox.GotFocus -= SelectAllText;
                    textBox.KeyDown -= TextBox_KeyDown;
                }
            }
        }

        private static void TextBox_KeyDown(object sender, KeyRoutedEventArgs keyArgs)
        {   // preview key press
            TextBox tb = (TextBox)sender;
            if (keyArgs.KeyStatus.IsKeyReleased == false)
            {
                if (keyArgs.Key != VirtualKey.Delete && keyArgs.Key != VirtualKey.Back &&
                    keyArgs.Key != VirtualKey.Tab && keyArgs.Key != VirtualKey.Enter)
                {
                    keyArgs.Handled = !IsValidNumericEntry(keyArgs.Key);
                    if (keyArgs.Handled == false)
                    {   // valid digit or decimal separator - check if text will be valid as a double or int
                        if (IsKeyDecimalSeparator(keyArgs.Key) == true)
                        {
                            if (string.IsNullOrWhiteSpace(tb.Text) || (tb.Text.Contains(CultureInfoHelper.DecimalSeparator) ||
                                tb.SelectionLength == tb.Text.Length))
                            {
                                keyArgs.Handled = true;
                            }
                        }
                    }
                }
            }
        }
        #endregion


        #region Methods
        private static bool IsValidNumericEntry(VirtualKey key)
        {
            return IsKeyDigit(key) || IsKeyDecimalSeparator(key);
        }


        private static bool IsKeyDigit(VirtualKey key)
        {
            return (key >= VirtualKey.Number0 && key <= VirtualKey.Number9) || (key >= VirtualKey.NumberPad0 && key <= VirtualKey.NumberPad9);
        }


        private static bool IsKeyDecimalSeparator(VirtualKey key)
        {
            bool isDecimalSeparator = false;
            if ((key == (VirtualKey)188 && CultureInfoHelper.DecimalSeparator[0] == ',') || 
                (key == (VirtualKey)190 && CultureInfoHelper.DecimalSeparator[0] == '.') ||
                (key == VirtualKey.Space && CultureInfoHelper.DecimalSeparator[0] == ' '))
            {
                isDecimalSeparator = true;
            }

            return isDecimalSeparator;
        }
        #endregion


        #region Event Handlers
        private static void SelectAllText(object sender, RoutedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            tb.SelectAll();
            
        }

        private static void OnMaxLimitPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is String)
            {
                Int32 newValue;
                bool conversionSuccess = Int32.TryParse(e.NewValue.ToString(), out newValue);
                MaxNumberValue = (conversionSuccess) ? newValue : DefaultMaxValue;
            }
        }

        private static void OnIsNumericOnlyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            NumericOnly = (bool)(e.NewValue);
            if (NumericOnly)
            {
                (d as TextBox).KeyDown += TextBox_KeyDown;
            }
            else
            {
                (d as TextBox).KeyDown -= TextBox_KeyDown;
            }
        }
        #endregion
    }
}
