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
using System.Windows.Threading;


namespace DeckStats {
	/// <summary>
	/// Interaction logic for GameControl.xaml
	/// </summary>
	public partial class GameControl : UserControl {
private DispatcherTimer timer;
		private int timeCounter = 0;
		private bool gameStarted = false;
		private bool mulligan = false;
		private int turns = 0;
		Deck deck;
		MainWindow mw;

		public GameControl(Deck deck, MainWindow main) {
			InitializeComponent();
			this.deck = deck;
			mw = main;
			NameLabel.Content = deck.Name;
			timer = new DispatcherTimer();
			timer.Interval = new TimeSpan(0,0,1);
			timer.Tick += timer_Elapsed;
		}

		private void timer_Elapsed(object sender, EventArgs e) {
			++timeCounter;
			ChangeTime(timeCounter);
		}

		private void StartGameButton_Click(object sender, RoutedEventArgs e) {
			timer.Start();
			gameStarted = true;
		}

		public void ChangeTime(int time) {
			int minutes = time / 60;
			int seconds = time % 60;
			TimeLabel.Content = "Time elapsed: " + minutes + "mins, "+seconds+" sec";
		}

		private void MatchOverButton_Click(object sender, RoutedEventArgs e) {
			if (gameStarted) {
				timer.Stop();

				string sMessageBoxText = "Did you win?";
				string sCaption = "Win/Loose";

				MessageBoxButton btnMessageBox = MessageBoxButton.YesNo;
				MessageBoxImage icnMessageBox = MessageBoxImage.Exclamation;

				MessageBoxResult rsltMessageBox = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);

				switch (rsltMessageBox) {
					case MessageBoxResult.Yes:
						deck.gamePlayed(true, timeCounter, turns, mulligan);
						break;

					case MessageBoxResult.No:
						deck.gamePlayed(false, timeCounter, turns, mulligan);
						break;
				}
				EndTypeWindow ew = new EndTypeWindow(deck, this);
				ew.Show();
			}
		}

		public void EndGame() {
			mw.SaveDeck(deck);
			mw.Reset();
		}

		private void NoteButton_Click(object sender, RoutedEventArgs e) {
			if(CommentBox.Text != ""){
				deck.AddComment(CommentBox.Text);
			}
			CommentBox.Text = "";
		}

		private void MulliganButton_Click(object sender, RoutedEventArgs e) {
			if (mulligan == false) {
				MessageBoxButton btnMessageBox = MessageBoxButton.YesNo;
				MessageBoxImage icnMessageBox = MessageBoxImage.Exclamation;

				MessageBoxResult rsltMessageBox = MessageBox.Show("Did you take a Mulligan?", "Mulligan", btnMessageBox, icnMessageBox);

				switch (rsltMessageBox) {
					case MessageBoxResult.Yes:
						mulligan = true;
						break;

					case MessageBoxResult.No:
						break;
				}
			}
		}

		private void TurnButton_Copy_Click(object sender, RoutedEventArgs e) {
			turns++;
		}
	}
}
