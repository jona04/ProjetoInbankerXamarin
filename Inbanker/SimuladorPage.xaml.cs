using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Inbanker
{
	public partial class SimuladorPage : ContentPage
	{

		public DateTime data;
		public int dias;
		public string date;
		public string valor;
		Transacao trans;

		public SimuladorPage(string id_user_logado, string nome_user_logado, string id_usuario, string nome, string img)
		{
			InitializeComponent();

			trans = new Transacao
			{
				//trans_dias = dias,
				//trans_valor = valor,
				trans_id_user1 = id_user_logado,
				trans_id_user2 = id_usuario,
				trans_nome_user1 = nome_user_logado,
				trans_nome_user2 = nome,
				//trans_vencimento = date,
			};

			nome_user.Text = nome;
			img_user.Source = img;


			//setamos um valor maximo para o datepicker
			DateTime dayNow = DateTime.Now;
			DateTime expiryDay = dayNow.AddDays(60 + 1);
			date_vencimento.SetValue(DatePicker.MinimumDateProperty, dayNow);
			date_vencimento.SetValue(DatePicker.MaximumDateProperty, expiryDay);

			valor_pedido.SelectedIndexChanged += (sender, e) =>
			{
				int selectedIndex = valor_pedido.SelectedIndex;
				//if (selectedIndex == -1)
				//	return;

				valor = valor_pedido.Items[selectedIndex];

				trans.trans_valor = valor_pedido.Items[selectedIndex];
			};
			date_vencimento.DateSelected += (sender, e) =>
			{
				dias = (date_vencimento.Date - dayNow).Days;
				date = date_vencimento.Date.ToString("dd/MM/yyyy");

				trans.trans_dias = (date_vencimento.Date - dayNow).Days;
				trans.trans_vencimento = date_vencimento.Date.ToString("dd/MM/yyyy");
			};

			btn_Verificar.Clicked += async (sender, e) =>
			{
				var result = await IsValid();
				if (result)
				{
					await Navigation.PushAsync(new ResultadoSimulador(trans));
					//await DisplayAlert ("Alerta", "Valor :"+valor,"Ok");
				}
			};
		}

		//valida os campos nome e endereco no formulario
		private async Task<bool> IsValid()
		{
			if (string.IsNullOrEmpty(date))
			{
				await DisplayAlert("Alerta", "Data deve ser preenchido", "Ok");
				return false;
			};
			if (string.IsNullOrEmpty(valor))
			{
				await DisplayAlert("Alerta", "Valor deve ser preenchido", "Ok");
				return false;
			};


			return true;
		}
	}
}

