using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Inbanker
{
	public partial class VerPedidosEnviados : ContentPage
	{
		public VerPedidosEnviados(Transacao trans)
		{
			InitializeComponent();


			Title = "Pedido Enviado";

			nome_usuario.Text = trans.trans_nome_user2;

			//fomula para calcular o valor total a ser pago, ao mesmo tempo que arredondamos o resultado final para 2 casas decimais depois da virgula
			double capital = double.Parse(trans.trans_valor);
			//double juros_mensal = Math.Round(capital * (1+(0.00132667*dias)),2);


			double juros_mensal = Math.Round(capital * (0.00066333 * trans.trans_dias), 2);
			double taxa_fixa = Math.Round(capital * 0.0099, 2, MidpointRounding.ToEven);

			double total = juros_mensal + taxa_fixa + capital;

			string juros_mensal2 = String.Format("{0:0.00}", juros_mensal);
			string taxa_fixa2 = String.Format("{0:0.00}", taxa_fixa);

			valor_juros.Text = juros_mensal2;
			valor_taxa_fixa.Text = "Valor de serviço: R$ " + taxa_fixa2;

			valor_solicitado.Text = trans.trans_valor;
			lbldata_vencimento.Text = trans.trans_vencimento;
			dias_corrido.Text = trans.trans_data_pedido;
			valor_total_pago.Text = total.ToString();


			//manipulamos o xmal de acordo com a resposta dada ao pedido
			switch (int.Parse(trans.trans_resposta_pedido))
			{
				case (0): //usuario ainda nao respondeu se aceita ou nao o pedido de emprestimo
					msg.Text = "Voce esta aguardando o usuario aceitar ou recusar seu pedido.";
					break;
				case (1): // o usuario recusou o pedido de emprestimo
					msg.Text = "O usuario recusou seu pedido de emprestimo.";
					break;
				case (2): // o usuario aceitou o pedido de emprestimo

					if (trans.trans_recebimento_empre.Equals("0"))
					{
						stack_confirm_receb.IsVisible = true;
						msg.Text = "O usuario aceitou seu pedido de emprestimo.";
						msg_confirm_recb.Text = "Voce confirma o recebimento do valor?";
					}
					else {

						if (trans.trans_resposta_pagamento.Equals("0")) //usuario 1 ainda esta para solicitar quitacao de pagamento
						{ 
							stack_confirm_receb.IsVisible = false;
							stack_solicitar_pag.IsVisible = true;
							msg.Text = "Solicite a quitaçao do valor pedido em emprestimo.";
						}
						if (trans.trans_resposta_pagamento.Equals("1")) //usuario 1 solicitou quitacao de pagamento e esta no aguardo
						{
							stack_confirm_receb.IsVisible = false;
							stack_solicitar_pag.IsVisible = false;
							msg.Text = "Voce realizou uma solicitaçao de quitaçao do valor pedido para emprestimo. Aguarde a resposta do " + trans.trans_nome_user2;

						}
						if (trans.trans_resposta_pagamento.Equals("2")) //usuario 2 resposdeu negativamente a quitaçao do valor emprestado
						{
							stack_confirm_receb.IsVisible = false;
							stack_solicitar_pag.IsVisible = true;
							msg.Text = "Seu pedido de quitaçao foi negado, solicite a quitaçao do valor pedido em emprestimo novamente.";
						}
						if (trans.trans_resposta_pagamento.Equals("3")) //usuario 2 resposdeu positivamente a quitacao do valor emprestado
						{
							stack_confirm_receb.IsVisible = false;
							stack_solicitar_pag.IsVisible = false;
							msg.Text = "O emprestimo reslizado com "+ trans.trans_nome_user2+" foi finalizado com sucesso.";
						}

					}

					break;
			}

			btn_sim.Clicked += async (sender, e) =>
			{
				if (senha.Text == "admin")
				{


					//fazemos cadastro do 
					ServiceWrapper serviceWrapper = new ServiceWrapper();
					var result = await serviceWrapper.ConfirmaRecebimentoEmprestimo(1, trans.trans_id);
					//string[]colunas = result.Split(',');
					lblNome.Text = "resultado " + result;

					if (result.Equals("ok"))
					{
						stack_confirm_receb.IsVisible = false;
						stack_solicitar_pag.IsVisible = true;
						msg.Text = "Solicite a quitaçao do valor pedido em emprestimo.";


					//	stack_btn_acc_pedido.IsVisible = false;
					//	stack_msg_pedido.IsVisible = true;
					//	msg_pedido.Text = "Voce recusou esse pedido de emprestimo.";

					//	//await DisplayAlert ("Inbanker", "Pedido foi enviado para "+nome,"Ok");

					//	var result2 = await serviceWrapper.EnviarNotificacaoRespostaUsuario(trans_id, 1);
					//	lblNome2.Text = "get call says: " + result2;

					}

				}
				else {
					await DisplayAlert("Alerta", "Por favor informe sua senha correta", "Ok");
				}

			};

			btn_solicitar_pags.Clicked += async (sender, e) =>
			{
				if (senha.Text == "admin")
				{

				//	//await DisplayAlert ("Inbanker", "Pedido foi enviado para "+nome,"Ok");
					ServiceWrapper serviceWrapper = new ServiceWrapper();
					var result2 = await serviceWrapper.SolicitarPagamentoEmprestimo(trans.trans_id,1);
					lblNome.Text = "get call says: " + result2;

					if (result2.Equals("ok"))
					{
						stack_confirm_receb.IsVisible = false;
						stack_solicitar_pag.IsVisible = false;
						msg.Text = "Voce realizou uma solicitaçao de quitaçao do valor pedido para emprestimo. Aguarde a resposta do "+ trans.trans_nome_user2;

						var result = await serviceWrapper.EnviarNotificacaoConfirmPagamento(trans);
						//lblNome2.Text = "get call says: " + result;

					}

				}
				else {
					await DisplayAlert("Alerta", "Por favor informe sua senha correta", "Ok");
				}

			};


		}
	}
}

