using HyperCore.Common;
using HyperCore.Data;
using HyperCore.IO;
using HyperCore.Utilities;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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

		public MainWindow()
		{
			InitializeComponent();
			deck = new Deck();
			gdDeck.DataContext = deck;
		}

		private void Button_Refresh(object sender, RoutedEventArgs e)
		{
			if (tdDownload != null && tdDownload.ThreadState != ThreadState.Stopped)
			{
				MessageBox.Show("A dwonloading thread is already running.");
				return;
			}

			//First load set list from local
			var sets = Database.LoadSets(DBPath);
			if (sets.ToList().Count == 0)
			{
				//If no sets are loaded, grab data from online and save to local
				sets = ParseSet.Parse();
				Database.Save(sets, DBPath);
			}

			var lbs = new List<CheckBox>();
			foreach (var set in Database.LoadSets(DBPath))
			{
				lbs.Add(new CheckBox() { Content = set });
			}
			lbSets.ItemsSource = lbs;

		}

		private void Button_Download(object sender, RoutedEventArgs e)
		{
			if (tdDownload!= null && tdDownload.ThreadState != ThreadState.Stopped)
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
				var data = Database.LoadCards(path);
				lvCards.ItemsSource = data;
			}
		}

		private void Button_Add(object sender, RoutedEventArgs e)
		{
			if (lvCards.SelectedValue != null)
			{
				var card = lvCards.SelectedValue as Card;
				if (!deck.MainBoard.ContainsKey(card))
				{
					deck.MainBoard.Add(card, 1);
				}
				else
				{
					deck.MainBoard[card] += 1;
				}
			}

		}

		private void Button_Export(object sender, RoutedEventArgs e)
		{
			if (deck.MainBoard.Count + deck.SideBoard.Count > 0)
			{
				new Extern(DBPath).Export(deck, "mydeck.deck", FILETYPE.Virtual_Play_Table);
			}

		}

		private void Button_Open(object sender, RoutedEventArgs e)
		{
			OpenFileDialog dlg = new OpenFileDialog();
			dlg.Filter = "Virtual Play Table(*.deck)|*.deck|All(*.*)|*.*";
			dlg.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
			dlg.RestoreDirectory = true;
			if (dlg.ShowDialog() == true)
			{
				var path = dlg.FileName;
				deck = new Extern(DBPath).Open(path);
				gdDeck.DataContext = deck;
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

				ParseCard.Parse(card, LANGUAGE.Chinese_Simplified);
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
				var result =MessageBox.Show("Abort and quit?", "Downloading is not finished!", MessageBoxButton.YesNo);
				if (result == MessageBoxResult.No)
				{
					e.Cancel = true;
				}
				
			}
			
		}

		private void Button_Add_Side(object sender, RoutedEventArgs e)
		{
			if (lvCards.SelectedValue != null)
			{
				var card = lvCards.SelectedValue as Card;
				if (!deck.SideBoard.ContainsKey(card))
				{
					deck.SideBoard.Add(card, 1);
				}
				else
				{
					deck.SideBoard[card] += 1;
				}
			}
		}

		private void lvCards_MouseMove(object sender, MouseEventArgs e)
		{
			if (e.LeftButton == MouseButtonState.Pressed)
			{
				Card card = (sender as ListViewItem).Content as Card;
			}
			
		}

	}
}
