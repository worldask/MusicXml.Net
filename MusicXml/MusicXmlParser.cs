﻿using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using MusicXml.Domain;
using Encoding = MusicXml.Domain.Encoding;

namespace MusicXml
{
	public static class MusicXmlParser
	{
		public static Score GetScore(string filename)
		{
			var document = GetXmlDocument(filename);

			var score = new Score();

			var movementTitleNode = document.SelectSingleNode("score-partwise/movement-title");
			if (movementTitleNode != null)
				score.MovementTitle = movementTitleNode.InnerText;

			score.Identification = GetIdentification(document);
			
			var partNodes = document.SelectNodes("score-partwise/part-list/score-part");
			
			if (partNodes != null)
			{
				foreach (XmlNode partNode in partNodes)
				{
					var part = new Part();
					score.Parts.Add(part);

					if (partNode.Attributes != null)
						part.Id = partNode.Attributes["id"].InnerText;
					
					var partNameNode = partNode.SelectSingleNode("part-name");
					
					if (partNameNode != null)
						part.Name = partNameNode.InnerText;

					var measuresXpath = string.Format("//part[@id='{0}']/measure", part.Id);

					var measureNodes = partNode.SelectNodes(measuresXpath);

					if (measureNodes != null)
					{
						foreach (XmlNode measureNode in measureNodes)
						{
							var measure = new Measure();

							if (measureNode.Attributes != null)
							{
								var measureWidthAttribute = measureNode.Attributes["width"];
								decimal w;
								if (measureWidthAttribute != null && decimal.TryParse(measureWidthAttribute.InnerText, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture,out w))
									measure.Width = w;
							}

							var attributesNode = measureNode.SelectSingleNode("attributes");

							if (attributesNode != null)
							{
								measure.Attributes = new MeasureAttributes();

								var divisionsNode = attributesNode.SelectSingleNode("divisions");
								if (divisionsNode != null)
									measure.Attributes.Divisions = Convert.ToInt32(divisionsNode.InnerText);

								var keyNode = attributesNode.SelectSingleNode("key");

								if (keyNode != null)
								{
									measure.Attributes.Key = new Key();

									var fifthsNode = keyNode.SelectSingleNode("fifths");
									if (fifthsNode != null)
										measure.Attributes.Key.Fifths = Convert.ToInt32(fifthsNode.InnerText);

									var modeNode = keyNode.SelectSingleNode("mode");
									if (modeNode != null)
										measure.Attributes.Key.Mode = modeNode.InnerText;
								}
								
								measure.Attributes.Time = GetTime(attributesNode);

								measure.Attributes.Clef = GetClef(attributesNode);
							}

							var childNodes = measureNode.ChildNodes;

							foreach (XmlNode node in childNodes)
							{
								MeasureElement measureElement = null;

								if (node.Name == "note") {
									var newNote = GetNote (node);
									measureElement = new MeasureElement { Type = MeasureElementType.Note, Element = newNote };
								} else if (node.Name == "harmony") {
									var newHarmony = GetHarmony (node);
									measureElement = new MeasureElement { Type = MeasureElementType.Harmony, Element = newHarmony };
								}
								else if (node.Name == "backup")
								{
									measureElement = new MeasureElement {Type = MeasureElementType.Backup, Element = GetBackupElement(node)};
								}
								else if (node.Name == "forward")
								{
									measureElement = new MeasureElement {Type = MeasureElementType.Forward, Element = GetForwardElement(node)};
								}

								if (measureElement != null)
									measure.MeasureElements.Add(measureElement);
							}
							
							part.Measures.Add(measure);
						}
					}
				}
			}

			return score;
		}

		private static Forward GetForwardElement(XmlNode node)
		{
			var forward = new Forward();

			var forwardNode = node.SelectSingleNode("duration");

			if (forwardNode != null)
			{
				forward.Duration = Convert.ToInt32(forwardNode.InnerText);
			}

			return forward;
		}

		private static Backup GetBackupElement(XmlNode node)
		{
			var backup = new Backup();

			var backupNode = node.SelectSingleNode("duration");

			if (backupNode != null)
			{
				backup.Duration = Convert.ToInt32(backupNode.InnerText);
			}

			return backup;
		}

