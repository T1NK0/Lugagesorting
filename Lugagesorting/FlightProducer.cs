using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace Lugagesorting
{
    public class FlightProducer
    {
        Random random = new Random();

        public void GenerateFlights()
        {
            while (Thread.CurrentThread.IsAlive)
            {
                if (Monitor.TryEnter(Manager.flightPlans))
                {

                    while (Manager.flightPlans[49] != null)
                    {
                        Monitor.Wait(Manager.flightPlans);
                    }

                    for (int i = 0; i < Manager.flightPlans.Length; i++)
                    {
                        int destination = random.Next(0, 3);
                        string destinationNumber = ((Destination)destination).ToString().ToUpper();
                        string planeNumber = destinationNumber[0].ToString() + destinationNumber[1].ToString() + (random.Next(100, 900)).ToString();
                        DateTime departureTime = DateTime.Now.AddSeconds(random.Next(120, 180));
                        int gateNumber = random.Next(0, Manager.gates.Length);

                        if (Monitor.TryEnter(Manager.gates[gateNumber]))
                        {
                            if (Manager.gates[gateNumber].FlightPlan == null)
                            {
                                Manager.gates[gateNumber].FlightPlan = new FlightPlan(planeNumber, gateNumber, 300, (Destination)destination, departureTime);
                                Manager.flightPlans[i] = Manager.gates[gateNumber].FlightPlan;
                                Debug.WriteLine($"Flight {Manager.gates[gateNumber].FlightPlan.PlaneNumber} is going to gate {gateNumber}, and is departing at {Manager.gates[gateNumber].FlightPlan.DepartureTime} to {Manager.gates[gateNumber].FlightPlan.destinations}");
                            }
                        }
                    }

                    Monitor.PulseAll(Manager.flightPlans);
                    Monitor.Exit(Manager.flightPlans);
                }
            }
        }
    }
}
