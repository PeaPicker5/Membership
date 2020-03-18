﻿using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Membership.Common.Controls
{
    public abstract class DetailsBaseControl<T> : UserControl, INotifyPropertyChanged where T : class
    {

        public T ItemData
        {
            get { return (T)GetValue(ItemDataProperty); }
            set { SetValue(ItemDataProperty, value); }
        }
        public static readonly DependencyProperty ItemDataProperty = 
            DependencyProperty.Register("ItemData", typeof(T), typeof(DetailsBaseControl<T>), 
                new PropertyMetadata(OnItemChanged));

        public bool IsAdding
        {
            get { return GetValue(IsAddingProperty) is bool && (bool)GetValue(IsAddingProperty); }
            set { SetValue(IsAddingProperty, value); }
        }
        public static readonly DependencyProperty IsAddingProperty =
            DependencyProperty.Register("IsAdding", typeof(bool), typeof(DetailsBaseControl<T>),
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
            add { AddHandler(ItemChangedEvent, value); }
            remove { RemoveHandler(ItemChangedEvent, value); }
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