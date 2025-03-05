namespace Sudoku.Engine.Logging {
	public class UnitDetail : LogDetail {
		private readonly Unit _unit;
		private readonly GameColors _color;
		public UnitDetail(LogItem logItem, Unit unit, GameColors color = GameColors.LightYellow) : base(logItem) {
			Description = unit.ToString();
			_unit = unit;
			_color = color;
		}
		public override void ShowItem(Grid grid) {
			ClearHighlighting();
			foreach (Cell cell in _unit.Cells) {
				cell.Color = _color;
			}
			base.ShowItem(grid);
		}
	}
}