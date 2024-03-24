using System;
using System.Collections.Generic;

namespace Sudoku.Engine.Logging {
	public class HiddenSubsetLog : LogItem {
		private readonly Unit _unit;
		private readonly List<Tuple<Char, List<Cell>>> _groups;
		private readonly List<Tuple<Cell, Char>> _eliminatedCandidates;
		public HiddenSubsetLog(Grid grid, Unit unit, List<Tuple<Char, List<Cell>>> groups, List<Tuple<Cell, Char>> eliminatedCandidates) : base(grid) {
			_unit = _grid.Units[unit.UnitIndex - 1];
			_groups = [];
			foreach (Tuple<Char, List<Cell>> thisGroup in groups) {
				List<Cell> thisGroupCells = [];
				foreach (Cell thisCell in thisGroup.Item2) {
					thisGroupCells.Add(_grid.Cells[thisCell.Index]);
				}
				_groups.Add(new Tuple<Char, List<Cell>>(thisGroup.Item1, thisGroupCells));
			}
			_eliminatedCandidates = [];
			foreach (Tuple<Cell, Char> thisEliminatedCandidate in eliminatedCandidates) {
				_eliminatedCandidates.Add(new Tuple<Cell, Char>(_grid.Cells[thisEliminatedCandidate.Item1.Index], thisEliminatedCandidate.Item2));
			}
		}
		public override LogDetail Details {
			get {
				LogDetail details = new(this, $"Hidden {setNumbering[_groups.Count - 1]}");
				details.SubDetails.Add(new(this, $"Unit: {_unit}"));
				LogDetail groupDetails = new(this, "Groups");
				details.SubDetails.Add(groupDetails);
				foreach (Tuple<Char, List<Cell>> thisGroup in _groups) {
					LogDetail groupCandidateDetails = new(this, $"Candidate {thisGroup.Item1}");
					groupDetails.SubDetails.Add(groupCandidateDetails);
					foreach (Cell thisGroupCell in thisGroup.Item2) {
						LogDetail groupCandidateCellDetails = new CellCandidateDetail(this, thisGroupCell, thisGroup.Item1, GameColors.DarkGreen);
						groupCandidateDetails.SubDetails.Add(groupCandidateCellDetails);
					}
				}
				LogDetail eliminatedCandidateDetails = new(this, "Eliminated Candidates");
				details.SubDetails.Add(eliminatedCandidateDetails);
				foreach (Tuple<Cell, Char> thisEliminatedCandidate in _eliminatedCandidates) {
					eliminatedCandidateDetails.SubDetails.Add(new CellCandidateDetail(this, thisEliminatedCandidate.Item1, thisEliminatedCandidate.Item2, GameColors.DarkRed));
				}
				return details;
			}
		}
		public override void ApplyColoring() {
			foreach (Cell thisCell in _unit.Cells) {
				thisCell.Color = GameColors.LightYellow;
			}
			foreach (Tuple<Char, List<Cell>> thisGroup in _groups) {
				foreach (Cell thisCell in thisGroup.Item2) {
					thisCell.SetCandidateColor(thisGroup.Item1, GameColors.DarkGreen);
				}
			}
			foreach (Tuple<Cell, Char> thisEliminatedCandidate in _eliminatedCandidates) {
				thisEliminatedCandidate.Item1.SetCandidateColor(thisEliminatedCandidate.Item2, GameColors.DarkRed);
			}
		}
	}
}
