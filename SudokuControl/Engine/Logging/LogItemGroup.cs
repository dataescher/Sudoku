using System;
using System.Collections.Generic;

namespace Sudoku.Engine.Logging {
	public class LogItemGroup {
		public String Description { get; set; }
		public List<LogItem> Items { get; private set; }
		public LogItemGroup(String description = "") {
			Description = description;
			Items = [];
		}
	}
}
