using HyperCore.IO;
using System.Windows.Controls;

namespace HyperMTG
{
	/// <summary>
	/// Interaction logic for DeckPage.xaml
	/// </summary>
	public partial class DeckPage : Page
	{
		public DeckPage()
		{
			InitializeComponent();
			LoadAppData();
		}

		private void LoadAppData()
		{
			var path = "AppData.dat";
			treeViewColor.ItemsSource = AppData.Instance.Load("colors", path);
			treeViewFormat.ItemsSource = AppData.Instance.Load("formats", path);
			treeViewRarity.ItemsSource = AppData.Instance.Load("rarities", path);
			treeViewType.ItemsSource = AppData.Instance.Load("types", path);
		}
	}
}
