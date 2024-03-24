using System;
using System.Collections.Generic;

namespace Sudoku.Engine.Logging {
	public class LockedCandidateLog : LogItem {
		private readonly Unit _lockedUnit;
		private readonly Unit _intersectingUnit;
		private readonly Char _candidate;
		private readonly List<Cell> _lockedCells;
		private readonly List<Cell> _removeCells;
		public LockedCandidateLog(Grid grid, Unit lockedUnit, Unit intersectingUnit, Char candidate, List<Cell> lockedCells, List<Cell> removeCells) : base(grid) {
			_lockedUnit = _grid.Units[lockedUnit.UnitIndex - 1];
			_intersectingUnit = _grid.Units[intersectingUnit.UnitIndex - 1];
			_candidate = candidate;
			_lockedCells = [];
			foreach (Cell lockedCell in lockedCells) {
				_lockedCells.Add(_grid.Cells[lockedCell.Index]);
			}
			_removeCells = [];
			foreach (Cell removeCell in removeCells) {
				_removeCells.Add(_grid.Cells[removeCell.Index]);
			}
		}
		public override LogDetail Details {
			get {
				String type;
				if (_lockedUnit is Box) {
					type = "Pointing";
				} else if (_intersectingUnit is Box) {
					type = "Claiming";
				} else if (_lockedUnit is Row) {
					type = "Row";
				} else {
					type = "Column";
				}
				LogDetail details = new(this) {
					Description = $"Locked {setNumbering[_lockedCells.Count - 1]} - {type}"
				};
				details.SubDetails.Add(new(this, $"Candidate: {_candidate}"));
				details.SubDetails.Add(
					new(this, "Locked unit") {
						SubDetails = [new UnitDetail(this, _lockedUnit)]
					}
				);
				details.SubDetails.Add(
					new(this, "Intersecting unit") {
						SubDetails = [new UnitDetail(this, _intersectingUnit)]
					}
				);
				LogDetail lockCellDetails = new(this, "Locked candidate cells");
				details.SubDetails.Add(lockCellDetails);
				foreach (Cell thisLockedCell in _lockedCells) {
					lockCellDetails.SubDetails.Add(new CellCandidateDetail(this, thisLockedCell, _candidate, GameColors.DarkGreen));
				}
				LogDetail removeCellDetails = new(this, "Remove candidate cells");
				details.SubDetails.Add(removeCellDetails);
				foreach (Cell thisRemoveCell in _removeCells) {
					removeCellDetails.SubDetails.Add(new CellCandidateDetail(this, thisRemoveCell, _candidate, GameColors.DarkRed));
				}
				return details;
			}
		}

		public override void ApplyColoring() {
			foreach (Cell thisCell in _lockedUnit.Cells) {
				thisCell.Color = GameColors.LightGreen;
			}
			foreach (Cell thisCell in _intersectingUnit.Cells) {
				thisCell.Color = GameColors.LightRed;
			}
			foreach (Cell thisCell in _lockedUnit.IntersectingCells(_intersectingUnit)) {
				thisCell.Color = GameColors.LightYellow;
			}
			foreach (Cell thisCell in _lockedCells) {
				thisCell.SetCandidateColor(_candidate, GameColors.DarkGreen);
			}
			foreach (Cell thisCell in _removeCells) {
				thisCell.SetCandidateColor(_candidate, GameColors.DarkRed);
			}
		}
	}
}
