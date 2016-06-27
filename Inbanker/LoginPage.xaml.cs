using Xamarin.Forms;

namespace Inbanker
{
	public partial class LoginPage : ContentPage
	{
		public LoginPage()
		{
			InitializeComponent();

			Title = "Inbanker";

			this.btnLogar.Clicked += async (sender, e) =>
			{
				await Navigation.PushAsync(new Login());
			};

		}
	}
}

