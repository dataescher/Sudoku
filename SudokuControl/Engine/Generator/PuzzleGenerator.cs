using System;
using System.Collections.Generic;

namespace Sudoku.Engine {
	public partial class Grid {

		public static void Shuffle(List<Char> list) {
			Random rnd = new(Environment.TickCount);
			Int32 n = list.Count;
			while (n > 1) {
				n--;
				Int32 k = rnd.Next(n + 1);
				(list[n], list[k]) = (list[k], list[n]);
			}
		}
		public static Grid GeneratePuzzle(Int32 boxWidth, Int32 boxHeight) {
			Grid grid = new(boxWidth, boxHeight);
			CandidateGenerator candidateGenerator = new(grid);
			_ = candidateGenerator.Solve(true, null, true);
			_ = GenerateRandomGrid(ref grid);
			// Now, try to eliminate as many values as possible with the grid still having one unique solution
			List<Cell> valueCells = [];
			foreach (Cell thisCell in grid.Cells) {
				if (!thisCell.Empty) {
					valueCells.Add(thisCell);
				}
			}
			Random rnd = new();
			for (Int32 attemptIdx = 0; attemptIdx < valueCells.Count; attemptIdx++) {
				Int32 cellToEvaluateIdx = rnd.Next(0, valueCells.Count);
				Cell cellToEvaluate = valueCells[cellToEvaluateIdx];
				Char backupValue = cellToEvaluate._value;
				cellToEvaluate._value = Cell.EmptyChar;
				List<String> solutions = [];
				Grid testGrid = grid.Clone() as Grid;
				Int32 bruteForceMaxTries = -1;
				BruteForceSolveEngine bruteForceSolveEngine = new(testGrid, 2, bruteForceMaxTries);
				_ = bruteForceSolveEngine.TrySolveBruteForce(ref solutions, 2, ref bruteForceMaxTries);
				if (solutions.Count != 1) {
					// Value removal leads to multiple solutions
					cellToEvaluate._value = backupValue;
				}
				valueCells.RemoveAt(cellToEvaluateIdx);
			}
			// Clean up the grid
			foreach (Cell thisCell in grid.Cells) {
				thisCell.ClearCandidates();
				thisCell.UserCell = false;
			}
			return grid;
		}
		internal static Boolean GenerateRandomGrid(ref Grid grid) {
			Boolean result = false;
			Int32 dimension = grid._boxWidth * grid._boxHeight;
			Random rnd = new();
			// First, just populate the first row with all the candidates in order
			foreach (Row row in grid.Rows) {
				foreach (Cell cell in row.Cells) {
					Int32 offset = (cell.Column.Index - 1 + ((cell.Row.Index - 1) % grid._boxHeight * grid._boxWidth) + ((cell.Row.Index - 1) / grid._boxHeight)) % dimension;
					cell._value = grid.Candidates[offset];
				}
			}
			// Now make every row a shifted version of these cells
			for (Int32 iteration = 0; iteration < 1000; iteration++) {
				Box box = grid.Boxes[rnd.Next(0, dimension)];
				// Swap two of the rows, two of the columns
				Column col1 = box.Columns[rnd.Next(0, grid._boxWidth)];
				Column col2 = box.Columns[rnd.Next(0, grid._boxWidth)];
				if (col1 != col2) {
					for (Int32 cellIdx = 0; cellIdx < dimension; cellIdx++) {
						(col2.Cells[cellIdx]._value, col1.Cells[cellIdx]._value) = (col1.Cells[cellIdx]._value, col2.Cells[cellIdx]._value);
					}
				}
				Row row1 = box.Rows[rnd.Next(0, grid._boxHeight)];
				Row row2 = box.Rows[rnd.Next(0, grid._boxHeight)];
				if (row1 != row2) {
					for (Int32 cellIdx = 0; cellIdx < dimension; cellIdx++) {
						(row2.Cells[cellIdx]._value, row1.Cells[cellIdx]._value) = (row1.Cells[cellIdx]._value, row2.Cells[cellIdx]._value);
					}
				}

				// Swap two of the rows, two of the columns
				Int32 boxCol1 = rnd.Next(0, grid._boxHeight);
				Int32 boxCol2 = rnd.Next(0, grid._boxHeight);
				if (boxCol1 != boxCol2) {
					for (Int32 colIdx = 0; colIdx < grid.BoxWidth; colIdx++) {
						for (Int32 rowIdx = 0; rowIdx < dimension; rowIdx++) {
							Cell cell1 = grid[colIdx + (boxCol1 * grid.BoxWidth), rowIdx];
							Cell cell2 = grid[colIdx + (boxCol2 * grid.BoxWidth), rowIdx];
							(cell2._value, cell1._value) = (cell1._value, cell2._value);
						}
					}
				}
				Int32 boxRow1 = rnd.Next(0, grid._boxWidth);
				Int32 boxRow2 = rnd.Next(0, grid._boxWidth);
				if (boxRow1 != boxRow2) {
					for (Int32 rowIdx = 0; rowIdx < grid.BoxHeight; rowIdx++) {
						for (Int32 colIdx = 0; colIdx < dimension; colIdx++) {
							Cell cell1 = grid[colIdx, rowIdx + (boxCol1 * grid.BoxHeight)];
							Cell cell2 = grid[colIdx, rowIdx + (boxCol2 * grid.BoxHeight)];
							(cell2._value, cell1._value) = (cell1._value, cell2._value);
						}
					}
				}
				// Swap two candidates on the grid
				Char candidate1 = grid.Candidates[rnd.Next(0, grid.Candidates.Length)];
				Char candidate2 = grid.Candidates[rnd.Next(0, grid.Candidates.Length)];
				if (candidate1 != candidate2) {
					foreach (Cell cell in grid.Cells) {
						if (cell._value == candidate1) {
							cell._value = candidate2;
						} else if (cell._value == candidate2) {
							cell._value = candidate1;
						}
					}
				}
			}
			return result;
		}
	}
}
