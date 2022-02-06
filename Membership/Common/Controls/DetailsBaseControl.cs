using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Membership.Common.Controls
{
    public abstract class DetailsBaseControl<T> : UserControl, INotifyPropertyChanged where T : class
    {

        public T ItemData
        {
            get => (T)GetValue(ItemDataProperty);
            set => SetValue(ItemDataProperty, value);
        }
        public static readonly DependencyProperty ItemDataProperty = 
            DependencyProperty.Register("ItemData", typeof(T), typeof(DetailsBaseControl<T>), 
                new PropertyMetadata(OnItemChanged));

        public bool IsAdding
        {
            get => GetValue(IsAddingProperty) is bool && (bool)GetValue(IsAddingProperty);
            set => SetValue(IsAddingProperty, value);
        }
        public static readonly DependencyProperty IsAddingProperty =
            DependencyProperty.Register("IsAdding", typeof(bool), typeof(DetailsBaseControl<T>),
                new FrameworkPropertyMetadata(
                    (dependencyObject, eventArgs) =>
                    {
                        if (!(dependencyObject is DetailsBaseControl<T> control))
                        {
                            return;
                        }
                        control.InvokePropertyChanged(@"TitleText");
                    }));

        public bool IsEditing
        {
            get => GetValue(IsEditingProperty) is bool && (bool)GetValue(IsEditingProperty);
            set => SetValue(IsEditingProperty, value);
        }
        public static readonly DependencyProperty IsEditingProperty =
            DependencyProperty.Register("IsEditing", typeof(bool), typeof(DetailsBaseControl<T>),
                new FrameworkPropertyMetadata(
                    (dependencyObject, eventArgs) =>
                    {
                        var control = dependencyObject as DetailsBaseControl<T>;
                        if (control == null)
                        {
                            return;
                        }
                        control.InvokePropertyChanged(@"TitleText");
                    }));

        public static readonly DependencyProperty IsAddingProtectedProperty =
            DependencyProperty.Register("IsAddingProtected", typeof(bool), typeof(DetailsBaseControl<T>),
                new FrameworkPropertyMetadata(
                    (dependencyObject, eventArgs) =>
                    {
                        var control = dependencyObject as DetailsBaseControl<T>;
                        if (control == null)
                        {
                            return;
                        }
                        control.InvokePropertyChanged(@"TitleText");
                    }));

        public bool IsAddingProtected
        {
            get => GetValue(IsAddingProtectedProperty) is bool && (bool)GetValue(IsAddingProtectedProperty);
            set => SetValue(IsAddingProtectedProperty, value);
        }

        public static readonly DependencyProperty IsEditProtectedProperty =
            DependencyProperty.Register("IsEditProtected", typeof(bool), typeof(DetailsBaseControl<T>),
                new FrameworkPropertyMetadata(
                    (dependencyObject, eventArgs) =>
                    {
                        var control = dependencyObject as DetailsBaseControl<T>;
                        if (control == null)
                        {
                            return;
                        }
                        control.InvokePropertyChanged(@"TitleText");
                    }));

        public bool IsEditProtected
        {
            get => GetValue(IsEditProtectedProperty) is bool && (bool)GetValue(IsEditProtectedProperty);
            set => SetValue(IsEditProtectedProperty, value);
        }

        public static readonly DependencyProperty IsDeleteProtectedProperty =
            DependencyProperty.Register("IsDeleteProtected", typeof(bool), typeof(DetailsBaseControl<T>),
                new FrameworkPropertyMetadata(
                    (dependencyObject, eventArgs) =>
                    {
                        var control = dependencyObject as DetailsBaseControl<T>;
                        if (control == null)
                        {
                            return;
                        }
                        control.InvokePropertyChanged(@"TitleText");
                    }));

        public bool IsDeleteProtected
        {
            get => GetValue(IsDeleteProtectedProperty) is bool && (bool)GetValue(IsDeleteProtectedProperty);
            set => SetValue(IsDeleteProtectedProperty, value);
        }

        #region Events

        private static void OnItemChanged(DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs eventArgs)
        {
            var control = dependencyObject as DetailsBaseControl<T>;
            if (control == null)
            {
                return;
            }

            control.RaiseEvent(new RoutedEventArgs(ItemChangedEvent));

            control.LoadItem();

            control.InvokePropertyChanged(@"TitleText");
        }

        public event EventHandler<EventArgs> ItemUpdated;

        protected virtual void InvokeItemUpdatedEvent()
        {
            var handler = ItemUpdated;
            handler?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region Routed Events

        public static readonly RoutedEvent ItemChangedEvent = EventManager.RegisterRoutedEvent("ItemChanged",
            RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(DetailsBaseControl<T>));

        public event RoutedEventHandler ItemChanged
        {
            add => AddHandler(ItemChangedEvent, value);
            remove => RemoveHandler(ItemChangedEvent, value);
        }

        #endregion

        #region INotifyPropertyChanged Implementation

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void InvokePropertyChanged(string propertyName = null)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        protected virtual void LoadItem()
        {
            IsEditing = false;
            IsAdding = false;
        }

        protected virtual void ResetControl()
        {
            InvokeItemUpdatedEvent();

            IsEditing = false;
            IsAdding = false;
        }
    }
}