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
    /// 
    public partial class MainWindow : Window
    {
        private bool isFirstDeviceSelected = false;
        private static int routerCtr = 0;
        private static int switchCtr = 0;
        private static int hostCtr = 0;
        private IDictionary<string, Device> devices = new Dictionary<string, Device>();
        string selectedElement;

        private const int _IMAGE_RES = 70;

        Point offset;
        UIElement element = null;
        public MainWindow()
        {
            InitializeComponent();
        }

        //mouse down event
        private void UserCTLR_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (connectivityChkBx.IsChecked == true)
            {
                UserCTLR_MouseClick(sender, e);
                return;
            }
            this.element = sender as UIElement;
            
            offset = e.GetPosition(this.canvas);
            this.offset.Y -= Canvas.GetTop(this.element);
            this.offset.X -= Canvas.GetLeft(this.element);
            this.canvas.CaptureMouse();
        }

        private void UserCTLR_MouseClick(object sender, MouseButtonEventArgs e)
        {
            Image temp = new Image();
            if (isFirstDeviceSelected == false)
            {
                selectedElement = temp.Name;
                isFirstDeviceSelected = true;
                return;
            }
            temp = sender as Image;
            devices[selectedElement].addNeighbourDevice(temp.Name);
            isFirstDeviceSelected=false;
            drawLines();
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

            if (this.element == null)
                return;
            this.offset.Y = Canvas.GetTop(this.element);
            this.offset.X = Canvas.GetLeft(this.element);
            Image tempImg = new Image();
            tempImg = this.element as Image;
            devices[tempImg.Name].changePoint(offset);
            tempImg = null;
            this.element = null;
            drawLines();
            canvas.ReleaseMouseCapture();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        //adds image to the canvas
        private void image_btnClick(object sender, RoutedEventArgs e)
        {
            Image image = new Image();
            string source = "";
            if (sender.Equals(routerBtn))
            {
                source = "/Res/router.png";
                image.Name = "Router" + routerCtr;
                devices.Add(image.Name, new Device("Router" + routerCtr, offset));
                routerCtr++;
            }
            else if (sender.Equals(switchBtn))
            {
                source = "/Res/switch.png";
                image.Name = "Switch" + switchCtr;
                devices.Add(image.Name, new Device("Switch" + switchCtr, offset));
                switchCtr++;
            }
            else if (sender.Equals(hostBtn))
            {
                source = "/Res/host.png";
                image.Name = "Host" + hostCtr;
                devices.Add(image.Name, new Device("Host" + hostCtr, offset));
                hostCtr++;
            }

            image.Source = new BitmapImage(new Uri(source, UriKind.Relative));
            image.Height = _IMAGE_RES;
            image.Width = _IMAGE_RES;
            Canvas.SetTop(image, 20);
            Canvas.SetLeft(image, 20);
            image.PreviewMouseDown += UserCTLR_PreviewMouseDown;
            canvas.Children.Add(image);
        }

        private void drawLines()
        {
            deleteAllLines();
            foreach(var key in devices)
            {
                string[] neighbours = new string[devices[key.Key].getNeighbourDeviceCount()];
                neighbours = devices[key.Key].getNeighbours();
                for (int i = 0; i < neighbours.Length; i++)
                {
                    Line myLine = new Line();
                    myLine.Stroke = Brushes.Black;
                    myLine.X1 = devices[key.Key].getPos().X + _IMAGE_RES / 2;
                    myLine.X2 = devices[neighbours[i]].getPos().X  + _IMAGE_RES / 2;
                    myLine.Y1 = devices[key.Key].getPos().Y + _IMAGE_RES / 2;
                    myLine.Y2 = devices[neighbours[i]].getPos().Y + _IMAGE_RES / 2;

                    myLine.StrokeThickness = 1;

                    canvas.Children.Add(myLine);
                }
            }
        }

        private void deleteAllLines()
        {
            UIElement tempElement;

            for (int ctr = 0; ctr < canvas.Children.Count; ctr++)
            {
                tempElement = canvas.Children[ctr];
                if (tempElement is Line)
                {
                    canvas.Children.Remove(tempElement);
                    ctr--;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Table table = new Table(devices, NetworkIPAddressTbx.Text, networkNameTbx.Text);
            table.createTable(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
        }
    }
}
