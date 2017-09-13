using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;


namespace TestCustomControl.Behaviors
{
    public static class TextBoxBehaviors
    {
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


        #region Connect event handlers
        private static void IsSelectAllTextPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                if ((bool)e.NewValue)
                {
                    textBox.GotFocus += SelectAllText;
                }
                else
                {
                    textBox.GotFocus -= SelectAllText;
                }
            }
        }
        #endregion

        #region Event Handlers
        private static void SelectAllText(object sender, RoutedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            tb.SelectAll();
            
        }
        #endregion
    }
}
