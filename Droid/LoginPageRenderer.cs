using System;
using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Inbanker;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin.Auth;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Android.Preferences;
using Android.App;

[assembly: ExportRenderer(typeof(Login), typeof(Inbanker.Droid.LoginPageRenderer))]

namespace Inbanker.Droid
{
	public class LoginPageRenderer : PageRenderer
	{

		// On Android:
		IEnumerable<Account> accounts;
		Account facebook;

		ISharedPreferences prefs;
		ISharedPreferencesEditor editor;

		//ira amarzenar os dados do usuario recem logado
		Usuario eu;

		public LoginPageRenderer()
		{
			var activity = this.Context as Activity;

			prefs = PreferenceManager.GetDefaultSharedPreferences(Forms.Context);
			editor = prefs.Edit();

			// On Android:
			accounts = AccountStore.Create(Forms.Context).FindAccountsForService("Facebook");

			//se existir uma conta armazenada fazemos o login, se nao somos redirecionados para a tela de login
			if (accounts.Count() > 0)
			{
				var enumerable = accounts as IList<Account> ?? accounts.ToList();
				facebook = enumerable.First();

				VerificaUser();
			}
			else {
				var auth = new OAuth2Authenticator(
					clientId: "1028244680574076", // your OAuth2 client id
					scope: "user_friends", // the scopes for the particular API you're accessing, delimited by "+" symbols
					authorizeUrl: new Uri("https://m.facebook.com/dialog/oauth/"),
					redirectUrl: new Uri("https://www.facebook.com/connect/login_success.html"));


				auth.Completed += async (sender, eventArgs) =>
				{

					if (eventArgs.IsAuthenticated)
					{

						// On Android:
						AccountStore.Create(Forms.Context).Save(eventArgs.Account, "Facebook");

						var parameters = new Dictionary<string, string>();
						//parameters.Add("fields", "friends{id,name,picture{url}},id,name,picture{url}");
						parameters.Add("fields", "id,name,picture.type(large),friends{id,name,picture.type(large){url}}");

						//var accessToken = eventArgs.Account.Properties ["access_token"].ToString ();
						//var expiresIn = Convert.ToDouble (eventArgs.Account.Properties ["expires_in"]);
						//var expiryDate = DateTime.Now + TimeSpan.FromSeconds (expiresIn);

						var request = new OAuth2Request("GET", new Uri("https://graph.facebook.com/me"), parameters, eventArgs.Account);
						var response = await request.GetResponseAsync();
						var obj = JObject.Parse(response.GetResponseText());

						var picture = obj["picture"].ToString();
						var friends = obj["friends"].ToString();
						var id = obj["id"].ToString().Replace("\"", "");
						var nome = obj["name"].ToString().Replace("\"", "");

						var obj_friends = JObject.Parse(friends);
						var lista_friends = obj_friends["data"].ToString();

						var obj_picture = JObject.Parse(picture);
						var picture_data = obj_picture["data"].ToString();

						var obj_picture2 = JObject.Parse(picture_data);
						var picture_url = obj_picture2["url"].ToString();

						//adicionamos os dados do usuario recem logado no objeto
						eu = new Usuario
						{
							id_usuario = id,
							nome_usuario = nome,
							url_img = picture_url,
						};

						var usu = JsonConvert.DeserializeObject<List<Amigos>>(lista_friends);

						editor.PutString("usu_id_face", id);
						editor.PutString("usu_nome", nome);
						editor.PutString("usu_picture", picture_url);
						// editor.Commit();    // applies changes synchronously on older APIs
						editor.Apply();  // applies changes asynchronously on newer APIs

						//string token_gcm = prefs.GetString ("token_gcm","");

						//fazemos verificaçao do play service e registro do GCM
						VerifyPlayServices verify = new VerifyPlayServices();
						verify.IsPlayServicesAvailable();

						//friends.
						await App.NavigateToLista(eu,usu);



					}
					else {
						await App.NavigateToLista(null,null);
					}
				};
				activity.StartActivity(auth.GetUI(activity));

			}

		}

		public async void VerificaUser()
		{
			var parameters = new Dictionary<string, string>();
			//parameters.Add("fields", "friends{id,name,picture{url}},id,name,picture.type(large)");
			parameters.Add("fields","id,name,picture.type(large),friends{id,name,picture.type(large){url}}");

			var request = new OAuth2Request("GET", new Uri("https://graph.facebook.com/me"), parameters, facebook);
			var response = await request.GetResponseAsync();
			var obj = JObject.Parse(response.GetResponseText());

			var picture = obj["picture"].ToString();
			var friends = obj["friends"].ToString();
			var id = obj["id"].ToString().Replace("\"", "");
			var nome = obj["name"].ToString().Replace("\"", "");

			var obj_friends = JObject.Parse(friends);
			var lista_friends = obj_friends["data"].ToString();

			var obj_picture = JObject.Parse(picture);
			var picture_data = obj_picture["data"].ToString();

			var obj_picture2 = JObject.Parse(picture_data);
			var picture_url = obj_picture2["url"].ToString();

			//adicionamos os dados do usuario recem logado no objeto
			eu = new Usuario
			{
				id_usuario = id,
				nome_usuario = nome,
				url_img = picture_url,
			};

			var usu = JsonConvert.DeserializeObject<List<Amigos>>(lista_friends);

			//armazenamos dados no sharedpreferences
			prefs = PreferenceManager.GetDefaultSharedPreferences(Forms.Context);
			editor = prefs.Edit();
			editor.PutString("usu_id_face", id);
			editor.PutString("usu_nome", nome);
			// editor.Commit();    // applies changes synchronously on older APIs
			editor.Apply();  // applies changes asynchronously on newer APIs

			//fazemos verificaçao do play service e registro do GCM
			//VerifyPlayServices verify = new VerifyPlayServices();
			//verify.IsPlayServicesAvailable ();

			//friends.
			await App.NavigateToLista(eu,usu);


		}
	}
}