		private static Harmony GetHarmony(XmlNode node)
		{
			var harmony = new Harmony();

			var rootNode = node.SelectSingleNode("root");
			if (rootNode != null) {
				var rootStepNode = rootNode.SelectSingleNode("root-step");
				if (rootStepNode != null)
					harmony.RootStep = rootStepNode.InnerText;

				var rootAlterNode = rootNode.SelectSingleNode("root-alter");
				if (rootAlterNode != null)
					harmony.RootAlter = Convert.ToInt32(rootAlterNode.InnerText);
			}

			var kindNode = node.SelectSingleNode("kind");
			if (kindNode != null)
				harmony.Kind = kindNode.InnerText;

			return harmony;
		}

		private static Note GetNote(XmlNode noteNode)
		{
			var note = new Note();

			var typeNode = noteNode.SelectSingleNode("type");
			if (typeNode != null)
				note.Type = typeNode.InnerText;

			var voiceNode = noteNode.SelectSingleNode("voice");
			if (voiceNode != null)
				note.Voice = Convert.ToInt32(voiceNode.InnerText);

			var durationNode = noteNode.SelectSingleNode("duration");
			if (durationNode != null)
				note.Duration = Convert.ToInt32(durationNode.InnerText);

			note.Lyric = GetLyric(noteNode);

			note.Pitch = GetPitch(noteNode);

			note.Notations = GetNotations(noteNode);

			var staffNode = noteNode.SelectSingleNode("staff");
			if (staffNode != null)
				note.Staff = Convert.ToInt32(staffNode.InnerText);

			var chordNode = noteNode.SelectSingleNode("chord");
			if (chordNode != null)
				note.IsChordTone = true;
			
			var restNode = noteNode.SelectSingleNode("rest");
			if (restNode != null)
				note.IsRest = true;

			return note;
		}

		private static Pitch GetPitch(XmlNode noteNode)
		{
			var pitch = new Pitch();
			var pitchNode = noteNode.SelectSingleNode("pitch");
			if (pitchNode != null)
			{
				var stepNode = pitchNode.SelectSingleNode("step");
				if (stepNode != null)
					pitch.Step = stepNode.InnerText[0];

				var alterNode = pitchNode.SelectSingleNode("alter");
				if (alterNode != null)
					pitch.Alter = Convert.ToInt32(alterNode.InnerText);

				var octaveNode = pitchNode.SelectSingleNode("octave");
				if (octaveNode != null)
					pitch.Octave = Convert.ToInt32(octaveNode.InnerText);
			}
			else
			{
				return null;
			}

			return pitch;
		}

		private static Notations GetNotations(XmlNode noteNode)
		{
			var notations = new Notations();
			var notationsNode = noteNode.SelectSingleNode("notations");
			if (notationsNode != null)
			{
				var technicalNode = notationsNode.SelectSingleNode("technical");
				if (technicalNode != null)
					notations.Technical = GetTechnical(technicalNode);

				var gp7Node = notationsNode.SelectSingleNode("//processing-instruction(\"GP7\")") as XmlProcessingInstruction;
				if (gp7Node != null) {
					XmlDocument doc = new XmlDocument ();
					doc.LoadXml (gp7Node.Data);

					if (doc != null) {
						var rootNode = doc.SelectSingleNode ("root");
						if (rootNode != null)
							notations.Root = GetRoot (rootNode);
					}
				}

				var articulationsNode = notationsNode.SelectSingleNode("articulations");
				if (articulationsNode != null)
					notations.Articulations = articulationsNode.InnerXml;

				var dynamicsNode = notationsNode.SelectSingleNode("dynamics");
				if (dynamicsNode != null)
					notations.Dynamics = dynamicsNode.InnerXml;
			}
			else
			{
				return null;
			}

			return notations;
		}

		private static Technical GetTechnical(XmlNode node)
		{
			var technical = new Technical();

			if (node != null)
			{
				var fretNode = node.SelectSingleNode("fret");
				if (fretNode != null)
					technical.Fret = Convert.ToInt32(fretNode.InnerText);

				var stringNode = node.SelectSingleNode("string");
				if (stringNode != null)
					technical.String = Convert.ToInt32(stringNode.InnerText);
			}
			else
			{
				return null;
			}

			return technical;
		}

		private static Root GetRoot(XmlNode node)
		{
			var Root = new Root();

			if (node != null)
			{
				var brushNode = node.SelectSingleNode("brush");
				if (brushNode != null)
					Root.Brush = brushNode.Attributes["type"].InnerText;
			}
			else
			{
				return null;
			}

			return Root;
		}

