using Sudoku.Engine.Logging;
using System;
using System.Collections.Generic;

namespace Sudoku.Engine.Solver {
	public class DuplicateCheck : SolveEngine {
		public DuplicateCheck(Grid grid) : base(grid) { }
		public override Int32 Solve(Boolean solveMultiple, LogItemGroup log, Boolean modifyGrid = true) {
			Dictionary<Unit, Dictionary<Char, List<Cell>>> duplicates = null;
			Int32 duplicatesFound = 0;
			foreach (Unit unit in _grid.Units) {
				// This dictionary contains information for the first cell of a given value
				Dictionary<Char, Cell> unitCellData = [];
				foreach (Cell cell in unit.Cells) {
					if (!cell.Empty) {
						if (unitCellData.TryGetValue(cell.Value, out Cell duplicate)) {
							if (duplicates is null) {
								// Found the first duplicate
								duplicates = new() { { unit, new Dictionary<Char, List<Cell>>() { { cell.Value, new List<Cell>() { duplicate, cell } } } } };
							} else if (duplicates.TryGetValue(unit, out Dictionary<Char, List<Cell>> unitDuplicateData)) {
								if (unitDuplicateData.TryGetValue(cell.Value, out List<Cell> unitDuplicateCells)) {
									unitDuplicateCells.Add(cell);
								} else {
									unitDuplicateData.Add(cell.Value, [cell]);
								}
							} else {
								duplicates.Add(unit, new Dictionary<Char, List<Cell>>() { { cell.Value, new List<Cell>() { duplicate, cell } } });
							}
							duplicatesFound++;
							if (!solveMultiple) {
								break;
							}
						} else {
							// Duplicate has not been seen yet
							unitCellData.Add(cell.Value, cell);
						}
					}
				}
				if (!solveMultiple && (duplicates is not null)) {
					break;
				}
			}
			// Log the duplicates here
			log?.Items.Add(new DuplicateLog(_grid, duplicates));
			return duplicatesFound;
		}
	}
}
