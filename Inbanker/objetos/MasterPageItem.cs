using System;

using Xamarin.Forms;

namespace Inbanker
{
	public class MasterPageItem
	{
		public string Title { get; set; }

		public string IconSource { get; set; }

		public Type TargetType { get; set; }

		public object[] Args { get; set; }

		public object[] Args2 { get; set; }

		public int ParamType { get; set; } //para saber quais parametros sera passado a detail page
	}
}


