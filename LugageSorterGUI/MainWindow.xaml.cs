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
            SorterEventController();
            GateEventController();
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
                                if (((CheckinQueueEvent)e).CounterNumber == 0)
                                {
                                    //Looks at our label (counter1) and checks if the event, e, and checks our queue, and tries to get the amount and make it a string.
                                    lbl_Counter1.Content = "Counter: " + ((CheckinQueueEvent)e).CounterNumber;
                                    lbl_Counter1Queue.Content = ((CheckinQueueEvent)e).Amount;
                                }
                                if (((CheckinQueueEvent)e).Status)
                                {
                                    lbl_Counter1State.Background = new SolidColorBrush(Colors.Green);
                                    lbl_Counter1State.Content = "Open";
                                }
                                else
                                {
                                    lbl_Counter1State.Background = new SolidColorBrush(Colors.Red);
                                    lbl_Counter1State.Content = "Closed";
                                }
                                break;
                            case 1:
                                if (((CheckinQueueEvent)e).CounterNumber == 1)
                                {
                                    //Looks at our label (counter1) and checks if the event, e, and checks our queue, and tries to get the amount and make it a string.
                                    lbl_Counter2.Content = "Counter: " + ((CheckinQueueEvent)e).CounterNumber;
                                    lbl_Counter2Queue.Content = ((CheckinQueueEvent)e).Amount;
                                }
                                if (((CheckinQueueEvent)e).Status)
                                {
                                    lbl_Counter2State.Background = new SolidColorBrush(Colors.Green);
                                    lbl_Counter2State.Content = "Open";
                                }
                                else
                                {
                                    lbl_Counter2State.Background = new SolidColorBrush(Colors.Red);
                                    lbl_Counter2State.Content = "Closed";
                                }
                                break;
                            case 2:
                                if (((CheckinQueueEvent)e).CounterNumber == 2)
                                {
                                    lbl_Counter3.Content = "Counter: " + ((CheckinQueueEvent)e).CounterNumber;
                                    lbl_Counter3Queue.Content = ((CheckinQueueEvent)e).Amount;
                                    lbl_Counter3State.Background = new SolidColorBrush(Colors.Green);
                                    lbl_Counter3State.Content = "Open";
                                }
                                if (((CheckinQueueEvent)e).Status)
                                {
                                    lbl_Counter3State.Background = new SolidColorBrush(Colors.Green);
                                    lbl_Counter3State.Content = "Open";
                                }
                                else
                                {
                                    lbl_Counter3State.Background = new SolidColorBrush(Colors.Red);
                                    lbl_Counter3State.Content = "Closed";
                                }
                                break;
                        }

                    }));
                }
            }
        }

        private void LugageSorterListener(object sender, EventArgs e)
        {
            if (e is LugageSorterEvent)
            {
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
                {
                    lbl_SorterAmount.Content = ((LugageSorterEvent)e).Amount;
                    lbl_LugageGettingSorted.Content = ((LugageSorterEvent)e).Lugage;
                    //lbl_LugageToGate.Content = ((LugageSorterEvent)e).
                }));
            }

        }

        private void GateEventListener(object sender, EventArgs e)
        {
            if (e is GateEvent)
            {
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
                {
                    switch (((GateEvent)e).GateNumber)
                    {
                        case 0:
                            if (((GateEvent)e).GateNumber == 0)
                            {
                                //Looks at our label (counter1) and checks if the event, e, and checks our queue, and tries to get the amount and make it a string.
                                lbl_Gate1Number.Content = "Gate: " + ((GateEvent)e).GateNumber;
                                lbl_Gate1LugageAmount.Content = ((GateEvent)e).Amount;
                            }
                            if (((GateEvent)e).Status)
                            {
                                lbl_Gate1State.Background = new SolidColorBrush(Colors.Green);
                                lbl_Gate1State.Content = "Open";
                            }
                            else
                            {
                                lbl_Gate1State.Background = new SolidColorBrush(Colors.Red);
                                lbl_Gate1State.Content = "Closed";
                            }
                            break;
                        case 1:
                            if (((GateEvent)e).GateNumber == 1)
                            {
                                //Looks at our label (counter1) and checks if the event, e, and checks our queue, and tries to get the amount and make it a string.
                                lbl_Gate2Number.Content = "Gate: " + ((GateEvent)e).GateNumber;
                                lbl_Gate2LugageAmount.Content = ((GateEvent)e).Amount;
                            }
                            if (((GateEvent)e).Status)
                            {
                                lbl_Gate2State.Background = new SolidColorBrush(Colors.Green);
                                lbl_Gate2State.Content = "Open";
                            }
                            else
                            {
                                lbl_Gate2State.Background = new SolidColorBrush(Colors.Red);
                                lbl_Gate2State.Content = "Closed";
                            }
                            break;
                        case 2:
                            if (((GateEvent)e).GateNumber == 2)
                            {
                                //Looks at our label (counter1) and checks if the event, e, and checks our queue, and tries to get the amount and make it a string.
                                lbl_Gate3Number.Content = "Gate: " + ((GateEvent)e).GateNumber;
                                lbl_Gate3LugageAmount.Content = ((GateEvent)e).Amount;
                            }
                            if (((GateEvent)e).Status)
                            {
                                lbl_Gate3State.Background = new SolidColorBrush(Colors.Green);
                                lbl_Gate3State.Content = "Open";
                            }
                            else
                            {
                                lbl_Gate3State.Background = new SolidColorBrush(Colors.Red);
                                lbl_Gate3State.Content = "Closed";
                            }
                            break;
                    }
                }));
            }
        }

        private void SorterEventController()
        {
            LugageSorterEventController lugageSorterEventController = new LugageSorterEventController();
            lugageSorterEventController.lugageSorterEventHandler += LugageSorterListener;
        }

        private void StartCounterQueueEventController()
        {
            for (int i = 0; i < Manager.counters.Length; i++)
            {
                LugageEventController lugageEventController = new LugageEventController(i);
                lugageEventController.lugageCreationEventHandler += LugageCreatedListener;
            }
        }

        private void GateEventController()
        {
            for (int i = 0; i < Manager.gates.Length; i++)
            {
                GateEventController gateEventController = new GateEventController(i);
                gateEventController.GateCheckinEventHandler += GateEventListener;
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
