using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HyperCore.Data;
using HyperCore.IO;

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

			}
			
		}

		private void Button_Click_2(object sender, RoutedEventArgs e)
		{
			if (lvCards.ItemsSource != null)
			{
				var path = String.Format("data{0}.xml", System.DateTime.Now.ToString("-yy-MM-dd-HH-mm-ss"));
				Database.Save(lvCards.ItemsSource as IEnumerable<HyperCore.Common.Card>, path);
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
