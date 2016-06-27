using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Inbanker
{
	public partial class App : Application
	{

		public static Action HideLoginView
		{
			get
			{
				return new Action(() => Current.MainPage.Navigation.PopModalAsync());
			}
		}

		public async static Task NavigateToLista(Usuario eu,List<Amigos> list_amigos)
		{
			await Current.MainPage.Navigation.PushAsync(new MainPageCS(eu,list_amigos));
		}

		//public App()
		//{
		//	InitializeComponent();

		//	MainPage = new NavigationPage(new LoginPage());
		//}

		public App(string notification,Transacao transacao)
		{
			InitializeComponent();

			if (notification.Equals("false"))
			{
				MainPage = new NavigationPage(new LoginPage());
			}
			else {
				if (notification.Equals("receber_pedido"))
				{
					// The root page of your application
					MainPage = new NavigationPage(new VerPedidoRecebido(transacao));
				}
				else 
				{
					// The root page of your application
					MainPage = new NavigationPage(new VerPedidosEnviados(transacao));
				}

			}
		}

		protected override void OnStart()
		{
			// Handle when your app starts
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}
	}
}

