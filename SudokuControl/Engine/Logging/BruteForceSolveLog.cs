using System;
using System.Collections.Generic;

namespace Sudoku.Engine.Logging {
	public class BruteForceSolveLog : LogItem {
		private readonly List<String> _solutions;
		public BruteForceSolveLog(Grid grid, List<String> solutions) : base(grid) {
			_solutions = solutions;
		}

		public override LogDetail Details {
			get {
				LogDetail details;
				if ((_solutions is null) || (_solutions.Count == 0)) {
					details = new(this, "Brute force solve engine - No solutions found");
				} else {
					if (_solutions.Count == 1) {
						details = new(this, $"Brute force solve engine - Unique solution found");
					} else {
						details = new(this, $"Brute force solve engine - {_solutions.Count} solutions found");
					}
					foreach (String solution in _solutions) {
						details.SubDetails.Add(new GridLogDetail(this, solution));
					}
				}
				return details;
			}
		}

		public override void ApplyColoring() { }
	}
}