﻿using System;
using NUnit.Framework;

namespace MusicXml.Unit.Tests
{
	[TestFixture]
    public class XScoreTests
    {
		private XScore _scoreWithStaffValues;

		[TestFixtureSetUp]
		public void SetUp()
		{
			_scoreWithStaffValues = new XScore("TestData/MusicXmlWithStaffValues.xml");
		}

		[Test]
		public void Populates_movement_title()
		{
			const string knownMovementTitle = "Im wunderschönen Monat Mai";
			Assert.That(_scoreWithStaffValues.MovementTitle, Is.EqualTo(knownMovementTitle));
		}

		[Test]
		public void Identification_is_not_null()
		{
			Assert.That(_scoreWithStaffValues.Identification, Is.Not.Null);
		}

		[Test]
		public void Populates_identification_composer()
		{
			const string knownComposer = "Robert Schumann";

			Assert.That(_scoreWithStaffValues.Identification.Composer, Is.EqualTo(knownComposer));
		}

		[Test]
		public void Populates_identification_rights()
		{
			const string knownRights = "Copyright © 2002 Recordare LLC";
			Assert.That(_scoreWithStaffValues.Identification.Rights, Is.EqualTo(knownRights));
		}

		[Test]
		public void Identification_encoding_is_not_null()
		{
			Assert.That(_scoreWithStaffValues.Identification.Encoding, Is.Not.Null);
		}

		[Test]
		public void Populates_encoding_description()
		{
			const string knownEncodingDescription = "This is a sample description\r\nacross multiple lines\r\n";

			Assert.That(_scoreWithStaffValues.Identification.Encoding.Description, Is.EqualTo(knownEncodingDescription));
		}

		[Test]
		public void Populates_encoding_date()
		{
			var knownEncodingDate = new DateTime(2011, 08, 08);

			Assert.That(_scoreWithStaffValues.Identification.Encoding.EncodingDate, Is.EqualTo(knownEncodingDate));
		}

		[Test]
		public void Populates_encoding_software()
		{
			const string knownEncodingSoftware = "Finale 2011 for Windows\r\nDolet 6.0 for Finale\r\n";

			Assert.That(_scoreWithStaffValues.Identification.Encoding.Software, Is.EqualTo(knownEncodingSoftware));
		}

		[Test]
		public void Parts_is_not_null()
		{
			Assert.That(_scoreWithStaffValues.Parts, Is.Not.Null);
		}

		[Test]
		public void Parts_contains_correct_number_of_parts()
		{
			const int knownNumberOfParts = 2;
			Assert.That(_scoreWithStaffValues.Parts.Count, Is.EqualTo(knownNumberOfParts));
		}

		[Test]
		public void Populates_part_id()
		{
			const string knownPartId = "P1";
			var part = _scoreWithStaffValues.Parts[0];

			Assert.That(part.Id, Is.EqualTo(knownPartId));
		}

		[Test]
		public void Populates_part_name()
		{
			const string knownPartName = "Voice";
			var part = _scoreWithStaffValues.Parts[0];

			Assert.That(part.Name, Is.EqualTo(knownPartName));
		}

		[Test]
		public void Part_measures_is_not_null()
		{
			var part = _scoreWithStaffValues.Parts[0];

			Assert.That(part.Measures, Is.Not.Null);
		}

		[Test]
		public void Part_contains_correct_number_of_measures()
		{
			var part = _scoreWithStaffValues.Parts[0];

			const int knownMeasuresCount = 27;

			Assert.That(part.Measures.Count, Is.EqualTo(knownMeasuresCount));
		}
		
		[Test]
		public void Populates_measure_width()
		{
			var part = _scoreWithStaffValues.Parts[0];
			var measure = part.Measures[0];

			const int knownWidth = 198;

			Assert.That(measure.Width, Is.EqualTo(knownWidth));
		}

		
		[Test]
		public void Measure_attributes_is_not_null()
		{
			var part = _scoreWithStaffValues.Parts[0];

			var measure = part.Measures[0];

			Assert.That(measure.Attributes, Is.Not.Null);
		}

