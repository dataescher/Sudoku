using System;

namespace Sudoku.Engine.History {
	public class ChangeCellValueAction : Action {
		public override void Undo() {
			Cell.Value = ValueBefore;
		}
		public override void Redo() {
			Cell.Value = ValueAfter;
		}
		public Char ValueBefore { get; }
		public Char ValueAfter { get; }
		public Cell Cell { get; }
		public ChangeCellValueAction(Cell cell, Char valueBefore, Char valueAfter) {
			Cell = cell;
			ValueBefore = valueBefore;
			ValueAfter = valueAfter;
		}
	}
}