using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Inbanker
{
	public partial class MainPage : MasterDetailPage
	{
		public MainPage(Usuario eu)
		{
			InitializeComponent();

			//masterPage.ListView.ItemSelected += OnItemSelected;

			if (Device.OS == TargetPlatform.Windows)
			{
				Master.Icon = "icon.png";
			}

			masterPage.ListView.ItemSelected += (sender, e) =>
			{ 

				var item = e.SelectedItem as MasterPageItem;
				if (item != null)
				{

					//MessagingCenter.Subscribe<MasterPageItem>(this, "MenuCellMessage", (sender_arg) => NavigateTo(item.TargetType, eu));

					Detail = new NavigationPage((Page)Activator.CreateInstance(item.TargetType, eu));

					masterPage.ListView.SelectedItem = null;
					IsPresented = false;
				}

			};
		}


		//void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
		//{
			
		//	var item = e.SelectedItem as MasterPageItem;
		//	if (item != null)
		//	{

		//		//MessagingCenter.Subscribe<MasterPageItem>(this, "MenuCellMessage", (sender_arg) => NavigateTo(item.TargetType, eu));

		//		Detail = new NavigationPage((Page)Activator.CreateInstance(item.TargetType,eu));

		//		//masterPage.ListView.SelectedItem = null;
		//		//IsPresented = false;
		//	}
		//}



		//void NavigateTo(Type targetType, object[] args)
		//{

		//	Detail = new NavigationPage((Page)Activator.CreateInstance(item.TargetType));

		//	masterPage.ListView.SelectedItem = null;
		//	IsPresented = false;

		//	Page displayPage = (Page)Activator.CreateInstance(targetType, args);

		//	Navigation.PushModalAsync(new NavigationPage(displayPage), true);
		//}


	}
}

