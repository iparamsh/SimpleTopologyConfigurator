using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SimpleTopologyConfigurator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Point offset;
        UIElement element = null;
        public MainWindow()
        {
            InitializeComponent();
        }

        //mouse down event
        private void UserCTLR_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.element = sender as UIElement;
            offset = e.GetPosition(this.canvas);
            this.offset.Y -= Canvas.GetTop(this.element);
            this.offset.X -= Canvas.GetLeft(this.element);
            this.canvas.CaptureMouse();
        }

        //mouse move event
        private void canvas_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (this.element == null)
                return;
            var position = e.GetPosition(sender as UIElement);
            Canvas.SetTop(this.element, position.Y - this.offset.Y);
            Canvas.SetLeft(this.element, position.X - this.offset.X);
        }



        //mouse up event
        private void UserCTLR_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            this.element = null;
            canvas.ReleaseMouseCapture();
        }

        //adds image to the canvas
        private void image_btnClick(object sender, RoutedEventArgs e)
        {
            string source = "";
            if (sender.Equals(routerBtn))
                source = "/Res/router.png";
            else if (sender.Equals(switchBtn))
                source = "/Res/switch.png";
            else if (sender.Equals(hostBtn))
                source = "/Res/host.png";
            Image image = new Image();
            image.Source = new BitmapImage(new Uri(source, UriKind.Relative));
            image.Height = 70;
            image.Width = 70;
            Canvas.SetTop(image, 20);
            Canvas.SetLeft(image, 20);
            image.PreviewMouseDown += UserCTLR_PreviewMouseDown;
            canvas.Children.Add(image);
        }
    }
}
