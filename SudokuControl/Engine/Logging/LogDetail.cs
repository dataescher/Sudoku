using System;
using System.Collections.Generic;

namespace Sudoku.Engine.Logging {
	public class LogDetail {
		protected readonly LogItem _logItem;
		public virtual String Description { get; set; }
		public List<LogDetail> SubDetails { get; set; }
		public LogDetail(LogItem logItem, String description) {
			_logItem = logItem;
			Description = description;
			SubDetails = new();
		}
		public LogDetail(LogItem logItem) : this(logItem, String.Empty) { }
		public override String ToString() {
			return Description;
		}
		protected void ClearHighlighting() {
			_logItem._grid.ClearHighlighting();
		}
		public virtual void ShowItem(Grid grid) {
			_logItem.ShowGrid(grid);
		}
	}
}
