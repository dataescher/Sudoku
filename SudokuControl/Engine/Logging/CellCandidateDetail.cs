using System;

namespace Sudoku.Engine.Logging {
	public class CellCandidateDetail : LogDetail {
		private readonly Cell _cell;
		private readonly Char _value;
		private readonly GameColors _color;
		public CellCandidateDetail(LogItem logItem, Cell cell, Char value, GameColors color = GameColors.LightYellow) : base(logItem) {
			Description = $"{cell}: {value}";
			_cell = cell;
			_value = value;
			_color = color;
		}
		public override void ShowItem(Grid grid) {
			ClearHighlighting();
			_cell.AddCandidate(_value);
			_cell.SetCandidateColor(_value, _color);
			base.ShowItem(grid);
		}
	}
}
