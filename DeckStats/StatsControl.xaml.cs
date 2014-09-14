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
	/// Interaction logic for StatsControl.xaml
	/// </summary>
	public partial class StatsControl : UserControl {

		MainWindow mw;
		private Deck deck;
		List<string> versions;
		public StatsControl(Deck deck, MainWindow main) {
			InitializeComponent();
			this.deck = deck;
			mw = main;
			NameLabel.Content = deck.Name;
			versions = new List<string>();
			for (int i = 1; i <= deck.Version; i++) {
				versions.Add("v."+i);
			}
			VersionBox.ItemsSource = versions;
			VersionBox.SelectedIndex = versions.Count-1;

			SetupView();
		}

		private void SetupView() {
			int gamesPlayed = 0;
			int gamesWon = 0;

			int agendaWins = 0;
			int agendaLoss = 0;
			int flatline = 0;
			int decking = 0;

			int mulligans = 0;
			int avgWinMinutes = 0;
			int avgLossMinutes = 0;

			int avgTurnsWin = 0;
			int avgTurnsLoss = 0;

			gamesPlayed = deck.GetGames(VersionBox.SelectedIndex).Count();

			if (gamesPlayed > 0) {
				foreach (Game game in deck.GetGames(VersionBox.SelectedIndex)) {
					if (game.won) gamesWon++;
					switch (game.endType) {
						case GameEndType.AgendaWin: ++agendaWins;
							break;
						case GameEndType.AgendaLoss: ++agendaLoss;
							break;
						case GameEndType.Decking: ++decking;
							break;
						case GameEndType.Flatline: ++flatline;
							break;
					}
					if (game.mulligan) mulligans++;
					if (game.won) {
						avgWinMinutes += game.playTimeMinutes;
						avgTurnsWin += game.turns;
					} else {
						avgLossMinutes += game.playTimeMinutes;
						avgTurnsLoss += game.turns;
					}
				}
				if (gamesWon > 0) {
					avgWinMinutes /= gamesWon;
					avgTurnsWin /= gamesWon;
				}
				if (gamesPlayed != gamesWon) {
					avgLossMinutes /= (gamesPlayed - gamesWon);
					avgTurnsLoss /= (gamesPlayed - gamesWon);
				} else {
					avgLossMinutes = 0;
					avgTurnsLoss = 0;
				}

				GamesPlayedLabel.Content = gamesPlayed;
				GamesWonLabel.Content = (int)(((float)gamesWon / (float)gamesPlayed) * 100) + "%"; ;

				if (deck.Corp) {
					DeckingLabel.Content = "Loss by Decking";
					FlatlineLabel.Content = "Won by Flatlining";
				} else {
					DeckingLabel.Content = "Won by Decking";
					FlatlineLabel.Content = "Loss by Flatlining";
				}
			}
			if (gamesPlayed > 0) {
				AgendaWinCountLabel.Content = (int)(((float)agendaWins / (float)gamesPlayed) * 100) + "%";
				AgendaLossCountLabel.Content = (int)(((float)agendaLoss / (float)gamesPlayed) * 100) + "%";
				FlatlineCountLabel.Content = (int)(((float)flatline / (float)gamesPlayed) * 100) + "%";
				DeckingCountLabel.Content = (int)(((float)decking / (float)gamesPlayed) * 100) + "%";
				MulliganCountLabel.Content = (int)(((float)mulligans / (float)gamesPlayed) * 100) + "%";
				TimeWinCountLabel.Content = avgWinMinutes + " min";
				TimeLossCountLabel.Content = avgLossMinutes + " min";
			} else {
				AgendaWinCountLabel.Content = 0 + "%";
				AgendaLossCountLabel.Content = 0 + "%";
				FlatlineCountLabel.Content = 0 + "%";
				DeckingCountLabel.Content = 0 + "%";
				MulliganCountLabel.Content = 0 + "%";
				TimeWinCountLabel.Content = 0 + " min";
				TimeLossCountLabel.Content = 0 + " min";
			}
			if (avgTurnsLoss > 0 || avgTurnsWin > 0) {
				TurnsWinCountLabel.Content = avgTurnsWin;
				TurnsLossCountLabel.Content = avgTurnsLoss;
			} else {
				TurnsWinLabel.Visibility = System.Windows.Visibility.Hidden;
				TurnsLossLabel.Visibility = System.Windows.Visibility.Hidden;
				TurnsWinCountLabel.Visibility = System.Windows.Visibility.Hidden;
				TurnsLossCountLabel.Visibility = System.Windows.Visibility.Hidden;
			}
		}

		private void BackButton_Click(object sender, RoutedEventArgs e) {
			mw.Reset();
		}

		private void VersionBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
			SetupView();
		}
	}
}
