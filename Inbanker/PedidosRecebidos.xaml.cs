using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace Inbanker
{
	public partial class PedidosRecebidos : ContentPage
	{
		public PedidosRecebidos(Usuario eu)
		{
			InitializeComponent();

			Title = "Pedidos Recebidos";

			VerificaListaTransacoes(eu);
		}

		public async void VerificaListaTransacoes(Usuario eu)
		{
			ServiceWrapper service = new ServiceWrapper();
			var result = await service.ListaPedidosRecebidos(eu.id_usuario);
			//carregando.Text = result;

			if (result.Equals("menor"))
			{
				carregando.Text = "Voce ainda nao recebeu nenhum pedido de emprestimo.";
			}
			else {

				stack_load.IsVisible = false;
				stack_lista.IsVisible = true;

				var trans = JsonConvert.DeserializeObject<List<Transacao>>(result);
				//carregando.Text = result;
				ListarTransacoes(trans);
			}

		}

		public void ListarTransacoes(List<Transacao> lista_trans)
		{

			//busca.TextChanged += Busca_TextChanged;

			lista.ItemsSource = lista_trans;

			lista.ItemTapped += async (sender, args) =>
			{
				var item = args.Item as Transacao;
				if (item == null)
					return;

				//await Navigation.PushAsync(new VerPedidoRecebido(item.trans_valor, item.trans_nome_user1, item.trans_dias.ToString(), item.trans_id, item.trans_resposta_pedido, item.trans_resposta_pagamento, ""));

				await Navigation.PushAsync(new VerPedidoRecebido(item));
			};

		}

	}
}

