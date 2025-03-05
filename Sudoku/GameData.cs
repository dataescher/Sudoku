using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Sudoku {
	[XmlRoot("Puzzles")]
	public class GameData {
		public class Category {
			public class Puzzle {
				public String Description { get; set; }
				public String Board { get; set; }
				public Puzzle(String description) {
					Description = description;
				}
				public Puzzle() : this(String.Empty) { }
			}
			[XmlElement("Description")]
			public String Description;
			[XmlArray("Puzzles")]
			[XmlArrayItem("Puzzle")]
			public List<Puzzle> Puzzles;
			public Boolean ShouldSerializePuzzles() {
				return Puzzles.Count > 0;
			}

			[XmlArray("Categories")]
			[XmlArrayItem("Category")]
			public List<Category> Categories;
			public Boolean ShouldSerializeCategories() {
				return Categories.Count > 0;
			}

			public Category(String description) {
				Description = description;
				Puzzles = new();
				Categories = new();
			}
			public Category() : this(String.Empty) { }
		}
		[XmlElement("Description")]
		public String Description;
		[XmlArray("Categories")]
		[XmlArrayItem("Category")]
		public List<Category> Categories;
		public Boolean ShouldSerializeCategories() {
			return Categories.Count > 0;
		}

		public GameData() {
			Categories = new();
		}

		private static XmlSerializer Serializer { get; }
		static GameData() {
			Serializer = new(typeof(GameData));
		}
		public static GameData Load(String filename) {
			GameData result;
			using (TextReader reader = new StreamReader(filename)) {
				result = Serializer.Deserialize(reader) as GameData;
			}
			return result;
		}
		public void Save(String filename) {
			using (TextWriter writer = new StreamWriter(filename)) {
				Serializer.Serialize(writer, this);
			}
		}
	}
}