using System;

namespace Sudoku.Engine.History {
	public class ChangeCandidateColorAction : Action {
		public override void Undo() {
			Cell.SetCandidateColor(Candidate, ColorBefore);
		}
		public override void Redo() {
			Cell.SetCandidateColor(Candidate, ColorAfter);
		}
		public Char Candidate { get; }
		public Cell Cell { get; }
		public GameColors ColorBefore { get; }
		public GameColors ColorAfter { get; }
		public ChangeCandidateColorAction(Cell cell, Char candidate, GameColors colorBefore, GameColors colorAfter) {
			Candidate = candidate;
			Cell = cell;
			ColorBefore = colorBefore;
			ColorAfter = colorAfter;
		}
	}
}