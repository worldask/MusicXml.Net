using System;

namespace MusicXml
{
	public class Harmony
	{
		internal Harmony ()
		{
			RootStep = "";
			RootAlter = 0;
			Kind = "";
		}

		public int RootAlter { get; internal set; }

		public string RootStep { get; internal set; }

		public string Kind { get; internal set; }

	}

}

