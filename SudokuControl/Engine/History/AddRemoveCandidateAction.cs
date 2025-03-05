using System;

namespace Sudoku.Engine.History {
	public class AddRemoveCandidateAction : Action {
		public override void Undo() {
			if (Remove) {
				Cell.AddCandidate(Candidate);
			} else {
				Cell.RemoveCandidate(Candidate);
			}
		}
		public override void Redo() {
			if (Remove) {
				Cell.RemoveCandidate(Candidate);
			} else {
				Cell.AddCandidate(Candidate);
			}
		}
		public Char Candidate { get; }
		public Cell Cell { get; }
		public Boolean Remove { get; }
		public AddRemoveCandidateAction(Cell cell, Char candidate, Boolean remove) {
			Candidate = candidate;
			Cell = cell;
			Remove = remove;
		}
	}
}