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
    ///performs a network simulation and uses
    ///dijkstras algorithm to find shortest path between two devices
    ///also makes MST using prims algorithm to make the network more efficient
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool isFirstDeviceSelected = false; //is there any device that is currently selected
        private static int routerCtr = 0;           //counter of routers
        private static int switchCtr = 0;           //counter of switches
        private static int hostCtr = 0;             //counter of hosts
        IDictionary<string, int> deviceIndexMap = new Dictionary<string, int>();            //device and it's indexes map
        private IDictionary<string, Device> devices = new Dictionary<string, Device>();     //all devices
        string selectedElement;     //selected element name
        MatrixTranslator translator = new MatrixTranslator();
        DijkstrasAlgorithm dijkstra = new DijkstrasAlgorithm();
        PrimsAlgorithm prims = new PrimsAlgorithm();


        private const int _IMAGE_RES = 70;  //size of each added image

        Point offset;   //device coordination
        UIElement? element = null;
        public MainWindow()
        {
            InitializeComponent();
        }

        //mouse down event
        private void UserCTLR_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (connectivityChkBx.IsChecked == true || tracertCheckBox.IsChecked == true)
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

        //mouse click event
        private void UserCTLR_MouseClick(object sender, MouseButtonEventArgs e)
        {
            Image? temp = sender as Image;
            if (isFirstDeviceSelected == false)
            {
                selectedElement = temp.Name;
                isFirstDeviceSelected = true;
                return;
            }
            if (connectivityChkBx.IsChecked == true)
            {
                devices[selectedElement].addNeighbourDevice(temp.Name);
                devices[selectedElement].addNeighbourPing(GetPingForDevice());
            }
            else
            {
                int[,] matrix = updateAndGetMatrix();
                int[] path = dijkstra.dijkstra(translator.GetMatrix(), deviceIndexMap[selectedElement], deviceIndexMap[temp.Name]);


                string result = "Shortest path:\n" + GetKeyByValue(deviceIndexMap, path[path.Length - 1]);
                for (int i = path.Length - 2; i >= 0; i--)
                {
                    result += " -> " + GetKeyByValue(deviceIndexMap, path[i]);
                }
                MessageBox.Show(result);
            }
            isFirstDeviceSelected = false;
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
            Image? tempImg = new Image();
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

        //draws all lines and other UI elements
        private void drawLines()
        {
            deleteAllLines();
            foreach(var key in devices)
            {
                //add names
                TextBlock nameTextBlock = new TextBlock();
                nameTextBlock = new TextBlock();
                nameTextBlock.Text = devices[key.Key].getName();
                Canvas.SetLeft(nameTextBlock, devices[key.Key].getPos().X + _IMAGE_RES / 4);
                Canvas.SetTop(nameTextBlock, devices[key.Key].getPos().Y + (_IMAGE_RES));

                canvas.Children.Add(nameTextBlock);

                string[] neighbours = new string[devices[key.Key].getNeighbourDeviceCount()];
                neighbours = devices[key.Key].getNeighbours();
                for (int i = 0; i < neighbours.Length; i++)
                {
                    //draw connectivity line
                    Line myLine = new Line();
                    myLine.Stroke = Brushes.Black;
                    myLine.X1 = devices[key.Key].getPos().X + _IMAGE_RES / 2;
                    myLine.X2 = devices[neighbours[i]].getPos().X  + _IMAGE_RES / 2;
                    myLine.Y1 = devices[key.Key].getPos().Y + _IMAGE_RES / 2;
                    myLine.Y2 = devices[neighbours[i]].getPos().Y + _IMAGE_RES / 2;
                    myLine.StrokeThickness = 1;

                    canvas.Children.Add(myLine);

                    //draw ping
                    int[] neighboursPing = new int[neighbours.Length];
                    neighboursPing = devices[key.Key].getNeightboursPing();
                    TextBlock textBlock = new TextBlock();
                    textBlock.Text = neighboursPing[i].ToString();
                    Canvas.SetLeft(textBlock, ((myLine.X1 + myLine.X2)/2));
                    Canvas.SetTop(textBlock, ((myLine.Y1 + myLine.Y2) / 2));

                    canvas.Children.Add(textBlock);

                    
                }
            }
        }
           
        //deletes all lines and other nesesarry UI elements
        private void deleteAllLines()
        {
            UIElement tempElement;

            for (int ctr = 0; ctr < canvas.Children.Count; ctr++)
            {
                tempElement = canvas.Children[ctr];
                if (tempElement is Line || tempElement is TextBlock)
                {
                    canvas.Children.Remove(tempElement);
                    ctr--;
                }
            }
        }

        //returns a valid device connection ping
        private int GetPingForDevice()
        {
            int ping = 0;
            if (pingTextBox.Text != "")
            {
                try
                {
                    ping = int.Parse(pingTextBox.Text);
                }
                catch
                {
                    MessageBox.Show("Wrong input!");
                    Application.Current.Shutdown(); //app shutdown
                }
            }
            else
            {
                Random rnd = new Random();
                ping = rnd.Next(300);
            }

            return ping;
        }

        //create table button click
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Table table = new Table(devices, NetworkIPAddressTbx.Text, networkNameTbx.Text);
            table.createTable(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
            MessageBox.Show("Table is done!");
        }

        //run prims algorithm button click
        private void conductMST_buttonClick(object sender, RoutedEventArgs e)
        {
            int[,] matrix = updateAndGetMatrix();
            matrix = prims.primsAlgorithm(matrix);

            for (int ctr = 0; ctr < matrix.GetLength(0); ctr++)
            {
                for (int ctr1 = 0; ctr1 < matrix.GetLength(1); ctr1++)
                {
                    if (matrix[ctr, ctr1] == 0)
                    {
                        devices[GetKeyByValue(deviceIndexMap, ctr)].deleteNetworkConnection(GetKeyByValue(deviceIndexMap, ctr1));
                    }
                }
            }

            drawLines();
        }

        //updates and returns matrix
        private int[,] updateAndGetMatrix()
        {
            translator.update(devices.Values.ToArray());
            deviceIndexMap = translator.getDictionary();

            return translator.GetMatrix();
        }

        //gets key by value in dictionary
        public static string GetKeyByValue(IDictionary<string, int> dictionary, int value)
        {
            foreach (var kvp in dictionary)
            {
                if (EqualityComparer<int>.Default.Equals(kvp.Value, value))
                {
                    return kvp.Key;
                }
            }

            throw new InvalidOperationException("Value not found in dictionary");
        }
    }
}
