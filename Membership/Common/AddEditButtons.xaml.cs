using System.Windows;

namespace Membership.Common
{
    public partial class AddEditButtons
    {
        public AddEditButtons()
        {
            InitializeComponent();
        }

        #region Dependency Properties

        public bool IsEditing
        {
            get { return (bool)GetValue(IsEditingProperty); }
            set { SetValue(IsEditingProperty, value); }
        }
        public static readonly DependencyProperty IsEditingProperty = DependencyProperty.Register("IsEditing",
            typeof(bool), typeof(AddEditButtons));

        public bool HasAnyItems
        {
            get { return (bool)GetValue(HasAnyItemsProperty); }
            set { SetValue(HasAnyItemsProperty, value); }
        }
        public static readonly DependencyProperty HasAnyItemsProperty =
            DependencyProperty.Register("HasAnyItems",
                typeof(bool), typeof(AddEditButtons));

        public bool IsAddingProtected
        {
            get { return (bool)GetValue(IsAddingProtectedProperty); }
            set { SetValue(IsAddingProtectedProperty, value); }
        }
        public static readonly DependencyProperty IsAddingProtectedProperty =
            DependencyProperty.Register("IsAddingProtected",
                typeof(bool), typeof(AddEditButtons));

        public bool IsEditProtected
        {
            get { return (bool)GetValue(IsEditProtectedProperty); }
            set { SetValue(IsEditProtectedProperty, value); }
        }
        public static readonly DependencyProperty IsEditProtectedProperty =
            DependencyProperty.Register("IsEditProtected",
                typeof(bool), typeof(AddEditButtons));

        public bool IsDeleteProtected
        {
            get { return (bool)GetValue(IsDeleteProtectedProperty); }
            set { SetValue(IsDeleteProtectedProperty, value); }
        }
        public static readonly DependencyProperty IsDeleteProtectedProperty =
            DependencyProperty.Register("IsDeleteProtected",
                typeof(bool), typeof(AddEditButtons));


        #endregion

        #region Routed Events

        public static readonly RoutedEvent AddItemEvent = EventManager.RegisterRoutedEvent(
            "AddItem",
            RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(AddEditButtons));

        public event RoutedEventHandler AddItem
        {
            add { AddHandler(AddItemEvent, value); }
            remove { RemoveHandler(AddItemEvent, value); }
        }

        public static readonly RoutedEvent CancelItemEvent = EventManager.RegisterRoutedEvent(
            "CancelItem",
            RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(AddEditButtons));

        public event RoutedEventHandler CancelItem
        {
            add { AddHandler(CancelItemEvent, value); }
            remove { RemoveHandler(CancelItemEvent, value); }
        }

        public static readonly RoutedEvent EditItemEvent = EventManager.RegisterRoutedEvent(
            "EditItem",
            RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(AddEditButtons));

        public event RoutedEventHandler EditItem
        {
            add { AddHandler(EditItemEvent, value); }
            remove { RemoveHandler(EditItemEvent, value); }
        }

        public static readonly RoutedEvent HideDetailsEvent = EventManager.RegisterRoutedEvent(
            "HideDetails",
            RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(AddEditButtons));

        public event RoutedEventHandler HideDetails
        {
            add { AddHandler(HideDetailsEvent, value); }
            remove { RemoveHandler(HideDetailsEvent, value); }
        }

        public static readonly RoutedEvent RemoveItemEvent = EventManager.RegisterRoutedEvent(
            "RemoveItem",
            RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(AddEditButtons));

        public event RoutedEventHandler RemoveItem
        {
            add { AddHandler(RemoveItemEvent, value); }
            remove { RemoveHandler(RemoveItemEvent, value); }
        }

        public static readonly RoutedEvent SaveItemEvent = EventManager.RegisterRoutedEvent(
            "SaveItem",
            RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(AddEditButtons));

        public event RoutedEventHandler SaveItem
        {
            add { AddHandler(SaveItemEvent, value); }
            remove { RemoveHandler(SaveItemEvent, value); }
        }

        #endregion

        #region Event Handlers

        private void HideDetailsButtonOnClick(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(HideDetailsEvent));
        }

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