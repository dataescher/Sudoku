namespace Sudoku.Engine.Logging {
	public class CellDetail : LogDetail {
		private readonly Cell _cell;
		private readonly GameColors _color;
		public CellDetail(LogItem logItem, Cell cell, GameColors color = GameColors.LightYellow) : base(logItem) {
			Description = cell.ToString();
			_cell = cell;
			_color = color;
		}
		public override void ShowItem(Grid grid) {
			ClearHighlighting();
			_cell.Color = _color;
			base.ShowItem(grid);
		}
	}
}