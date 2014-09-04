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
		public StatsControl(Deck deck, MainWindow main) {
			InitializeComponent();
			mw = main;
			NameLabel.Content = deck.Name;

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

			gamesPlayed = deck.games.Count();

			foreach (Game game in deck.games) {
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

			avgWinMinutes /= gamesWon;
			avgTurnsWin /= gamesWon;
			if (gamesPlayed != gamesWon) {
				avgLossMinutes /= (gamesPlayed - gamesWon);
				avgTurnsLoss /= (gamesPlayed - gamesWon);
			} else {
				avgLossMinutes = 0;
				avgTurnsLoss = 0;
			}
			
			


			GamesPlayedLabel.Content = gamesPlayed;
			GamesWonLabel.Content = gamesWon;
			
			if (deck.Corp) {
				DeckingLabel.Content = "Loss by Decking";
				FlatlineLabel.Content = "Won by Flatlining";
			} else {
				DeckingLabel.Content = "Won by Decking";
				FlatlineLabel.Content = "Loss by Flatlining";
			}
			AgendaWinCountLabel.Content = agendaWins;
			AgendaLossCountLabel.Content = agendaLoss;
			FlatlineCountLabel.Content = flatline;
			DeckingCountLabel.Content = decking;
			MulliganCountLabel.Content = mulligans;
			TimeWinCountLabel.Content = avgWinMinutes;
			TimeLossCountLabel.Content = avgLossMinutes;
			TurnsWinCountLabel.Content = avgTurnsWin;
			TurnsLossCountLabel.Content = avgTurnsLoss;
		}

		private void BackButton_Click(object sender, RoutedEventArgs e) {
			mw.Reset();
		}
	}
}
