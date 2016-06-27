using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Inbanker
{
	public class MainPageCS : MasterDetailPage
	{
		MasterPage masterPage;

		public MainPageCS(Usuario eu,List<Amigos> list_amigos)
		{

			NavigationPage.SetHasNavigationBar(this, false);

			masterPage = new MasterPage(eu);
			Master = masterPage;
			Detail = new NavigationPage(new ListaAmigos(eu,list_amigos));

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
					switch (item.ParamType)
					{
						case (1):
							Detail = new NavigationPage((Page)Activator.CreateInstance(item.TargetType, eu, list_amigos));
							break;
						case (2):
							Detail = new NavigationPage((Page)Activator.CreateInstance(item.TargetType, eu));
							break;
						default:
							Detail = new NavigationPage((Page)Activator.CreateInstance(item.TargetType, eu, list_amigos));
							break;
					}

					//if(item.ParamType == 1)
						

					//else
					//	Detail = new NavigationPage((Page)Activator.CreateInstance(item.TargetType));

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
		//		Detail = new NavigationPage((Page)Activator.CreateInstance(item.TargetType));
		//		masterPage.ListView.SelectedItem = null;
		//		IsPresented = false;
		//	}
		//}
	}
}


