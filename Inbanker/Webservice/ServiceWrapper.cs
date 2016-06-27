using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Inbanker
{
	public class ServiceWrapper
	{
		public async Task<string> GetData(string id)
		{
			using (var client = new HttpClient())
			{
				var result = await client.GetAsync("http://inbanker.com/webservice/teste.php?id=" + id);
				return await result.Content.ReadAsStringAsync();
			}
		}

		/*public async Task<string> RegisterUserJsonRequest(string id)
		{
			using (var client = new HttpClient())
			{               
				var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");               

				var result = await client.PostAsync("http://xamarin-rest-service.azurewebsites.net/demo/registeruser", content);
				return await result.Content.ReadAsStringAsync();
			}
		}*/

		public async Task<string> RegisterUserFormRequest(string id, string nome, string token,string img)
		{

			using (var client = new HttpClient())
			{


				var content = new FormUrlEncodedContent(new[]
					{
						new KeyValuePair<string, string>("usu_id_face", id),
						new KeyValuePair<string, string>("usu_nome", retiraAcentos(nome)), //retiramos acento para nao causar problemas no banco de dados
						new KeyValuePair<string, string>("usu_token", token),
						new KeyValuePair<string, string>("usu_img", img)
					});

				var result = await client.PostAsync("http://inbanker.com/webservice/adduser.php", content);
				return await result.Content.ReadAsStringAsync();
			}
		}

		public async Task<string> EnviarPedidoUsuario(Transacao trans)
		{

			using (var client = new HttpClient())
			{


				var content = new FormUrlEncodedContent(new[]
					{


						new KeyValuePair<string, string>("id_user_logado", trans.trans_id_user1),
						new KeyValuePair<string, string>("id_user2", trans.trans_id_user2),
					new KeyValuePair<string, string>("nome_user1", retiraAcentos(trans.trans_nome_user1)),
					new KeyValuePair<string, string>("nome_user2", retiraAcentos(trans.trans_nome_user2)),
						new KeyValuePair<string, string>("data", trans.trans_vencimento),
						new KeyValuePair<string, string>("valor", trans.trans_valor),
						new KeyValuePair<string, string>("dias", trans.trans_dias.ToString()),
					});

				var result = await client.PostAsync("http://inbanker.com/webservice/enviandopedido.php", content);
				return await result.Content.ReadAsStringAsync();
			}
		}

		public async Task<string> EnviarNotificacaoUsuario(Transacao trans,string trans_id)
		{

			using (var client = new HttpClient())
			{


				var content = new FormUrlEncodedContent(new[]
					{


						new KeyValuePair<string, string>("gcm_push", "gcm_push"),
						new KeyValuePair<string, string>("id_user2", trans.trans_id_user2),
						new KeyValuePair<string, string>("trans_id", trans_id),
					});

				var result = await client.PostAsync("http://inbanker.com/webservice/gcm_main.php", content);
				return await result.Content.ReadAsStringAsync();
			}
		}

		public async Task<string> EnviarNotificacaoRespostaUsuario(string trans_id,string trans_id_user1, int resposta)
		{

			using (var client = new HttpClient())
			{


				var content = new FormUrlEncodedContent(new[]
					{


						new KeyValuePair<string, string>("gcm_push", "gcm_push_resposta"),
						new KeyValuePair<string, string>("trans_id", trans_id),
					new KeyValuePair<string, string>("trans_id_user1", trans_id_user1),
						new KeyValuePair<string, string>("resposta", resposta.ToString()),
					});

				var result = await client.PostAsync("http://inbanker.com/webservice/gcm_main_respostapedido.php", content);
				return await result.Content.ReadAsStringAsync();
			}
		}

		public async Task<string> RespostaPedidoUsuario(int resposta, string trans_id)
		{

			using (var client = new HttpClient())
			{


				var content = new FormUrlEncodedContent(new[]
					{
						new KeyValuePair<string, string>("resposta", resposta.ToString()),
						new KeyValuePair<string, string>("trans_id", trans_id),
					});

				var result = await client.PostAsync("http://inbanker.com/webservice/respostapedido.php", content);
				return await result.Content.ReadAsStringAsync();
			}
		}

		public async Task<string> ListaPedidosFeitos(string id_usu)
		{

			using (var client = new HttpClient())
			{


				var content = new FormUrlEncodedContent(new[]
					{
					new KeyValuePair<string, string>("user_id", id_usu)
					});

				var result = await client.PostAsync("http://inbanker.com/webservice/lista_pedidosfeitos.php", content);
				return await result.Content.ReadAsStringAsync();
			}
		}

		public async Task<string> ListaPedidosRecebidos(string id_usu)
		{

			using (var client = new HttpClient())
			{


				var content = new FormUrlEncodedContent(new[]
					{
					new KeyValuePair<string, string>("user_id", id_usu)
					});

				var result = await client.PostAsync("http://inbanker.com/webservice/lista_pedidosrecebidos.php", content);
				return await result.Content.ReadAsStringAsync();
			}
		}

		public async Task<string> ConfirmaRecebimentoEmprestimo(int resp,string trans_id)
		{

			using (var client = new HttpClient())
			{


				var content = new FormUrlEncodedContent(new[]
					{
					new KeyValuePair<string, string>("trans_id", trans_id),
					new KeyValuePair<string, string>("resposta", resp.ToString())
					});

				var result = await client.PostAsync("http://inbanker.com/webservice/confirm_receb_empre.php", content);
				return await result.Content.ReadAsStringAsync();
			}
		}

		public async Task<string> SolicitarPagamentoEmprestimo(string trans_id,int resposta)
		{

			using (var client = new HttpClient())
			{
				
				var content = new FormUrlEncodedContent(new[]
					{
					new KeyValuePair<string, string>("trans_id", trans_id),
					new KeyValuePair<string, string>("resposta", resposta.ToString()),
					});

				var result = await client.PostAsync("http://inbanker.com/webservice/solicitar_pag_empre.php", content);
				return await result.Content.ReadAsStringAsync();
			}
		}

		public async Task<string> EnviarNotificacaoConfirmPagamento(Transacao trans)
		{

			using (var client = new HttpClient())
			{
				
				var content = new FormUrlEncodedContent(new[]
					{
					new KeyValuePair<string, string>("gcm_push", "gcm_push_pagamento"),
					new KeyValuePair<string, string>("trans_id", trans.trans_id),
					new KeyValuePair<string, string>("trans_id_user2", trans.trans_id_user2),
					});

				var result = await client.PostAsync("http://inbanker.com/webservice/gcm_main_confirm_pag_pedido.php", content);
				return await result.Content.ReadAsStringAsync();
			}
		}

		public async Task<string> RespostaConfirmPagamento(string trans_id,int resposta)
		{

			using (var client = new HttpClient())
			{

				var content = new FormUrlEncodedContent(new[]
					{
					new KeyValuePair<string, string>("resposta", resposta.ToString()),
					new KeyValuePair<string, string>("trans_id", trans_id),
					});

				var result = await client.PostAsync("http://inbanker.com/webservice/resposta_confirm_pag_pedido.php", content);
				return await result.Content.ReadAsStringAsync();
			}
		}

		public async Task<string> EnviarNotificacaoRespostaConfirmPagamento(Transacao trans,int resposta)
		{

			using (var client = new HttpClient())
			{

				var content = new FormUrlEncodedContent(new[]
					{
					new KeyValuePair<string, string>("gcm_push", "gcm_push_resposta_pagamento"),
					new KeyValuePair<string, string>("trans_id", trans.trans_id),
					new KeyValuePair<string, string>("trans_id_user1", trans.trans_id_user1),
					new KeyValuePair<string, string>("resposta", resposta.ToString()),
					});

				var result = await client.PostAsync("http://inbanker.com/webservice/gcm_main_resposta_confirm_pag_pedido.php", content);
				return await result.Content.ReadAsStringAsync();
			}
		}

		private string retiraAcentos(string texto)
		{
			string comAcentos = "ÄÅÁÂÀÃäáâàãÉÊËÈéêëèÍÎÏÌíîïìÖÓÔÒÕöóôòõÜÚÛüúûùÇç";
			string semAcentos = "AAAAAAaaaaaEEEEeeeeIIIIiiiiOOOOOoooooUUUuuuuCc";
			for (int i = 0; i < comAcentos.Length; i++)
			{
				texto = texto.Replace(comAcentos[i].ToString(), semAcentos[i].ToString());
			}
			return texto;
		}
	
	}
}

