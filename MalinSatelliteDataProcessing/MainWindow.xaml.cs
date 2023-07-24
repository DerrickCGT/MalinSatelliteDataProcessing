using System;
using Galileo;
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

namespace MalinSatelliteDataProcessing

    // up to 4.4, required to add input sigma and mu to 4.2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // 4.1	Create two data structures using the LinkedList<T> class from the C# Systems.Collections.Generic namespace. The data must be of type “double”; you are not permitted to use any additional classes, nodes, pointers or data structures (array, list, etc) in the implementation of this application. The two LinkedLists of type double are to be declared as global within the “public partial class”.
        LinkedList<Double> sensorDataA = new LinkedList<Double>();
        LinkedList<Double> sensorDataB = new LinkedList<Double>();

        

        // 4.2	Copy the Galileo.DLL file into the root directory of your solution folder and add the appropriate reference in the solution explorer. Create a method called “LoadData” which will populate both LinkedLists. Declare an instance of the Galileo library in the method and create the appropriate loop construct to populate the two LinkedList; the data from Sensor A will populate the first LinkedList, while the data from Sensor B will populate the second LinkedList. The LinkedList size will be hardcoded inside the method and must be equal to 400. The input parameters are empty, and the return type is void.
        private void LoadData()
        {
            sensorDataA.Clear();
            sensorDataB.Clear();
            int maxSize = 400;
            Galileo.ReadData galileoReadData = new Galileo.ReadData();
            
            for (int i = 0; i < maxSize; i++)
            {
                double sensorAValue = galileoReadData.SensorA(50, 10);
                sensorDataA.AddLast(sensorAValue);

                double sensorBValue = galileoReadData.SensorB(50, 10);
                sensorDataB.AddLast(sensorBValue);
            }
        }

        //4.3	Create a custom method called “ShowAllSensorData” which will display both LinkedLists in a ListView. Add column titles “Sensor A” and “Sensor B” to the ListView. The input parameters are empty, and the return type is void.
        private void ShowAllSensorData()
        {

            //List<double> aSensorList = sensorDataA.ToList();
            //List<double> bSensorList = sensorDataB.ToList();

            //var sensorDataItems = aSensorList.Zip(bSensorList, (sensorA, sensorB) => new { SensorA = sensorA, SensorB = sensorB });

            //sensorListView.ItemsSource = sensorDataItems;

            var sensorDataItems = new List<object>();

            var aNode = sensorDataA.First;
            var bNode = sensorDataB.First;

            while (aNode != null && bNode != null)
            {
                var dataItem = new { SensorA = aNode.Value, SensorB = bNode.Value };
                sensorDataItems.Add(dataItem);

                aNode = aNode.Next;
                bNode = bNode.Next;
            }

            sensorListView.ItemsSource = sensorDataItems;
        }

        //4.4	Create a button and associated click method that will call the LoadData and ShowAllSensorData methods. The input parameters are empty, and the return type is void.
        private void loadButton_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
            ShowAllSensorData();
        }

    }
    
}
