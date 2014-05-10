using HyperCore.Common;
using HyperCore.Data;
using HyperCore.IO;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;

namespace Demo
{
	/// <summary>
	/// MainWindow.xaml 的交互逻辑
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			var sets = ParseSet.Parse();
			lbSets.ItemsSource = sets;
		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			if (lbSets.SelectedValue != null)
			{
				var set = lbSets.SelectedValue.ToString().Split(new char[] { '(', ')' }, StringSplitOptions.RemoveEmptyEntries);
				var cards = ParseCard.GetCards(set[0], set[1]);
				foreach (var card in cards)
				{
					ParseCard.Parse(card,LANGUAGE.English);
				}
			}
			
		}

		private void Button_Click_2(object sender, RoutedEventArgs e)
		{
			if (lvCards.ItemsSource != null)
			{
				var path = String.Format("data{0}.xml", System.DateTime.Now.ToString("-yy-MM-dd-HH-mm-ss"));
				var data = lvCards.ItemsSource as IEnumerable<Card>;
				var newdata = new List<Card>(data);
				Database.Save(newdata, "data.xml");
			}
			
		}

		private void Button_Click_3(object sender, RoutedEventArgs e)
		{
			var path = "data.xml";
			var data = Database.Load(path);
			lvCards.ItemsSource = data;
			
		}
	}
}
