using System;

namespace MusicXml
{
	public class Technical
	{
		internal Technical ()
		{
			Fret = 0;
			String = 0;
		}

		public int Fret { get; internal set; }

		public int String { get; internal set; }

	}
}