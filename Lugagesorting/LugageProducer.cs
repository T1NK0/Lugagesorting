﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace Lugagesorting
{
    public class LugageProducer
    {
        Random random = new Random();

        public void GenerateLugage()
        {
            while (Thread.CurrentThread.IsAlive)
            {
                for (int i = 0; i < Manager.counters.Length; i++)
                {
                    Counter counter = Manager.counters[i];

                    for (int j = 0; j < Manager.counters[i].CounterLugageQueue.Length; j++)
                    {
                        if (Monitor.TryEnter(counter.CounterLugageQueue))
                        {
                            int randomFlightplanIndex = random.Next(0, Manager.flightPlans.Length);
                            if (Manager.flightPlans[randomFlightplanIndex] != null)
                            {
                                if (Monitor.TryEnter(Manager.flightPlans[randomFlightplanIndex]))
                                {
                                    string lugageNumber = Manager.flightPlans[randomFlightplanIndex].PlaneNumber.ToString() + random.Next(0, 50).ToString();

                                    //if (Manager.counters[i].CounterLugageQueue[j].LugageNumber == lugageNumber)
                                    //{
                                    //    lugageNumber = Manager.flightPlans[randomFlightplanIndex].PlaneNumber.ToString() + random.Next(0, 50).ToString();
                                    //}

                                    Lugage lugage = new Lugage(lugageNumber, random.Next(1, 10000), Manager.flightPlans[randomFlightplanIndex].PlaneNumber);

                                    Debug.WriteLine($"Lugage {lugage.LugageNumber} has been created in counter {counter.CounterNumber}");

                                    while (!counter.AddToCheckinCounterQueue(lugage))
                                    {
                                        Console.WriteLine($"Counter {counter.CounterNumber}'s queue is now full");
                                        Monitor.Wait(counter.CounterLugageQueue, 2000);
                                    }

                                    //Monitor.PulseAll(Manager.flightPlans[randomFlightplanIndex]);
                                    //Monitor.Exit(Manager.flightPlans[randomFlightplanIndex]);
                                }
                                Thread.Sleep(500);
                            }

                            Monitor.PulseAll(counter.CounterLugageQueue);
                            Monitor.Exit(counter.CounterLugageQueue);
                        }
                    }
                }
            }
        }
    }
}
