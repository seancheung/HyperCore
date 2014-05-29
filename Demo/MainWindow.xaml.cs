using HyperCore.Common;
using HyperCore.Data;
using HyperCore.IO;
using HyperCore.Utilities;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace Demo
{

	public partial class MainWindow : Window
	{
		public static string DBPath = "data.hd";

		Deck deck;

		//Max thread amount for downloading
		private const int maxThread = 10;

		//Saving file lock
		private static object _lock = new object();

		//Dowanloading thread
		private Thread tdDownload;

		public LANGUAGE lang = LANGUAGE.Chinese_Simplified;

		public MainWindow()
		{
			InitializeComponent();
			deck = new Deck();
			gdDeck.DataContext = deck;
			if (!File.Exists(DBPath))
			{
				try
				{
					Database.Save(new List<string>(), DBPath);
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message);
				}
			}

		}

		private void Button_Refresh(object sender, RoutedEventArgs e)
		{
			if (tdDownload != null && tdDownload.ThreadState != ThreadState.Stopped)
			{
				MessageBox.Show("A dwonloading thread is already running.");
				return;
			}

			try
			{
				//First load set list from local
				var sets = Database.LoadSets(DBPath);
				if (sets.ToList().Count == 0)
				{
					//If no sets are loaded, grab data from online and save to local
					sets = ParseSet.Parse();
					Database.Save(sets, DBPath);
				}

				var lbs = new List<CheckBox>();
				var localSets = Database.LoadCards(DBPath).Select(c => c.SetCode);
				foreach (var set in Database.LoadSets(DBPath))
				{
					//Mark local database
					lbs.Add(new CheckBox()
					{
						Content = set,
						Foreground = localSets.Contains(set.SplitSetName()[1]) ?
									 new SolidColorBrush(Colors.DodgerBlue) : new SolidColorBrush(Colors.Black)
					});
				}
				lbSets.ItemsSource = lbs;

				var formats = Database.LoadFormats(DBPath);
				combFormat.ItemsSource = formats;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}

		}

		private void Button_Download(object sender, RoutedEventArgs e)
		{
			if (tdDownload != null && tdDownload.ThreadState != ThreadState.Stopped)
			{
				MessageBox.Show("A dwonloading thread is already running.");
				return;
			}


			List<string> sets = new List<string>();

			foreach (CheckBox item in lbSets.Items)
			{
				if (item.IsChecked == true)
				{
					sets.Add(item.Content.ToString());
				}

			}

			if (sets.Count == 0)
			{
				return;
			}
			tdDownload =
				new Thread(() =>
			{
				try
				{
					foreach (var s in sets)
					{
						Dispatcher.Invoke((Action)delegate
						{
							progTxt.Text = String.Format("Fetching ID list {0}", s);
							progBar.Value = 0;
						});

						var cards = ParseCard.GetCards(s.SplitSetName()[0], s.SplitSetName()[1]).ToList();

						var n = cards.Count;

						Dispatcher.Invoke((Action)delegate
						{
							progBar.Maximum = n;
						});

						var tps = new List<List<Card>>();
						for (int i = 0; i < maxThread - 1; i++)
						{
							tps.Add(cards.GetRange(n / maxThread * i, n / maxThread));
						}
						tps.Add(cards.GetRange(n / maxThread * (maxThread - 1), n / maxThread + n % maxThread));

						WaitCallback waitCallback = new WaitCallback(DownloadCards);
						WaitHandle[] waitHandles = new WaitHandle[maxThread];

						for (int i = 0; i < maxThread; i++)
						{
							waitHandles[i] = new AutoResetEvent(false);
							ThreadPool.QueueUserWorkItem(waitCallback, new object[] { tps[i], waitHandles[i] });
						}

						WaitHandle.WaitAll(waitHandles);
					}

					Dispatcher.Invoke((Action)delegate
					{
						progTxt.Text = "Downloading complete!";
						progBar.Value = 0;
					});
				}
				catch (Exception ex)
				{
					Dispatcher.Invoke((Action)delegate
					{
						MessageBox.Show(ex.Message);
					});
				}
			});

			tdDownload.Start();

		}

		private void Button_Load(object sender, RoutedEventArgs e)
		{
			OpenFileDialog dlg = new OpenFileDialog();
			dlg.Filter = "Database(*.hd)|*.hd|All(*.*)|*.*";
			dlg.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
			dlg.RestoreDirectory = true;
			if (dlg.ShowDialog() == true)
			{
				var path = dlg.FileName;
				try
				{
					var data = Database.LoadCards(path);
					lvCards.ItemsSource = data;
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message);
				}
			}
		}

		private void Button_Export(object sender, RoutedEventArgs e)
		{
			if (deck.MainBoard.Count + deck.SideBoard.Count > 0)
			{
				deck.Comment = new TextRange(rtCmnt.Document.ContentStart, rtCmnt.Document.ContentEnd).Text;
				deck.Comment += DateTime.Now;
				
				SaveFileDialog dlg = new SaveFileDialog();
				dlg.Filter = "HyperDeck(*.hdeck)|*.hdeck|Virtual Playtable(*.deck)|*.deck|Magic Workstation(*.mwDeck)|*.mwDeck|Mage(*.txt)|*.txt|MTGO(*.txt)|*.txt";
				dlg.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
				dlg.RestoreDirectory = true;
				if (dlg.ShowDialog() == true)
				{
					var path = dlg.FileName;
					deck.Name = dlg.SafeFileName;
					deck.Name = deck.Name.Remove(deck.Name.LastIndexOf("."));
					bool status = false;

					try
					{
						switch (dlg.FilterIndex)
						{
							case 1:

								break;
							case 2:
								new Extern(DBPath).Export(deck, path, FILETYPE.Virtual_Play_Table);
								status = true;
								break;
							case 3:
								new Extern(DBPath).Export(deck, path, FILETYPE.Magic_Workstation);
								status = true;
								break;
							case 4:
								new Extern(DBPath).Export(deck, path, FILETYPE.Mage);
								status = true;
								break;
							case 5:
								new Extern(DBPath).Export(deck, path, FILETYPE.Magic_Online);
								status = true;
								break;
							default:

								break;
						}

						if (status)
						{
							MessageBox.Show("Successfully saved");
						}
					}
					catch (Exception ex)
					{
						MessageBox.Show(ex.Message);
					}
					
				}
			}

		}

		private void Button_Open(object sender, RoutedEventArgs e)
		{
			OpenFileDialog dlg = new OpenFileDialog();
			dlg.Filter = "HyperDeck(*.hdeck)|*.hdeck|Virtual Playtable(*.deck)|*.deck|Magic Workstation(*.mwDeck)|*.mwDeck|Mage(*.txt)|*.txt|MTGO(*.txt)|*.txt|All(*.*)|*.*";
			dlg.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
			dlg.RestoreDirectory = true;
			if (dlg.ShowDialog() == true)
			{
				try
				{
					var path = dlg.FileName;
					deck = new Extern(DBPath).Open(path);
					gdDeck.DataContext = deck;
					rtCmnt.Document = new FlowDocument();
					rtCmnt.Document.Blocks.Add(new Paragraph(new Run(deck.Comment)));
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message);
				}
			}
		}

		private void DownloadCards(object obj)
		{
			object[] o = obj as object[];
			List<Card> cards = o[0] as List<Card>;
			AutoResetEvent waitHandle = (AutoResetEvent)o[1];

			foreach (var card in cards)
			{
				Dispatcher.Invoke((Action)delegate
				{
					progTxt.Text = String.Format("Now processing {0}:{1}", card.SetCode, card.ID);
					progBar.Value++;
				});

				ParseCard.Parse(card, lang);
			}

			lock (_lock)
			{
				//Save Data
				Database.Save(cards, DBPath);
			}

			//Set the current thread state as finished
			waitHandle.Set();
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (tdDownload != null && tdDownload.ThreadState != ThreadState.Stopped)
			{
				var result = MessageBox.Show("Abort and quit?", "Downloading is not finished!", MessageBoxButton.YesNo);
				if (result == MessageBoxResult.No)
				{
					e.Cancel = true;
				}
				tdDownload.Abort();
			}

		}

		private void Button_Formats(object sender, RoutedEventArgs e)
		{
			try
			{
				var formats = ParseFormat.Parse().ToList();
				Database.Save(formats, DBPath);
				MessageBox.Show("Successfuly updated formats");
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		private void combFormat_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if ((combFormat.SelectedValue as Format).Type == FORMAT.Block)
			{
				comboFormatSub.Visibility = Visibility.Visible;
				var sets = (combFormat.SelectedValue as Format).LegalSets;
				List<string> blockNames = new List<string>();
				sets.ForEach(s => blockNames.Add(s.SplitSetName()[0].Trim()));
				comboFormatSub.ItemsSource = blockNames;
			}
			else
			{
				comboFormatSub.Visibility = Visibility.Collapsed;

				var sets = (combFormat.SelectedValue as Format).LegalSets;
				if (sets.Count == 0)
				{
					foreach (CheckBox item in lbSets.Items)
					{
						item.IsChecked = true;
					}
				}
				else
				{
					var setNames = new List<string>();
					sets.ForEach(s => setNames.Add(s.SplitSetName()[0].Trim().ToLower()));

					foreach (CheckBox item in lbSets.Items)
					{
						if (setNames.Contains(item.Content.ToString().SplitSetName()[0].Trim().ToLower()))
						{
							item.IsChecked = true;
						}
						else
						{
							item.IsChecked = false;
						}
					}
				}
			}
		}

		private void comboFormatSub_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var block = (combFormat.SelectedValue as Format).LegalSets.Find(s => s.StartsWith(comboFormatSub.SelectedValue.ToString())).ToLower();
			if (block != null)
			{
				var sets = block.SplitSetName()[1].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
				for (int i = 0; i < sets.Length; i++)
				{
					if (sets[i].Contains("["))
					{
						sets[i] = sets[i].Remove(sets[i].IndexOf("["));
					}
					sets[i] = sets[i].Trim();
				}

				foreach (CheckBox item in lbSets.Items)
				{
					if (sets.Contains(item.Content.ToString().SplitSetName()[0].Trim().ToLower()))
					{
						item.IsChecked = true;
					}
					else
					{
						item.IsChecked = false;
					}
				}
			}
		}

		private void ListView_Drop_Main(object sender, DragEventArgs e)
		{
			
			var card = e.Data.GetData(typeof(Card)) as Card;
			if (card != null)
			{
				if (deck.MainBoard.ContainsKey(card))
				{
					deck.MainBoard[card] += 1;
				}
				else
				{
					deck.MainBoard.Add(card, 1);
				}

			}
			else
			{
				var data = e.Data.GetData(typeof(KeyValuePair<Card, int>));
				if (data != null)
				{
					card = ((KeyValuePair<Card, int>)data).Key;
					if (card != null)
					{
						if (deck.MainBoard.ContainsKey(card))
						{
							deck.MainBoard[card] += 1;
						}
						else
						{
							deck.MainBoard.Add(card, 1);
						}

					}
				}

			}
		}

		private void ListView_Drop_Side(object sender, DragEventArgs e)
		{
			var card = e.Data.GetData(typeof(Card)) as Card;
			if (card != null)
			{
				if (deck.SideBoard.ContainsKey(card))
				{
					deck.SideBoard[card] += 1;
				}
				else
				{
					deck.SideBoard.Add(card, 1);
				}
			}
			else
			{
				var data = e.Data.GetData(typeof(KeyValuePair<Card, int>));
				if (data != null)
				{
					card = ((KeyValuePair<Card, int>)data).Key;
					if (card != null)
					{
						if (deck.SideBoard.ContainsKey(card))
						{
							deck.SideBoard[card] += 1;
						}
						else
						{
							deck.SideBoard.Add(card, 1);
						}

					}
				}

			}
		}

		private void ListView_DragLeave_Main(object sender, DragEventArgs e)
		{
			var data = e.Data.GetData(typeof(KeyValuePair<Card, int>));
			if (data != null)
			{
				var card = ((KeyValuePair<Card, int>)data).Key;
				if (deck.MainBoard.ContainsKey(card))
				{
					deck.MainBoard[card] -= 1;
					if (deck.MainBoard[card] < 1)
					{
						deck.MainBoard.Remove(card);
					}
				}
			}
		}

		private void ListView_DragLeave_Side(object sender, DragEventArgs e)
		{
			var data = e.Data.GetData(typeof(KeyValuePair<Card, int>));
			if (data != null)
			{
				var card = ((KeyValuePair<Card, int>)data).Key;
				if (deck.SideBoard.ContainsKey(card))
				{
					deck.SideBoard[card] -= 1;
					if (deck.SideBoard[card] < 1)
					{
						deck.SideBoard.Remove(card);
					}
				}
			}

		}

		private void ListviewItem_MouseMove(object sender, MouseEventArgs e)
		{
			if (e.LeftButton == MouseButtonState.Pressed)
			{
				var data = (sender as ListViewItem).Content;
				if (data != null)
				{
					DragDrop.DoDragDrop(sender as ListViewItem, data, DragDropEffects.Copy);
				}

			}
		}

		private void ListviewItem_MouseMove_Main(object sender, MouseEventArgs e)
		{
			if (e.LeftButton == MouseButtonState.Pressed)
			{
				var data = (sender as ListViewItem).Content;
				if (data != null)
				{
					DragDrop.DoDragDrop(sender as ListViewItem, data, DragDropEffects.Move);

					var card = ((KeyValuePair<Card, int>)data).Key;
					if (deck.MainBoard.ContainsKey(card))
					{
						deck.MainBoard[card] -= 1;
						if (deck.MainBoard[card] < 1)
						{
							deck.MainBoard.Remove(card);
						}
					}
				}

			}
		}

		private void ListviewItem_MouseMove_Side(object sender, MouseEventArgs e)
		{
			if (e.LeftButton == MouseButtonState.Pressed)
			{
				var data = (sender as ListViewItem).Content;
				if (data != null)
				{
					DragDrop.DoDragDrop(sender as ListViewItem, data, DragDropEffects.Move);

					var card = ((KeyValuePair<Card, int>)data).Key;
					if (deck.SideBoard.ContainsKey(card))
					{
						deck.SideBoard[card] -= 1;
						if (deck.SideBoard[card] < 1)
						{
							deck.SideBoard.Remove(card);
						}
					}
				}

			}
		}

		private void Button_ForceUpdate(object sender, RoutedEventArgs e)
		{
			var card = lvCards.SelectedValue;
			if (card!=null)
			{
				try
				{
					Card newCard = card as Card;
					if (newCard != null)
					{
						ParseCard.Parse(newCard, lang);
					}

					Database.Save(new Card[] { newCard }, DBPath);
					MessageBox.Show("Updating complete, please reload data");
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message);
				}
			}
			
		}

	}
}
