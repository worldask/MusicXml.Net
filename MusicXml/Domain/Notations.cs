using System;

namespace MusicXml
{
	public class Notations
	{
		internal Notations ()
		{
			Technical = new Technical();
			Articulations = "";
			Dynamics = "";
		}

		public Technical Technical;
		public string Articulations { get; internal set; }

		public string Dynamics { get; internal set; }
	}
}
