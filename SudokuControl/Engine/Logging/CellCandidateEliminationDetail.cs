namespace Sudoku.Engine.Logging {
	internal class CellCandidateEliminationDetail : LogDetail {
		private readonly Unit _unit;
		private readonly Cell _containCell;
		private readonly Cell _eliminateCell;
		public CellCandidateEliminationDetail(LogItem logItem, Unit unit, Cell containCell, Cell eliminateCell) : base(logItem) {
			Description = $"{eliminateCell} candidate {containCell._value} due to {containCell}";
			_unit = unit;
			_containCell = containCell;
			_eliminateCell = eliminateCell;
		}
		public override void ShowItem(Grid grid) {
			base.ShowItem(grid);
			foreach (Cell cell in _unit.Cells) {
				cell.Color = GameColors.LightYellow;
			}
			_eliminateCell.SetCandidateColor(_containCell._value, GameColors.DarkRed);
			_containCell.Color = GameColors.LightGreen;
		}
	}
}
