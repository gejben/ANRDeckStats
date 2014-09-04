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
using System.Windows.Shapes;

namespace DeckStats {
	/// <summary>
	/// Interaction logic for StartGameWindow.xaml
	/// </summary>
	/// 
	public partial class SelectDeckWindow : Window {

		private Dictionary<string, Deck> decks;
		MainWindow mainWin;

		public SelectDeckWindow(Dictionary<string, Deck> decks, MainWindow main) {
			this.decks = decks;
			InitializeComponent();
			DeckSelector.ItemsSource = decks.Keys;
			mainWin = main;
			DeckSelector.SelectedIndex = 0;
		}

		private void DeckSelected_Click(object sender, RoutedEventArgs e) {
			mainWin.Select(DeckSelector.SelectedItem.ToString());
			this.Close();
		}

	}
}
