using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows;
using Microsoft.Win32;
using System.Collections.ObjectModel;

namespace WpfOsztalyzas
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string fajlNev = "naplo.txt";
        //Így minden metódus fogja tudni használni.
        ObservableCollection<Osztalyzat> jegyek = new ObservableCollection<Osztalyzat>();

        public MainWindow()
        {
            InitializeComponent();
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                fajlNev = openFileDialog.FileName;
            // todo Fájlok kitallózásával tegye lehetővé a naplófájl kiválasztását!
            // Ha nem választ ki semmit, akkor "naplo.csv" legyen az állomány neve. A későbbiekben ebbe fog rögzíteni a program.

            // todo A kiválasztott naplót egyből töltse be és a tartalmát jelenítse meg a datagrid-ben!
            double jegyekosszege = 0;
            jegyek.Clear();
            StreamReader sr = new StreamReader(fajlNev);
            while (!sr.EndOfStream)
            {
                string[] mezok = sr.ReadLine().Split(";");
                Osztalyzat ujJegy = new Osztalyzat(mezok[0], mezok[1], mezok[2], int.Parse(mezok[3]), mezok[4]);
                jegyekosszege += double.Parse(mezok[3]);
                jegyek.Add(ujJegy);
            }
            sr.Close();
            dgJegyek.ItemsSource = jegyek;
            lblJegyekSzama.Content = jegyek.Count;
            lblAtlag.Content = Math.Round((jegyekosszege / jegyek.Count), 2);
            lblPath.Content = fajlNev;
        }

        private void btnRogzit_Click(object sender, RoutedEventArgs e)
        {
            //todo Ne lehessen rögzíteni, ha a következők valamelyike nem teljesül!
            // a) - A név legalább két szóból álljon és szavanként minimum 3 karakterből!
            //      Szó = A szöközökkel határolt karaktersorozat.
            // b) - A beírt dátum újabb, mint a mai dátum

            //todo A rögzítés mindig az aktuálisan megnyitott naplófájlba történjen!
            if (txtNev.Text != "" && datDatum.Text != "")
            {
                DateTime currentDateTime = DateTime.Now;
                DateTime selectedDateTime = datDatum.SelectedDate.Value;
                string[] nevek = txtNev.Text.Split(" ");
                if (nevek.Length > 1 && nevek[0].Length > 2 && nevek[1].Length > 2 && selectedDateTime < currentDateTime)
                {
                    //A CSV szerkezetű fájlba kerülő sor előállítása
                    string csvSor = $"{txtNev.Text};{datDatum.Text};{cboTantargy.Text};{sliJegy.Value};{nevek[0]}";
                    //Megnyitás hozzáfűzéses írása (APPEND)
                    StreamWriter sw = new StreamWriter(fajlNev, append: true);
                    sw.WriteLine(csvSor);
                    sw.Close();
                    //todo Az újonnan felvitt jegy is jelenjen meg a datagrid-ben!
                    double jegyekosszege = 0;
                    jegyek.Clear();
                    StreamReader sr = new StreamReader(fajlNev);
                    while (!sr.EndOfStream)
                    {
                        string[] mezok = sr.ReadLine().Split(";");
                        Osztalyzat ujJegy = new Osztalyzat(mezok[0], mezok[1], mezok[2], int.Parse(mezok[3]), mezok[4]);
                        jegyekosszege += double.Parse(mezok[3]);
                        jegyek.Add(ujJegy);
                    }
                    sr.Close();
                    dgJegyek.ItemsSource = jegyek;
                    lblJegyekSzama.Content = jegyek.Count;
                    lblAtlag.Content = Math.Round((jegyekosszege / jegyek.Count), 2);

                }
                else
                {
                    MessageBox.Show("nem jo");
                }
            }
            else
            {
                MessageBox.Show("nem jo");
            }

        }

        private void btnBetolt_Click(object sender, RoutedEventArgs e)
        {
            double jegyekosszege = 0;
            jegyek.Clear(); 
            StreamReader sr = new StreamReader(fajlNev);
            while (!sr.EndOfStream) 
            {
                string[] mezok = sr.ReadLine().Split(";");
                Osztalyzat ujJegy = new Osztalyzat(mezok[0], mezok[1], mezok[2], int.Parse(mezok[3]), mezok[4]);
                jegyekosszege += double.Parse(mezok[3]);
                jegyek.Add(ujJegy);
            }
            sr.Close(); 
            dgJegyek.ItemsSource = jegyek;
            lblJegyekSzama.Content = jegyek.Count;
            lblAtlag.Content = Math.Round((jegyekosszege / jegyek.Count), 2);
        }

        private void sliJegy_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            lblJegy.Content = sliJegy.Value; //Több alternatíva van e helyett! Legjobb a Data Binding!
        }

        //todo Felület bővítése: Az XAML átszerkesztésével biztosítsa, hogy láthatóak legyenek a következők!
        // - A naplófájl neve
        // - A naplóban lévő jegyek száma
        // - Az átlag

        //todo Új elemek frissítése: Figyeljen rá, ha új jegyet rögzít, akkor frissítse a jegyek számát és az átlagot is!

        //todo Helyezzen el alkalmas helyre 2 rádiónyomógombot!
        //Feliratok: [■] Vezetéknév->Keresztnév [O] Keresztnév->Vezetéknév
        //A táblázatban a név azserint szerepeljen, amit a rádiónyomógomb mutat!
        //A feladat megoldásához használja fel a ForditottNev metódust!
        //Módosíthatja az osztályban a Nev property hozzáférhetőségét!
        //Megjegyzés: Felételezzük, hogy csak 2 tagú nevek vannak
    }
}

