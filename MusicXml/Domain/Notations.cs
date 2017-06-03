using System;

namespace MusicXml
{
	public class Notations
	{
		internal Notations ()
		{
			Technical = new Technical();
			Root = new Root ();
			Articulations = "";
			Dynamics = "";
		}

		public Technical Technical;
		public Root Root;
		public string Articulations { get; internal set; }

		public string Dynamics { get; internal set; }
	}
}
