using HyperCore.Common;
using HyperCore.Data;
using HyperCore.IO;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using Microsoft.Win32;
using HyperCore.Utilities;

namespace Demo
{

	public partial class MainWindow : Window
	{
		public static string DBPath = "data.hd";

		Deck deck = new Deck();

		public MainWindow()
		{
			InitializeComponent();
			gdDeck.DataContext = deck;
		}

		private void Button_Refresh(object sender, RoutedEventArgs e)
		{
			//var sets = ParseSet.Parse();
			//lbSets.ItemsSource = sets;
			//lbSets.ItemsSource = Database.LoadSets(DBPath);
			//Database.Save(sets, DBPath);
			//Extern.VPT.Open("m14.deck");
		}

		private void Button_Download(object sender, RoutedEventArgs e)
		{
			if (lbSets.SelectedValue != null)
			{
				var set = lbSets.SelectedValue.ToString().SplitSetName();
				var cards = ParseCard.GetCards(set[0], set[1]);
				foreach (var card in cards)
				{
					ParseCard.Parse(card, LANGUAGE.English);
				}
			}

		}

		private void Button_Save(object sender, RoutedEventArgs e)
		{
			if (lvCards.ItemsSource != null)
			{

				SaveFileDialog dlg = new SaveFileDialog();
				dlg.Filter = "Database(*.hd)|*.hd|All(*.*)|*.*";
				dlg.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
				dlg.RestoreDirectory = true;
				if (dlg.ShowDialog() == true)
				{
					var path = dlg.FileName;
					var data = lvCards.ItemsSource as IEnumerable<Card>;
					Database.Save(data, path);
				}
			}

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
	}
}
