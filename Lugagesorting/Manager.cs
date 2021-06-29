using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace Lugagesorting
{
    /// <summary>
    /// Manages all the classes used in the projekt
    /// </summary>
    public class Manager
    {
        public static Gate[] gates = new Gate[3];
        public static Counter[] counters = new Counter[3];
        static Sorter sorter = new Sorter();

        LugageProducer lugageProducer = new LugageProducer();
        FlightProducer flightProducer = new FlightProducer();

        public static FlightPlan[] flightPlans = new FlightPlan[3];
        public static Lugage[] sorterConveyorbelt = new Lugage[300];

        public void SimulationStart()
        {
            for (int i = 0; i < gates.Length; i++)
            {
                gates[i] = new Gate(i);
                Debug.WriteLine($"Gate: {gates[i].GateNumber} has been created");
                //PrintEvent?.Invoke(new DataPrinter($"Gate: {gates[i].GateNumber} has been created", DataPrinter.DataTypePrint.ManagerData));
            }

            for (int i = 0; i < counters.Length; i++)
            {
                counters[i] = new Counter(i);
                Debug.WriteLine($"Counter: {counters[i].CounterNumber} has been created");
                //PrintEvent?.Invoke(new DataPrinter($"Counter: {counters[i].CounterNumber} has been created", DataPrinter.DataTypePrint.ManagerData));
            }

            // ------DATA CREATERS------ //
            //Create planes thread
            Thread planeCreaterThread = new Thread(flightProducer.GenerateFlights);
            planeCreaterThread.Start();

            //Create luage lugage
            Thread lugageCreaterThread = new Thread(lugageProducer.GenerateLugage);
            lugageCreaterThread.Start();

            //Create sorter thread
            Thread threadSorter = new Thread(sorter.SortLugage);
            threadSorter.Start();
        }
    }
}



