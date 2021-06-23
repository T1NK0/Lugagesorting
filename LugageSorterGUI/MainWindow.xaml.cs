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

namespace LugageSorterGUI
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

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

        //private void StartSimulation_Click(object sender, RoutedEventArgs e)
        //{

        //}
    }
}
