using System;
using System.Collections.Generic;

namespace Sudoku.Engine.Logging {
	public class PointingSubsetLog : LogItem {
		private readonly Box _box;
		private readonly Unit _intersectingUnit;
		private readonly Char _candidate;
		private readonly List<Cell> _pointingCandidateCells;
		private readonly List<Cell> _eliminatedCandidateCells;
		public PointingSubsetLog(Grid grid, Box box, Unit intersectingUnit, Char candidate, List<Cell> evaluatedCells, List<Cell> eliminatedCandidateCells) : base(grid) {
			_box = _grid.Boxes[box.Index - 1];
			_intersectingUnit = _grid.Units[intersectingUnit.UnitIndex - 1];
			_candidate = candidate;
			_pointingCandidateCells = new();
			foreach (Cell thisCell in evaluatedCells) {
				_pointingCandidateCells.Add(_grid.Cells[thisCell.Index]);
			}
			_eliminatedCandidateCells = new();
			foreach (Cell thisCell in eliminatedCandidateCells) {
				_eliminatedCandidateCells.Add(_grid.Cells[thisCell.Index]);
			}
		}
		public override LogDetail Details {
			get {
				LogDetail details = new(this, $"Pointing {setNumbering[_pointingCandidateCells.Count - 1]}");
				details.SubDetails.Add(new UnitDetail(this, _box));
				details.SubDetails.Add(new(this, $"Intersecting Unit: {_intersectingUnit}"));
				details.SubDetails.Add(new(this, $"Candidate: {_candidate}"));
				LogDetail evaluatedCellsDetail = new(this, "Evaluated Cells");
				details.SubDetails.Add(evaluatedCellsDetail);
				foreach (Cell evaluatedCell in _pointingCandidateCells) {
					evaluatedCellsDetail.SubDetails.Add(new CellCandidateDetail(this, evaluatedCell, _candidate, GameColors.DarkGreen));
				}
				LogDetail eliminatedCandidates = new(this, "Elminated candidates");
				details.SubDetails.Add(eliminatedCandidates);
				foreach (Cell eliminatedCandidateCell in _eliminatedCandidateCells) {
					eliminatedCandidates.SubDetails.Add(new CellCandidateDetail(this, eliminatedCandidateCell, _candidate, GameColors.DarkRed));
				}
				return details;
			}
		}
		public override void ApplyColoring() {
			foreach (Cell thisCell in _box.Cells) {
				thisCell.Color = GameColors.LightGreen;
			}
			foreach (Cell thisCell in _intersectingUnit.Cells) {
				thisCell.Color = GameColors.LightRed;
			}
			foreach (Cell thisCell in _box.IntersectingCells(_intersectingUnit)) {
				thisCell.Color = GameColors.LightYellow;
			}
			foreach (Cell thisCell in _pointingCandidateCells) {
				thisCell.SetCandidateColor(_candidate, GameColors.DarkGreen);
			}
			foreach (Cell thisCell in _eliminatedCandidateCells) {
				thisCell.SetCandidateColor(_candidate, GameColors.DarkRed);
			}
		}
	}
}