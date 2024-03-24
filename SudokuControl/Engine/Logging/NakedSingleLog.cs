using System;
using System.Collections.Generic;

namespace Sudoku.Engine.Logging {
	public class NakedSingleLog : LogItem {
		private readonly List<Tuple<Char, Cell>> _nakedSingles;
		public NakedSingleLog(Grid grid, List<Tuple<Char, Cell>> nakedSingles) : base(grid) {
			_nakedSingles = [];
			foreach (Tuple<Char, Cell> thisNakedSingle in nakedSingles) {
				_nakedSingles.Add(new Tuple<Char, Cell>(thisNakedSingle.Item1, _grid.Cells[thisNakedSingle.Item2.Index]));
			}
		}
		public override LogDetail Details {
			get {
				LogDetail details = new(this);
				if (_nakedSingles.Count == 0) {
					details.Description = "No naked singles found.";
				} else {
					details.Description = $"Naked singles found: {_nakedSingles.Count}.";
					foreach (Tuple<Char, Cell> thisNakedSingle in _nakedSingles) {
						details.SubDetails.Add(new CellCandidateDetail(this, thisNakedSingle.Item2, thisNakedSingle.Item1, GameColors.DarkGreen));
					}
				}
				return details;
			}
		}

		public override void ApplyColoring() {
			foreach (Tuple<Char, Cell> thisCell in _nakedSingles) {
				thisCell.Item2.SetCandidateColor(thisCell.Item1, GameColors.DarkGreen);
			}
		}
	}
}
