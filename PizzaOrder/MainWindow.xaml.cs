using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace PizzaOrder
{
    public class Order
    {
        public List<Pizza> pizzaList = new List<Pizza>();
        public List<string> csv = new List<string>();
    }
    public class Pizza
    {
        public string name { get; set; }
        public double cost { get; set; }
        public Souce souce { get; set; }
        public double size { get; set; }
        public Pizza(string name, int cost, Souce souce, int Size)
        {
            this.name = name;
            this.cost = cost;
            this.souce = souce;
            this.size = Size;
        }
    }
    public class Souce
    {
        public string name { get; set; }
        public int cost { get; set; }
        public Souce(string name, int cost)
        {
            this.name = name;
            this.cost = cost;
        }
    }
    public partial class MainWindow : Window
    {
        List<Souce> Souces = new List<Souce>();
        Order orderscur = new Order();
        public MainWindow()
        {
            InitializeComponent();

            string[] soucess = File.ReadAllLines(Directory.GetCurrentDirectory() + "/../../../szosz.txt");
            foreach (var item in soucess)
            {
                Souces.Add(new Souce(item.Split(';')[0], int.Parse(item.Split(';')[1])));
            }
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Cart_Click(object sender, RoutedEventArgs e)
        {
            if(Size.Text.Length > 0 && PizzaType.Text.Length > 0 && SauceType.Text.Length > 0 && Number.Text.Length > 0) 
            {
                int times = int.Parse(Number.Text);

                Pizza currPizza = new Pizza(PizzaType.Text.Split(' ')[0], 2000, Souces.Where(x => x.name == SauceType.Text.Split(' ')[0]).ToList()[0], int.Parse(Size.Text.Split(' ')[0]));
                currPizza.cost += currPizza.souce.cost;
                currPizza.cost = currPizza.cost * (1.0 + currPizza.size / 100.0);

                for (int i = 0; i < times; i++) orderscur.pizzaList.Add(currPizza);

                orderscur.csv.Add($"{currPizza.name};{currPizza.size};{times};{currPizza.souce.name};{currPizza.cost}");
            }
        }

        private void order_Click(object sender, RoutedEventArgs e)
        {
            if(orderscur.pizzaList.Count() > 0)
            {
                TextWriter tsw = new StreamWriter(@"D:\JaniPatrik\order.txt", true);
                for (int i = 0; i < orderscur.csv.Count(); i++)
                {
                    tsw.WriteLine(orderscur.csv[i]);
                }
                tsw.Close();

                PizzaType.SelectedIndex = -1;
                SauceType.SelectedIndex = -1;
                Size.SelectedIndex = -1;
                Number.Text = "";

                orderscur = new Order();
            }
        }

        private void PizzaType_DropDownClosed(object sender, EventArgs e)
        {
            try
            {
                BitmapImage logo = new BitmapImage();
                logo.BeginInit();
                logo.UriSource = new Uri($@"D:\JaniPatrik\Képek\{PizzaType.Text.Split(' ')[0]}.png");
                logo.EndInit();
                img.Source = logo;
            }
            catch { }
        }
    }
}