		private static Lyric GetLyric(XmlNode noteNode)
		{
			var lyric = new Lyric();

			var lyricNode = noteNode.SelectSingleNode("lyric");
			if (lyricNode != null)
			{
				var syllabicNode = lyricNode.SelectSingleNode("syllabic");

				var syllabicText = string.Empty;

				if (syllabicNode != null)
					syllabicText = syllabicNode.InnerText;

				switch (syllabicText)
				{
					case "":
						lyric.Syllabic = Syllabic.None;
						break;
					case "begin":
						lyric.Syllabic = Syllabic.Begin;
						break;
					case "single":
						lyric.Syllabic = Syllabic.Single;
						break;
					case "end":
						lyric.Syllabic = Syllabic.End;
						break;
					case "middle":
						lyric.Syllabic = Syllabic.Middle;
						break;
				}

				var textNode = lyricNode.SelectSingleNode("text");
				if (textNode != null)
					lyric.Text = textNode.InnerText;
			}
			return lyric;
		}

		private static Clef GetClef(XmlNode attributesNode)
		{
			var clef = new Clef();

			var clefNode = attributesNode.SelectSingleNode("clef");

			if (clefNode != null)
			{
				var lineNode = clefNode.SelectSingleNode("line");
				if (lineNode != null)
					clef.Line = Convert.ToInt32(lineNode.InnerText);

				var signNode = clefNode.SelectSingleNode("sign");
				if (signNode != null)
					clef.Sign = signNode.InnerText;
			}
			return clef;
		}

		private static Time GetTime(XmlNode attributesNode)
		{
			var time = new Time();

			var timeNode = attributesNode.SelectSingleNode("time");
			if (timeNode != null)
			{
				var beatsNode = timeNode.SelectSingleNode("beats");

				if (beatsNode != null)
					time.Beats = Convert.ToInt32(beatsNode.InnerText);

				var beatTypeNode = timeNode.SelectSingleNode("beat-type");

				if (beatTypeNode != null)
					time.Mode = beatTypeNode.InnerText;

				var symbol = TimeSymbol.Normal;

				if (timeNode.Attributes != null)
				{
					var symbolAttribute = timeNode.Attributes["symbol"];

					if (symbolAttribute != null)
					{
						switch (symbolAttribute.InnerText)
						{
							case "common":
								symbol = TimeSymbol.Common;
								break;
							case "cut":
								symbol = TimeSymbol.Cut;
								break;
							case "single-number":
								symbol = TimeSymbol.SingleNumber;
								break;
						}
					}
				}

				time.Symbol = symbol;
			}
			return time;
		}

		private static Identification GetIdentification(XmlNode document)
		{
			var identificationNode = document.SelectSingleNode("score-partwise/identification");

			if (identificationNode != null)
			{
				var identification = new Identification();

				var composerNode = identificationNode.SelectSingleNode("creator[@type='composer']");
				identification.Composer = composerNode != null ? composerNode.InnerText : string.Empty;

				var rightsNode = identificationNode.SelectSingleNode("rights");
				identification.Rights = rightsNode != null ? rightsNode.InnerText : string.Empty;

				identification.Encoding = GetEncoding(identificationNode);

				return identification;
			}

			return null;
		}

		private static Encoding GetEncoding(XmlNode identificationNode)
		{
			var encodingNode = identificationNode.SelectSingleNode("encoding");

			var encoding = new Encoding();

			if (encodingNode != null)
			{
				encoding.Software = GetInnerTextOfChildTag(encodingNode, "software");

				encoding.Description = GetInnerTextOfChildTag(encodingNode, "encoding-description");

				var encodingDate = encodingNode.SelectSingleNode("encoding-date");
				if (encodingDate != null)
					encoding.EncodingDate = Convert.ToDateTime(encodingDate.InnerText);
			}

			return encoding;
		}

		private static string GetInnerTextOfChildTag(XmlNode encodingNode, string tagName)
		{
			var softwareStringBuilder = new StringBuilder();
			
			var encodingSoftwareNodes = encodingNode.SelectNodes(tagName);

			if (encodingSoftwareNodes != null)
			{
				foreach (XmlNode node in encodingSoftwareNodes)
				{
					softwareStringBuilder.AppendLine(node.InnerText);
				}
			}

			return softwareStringBuilder.ToString();
		}

		private static XmlDocument GetXmlDocument(string filename)
		{
			var document = new XmlDocument();

			var xml = GetFileContents(filename);
			document.XmlResolver = null;
			document.LoadXml(xml);

			return document;
		}

		private static string GetFileContents(string filename)
		{
			using (var fileStream = new FileStream(filename, FileMode.Open))
			using (var streamReader = new StreamReader(fileStream))
			{
				return streamReader.ReadToEnd();
			}
		}
	}
}
