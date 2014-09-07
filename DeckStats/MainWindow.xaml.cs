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

using System.Xml;
using System.IO;

using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;

using Microsoft.Win32;

namespace DeckStats {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {
		StringBuilder output = new StringBuilder();

		enum states{
			startGame,
			checkStats,
			updateDeck
		};

		states state = states.startGame;

		MainControl mc;

		string DeckPath = @"Decks//";
		const string ending = ".json";
		private Dictionary<string,Deck> decks;
		private List<string> deckNameList;

		public void Select(string deckName) {
			if (state == states.startGame) {
				startGame(deckName);
			} else if (state == states.checkStats) {
				showStats(deckName);
			}else if(state == states.updateDeck){
				MakeUpdate(deckName);
			}
		}

		public void startGame(string deckName) {
			Deck deck = decks[deckName];
			this.Content = new GameControl(deck, this);
		}

		private void showStats(string deckName) {
			Deck deck = decks[deckName];
			this.Content = new StatsControl(deck, this);
		}



		public MainWindow() {
			InitializeComponent();
			this.ResizeMode = System.Windows.ResizeMode.NoResize;
			mc = new MainControl(this);
			this.Content = mc;
			decks = new Dictionary<string, Deck>();
			deckNameList = new List<string>();

			if (File.Exists(DeckPath + "deckList.json")) {
				deckNameList = JsonConvert.DeserializeObject<List<string>>(File.ReadAllText(DeckPath + "deckList.json"));
				foreach (var file in deckNameList) {
					Deck deck = JsonConvert.DeserializeObject<Deck>(File.ReadAllText(DeckPath + file + ending));
					decks.Add(file, deck);
				}
			}
		}

		public void Reset() {
			this.Content = mc;
		}

		public void StartGame() {
			state = states.startGame;
			SelectDeckWindow win = new SelectDeckWindow(decks, this);
			win.Show();
		}

		public void ImportDeck() {
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Filter = "OCTGN Deck files (.o8d)|*.o8d";
			ofd.FilterIndex = 0;
			ofd.Multiselect = true;

			bool? userClickedOK = ofd.ShowDialog();


			if (userClickedOK == true) {

				bool isExists = System.IO.Directory.Exists("Decks");

				if (!isExists)
					System.IO.Directory.CreateDirectory("Decks");

				// Open the selected file to read.
				foreach (string filename in ofd.FileNames) {
					string deckName = System.IO.Path.GetFileName(filename);
					deckName = deckName.Remove(deckName.IndexOf('.'));
					Deck deck = new Deck(deckName);
					deckNameList.Add(deckName);

					System.IO.Stream fileStream = ofd.OpenFile();



					using (XmlReader reader = XmlReader.Create(fileStream)) {
						// Parse the file and display each of the nodes.
						bool id = false;
						while (reader.Read()) {

							switch (reader.Name) {
								case "section":
									switch (reader.GetAttribute("name")) {
										case "Identity":
											id = true;
											break;
										case "R&D / Stack":
											output.Append("Deck:\n");
											break;
									}
									break;
								case "card":
									if (reader.AttributeCount > 0) {
										if (id == false) {
											deck.AddCard(Convert.ToInt32(reader.GetAttribute("qty")), reader.ReadElementContentAsString());
										} else {
											deck.Identity = (reader.ReadElementContentAsString());
											id = false;
										}

									}
									break;
							}
						}
					}

					string sMessageBoxText = "IS this a Corp deck?";
					string sCaption = "Corp or Runner?";

					MessageBoxButton btnMessageBox = MessageBoxButton.YesNo;
					MessageBoxImage icnMessageBox = MessageBoxImage.Exclamation;

					MessageBoxResult rsltMessageBox = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);

					switch (rsltMessageBox) {
						case MessageBoxResult.Yes:
							deck.Corp = true;
							break;

						case MessageBoxResult.No:
							deck.Corp = false;
							break;
					}

					decks.Add(deckName, deck);
					SaveDeck(deck);
					fileStream.Close();
				}
			}			
		}

		public void MakeUpdate(string dName) {
			//MessageBox.Show("Feature not implemented yet");

			OpenFileDialog ofd = new OpenFileDialog();
			ofd.Filter = "OCTGN Deck files (.o8d)|*.o8d";
			ofd.FilterIndex = 0;
			ofd.Multiselect = false;

			bool? userClickedOK = ofd.ShowDialog();


			if (userClickedOK == true) {

				bool isExists = System.IO.Directory.Exists("Decks");

				if (!isExists)
					System.IO.Directory.CreateDirectory("Decks");

				// Open the selected file to read.
				string deckName = System.IO.Path.GetFileName(ofd.FileName);
				deckName = deckName.Remove(deckName.IndexOf('.'));
				Deck deck = new Deck(deckName);

				System.IO.Stream fileStream = ofd.OpenFile();

				using (XmlReader reader = XmlReader.Create(fileStream)) {
					// Parse the file and display each of the nodes.
					bool id = false;
					while (reader.Read()) {

						switch (reader.Name) {
							case "section":
								switch (reader.GetAttribute("name")) {
									case "Identity":
										id = true;
										break;
									case "R&D / Stack":
										output.Append("Deck:\n");
										break;
								}
								break;
							case "card":
								if (reader.AttributeCount > 0) {
									if (id == false) {
										deck.AddCard(Convert.ToInt32(reader.GetAttribute("qty")), reader.ReadElementContentAsString());
									} else {
										deck.Identity = (reader.ReadElementContentAsString());
										id = false;
									}
								}
								break;
						}
					}
				}
				decks[dName].Update(deck);
			}
		}

		public void SaveDeck(Deck deck) {
			string json = JsonConvert.SerializeObject(deck, Newtonsoft.Json.Formatting.Indented);
			File.WriteAllText(DeckPath + deck.Name + ".json", json);
			File.WriteAllText(DeckPath + "deckList.json", JsonConvert.SerializeObject(deckNameList, Newtonsoft.Json.Formatting.Indented));
		}

		public void Exit() {
			foreach (KeyValuePair<string,Deck> deck in decks){
				SaveDeck(deck.Value);
			}
			Environment.Exit(0);
		}

		public void DeckStats() {
			state = states.checkStats;
			SelectDeckWindow win = new SelectDeckWindow(decks, this);
			win.Show();
		}

		public void UpdateDeck() {
			state = states.updateDeck;
			SelectDeckWindow win = new SelectDeckWindow(decks, this);
			win.Show();
		}

	}
}
