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

namespace DeckStats {
	/// <summary>
	/// Interaction logic for MainControl.xaml
	/// </summary>
	public partial class MainControl : UserControl {
		MainWindow mw;
		public MainControl(MainWindow main) {
			InitializeComponent();
			mw = main;
		}

		private void Start_Click(object sender, RoutedEventArgs e) {
			mw.StartGame();
		}

		private void Load_Click(object sender, RoutedEventArgs e) {
			mw.ImportDeck();
		}

		private void Stats_Click(object sender, RoutedEventArgs e) {
			mw.DeckStats();
		}

		private void Exit_Click(object sender, RoutedEventArgs e) {
			mw.Exit();
		}

		private void Update_Click(object sender, RoutedEventArgs e) {
			mw.UpdateDeck();
		}

		private void Remove_Click(object sender, RoutedEventArgs e) {
			mw.RemoveDeck();
		}
	}
}
