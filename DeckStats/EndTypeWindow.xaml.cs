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
	/// Interaction logic for EndTypeWindow.xaml
	/// </summary>
	public partial class EndTypeWindow : Window {

		Game game;
		bool corp;
		Deck deck;

		public EndTypeWindow(Deck d, Game g, bool c) {
			InitializeComponent();
			game = g;
			corp = c;
			deck = d;
			if (game.won) {
				Label.Content = "How did you win?";
			}else{
				Label.Content = "How did you loose?";
			}
			if (corp) {
				if (game.won) {
					Condition1.Content = "Agenda";
					Condition2.Content = "Flatline";
				} else {
					Condition1.Content = "Agenda";
					Condition2.Content = "Decking";
				}
			} else {
				if (game.won) {
					Condition1.Content = "Agenda";
					Condition2.Content = "Decking";
				} else {
					Condition1.Content = "Agenda";
					Condition2.Content = "Flatline";
				}
			}
		}

		private void Condition1_Click(object sender, RoutedEventArgs e) {
				if (game.won) {
					game.endType = GameEndType.AgendaWin;
				} else {
					game.endType = GameEndType.AgendaLoss;
				}
				deck.Save();
				this.Close();
		}

		private void Condition2_Click(object sender, RoutedEventArgs e) {
			if (corp) {
				if (game.won) {
					game.endType = GameEndType.Flatline;
				} else {
					game.endType = GameEndType.Decking;
				}
			} else {
				if (game.won) {
					game.endType = GameEndType.Decking;
				} else {
					game.endType = GameEndType.Flatline;
				}
			}
			deck.Save();
			this.Close();
		}
	}
}
