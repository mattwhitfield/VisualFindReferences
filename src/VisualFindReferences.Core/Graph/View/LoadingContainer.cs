namespace VisualFindReferences.Core.Graph.View
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    public class LoadingContainer : ContentControl
    {

        public static readonly DependencyProperty IsBusyProperty = DependencyProperty.Register(nameof(IsBusy), typeof(bool), typeof(LoadingContainer), new PropertyMetadata(default(bool), IsBusyChanged));

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof(Text), typeof(string), typeof(LoadingContainer), new PropertyMetadata(string.Empty, TextChanged));

        public static readonly DependencyProperty MyPropertyProperty = DependencyProperty.Register(nameof(Cancel), typeof(ICommand), typeof(LoadingContainer), new PropertyMetadata(null, CancelCommandChanged));

        private static void CancelCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is LoadingContainer container)
            {
                SetTemplateProperty<Button>(container, "PART_Cancel", x => x.Command = container.Cancel);
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            SetTemplateProperty<Button>(this, "PART_Cancel", x => x.Command = Cancel);
        }

        private static void TextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is LoadingContainer container)
            {
                SetTemplateProperty<TextBlock>(container, "PART_Label", x => x.Text = container.Text);
            }
        }

        public ICommand Cancel
        {
            get { return (ICommand)GetValue(MyPropertyProperty); }
            set { SetValue(MyPropertyProperty, value); }
        }

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public bool IsBusy
        {
            get => (bool)GetValue(IsBusyProperty);
            set => SetValue(IsBusyProperty, value);
        }

        private static void IsBusyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is LoadingContainer container)
            {
                var visibility = container.IsBusy ? Visibility.Visible : Visibility.Collapsed;
                SetTemplateProperty<TextBlock>(container, "PART_Label", x => x.Text = container.Text);
                SetTemplateProperty<UIElement>(container, "PART_Label", x => x.Visibility = visibility);
                SetTemplateProperty<UIElement>(container, "PART_Spinner", x => x.Visibility = visibility);
                SetTemplateProperty<UIElement>(container, "PART_Border", x => x.Visibility = visibility);
                SetTemplateProperty<UIElement>(container, "PART_Cancel", x => x.Visibility = visibility);
            }
        }

        private static void SetTemplateProperty<T>(Control parent, string name, Action<T> action)
        {
            var foundElement = parent.Template?.FindName(name, parent);
            if (foundElement is T target)
            {
                action(target);
            }
        }
    }
}
