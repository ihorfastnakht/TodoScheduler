using System;
using System.Reflection;
using System.Windows.Input;
using TodoScheduler.Base;
using Xamarin.Forms;

namespace TodoScheduler.Behaviors
{
    public class EventToCommandBehavior : BehaviorBase<View>
    {
        #region properties

        public Delegate Handler { get; private set; }
        public EventInfo EventInfo { get; private set; }

        #endregion

        #region bindable properties

        public static readonly BindableProperty EventNameProperty = BindableProperty.Create("EventName", typeof(string),
            typeof(EventToCommandBehavior), null, propertyChanged: OnEventNameChanged);

        public static readonly BindableProperty CommandProperty = BindableProperty.Create("Command", typeof(ICommand),
            typeof(EventToCommandBehavior), null);

        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create("CommandParameter", typeof(object),
            typeof(EventToCommandBehavior), null);

        public static readonly BindableProperty InputConverterProperty = BindableProperty.Create("Converter", typeof(IValueConverter),
            typeof(EventToCommandBehavior), null);

        #endregion

        #region properties

        public string EventName
        {
            get { return (string)GetValue(EventNameProperty); }
            set { SetValue(EventNameProperty, value); }
        }

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public object CommandParameter
        {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        public IValueConverter Converter
        {
            get { return (IValueConverter)GetValue(InputConverterProperty); }
            set { SetValue(InputConverterProperty, value); }
        }

        #endregion

        #region protected override

        protected override void OnAttachedTo(View control)
        {
            base.OnAttachedTo(control);
            RegisterEvent(EventName);
        }

        protected override void OnDetachingFrom(View control)
        {
            DeregisterEvent(EventName);
            base.OnDetachingFrom(control);
        }

        #endregion

        #region private

        private void RegisterEvent(string eventName)
        {
            if (string.IsNullOrWhiteSpace(eventName)) return;
            EventInfo = AssociatedObject.GetType().GetRuntimeEvent(eventName);

            if (EventInfo == null)
                throw new Exception($"Can't register {eventName} event");

            MethodInfo method = typeof(EventToCommandBehavior).GetTypeInfo()
                                                              .GetDeclaredMethod("OnEventFired");

            Handler = method.CreateDelegate(EventInfo.EventHandlerType, this);
            EventInfo.AddEventHandler(AssociatedObject, Handler);
        }

        private void DeregisterEvent(string eventName)
        {
            if (string.IsNullOrWhiteSpace(eventName)) return;

            if (Handler == null) return;

            EventInfo = AssociatedObject.GetType().GetRuntimeEvent(eventName);

            if (EventInfo == null)
                throw new ArgumentException($"Can't de-register the '{eventName}' event.");

            EventInfo.RemoveEventHandler(AssociatedObject, Handler);
            Handler = null;
        }

        private void OnEventFired(object sender, object args)
        {
            if (Command == null) return;

            object parameter;

            if (CommandParameter != null)
                parameter = CommandParameter;
            else if (Converter != null)
                parameter = Converter.Convert(args, typeof(object), null, null);
            else
                parameter = args;

            if (Command.CanExecute(parameter))
                Command.Execute(parameter);
        }

        #endregion

        #region event handlers

        private static void OnEventNameChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var behavior = (EventToCommandBehavior)bindable;

            if (behavior.AssociatedObject == null)
                return;

            var oldEvent = (string)oldValue;
            var newEvent = (string)newValue;

            behavior.DeregisterEvent(oldEvent);
            behavior.RegisterEvent(newEvent);
        }
        #endregion
    }
}
