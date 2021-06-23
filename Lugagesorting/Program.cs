using System;
using System.Threading;

namespace Lugagesorting
{
    class Program
    {
        static void Main(string[] args)
        {
            //Create a new instance of our manager
            Manager manager = new Manager();

            //Create our manager thread
            Thread managerThread = new Thread(manager.SimulationStart);
            managerThread.Start();

            //Manager.PrintEvent += PrintDataEvent;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="printer">Printer chooses what we want to print</param>
        //private static void PrintDataEvent(DataPrinter printer)
        //{
        //    switch (printer.dataTypePrint)
        //    {
        //        case DataPrinter.DataTypePrint.BaggageData:
        //            break;
        //        case DataPrinter.DataTypePrint.ManagerData:
        //            Console.ForegroundColor = ConsoleColor.Green;
        //            Console.WriteLine(printer.Message);
        //            Console.ForegroundColor = ConsoleColor.White;
        //            break;
        //        default:
        //            break;
        //    }
        //}
    }
    /// <summary>
    /// Contains an enum of types of data we have (baggage data, Manager date). Can just get added to. For instance we could create a CheckInData as well to show the checkin data.
    /// </summary>
    //class DataPrinter
    //{
    //    //Chooses the datatype, you want to return
    //    public enum DataTypePrint
    //    {
    //        BaggageData,
    //        ManagerData
    //    }

    //    //Creates a string with the message.
    //    private string _message;

    //    public string Message
    //    {
    //        get { return _message; }
    //        set { _message = value; }
    //    }

    //    //Allows us to set the datatype depending on the choices in the Enum.
    //    public DataTypePrint dataTypePrint;

    //    /// <summary>
    //    /// Creates our DataPrinter, which sets the message and the datatype.
    //    /// </summary>
    //    /// <param name="message">Gets set to the message we would like to display</param>
    //    /// <param name="dateTypePrint">Gets set to the Datatype we wanna display (BaggageData or ManagerData)</param>
    //    public DataPrinter(string message, DataTypePrint dateTypePrint)
    //    {
    //        this.Message = message;
    //        this.dataTypePrint = dateTypePrint;
    //    }
    //}
}
