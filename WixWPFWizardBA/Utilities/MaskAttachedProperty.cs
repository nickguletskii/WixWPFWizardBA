namespace WixWPFWizardBA.Utilities
{
    using System;
    using System.Text.RegularExpressions;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    public static class MaskAttachedProperty
    {
        private static readonly DependencyPropertyKey MaskExpressionPropertyKey =
            DependencyProperty.RegisterAttachedReadOnly("MaskExpression",
                typeof(Regex),
                typeof(MaskAttachedProperty),
                new FrameworkPropertyMetadata());

        public static readonly DependencyProperty MaskProperty = DependencyProperty.RegisterAttached("Mask",
            typeof(string),
            typeof(MaskAttachedProperty),
            new FrameworkPropertyMetadata(OnMaskChanged));

        public static readonly DependencyProperty MaskExpressionProperty =
            MaskExpressionPropertyKey.DependencyProperty;

        public static string GetMask(TextBox textBox)
        {
            if (textBox == null)
                throw new ArgumentNullException(nameof(textBox));

            return (string) textBox.GetValue(MaskProperty);
        }

        public static void SetMask(TextBox textBox, string mask)
        {
            if (textBox == null) throw new ArgumentNullException(nameof(textBox));
            textBox.SetValue(MaskProperty, mask);
        }

        public static Regex GetMaskExpression(TextBox textBox)
        {
            if (textBox == null)
                throw new ArgumentNullException(nameof(textBox));

            return (Regex) textBox.GetValue(MaskExpressionProperty);
        }

        private static void SetMaskExpression(TextBox textBox, Regex regex)
        {
            textBox.SetValue(MaskExpressionPropertyKey, regex);
        }

        private static void textBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var textBox = (TextBox) sender;
            var maskExpression = GetMaskExpression(textBox);

            if (maskExpression == null)
                return;

            var proposedText = GetTextAfterAddingNewText(textBox, e.Text);

            if (!maskExpression.IsMatch(proposedText))
                e.Handled = true;
        }

        private static void textBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var textBox = (TextBox) sender;
            var maskExpression = GetMaskExpression(textBox);

            if (maskExpression == null)
                return;

            if (e.Key == Key.Space)
            {
                var proposedText = GetTextAfterAddingNewText(textBox, " ");

                if (!maskExpression.IsMatch(proposedText))
                {
                    e.Handled = true;
                }
            }
        }

        private static string GetTextAfterAddingNewText(TextBox textBox, string newText)
        {
            var text = textBox.Text;
            if (textBox.SelectionStart != -1)
            {
                text = text.Remove(textBox.SelectionStart, textBox.SelectionLength);
            }
            return text.Insert(textBox.CaretIndex, newText);
        }

        private static void OnMaskChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var textBox = (TextBox) dependencyObject;
            var mask = (string) e.NewValue;
            textBox.PreviewTextInput -= textBox_PreviewTextInput;
            textBox.PreviewKeyDown -= textBox_PreviewKeyDown;
            DataObject.RemovePastingHandler(textBox, PastingHandler);

            if (mask == null)
            {
                textBox.ClearValue(MaskProperty);
                textBox.ClearValue(MaskExpressionProperty);
            }
            else
            {
                textBox.SetValue(MaskProperty, mask);
                SetMaskExpression(textBox,
                    new Regex(mask, RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace));
                textBox.PreviewTextInput += textBox_PreviewTextInput;
                textBox.PreviewKeyDown += textBox_PreviewKeyDown;
                DataObject.AddPastingHandler(textBox, PastingHandler);
            }
        }

        private static void PastingHandler(object sender, DataObjectPastingEventArgs e)
        {
            var textBox = sender as TextBox;
            var maskExpression = GetMaskExpression(textBox);

            if (maskExpression == null)
            {
                return;
            }

            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                var pastedText = e.DataObject.GetData(typeof(string)) as string;
                var proposedText = GetTextAfterAddingNewText(textBox, pastedText);

                if (!maskExpression.IsMatch(proposedText))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }
    }
}