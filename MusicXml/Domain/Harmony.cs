using System;

namespace MusicXml
{
	public class Harmony
	{
		internal Harmony ()
		{
			rootStep = "";
			rootAlter = 0;
			Kind = "";
		}

		public int rootAlter { get; internal set; }

		public string rootStep { get; internal set; }

		public string Kind { get; internal set; }

	}

}

