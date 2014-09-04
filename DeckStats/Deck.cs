using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeckStats {
	public class Deck {

		public Deck(string name) {
			Name = name;
			cards = new List<Card>();
			comments = new List<string>();
			games = new List<Game>();
		}

		public string Name { get; set; }
		public string Identity { get; set; }
		public bool Corp { get; set; }

		public List<Card> cards { get; set; }
		public List<Game> games { get; set; }
		public List<string> comments{get;set;}


		public void AddComment(string comment) {
			comments.Add(comment);
		}

		public void AddCard(int number,string name) {
			Card card = new Card();
			card.Count = number;
			card.Name = name;

			cards.Add(card);
		}

		public void gamePlayed(bool won, int seconds, int turns, bool mulligan) {
			Game game = new Game();
			game.won = won;
			game.playTimeMinutes = seconds / 60;
			game.turns = turns;
			game.mulligan = mulligan;
			EndTypeWindow ew = new EndTypeWindow(game, Corp);
			ew.Show();
			games.Add(game);
		}


	}
}
