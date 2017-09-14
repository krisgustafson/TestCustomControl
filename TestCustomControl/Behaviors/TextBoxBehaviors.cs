using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;

namespace TestCustomControl.Behaviors
{
    /// <summary>
    /// Extra text box behaviors: 
    /// (1) select all text on gaining focus
    /// (2) Prevent entry of more than a maximum number
    /// </summary>
    public static class TextBoxBehaviors
    {
        #region Private Properties - used only for supplying a upper limit to the allowed numeric entry
        private static Int32 MaxNumberValue
        {
            get;
            set;
        }

        private static Int32 DefaultMaxValue
        {
            get => 2000;
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

        #region MaxLimit
        public static String GetMaxLimit(DependencyObject obj, String value)
        {
            return (String)obj.GetValue(MaxLimitProperty);
        }

        public static void SetMaxLimit(DependencyObject obj, String value)
        {
            obj.SetValue(MaxLimitProperty, value);
        }

        public static readonly DependencyProperty MaxLimitProperty =
            DependencyProperty.RegisterAttached(
                "MaxLimit",
                typeof(String),
                typeof(TextBoxBehaviors),
                new PropertyMetadata(String.Empty, new PropertyChangedCallback(OnMaxLimitPropertyChanged)));
        #endregion


        #region Connect event handlers
        private static void IsSelectAllTextPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                if ((bool)e.NewValue)
                {
                    textBox.KeyDown += TextBox_KeyDown;
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
        {
            if (keyArgs.KeyStatus.IsKeyReleased == false && 
                (keyArgs.Key != Windows.System.VirtualKey.Delete && keyArgs.Key != Windows.System.VirtualKey.Back))
            {
                if (sender != null)
                {
                    TextBox tb = (TextBox)sender;
                    if (tb != null)
                    {
                        if (String.IsNullOrEmpty(tb.Text))
                        {
                            tb.Text = "";
                        }
                        else
                        {
                            keyArgs.Handled = !NoNonDoubleCharacterText(tb.Text);
                        }
                    }
                }
            }
        }
        #endregion

        private static bool NoNonDoubleCharacterText(string text)
        {
            bool invalidNumber = true;

            foreach (char c in text)
            {
                if (!Char.IsDigit(c))
                {
                    invalidNumber = false;
                    break;
                }
            }

            return invalidNumber;
        }

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
        #endregion
    }
}
