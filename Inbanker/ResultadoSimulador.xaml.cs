using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Inbanker
{
	public partial class ResultadoSimulador : ContentPage
	{
		public ResultadoSimulador(Transacao trans)
		{
			InitializeComponent();


			//fomula para calcular o valor total a ser pago, ao mesmo tempo que arredondamos o resultado final para 2 casas decimais depois da virgula
			double capital = double.Parse(trans.trans_valor);
			//double juros_mensal = Math.Round(capital * (1+(0.00132667*dias)),2);

			double juros_mensal = Math.Round(capital * (0.00066333 * trans.trans_dias), 2);
			double taxa_fixa = Math.Round(capital * 0.0099, 2, MidpointRounding.ToEven);

			double total = juros_mensal + taxa_fixa + capital;

			string juros_mensal2 = String.Format("{0:0.00}", juros_mensal);
			string taxa_fixa2 = String.Format("{0:0.00}", taxa_fixa);

			//String.Format("{0:0.00}", 140.1);

			valor_juros.Text = "Valor do juros: R$ " + juros_mensal2;
			valor_taxa_fixa.Text = "Valor de serviço: R$ " + taxa_fixa2;

			nome_user.Text = trans.trans_nome_user2;
			valor_solicitado.Text = trans.trans_valor;
			data_vencimento.Text = trans.trans_vencimento;
			dias_pagamento.Text = trans.trans_dias.ToString();
			valor_total_pago.Text = total.ToString();

			btn_enviar_pedido.Clicked += async (sender, e) =>
			{
				if (senha.Text == "admin")
				{


					//fazemos cadastro do 
					ServiceWrapper serviceWrapper = new ServiceWrapper();
					var result = await serviceWrapper.EnviarPedidoUsuario(trans);
					string[] colunas = result.Split(',');
					lblNome.Text = "resultado " + result;

					if (colunas[0].Equals("ok"))
					{

						stack_btn.IsVisible = false;
						lblNome3.Text = "Pedido enviado, aguarde a resposta do(a) "+trans.trans_nome_user2;
						//await DisplayAlert ("Inbanker", "Pedido foi enviado para "+nome,"Ok");

						var result2 = await serviceWrapper.EnviarNotificacaoUsuario(trans,colunas[1]);
						//lblNome2.Text = "get call says: " + result2;

					}


				}
				else {
					await DisplayAlert("Alerta", "Por favor informe sua senha correta", "Ok");
				}

			};


		}
	}
}

