using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace HyperMTG
{

	public partial class Index : Page
	{
		DeckPage deckPage;
		DatabasePage databasePage;
		GalleryPage galleryPage;
		ConfigPage configPage;

		public Index()
		{
			InitializeComponent();
		}

		private void Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			NavigationService.Navigate(new Uri("Pages/" + (sender as Image).Tag, UriKind.Relative));
		}
	}
}
