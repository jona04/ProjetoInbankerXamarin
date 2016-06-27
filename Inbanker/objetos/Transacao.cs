using System;
using Xamarin.Forms;

namespace Inbanker
{
	public class Transacao
	{
		public string trans_id { get; set; }
		public string trans_id_user1 { get; set; }
		public string trans_id_user2 { get; set; }
		public string trans_nome_user1 { get; set; }
		public string trans_nome_user2 { get; set; }
		public string trans_valor { get; set; }
		public string trans_vencimento { get; set; }
		public string trans_data_pedido { get; set; }
		public int trans_dias { get; set; }
		public string trans_dia_pago { get; set; }
		public string trans_resposta_pedido { get; set; }
		public string trans_resposta_pagamento { get; set; }
		public string trans_recebimento_empre { get; set; }

	}
}

