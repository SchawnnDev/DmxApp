using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DmxAppLib;
using DmxAppLib.Data;

namespace DmxApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Lib _lib;
        private readonly Guid _guid;

        public MainWindow()
        {
            InitializeComponent();
            _lib = new Lib();
            _guid = Guid.Empty;
        }

        /**
         *  Spaghetti code :o
         */

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if (_lib.IsConnected) return;
            _lib.ConnectToController();

            for (var i = 0; i < 4; i++)
                _lib.SetDmxValue(_guid, new DmxValue(new DmxChannel(i), 0x0), 1);

            StatusLabel.Content = "Status: Connected";
        }

        private void Disconnect_OnClick(object sender, RoutedEventArgs e)
        {
            if (!_lib.IsConnected) return;
            _lib.Disconnect();
            StatusLabel.Content = "Status: Disconnected";
        }

        private void OnDragComplete(object sender, RoutedPropertyChangedEventArgs<double> routedPropertyChangedEventArgs)
        {
            if (!_lib.IsConnected)
                return;
            _lib.SetDmxValue(_guid, new DmxValue(new DmxChannel(0), (byte)Slider0.Value), 1);
            Slider0Value.Content = (int)Slider0.Value;
        }

        private void OnDragComplete1(object sender, RoutedPropertyChangedEventArgs<double> routedPropertyChangedEventArgs)
        {
            if (!_lib.IsConnected)
                return;
            _lib.SetDmxValue(_guid, new DmxValue(new DmxChannel(1), (byte)Slider1.Value), 1);
            Slider1Value.Content = (int)Slider1.Value;
        }

        private void OnDragComplete2(object sender, RoutedPropertyChangedEventArgs<double> routedPropertyChangedEventArgs)
        {
            if (!_lib.IsConnected)
                return;
            _lib.SetDmxValue(_guid, new DmxValue(new DmxChannel(2), (byte)Slider2.Value), 1);
            Slider2Value.Content = (int)Slider2.Value;
        }


        private void OnDragComplete3(object sender, RoutedPropertyChangedEventArgs<double> routedPropertyChangedEventArgs)
        {
            if (!_lib.IsConnected)
                return;
            _lib.SetDmxValue(_guid, new DmxValue(new DmxChannel(3), (byte)Slider3.Value), 1);
            Slider3Value.Content = (int)Slider3.Value;
        }
     
    }
}
