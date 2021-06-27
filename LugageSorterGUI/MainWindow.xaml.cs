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

            StartCounterQueueEventController();
        }

        private void LugageCreatedListener(object sender, EventArgs e)
        {
            if (Manager.counters[2] != null)
            {
                if (e is CheckinQueueEvent)
                {
                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
                    {
                        switch (((CheckinQueueEvent)e).CounterNumber)
                        {
                            case 0:
                                if (((CheckinQueueEvent)e).CounterNumber == 0 && ((CheckinQueueEvent)e).Status)
                                {
                                    //Looks at our label (counter1) and checks if the event, e, and checks our queue, and tries to get the amount and make it a string.
                                    lbl_Counter1Queue.Content = ((CheckinQueueEvent)e).Amount;
                                    lbl_Counter1State.Background = new SolidColorBrush(Colors.Green);
                                }
                                else
                                {
                                    lbl_Counter1State.Background = new SolidColorBrush(Colors.Red);
                                }
                                break;
                            case 1:
                                if (((CheckinQueueEvent)e).CounterNumber == 1 && ((CheckinQueueEvent)e).Status)
                                {
                                    //Looks at our label (counter1) and checks if the event, e, and checks our queue, and tries to get the amount and make it a string.
                                    lbl_Counter2Queue.Content = ((CheckinQueueEvent)e).Amount;
                                    lbl_Counter2State.Background = new SolidColorBrush(Colors.Green);
                                }
                                else
                                {
                                    lbl_Counter2State.Background = new SolidColorBrush(Colors.Red);
                                }
                                break;
                            case 2:
                                if (((CheckinQueueEvent)e).CounterNumber == 2 && ((CheckinQueueEvent)e).Status)
                                {
                                    //Looks at our label (counter1) and checks if the event, e, and checks our queue, and tries to get the amount and make it a string.
                                    lbl_Counter3Queue.Content = ((CheckinQueueEvent)e).Amount;
                                    lbl_Counter3State.Background = new SolidColorBrush(Colors.Green);
                                }
                                else
                                {
                                    lbl_Counter3State.Background = new SolidColorBrush(Colors.Red);
                                }
                                break;
                        }

                    }));
                }
            }
        }

        private void LugageSorterListener(object sender, EventArgs e)
        {

        }


        private void SorterEventController()
        {

        }

        private void StartCounterQueueEventController()
        {
            for (int i = 0; i < Manager.counters.Length; i++)
            {
                LugageEventController lugageEventController = new LugageEventController(i);
                lugageEventController.lugageCreationEventHandler += LugageCreatedListener;
            }
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
