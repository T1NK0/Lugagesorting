using System;
using System.Collections.Generic;
using System.Text;

namespace Lugagesorting
{
    /// <summary>
    /// Creates an enum with the destinations for the planes.
    /// </summary>
    public enum Destination
    {
        Germany,
        Netherlands,
        Finland
    }

    /// <summary>
    /// Creates a flightplan which contains information about the planes
    /// </summary>
    public class FlightPlan
    {
        private string _planeNumber;
        private int _gateNumber;
        private DateTime _departureTime;
        private int _planeSeats;

        public Destination destinations;

        public string PlaneNumber
        {
            get { return _planeNumber; }
            set { _planeNumber = value; }
        }

        public int GateNumber
        {
            get { return _gateNumber; }
            set { _gateNumber = value; }
        }

        public DateTime DepartureTime
        {
            get { return _departureTime; }
            set { _departureTime = value; }
        }

        public int PlaneSeats
        {
            get { return _planeSeats; }
            set { _planeSeats = value; }
        }


        public FlightPlan(string planeNumber, int gateNumber, int planeSeats, Destination destination, DateTime departureTime)
        {
            PlaneNumber = planeNumber;
            GateNumber = gateNumber;
            destinations = destination;
            DepartureTime = departureTime;
            PlaneSeats = planeSeats;
        }
    }
}
