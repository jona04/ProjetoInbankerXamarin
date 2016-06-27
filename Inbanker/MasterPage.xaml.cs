using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Inbanker
{
	public partial class MasterPage : ContentPage
	{
		public ListView ListView { get { return listView; } }

		public MasterPage(Usuario eu)
		{
			InitializeComponent();

			img.Source = eu.url_img;
			nome.Text = eu.nome_usuario;

			var masterPageItems = new List<MasterPageItem>();
			masterPageItems.Add(new MasterPageItem
			{
				Title = "Lista Amigos",
				//IconSource = "contacts.png",
				TargetType = typeof(ListaAmigos),
				ParamType = 1,
			});
			masterPageItems.Add(new MasterPageItem
			{
				Title = "Pedidos enviados",
				//IconSource = "contacts.png",
				TargetType = typeof(PedidosFeitos),
				ParamType = 2
			});
			masterPageItems.Add(new MasterPageItem
			{
				Title = "Pedidos recebidos",
				//IconSource = "todo.png",
				TargetType = typeof(PedidosRecebidos),
				ParamType = 2
			});
			masterPageItems.Add(new MasterPageItem
			{
				Title = "Sair",
				//IconSource = "reminders.png",
				//TargetType = typeof(ReminderPage)
			});

			listView.ItemsSource = masterPageItems;
		}
	}
}

