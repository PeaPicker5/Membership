using System.Windows;

namespace Membership.Common.Controls
{
    public partial class DetailsHeaderControl
    {
        public DetailsHeaderControl()
        {
            InitializeComponent();
        }

        #region Dependency Properties

        public bool IsEditing
        {
            get => (bool)GetValue(IsEditingProperty);
            set => SetValue(IsEditingProperty, value);
        }
        public static readonly DependencyProperty IsEditingProperty = DependencyProperty.Register("IsEditing",
            typeof(bool), typeof(DetailsHeaderControl));


        public static readonly DependencyProperty IsAddingProtectedProperty =
            DependencyProperty.Register("IsAddingProtected",
                typeof(bool), typeof(DetailsHeaderControl));

        public bool IsAddingProtected
        {
            get => (bool)GetValue(IsAddingProtectedProperty);
            set => SetValue(IsAddingProtectedProperty, value);
        }

        public static readonly DependencyProperty IsEditProtectedProperty =
            DependencyProperty.Register("IsEditProtected",
                typeof(bool), typeof(DetailsHeaderControl));

        public bool IsEditProtected
        {
            get => (bool)GetValue(IsEditProtectedProperty);
            set => SetValue(IsEditProtectedProperty, value);
        }

        public static readonly DependencyProperty IsDeleteProtectedProperty =
            DependencyProperty.Register("IsDeleteProtected",
                typeof(bool), typeof(DetailsHeaderControl));

        public bool IsDeleteProtected
        {
            get => (bool)GetValue(IsDeleteProtectedProperty);
            set => SetValue(IsDeleteProtectedProperty, value);
        }

        #endregion

        #region Routed Events

        public event RoutedEventHandler AddItem
        {
            add => AddHandler(AddItemEvent, value);
            remove => RemoveHandler(AddItemEvent, value);
        }
        public static readonly RoutedEvent AddItemEvent = EventManager.RegisterRoutedEvent(
            "AddItem",
            RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(DetailsHeaderControl));

        public event RoutedEventHandler CancelItem
        {
            add => AddHandler(CancelItemEvent, value);
            remove => RemoveHandler(CancelItemEvent, value);
        }
        public static readonly RoutedEvent CancelItemEvent = EventManager.RegisterRoutedEvent(
            "CancelItem",
            RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(DetailsHeaderControl));

        public event RoutedEventHandler EditItem
        {
            add => AddHandler(EditItemEvent, value);
            remove => RemoveHandler(EditItemEvent, value);
        }
        public static readonly RoutedEvent EditItemEvent = EventManager.RegisterRoutedEvent(
            "EditItem",
            RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(DetailsHeaderControl));

        public event RoutedEventHandler RemoveItem
        {
            add => AddHandler(RemoveItemEvent, value);
            remove => RemoveHandler(RemoveItemEvent, value);
        }
        public static readonly RoutedEvent RemoveItemEvent = EventManager.RegisterRoutedEvent(
            "RemoveItem",
            RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(DetailsHeaderControl));

        public event RoutedEventHandler SaveItem
        {
            add => AddHandler(SaveItemEvent, value);
            remove => RemoveHandler(SaveItemEvent, value);
        }
        public static readonly RoutedEvent SaveItemEvent = EventManager.RegisterRoutedEvent(
            "SaveItem",
            RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(DetailsHeaderControl));

        #endregion

        #region Event Handlers

        private void SaveButtonOnClick(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(SaveItemEvent));
        }

        private void CancelButtonOnClick(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(CancelItemEvent));
        }

        private void EditButtonOnClick(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(EditItemEvent));
        }

        private void RemoveButtonOnClick(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(RemoveItemEvent));
        }

        private void AddButtonOnClick(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(AddItemEvent));
        }

        #endregion
    }
}