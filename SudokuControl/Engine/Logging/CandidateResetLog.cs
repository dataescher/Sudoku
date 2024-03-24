using System;
using System.Collections.Generic;

namespace Sudoku.Engine.Logging {
	public class CandidateResetLog : LogItem {
		private readonly List<Tuple<Cell, Char>> _candidatesReset;
		public CandidateResetLog(Grid grid, List<Tuple<Cell, Char>> candidatesReset) : base(grid) {
			if (candidatesReset == null) {
				_candidatesReset = null;
			} else {
				_candidatesReset = [];
				foreach (Tuple<Cell, Char> thisCandidateReset in candidatesReset) {
					_candidatesReset.Add(new Tuple<Cell, Char>(_grid.Cells[thisCandidateReset.Item1.Index], thisCandidateReset.Item2));
				}
			}
		}
		public override LogDetail Details {
			get {
				LogDetail details;
				if (_candidatesReset == null) {
					details = new(this, "No candidates were reset");
				} else {
					details = new(this, $"{_candidatesReset.Count} candidates reset");
					foreach (Tuple<Cell, Char> thisCandidateReset in _candidatesReset) {
						details.SubDetails.Add(new CellCandidateDetail(this, thisCandidateReset.Item1, thisCandidateReset.Item2, GameColors.DarkGreen));
					}
				}
				return details;
			}
		}

		public override void ApplyColoring() {
			if (_candidatesReset is not null) {
				foreach (Tuple<Cell, Char> thisCandidateReset in _candidatesReset) {
					thisCandidateReset.Item1.SetCandidateColor(thisCandidateReset.Item2, GameColors.DarkGreen);
				}
			}
		}
	}
}