		[Test]
		public void Populates_measure_divisions()
		{
			var part = _scoreWithStaffValues.Parts[0];

			var measure = part.Measures[0];

			const int knownMeasureDivisions = 8;

			Assert.That(measure.Attributes.Divisions, Is.EqualTo(knownMeasureDivisions));
		}

		[Test]
		public void Measure_clef_is_not_null()
		{
			var part = _scoreWithStaffValues.Parts[0];

			var measure = part.Measures[0];

			Assert.That(measure.Attributes.Clef, Is.Not.Null);
		}

		[Test]
		public void Populates_measure_clef_line()
		{
			var part = _scoreWithStaffValues.Parts[0];

			var measure = part.Measures[0];

			var clef = measure.Attributes.Clef;

			const int knownClefLine = 2;

			Assert.That(clef.Line, Is.EqualTo(knownClefLine));
		}

		[Test]
		public void Populates_measure_clef_sign()
		{
			var part = _scoreWithStaffValues.Parts[0];

			var measure = part.Measures[0];

			var clef = measure.Attributes.Clef;

			const string knownClefSign = "G";

			Assert.That(clef.Sign, Is.EqualTo(knownClefSign));
		}

		[Test]
		public void Key_is_not_null()
		{
			var part = _scoreWithStaffValues.Parts[0];

			var measure = part.Measures[0];

			var key = measure.Attributes.Key;

			Assert.That(key, Is.Not.Null);
		}

		[Test]
		public void Populates_key_fifths()
		{
			var part = _scoreWithStaffValues.Parts[0];

			var measure = part.Measures[0];

			var key = measure.Attributes.Key;

			const int knownFifths = 3;

			Assert.That(key.Fifths, Is.EqualTo(knownFifths));
		}
		
		[Test]
		public void Populates_key_mode()
		{
			var part = _scoreWithStaffValues.Parts[0];

			var measure = part.Measures[0];

			var key = measure.Attributes.Key;

			const string knownMode = "major";

			Assert.That(key.Mode, Is.EqualTo(knownMode));
		}

		[Test]
		public void Measure_time_is_not_null()
		{
			var part = _scoreWithStaffValues.Parts[0];

			var measure = part.Measures[0];

			var time = measure.Attributes.Time;

			Assert.That(time, Is.Not.Null);
		}

		[Test]
		public void Populates_time_beats()
		{
			var part = _scoreWithStaffValues.Parts[0];

			var measure = part.Measures[0];

			var time = measure.Attributes.Time;

			const int knownBeats = 2;

			Assert.That(time.Beats, Is.EqualTo(knownBeats));
		}

		[Test]
		public void Populates_time_mode()
		{
			var part = _scoreWithStaffValues.Parts[0];

			var measure = part.Measures[0];

			var time = measure.Attributes.Time;

			const string knownTimeMode = "4";

			Assert.That(time.Mode, Is.EqualTo(knownTimeMode));
		}
		
		[Test]
		public void Populates_note_staff()
		{
			const int knownStaffValue = 1;
			const int knownPartWithStaffTags = 1;
			const int knownMeasureWithStaffTags = 0;
			const int firstNoteIndex = 0;

			var score = new XScore("TestData/MusicXmlWithStaffValues.xml");

			var part = score.Parts[knownPartWithStaffTags];
			var measure = part.Measures[knownMeasureWithStaffTags];
			var note = measure.Notes[firstNoteIndex];

			Assert.That(note.Staff, Is.EqualTo(knownStaffValue));
		}

		[Test, Ignore]
		public void Is_chord_tone_true_for_note_with_chord_tag()
		{
			const int knownChordToneIndex = 1;

			var score = new XScore("TestData/MusicXmlWithChords.xml");

			var part = score.Parts[0];
			var measure = part.Measures[0];
			var note = measure.Notes[knownChordToneIndex];

			Assert.That(note.IsChordTone, Is.True);
		}

		[Test, Ignore]
		public void Is_chord_tone_false_for_note_without_chord_tag()
		{
			var score = new XScore("TestData/MusicXmlWithChords.xml");

			var part = score.Parts[0];
			var measure = part.Measures[0];
			var note = measure.Notes[0];

			Assert.That(note.IsChordTone, Is.False);
		}
    }
}
