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
		private static string DBPath = "data.hd";
		public MainWindow()
		{
			InitializeComponent();
		}

		private void ClickRefresh(object sender, RoutedEventArgs e)
		{
			//var sets = ParseSet.Parse();
			//lbSets.ItemsSource = sets;
			lbSets.ItemsSource = Database.LoadSets(DBPath);
			//Database.Save(sets, DBPath);
			//Extern.VPT.Open("m14.deck");
		}

		private void ClickDownload(object sender, RoutedEventArgs e)
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

		private void ClickSave(object sender, RoutedEventArgs e)
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

		private void ClickOpen(object sender, RoutedEventArgs e)
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
	}
}
