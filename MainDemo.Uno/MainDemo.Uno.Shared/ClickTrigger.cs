using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Windows.Devices.Input;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace MainDemo.Uwp
{
    public class ClickTrigger : StateTriggerBase
    {
        public RippleAnimated TargetElement
        {
            get { return (RippleAnimated)GetValue(TargetElementProperty); }
            set { SetValue(TargetElementProperty, value); }
        }

        public static readonly DependencyProperty TargetElementProperty =
            DependencyProperty.Register("TargetElement", typeof(RippleAnimated), typeof(ClickTrigger),
                new PropertyMetadata(null, (d, e) => (d as ClickTrigger).AttachElement(e.NewValue as RippleAnimated)));

        RippleAnimated _targetElement;
        private void AttachElement(RippleAnimated fe)
        {
            if (_targetElement != null)
                _targetElement.PressedPointEvent = null;
            _targetElement = fe;
            _targetElement.PressedPointEvent = OnTapped;
        }
        private async void OnTapped()
        {
            SetActive(true);
            await Task.Delay(300);
            SetActive(false);
        }
    }
    public partial class RippleAnimated : ContentControl, INotifyPropertyChanged
    {
        protected override void OnPointerPressed(PointerRoutedEventArgs e)
        {
            if (e.Pointer.PointerDeviceType == PointerDeviceType.Mouse)
            {
                var p = e.GetCurrentPoint(this);
                CurrentPosition = new Point { X = p.Position.X - ActualWidth / 3, Y = p.Position.Y - ActualHeight / 3 };
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CurrentPosition"));
                PressedPointEvent?.Invoke();
            }
            base.OnPointerPressed(e);
        }
        public Point CurrentPosition { get; set; }
        public Action PressedPointEvent;
        public event PropertyChangedEventHandler PropertyChanged;
    }
}