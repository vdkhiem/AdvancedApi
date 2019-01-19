using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ScreenSoapAPIClient.Users;

namespace ScreenSoapAPIClient
{
	class Program
	{
		private static Screen context;
		/// <summary>
		/// Prerequisite for screen base soap API
		/// 1.Generate the WSDL File of the Web Services
		///		https://help-2018r2.acumatica.com/Help?ScreenId=ShowWiki&pageid=1eac8817-c782-4610-b7e1-dc2e831dcb42
		/// 2.Import the WSDL File Into the Development Environment
		///		https://help-2018r2.acumatica.com/Help?ScreenId=ShowWiki&pageid=0b2f1956-4612-40c5-a816-4ab4ae06f0bf 
		/// </summary>
		/// <param name="args"></param>
		static void Main(string[] args)
		{
			context = new Screen
			{
				CookieContainer = new System.Net.CookieContainer(),
				AllowAutoRedirect = AcumaticaApi.AllowAutoRedirect,
				EnableDecompression = AcumaticaApi.EnableDecompression,
				Timeout = AcumaticaApi.WebServiceTimeout,
				Url = AcumaticaApi.WebServiceUrl
			};

			var result = context.Login(AcumaticaApi.UserName, AcumaticaApi.Password);

			Console.WriteLine(result);

			InsertUser();

			Console.ReadLine();

		}

		private static void InsertUser()
		{
			var userScreen = context.SM201010GetSchema();
			context.SM201010Clear();

			context.SM201010Submit(
				new Command[]
				{
					userScreen.Actions.InsertUsers,
					new Value {Value = "sp8", LinkedCommand = userScreen.UserInformation.Login},
					new Value {Value = "sp@8", LinkedCommand = userScreen.UserInformation.Email},
					new Value {Value = "MYOB", LinkedCommand = userScreen.ExternalIdentities.ProviderName},
					new Value {Value = "my-user-key8", LinkedCommand = userScreen.ExternalIdentities.UserKey},
					new Value {Value = true.ToString(),LinkedCommand = userScreen.ExternalIdentities.Active},
					userScreen.Actions.SaveUsers
				});
			Console.WriteLine("Inserted user");
		}

		private static void UpdatePartnerUserIdentity()
		{
			var userScreen = context.SM201010GetSchema();
			context.SM201010Clear();

			context.SM201010Submit(
				new Command[]
				{
					new Value {Value = "sp5", LinkedCommand = userScreen.UserInformation.Login},
					new Value {Value = "MYOB", LinkedCommand = userScreen.ExternalIdentities.ProviderName},
					new Value {Value = "my-user-key", LinkedCommand = userScreen.ExternalIdentities.UserKey},
					new Value
					{
						Value = true.ToString(),
						LinkedCommand = userScreen.ExternalIdentities.Active
					},
					userScreen.Actions.SaveUsers
				});
			Console.WriteLine("Updated Partner User Identity");
		}
	}

	public class AcumaticaApi
	{
		public const bool AllowAutoRedirect = true;
		public const bool EnableDecompression = true;
		public const int WebServiceTimeout = 10000000;
		//public const string WebServiceUrl = "https://localhost:44400/Soap/USERS.asmx";
		public const string WebServiceUrl = "http://localhost:44100/Soap/USERS.asmx";

		public const string UserName = "admin@demonz";
		public const string Password = "password01";
	}
}
