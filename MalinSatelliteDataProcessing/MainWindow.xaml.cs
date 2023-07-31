using System;
using Galileo6;
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

        #region GlobalMethods
        // 4.1	Create two data structures using the LinkedList<T> class from the C# Systems.Collections.Generic namespace.
        // The data must be of type “double”; you are not permitted to use any additional classes, nodes, pointers or data
        // structures (array, list, etc) in the implementation of this application. The two LinkedLists of type double are
        // to be declared as global within the “public partial class”.
        LinkedList<Double> sensorDataA = new LinkedList<Double>();
        LinkedList<Double> sensorDataB = new LinkedList<Double>();



        // 4.2	Copy the Galileo.DLL file into the root directory of your solution folder and add the appropriate reference in the solution explorer.
        // Create a method called “LoadData” which will populate both LinkedLists.
        // Declare an instance of the Galileo library in the method and create the appropriate loop construct to populate the two LinkedList;
        // the data from Sensor A will populate the first LinkedList, while the data from Sensor B will populate the second LinkedList.
        // The LinkedList size will be hardcoded inside the method and must be equal to 400. The input parameters are empty, and the return type is void.
        private void LoadData()
        {
            sensorDataA.Clear();
            sensorDataB.Clear();
            int maxSize = 400;
            int mu = int.Parse(muUpDown.Text);
            int sigma = int.Parse(sigmaUpDown.Text);

            //Galileo.ReadData galileoReadData = new Galileo.ReadData();
            ReadData galileoReadData = new ReadData();

            for (int i = 0; i < maxSize; i++)
            {
                double sensorAValue = galileoReadData.SensorA(mu, sigma);
                sensorDataA.AddLast(sensorAValue);

                double sensorBValue = galileoReadData.SensorB(mu, sigma);
                sensorDataB.AddLast(sensorBValue);
            }
        }

        //4.3	Create a custom method called “ShowAllSensorData” which will display both LinkedLists in a ListView.
        //Add column titles “Sensor A” and “Sensor B” to the ListView. The input parameters are empty, and the return type is void.
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

        //4.4	Create a button and associated click method that will call the LoadData and ShowAllSensorData methods.
        //The input parameters are empty, and the return type is void.
        private void loadButton_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
            ShowAllSensorData();
            DisplayListboxData(sensorDataA, sensorAListBox);
            DisplayListboxData(sensorDataB, sensorBListBox);
        }
        
        #endregion

        #region UtilityMethods

        //4.5	Create a method called “NumberOfNodes” that will return an integer which is the number of nodes(elements) in a LinkedList.
        //The method signature will have an input parameter of type LinkedList, and the calling code argument is the linkedlist name.
        private int NumberOfNodes(LinkedList<double> LinkedList)
        {
            int nodesNumber = LinkedList.Count;

            return nodesNumber;
        }

        //4.6	Create a method called “DisplayListboxData” that will display the content of a LinkedList inside the appropriate ListBox.
        //The method signature will have two input parameters; a LinkedList, and the ListBox name.
        //The calling code argument is the linkedlist name and the listbox name.
        private void DisplayListboxData(LinkedList<double> LinkedList, ListBox ListBox)
        {
            ListBox.ItemsSource = null;
            ListBox.ItemsSource = LinkedList;
        }

        #endregion

        #region SortAndSearchMethods

        private Boolean SelectionSort(LinkedList<double> LinkedList)
        {
            int max = NumberOfNodes(LinkedList);

            for (int i = 0; i < max; i++)
            {
                int min = i;

                for (int j = i +1; j < max; j++)
                {
                    LinkedListNode<double> currentMin = LinkedList.Find(LinkedList.ElementAt(min));
                    LinkedListNode<double> currentJ = LinkedList.Find(LinkedList.ElementAt(j));
                    if (currentJ.Value < currentMin.Value)
                    {
                        min = j;                        
                    }                    
                }       
                double temp = currentMin.Value;
                currentMin.Value = currentJ.Value;
                currentJ.Value = temp;
            }
            return true;
        }

        private Boolean InsertionSort(LinkedList<double> LinkedList)
        {
            int max = NumberOfNodes(LinkedList);

            for (int i = 0; i < max; i++)
            {
                for (int j = i + 1; j > 0 && j < max; j--)
                {
                    LinkedListNode<double> current = LinkedList.Find(LinkedList.ElementAt(j));
                    LinkedListNode<double> previous = LinkedList.Find(LinkedList.ElementAt(j - 1));
                    if (previous.Value > current.Value)
                    {
                        double temp = previous.Value;
                        previous.Value = current.Value;
                        current.Value = temp;
                    }
                }
            }
            return true;
        }

        //private int BinarySearchIterative(LinkedList<double> LinkedList, int searchTarget, int min, int max)
        //{
        //    if (min <= max - 1)
        //    {
        //        int mid = (min + max) / 2;
        //        if (searchTarget == LinkedList.ElementAt(mid))
        //        {
        //            return mid;
        //        }
        //        else if (searchTarget < LinkedList.ElementAt(mid))
        //        {
        //            return BinarySearchRecursive(LinkedList, searchTarget, min, max);

        //        }
        //        else
        //        {
        //            return BinarySearchRecursive(LinkedList, searchTarget, min, max);
        //        }
        //    }
        //}

        //private int BinarySearchRecursive(LinkedList<double> LinkedList, int searchTarget, int min, int max)
        //{
        //    if (min <= max - 1)
        //    {
        //        int mid = (min + max) / 2;
        //        if (searchTarget == LinkedList.ElementAt(mid))
        //        {
        //            return mid;
        //        }
        //        else if (searchTarget < LinkedList.ElementAt(mid))
        //        {
        //            return BinarySearchRecursive(LinkedList, searchTarget, min, max);

        //        }
        //        else
        //        {
        //            return BinarySearchRecursive(LinkedList, searchTarget, min, max);
        //        }
        //    }
        //}

        #endregion

        #region UIButtonMethods

        private void aSelectionSortButton_Click(object sender, RoutedEventArgs e)
        {
            SelectionSort(sensorDataA);
            DisplayListboxData(sensorDataA, sensorAListBox);
        }

        private void aInsertionSortButton_Click(object sender, RoutedEventArgs e)
        {
            InsertionSort(sensorDataA);
            DisplayListboxData(sensorDataA, sensorAListBox);
        }

        #endregion


    }


}
