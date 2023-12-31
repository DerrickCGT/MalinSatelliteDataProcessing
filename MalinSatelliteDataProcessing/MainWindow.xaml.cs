﻿using System;
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
using System.Text.RegularExpressions;
using System.Diagnostics;

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
        /*
        4.1	Create two data structures using the LinkedList<T> class from the C# Systems.Collections.Generic namespace.
         The data must be of type “double”; you are not permitted to use any additional classes, nodes, pointers or data
         structures(array, list, etc) in the implementation of this application.The two LinkedLists of type double are
         to be declared as global within the “public partial class”.
        */
        LinkedList<Double> sensorDataA = new LinkedList<Double>();
        LinkedList<Double> sensorDataB = new LinkedList<Double>();



        /*
         4.2	Copy the Galileo.DLL file into the root directory of your solution folder and add the appropriate reference in the solution explorer.
         Create a method called “LoadData” which will populate both LinkedLists.
         Declare an instance of the Galileo library in the method and create the appropriate loop construct to populate the two LinkedList;
         the data from Sensor A will populate the first LinkedList, while the data from Sensor B will populate the second LinkedList.
         The LinkedList size will be hardcoded inside the method and must be equal to 400. The input parameters are empty, and the return type is void.
        */
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

        /*        
        4.3	Create a custom method called “ShowAllSensorData” which will display both LinkedLists in a ListView.
        Add column titles “Sensor A” and “Sensor B” to the ListView.The input parameters are empty, and the return type is void.
        */
        private void ShowAllSensorData()
        {

            /*
            Another method to load itemsource:
            List<double> aSensorList = sensorDataA.ToList();
            List<double> bSensorList = sensorDataB.ToList();
            var sensorDataItems = aSensorList.Zip(bSensorList, (sensorA, sensorB) => new { SensorA = sensorA, SensorB = sensorB });
            sensorListView.ItemsSource = sensorDataItems;
            */

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

        /*
        4.4	Create a button and associated click method that will call the LoadData and ShowAllSensorData methods.
        The input parameters are empty, and the return type is void.
        */
        private void loadButton_Click(object sender, RoutedEventArgs e)
        {
            ClearDisplay();
            LoadData();
            ShowAllSensorData();

        }
        
        #endregion

        #region UtilityMethods

        /*
        4.5	Create a method called “NumberOfNodes” that will return an integer which is the number of nodes(elements) in a LinkedList.
        The method signature will have an input parameter of type LinkedList, and the calling code argument is the linkedlist name.
        */
        private int NumberOfNodes(LinkedList<double> LinkedList)
        {
            return LinkedList.Count;
        }

        /* 
        4.6	Create a method called “DisplayListboxData” that will display the content of a LinkedList inside the appropriate ListBox.
        The method signature will have two input parameters; a LinkedList, and the ListBox name.
        The calling code argument is the linkedlist name and the listbox name.
        */
        private void DisplayListboxData(LinkedList<double> LinkedList, ListBox ListBox)
        {
            ListBox.ItemsSource = null;
            ListBox.ItemsSource = LinkedList;
        }

        /*
         This method will clear all display and enabled all button on the program.         
         */
        private void ClearDisplay()
        {
            sensorAListBox.ItemsSource = null;
            sensorBListBox.ItemsSource = null;
            aSearchTextBox.Clear();
            aBinaryIterativeTextBox.Clear();
            aBinaryRecursiveTextBox.Clear();
            aInsertionSortTextBox.Clear();
            aSelectionSortTextBox.Clear();
            bSearchTextBox.Clear();
            bBinaryIterativeTextBox.Clear();
            bBinaryRecursiveTextBox.Clear();
            bInsertionSortTextBox.Clear();
            bSelectionSortTextBox.Clear();
            disableASortButton(true);
            disableBSortButton(true);

        }

        #endregion

        #region SortAndSearchMethods
        /*       
        4.7	Create a method called “SelectionSort” which has a single input parameter of type LinkedList, 
        while the calling code argument is the linkedlist name.The method code must follow the pseudo code supplied below in the Appendix.
        The return type is Boolean.
        */

        private Boolean SelectionSort(LinkedList<double> LinkedList)
        {
            int max = NumberOfNodes(LinkedList);

            for (int i = 0; i < max; i++)
            {
                int min = i;

                for (int j = i + 1; j < max; j++)
                {
                    if (LinkedList.ElementAt(j) < LinkedList.ElementAt(min))
                    {
                        min = j;
                    }
                }
                LinkedListNode<double> currentMin = LinkedList.Find(LinkedList.ElementAt(min));
                LinkedListNode<double> currentI = LinkedList.Find(LinkedList.ElementAt(i));
                var temp = currentMin.Value;
                currentMin.Value = currentI.Value;
                currentI.Value = temp;
            }

            return true;
        }


        /*       
        4.8	Create a method called “InsertionSort” which has a single parameter of type LinkedList, 
        while the calling code argument is the linkedlist name. 
        The method code must follow the pseudo code supplied below in the Appendix. 
        The return type is Boolean.
        */
        private Boolean InsertionSort(LinkedList<double> LinkedList)
        {
            int max = NumberOfNodes(LinkedList);

            for (int i = 0; i < max - 1; i++)
            {
                for (int j = i + 1; j > 0; j--)
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

        /*
        4.9	Create a method called “BinarySearchIterative” which has the following four parameters: LinkedList, SearchValue, Minimum and Maximum.
        This method will return an integer of the linkedlist element from a successful search or the nearest neighbour value.
        The calling code argument is the linkedlist name, search value, minimum list size and the number of nodes in the list.
        The method code must follow the pseudo code supplied below in the Appendix.
        */
        private int BinarySearchIterative(LinkedList<double> LinkedList, int searchTarget, int min, int max)
        {
            
            while (min <= max - 1)
            {
                int mid = (min + max) / 2;
                if (searchTarget == LinkedList.ElementAt(mid))
                {
                    return mid;
                }
                else if (searchTarget < LinkedList.ElementAt(mid))
                {
                    max = mid - 1;                 

                }
                else
                {
                    min = mid + 1;
                    
                }
            }

            return (min + max) / 2;

        }

        /*
        4.10	Create a method called “BinarySearchRecursive” which has the following four parameters: LinkedList, SearchValue, Minimum and Maximum.
        This method will return an integer of the linkedlist element from a successful search or the nearest neighbour value.
        The calling code argument is the linkedlist name, search value, minimum list size and the number of nodes in the list.
        The method code must follow the pseudo code supplied below in the Appendix.
        */
        private int BinarySearchRecursive(LinkedList<double> LinkedList, int searchTarget, int min, int max)
        {
            if (min <= max - 1)
            {
                int mid = (min + max) / 2;
                if (searchTarget == LinkedList.ElementAt(mid))
                {
                    return mid;
                }
                else if (searchTarget < LinkedList.ElementAt(mid))
                {
                    return BinarySearchRecursive(LinkedList, searchTarget, min, mid -1);

                }
                else
                {
                    return BinarySearchRecursive(LinkedList, searchTarget, mid + 1, max);
                }
            }

            return (min + max) / 2;
        }

        #endregion

        #region UIButtonMethods
        /*
        4.11	Create four button click methods that will search the LinkedList for an integer value entered into a textbox on the form.The four methods are:
        1.	Method for Sensor A and Binary Search Iterative
        2.	Method for Sensor A and Binary Search Recursive
        3.	Method for Sensor B and Binary Search Iterative
        4.	Method for Sensor B and Binary Search Recursive
        The search code must check to ensure the data is sorted, then start a stopwatch before calling the search method.
        Once the search is complete the stopwatch will stop, and the number of ticks will be displayed in a read only textbox.
        Finally, the code/method will call the “DisplayListboxData” method and highlight the search target number and two values on each side.
        */
        private void aBinaryIterativeButton_Click(object sender, RoutedEventArgs e)
        {
            if (InsertionSort(sensorDataA))
            {
                int searchTarget;
                if(int.TryParse(aSearchTextBox.Text, out searchTarget))
                {
                    if (isValidSearchInput(sensorDataA, searchTarget))
                    {
                         long timer_ticks = TimerCalculateTicks(() =>
                        {
                            int searchIndex = BinarySearchIterative(sensorDataA, searchTarget, 0, NumberOfNodes(sensorDataA) - 1);
                        });

                        int searchIndex = BinarySearchIterative(sensorDataA, searchTarget, 0, NumberOfNodes(sensorDataA) - 1);
                        aBinaryIterativeTextBox.Text = timer_ticks.ToString() + " ticks";
                        DisplayListboxData(sensorDataA, sensorAListBox);
                        MessageBox.Show($"{searchTarget} is found on index {searchIndex}.", "Binary Iterative Search");
                        HighLight_ListBox(searchIndex, sensorAListBox, sensorDataA);
                        sensorAListBox.Focus();
                        
                    }
                    else
                    {
                        MessageBox.Show("Error! Search input is lesser than minimum value or greater than maximum value.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Error! Invalid Input", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }


            }            
        }

        private void aBinaryRecursiveButton_Click(object sender, RoutedEventArgs e)
        {

            if (InsertionSort(sensorDataA))
            {
                int searchTarget;
                if (int.TryParse(aSearchTextBox.Text, out searchTarget))
                {

                    if (isValidSearchInput(sensorDataA, searchTarget))
                    {                   
                        long timer_ticks = TimerCalculateTicks(() =>
                        {
                            int searchIndex = BinarySearchRecursive(sensorDataA, searchTarget, 0, NumberOfNodes(sensorDataA) - 1);
                        });

                        int searchIndex = BinarySearchRecursive(sensorDataA, searchTarget, 0, NumberOfNodes(sensorDataA) - 1);
                        aBinaryRecursiveTextBox.Text = timer_ticks.ToString() + " ticks";
                        DisplayListboxData(sensorDataA, sensorAListBox);
                        MessageBox.Show($"{searchTarget} is found on index {searchIndex}.", "Binary Recursive Search");
                        HighLight_ListBox(searchIndex, sensorAListBox, sensorDataA);
                        sensorAListBox.Focus();
                        
                    }
                    else
                    {
                        MessageBox.Show("Error! Search input is lesser than minimum value or greater than maximum value.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);                    
                    }
                }
                else
                {
                    MessageBox.Show("Error! Invalid Input", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
        }

        private void bBinaryIterativeButton_Click(object sender, RoutedEventArgs e)
        {
            if (InsertionSort(sensorDataB))
            {
                int searchTarget;
                if (int.TryParse(bSearchTextBox.Text, out searchTarget))
                {
                    if (isValidSearchInput(sensorDataB, searchTarget))
                    {
                        long timer_ticks = TimerCalculateTicks(() =>
                        {
                            int searchIndex = BinarySearchIterative(sensorDataB, searchTarget, 0, NumberOfNodes(sensorDataB) - 1);
                        });

                        int searchIndex = BinarySearchIterative(sensorDataB, searchTarget, 0, NumberOfNodes(sensorDataB) - 1);
                        bBinaryIterativeTextBox.Text = timer_ticks.ToString() + " ticks";
                        DisplayListboxData(sensorDataB, sensorBListBox);
                        MessageBox.Show($"{searchTarget} is found on index {searchIndex}.", "Binary Iterative Search");
                        HighLight_ListBox(searchIndex, sensorBListBox, sensorDataB);
                        sensorBListBox.Focus();
                        
                    }
                    else
                    {
                        MessageBox.Show("Error! Search input is lesser than minimum value or greater than maximum value.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Error! Invalid Input", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
                
        }

        private void bBinaryRecursiveButton_Click(object sender, RoutedEventArgs e)
        {
            if (InsertionSort(sensorDataB))
            {
                int searchTarget;
                if (int.TryParse(bSearchTextBox.Text, out searchTarget))
                {
                    if (isValidSearchInput(sensorDataB, searchTarget))
                    {
                        long timer_ticks = TimerCalculateTicks(() =>
                        {
                            int searchIndex = BinarySearchRecursive(sensorDataB, searchTarget, 0, NumberOfNodes(sensorDataB) - 1);
                        });

                        int searchIndex = BinarySearchRecursive(sensorDataB, searchTarget, 0, NumberOfNodes(sensorDataB) - 1);
                        bBinaryRecursiveTextBox.Text = timer_ticks.ToString() + " ticks";
                        DisplayListboxData(sensorDataB, sensorBListBox);
                        MessageBox.Show($"{searchTarget} is found on index {searchIndex}.", "Binary Recursive Search");
                        HighLight_ListBox(searchIndex, sensorBListBox, sensorDataB);
                        sensorBListBox.Focus();
                        
                    }
                    else
                    {
                        MessageBox.Show("Error! Search input is lesser than minimum value or greater than maximum value.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Error! Invalid Input", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }            
        }

        // Method to highlight the search target number and two values on each side, then scroll the listbox to selected indexes.
        private void HighLight_ListBox(int found, ListBox listBox, LinkedList<double> linkedList)
        {
            listBox.SelectedItems.Clear();

            int range = 2;
            int max_range = Math.Min(found + range, linkedList.Count - 1);
            int min_range = Math.Max(found - range, 0);
            listBox.ScrollIntoView(linkedList.ElementAt(found));

            for (int index = min_range; index <= max_range; index++)
            {
                var selectedItem = linkedList.ElementAt(index);
                listBox.SelectedItems.Add(selectedItem); 
            }

            
        }

        // To calculate the timer in ticks for performing an action
        private long TimerCalculateTicks(Action method)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            method();

            stopwatch.Stop();
            return stopwatch.ElapsedTicks;
        }

        //4.12	Create four button click methods that will sort the LinkedList using the Selection and Insertion methods.The four methods are:
        //1.	Method for Sensor A and Selection Sort
        //2.	Method for Sensor A and Insertion Sort
        //3.	Method for Sensor B and Selection Sort
        //4.	Method for Sensor B and Insertion Sort
        //The button method must start a stopwatch before calling the sort method.Once the sort is complete the stopwatch will stop,
        //and the number of milliseconds will be displayed in a read only textbox. Finally, the code/method will call the “ShowAllSensorData”
        //method and “DisplayListboxData” for the appropriate sensor.

        private void aSelectionSortButton_Click(object sender, RoutedEventArgs e)
        {
           
            long timer_milliseconds = TimerCalculateMilliseconds(() =>
            {
                SelectionSort(sensorDataA);
            });
            aSelectionSortTextBox.Text = timer_milliseconds.ToString() + " ms";
            DisplayListboxData(sensorDataA, sensorAListBox);
            disableASortButton(false);
        }

        private void aInsertionSortButton_Click(object sender, RoutedEventArgs e)
        {
            long timer_milliseconds = TimerCalculateMilliseconds(() =>
            {
                InsertionSort(sensorDataA);
            });
            aInsertionSortTextBox.Text = timer_milliseconds.ToString() + " ms";
            DisplayListboxData(sensorDataA, sensorAListBox);
            disableASortButton(false);
        }
                                  

        private void bSelectionSortButton_Click(object sender, RoutedEventArgs e)
        {
            long timer_milliseconds = TimerCalculateMilliseconds(() =>
            {
                SelectionSort(sensorDataB);
            });
            bSelectionSortTextBox.Text = timer_milliseconds.ToString() + " ms";
            DisplayListboxData(sensorDataB, sensorBListBox);
            disableBSortButton(false);

        }

        private void bInsertionSortButton_Click(object sender, RoutedEventArgs e)
        {
            long timer_milliseconds = TimerCalculateMilliseconds(() =>
            {
                InsertionSort(sensorDataB);
            });
            bInsertionSortTextBox.Text = timer_milliseconds.ToString() + " ms";
            DisplayListboxData(sensorDataB, sensorBListBox);
            disableBSortButton(false);
        }

        // Disable or Enable button after sort is performed (for data set A)
        private void disableASortButton(bool boolType)
        {
            aInsertionSortButton.IsEnabled = boolType;
            aSelectionSortButton.IsEnabled = boolType;
        }

        // Disable or Enable button after sort is performed (for data set B)
        private void disableBSortButton(bool boolType)
        {
            bInsertionSortButton.IsEnabled = boolType;
            bSelectionSortButton.IsEnabled = boolType;
        }

        // To calculate the timer in milliseconds for performing an action
        private long TimerCalculateMilliseconds(Action method)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            method();

            stopwatch.Stop();
            return stopwatch.ElapsedMilliseconds;
        }

        // 4.14	Add two textboxes for the search value; one for each sensor, ensure only numeric integer values can be entered.
        private void aSearchTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            
            Regex decimalRegex = new Regex(@"^[0-9]*$");
            e.Handled = !decimalRegex.IsMatch(((TextBox)sender).Text + e.Text);
        }

        // To validate the search input is between the minimum value and maximum value
        private Boolean isValidSearchInput(LinkedList<double> linkedList, int input)
        {
            double minValue = linkedList.First.Value;
            double maxValue = linkedList.Last.Value;

            if (input >= minValue && input <= maxValue)
            {
                return true;
            }
            else
            {                
                return false;               
            }
        }
        #endregion


    }
}
