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
using System.Threading;
using Lugagesorting;
using System.Windows.Threading;

namespace LugageSorterGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent(); //Auto generated

            StartCounterEventController();

            //private static void PrintDataEvent(DataPrinter printer)
            //{
            //    switch (printer.dataTypePrint)
            //    {
            //        case DataPrinter.DataTypePrint.BaggageData:
            //            break;
            //        case DataPrinter.DataTypePrint.ManagerData:
            //            Manager.Textbox.Manager.Text = printer.Message;
            //            break;
            //        default:
            //            break;
            //    }
            //}
        }

        private void EventListener(object sender, EventArgs e)
        {
            if (e is LugageEvent)
            {
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
                {
                    //Looks at our label (counter1) and checks if the event, e, and checks our queue, and tries to get the amount and make it a string.
                    lbl_Counter1Queue.Content = ((LugageEvent)e).LugageInCounterQueue.ToString();
                }));
            }

            if (e is LugageEvent)
            {
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
                {
                    //Looks at our label (counter1) and checks if the event, e, and checks our queue, and tries to get the amount and make it a string.
                    lbl_Counter2Queue.Content = ((LugageEvent)e).LugageInCounterQueue.ToString();
                }));
            }

            if (e is LugageEvent)
            {
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
                {
                    //Looks at our label (counter1) and checks if the event, e, and checks our queue, and tries to get the amount and make it a string.
                    lbl_Counter3Queue.Content = ((LugageEvent)e).LugageInCounterQueue.ToString();
                }));
            }
        }

        private void StartCounterEventController()
        {
            LugageEventController lugageEventController = new LugageEventController();

            lugageEventController.lugageEventHandler += EventListener;
        }


        //Start airport button click
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Run method StartAirport
            StartAirport();
        }

        /// <summary>
        /// Starts the airportmanagerthread, whichs calls the CreateManager.
        /// </summary>
        private void StartAirport()
        {
            Thread airportManagerThread = new Thread(CreateManager);
            airportManagerThread.Start();
        }

        /// <summary>
        /// Creates a manager, so we can target our airport items from the console application.
        /// </summary>
        private void CreateManager()
        {
            Manager manager = new Manager();
            manager.SimulationStart();
        }
    }
}
