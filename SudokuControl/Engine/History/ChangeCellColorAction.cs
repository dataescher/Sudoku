namespace Sudoku.Engine.History {
	public class ChangeCellColorAction : Action {
		public override void Undo() {
			Cell.Color = ColorBefore;
		}
		public override void Redo() {
			Cell.Color = ColorAfter;
		}
		public Cell Cell { get; }
		public GameColors ColorBefore { get; }
		public GameColors ColorAfter { get; }
		public ChangeCellColorAction(Cell cell, GameColors colorBefore, GameColors colorAfter) {
			Cell = cell;
			ColorBefore = colorBefore;
			ColorAfter = colorAfter;
		}
	}
}
