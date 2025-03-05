using Sudoku.Engine.Logging;
using System;

namespace Sudoku.Engine.Solver {
	public abstract class SolveEngine {
		protected readonly Grid _grid;
		public SolveEngine(Grid grid) {
			_grid = grid;
		}
		public abstract Int32 Solve(Boolean solveMultiple, LogItemGroup log, Boolean modifyGrid = true);
	}
}