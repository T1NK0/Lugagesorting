﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Lugagesorting
{
    /// <summary>
    /// Manages all the classes used in the projekt
    /// </summary>
    class Manager
    {
        public static Gate[] gates = new Gate[6];
        public static Counter[] counters = new Counter[10];

        LugageProducer lugageProducer = new LugageProducer();
        FlightProducer flightProducer = new FlightProducer();

        public static FlightPlan[] flightPlans = new FlightPlan[5];
        public static Lugage[] queueLugages = new Lugage[100];

        static Lugage[] counterBuffer = new Lugage[100];
        



        Counter counter = new Counter();
        FlightPlan flightPlan = new FlightPlan();

        public void SimulationStart()
        {
            // ------DATA CREATERS------ //
            //CreateLuage
            Thread lugageCreaterThread = new Thread(flightProducer.GenerateFlights);
            lugageCreaterThread.Start();

            //CreatePlanes
            Thread planeCreaterThread = new Thread(lugageProducer.GenerateLugage);
            planeCreaterThread.Start();

            for (int i = 1; i < gates.Length; i++)
            {
                gates[i] = new Gate(i);
            }

            //everything needs to run in here while the thread is alive. (while the program runs, this needs to run)
            while (Thread.CurrentThread.IsAlive)
            {
                // ------DATA OPEN / CLOSE------ //

                ////OpenCounter
                //Thread counterCreaterThread = new Thread();
                //counterCreaterThread.Start();

                ////Opengate
                //Thread gateCreaterThread = new Thread();
                //gateCreaterThread.Start();

                ////RunSorter
                //Thread sorterThread = new Thread();
                //sorterThread.Start();
            }
        }
    }
}



