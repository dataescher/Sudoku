using System;

namespace Sudoku.Engine.Logging {
	public class GridLogDetail : LogDetail {
		public GridLogDetail(LogItem logItem, String gridString) : base(logItem) {
			Description = gridString;
		}

		public override void ShowItem(Grid grid) {
			grid.FromString(Description);
		}
	}
}
