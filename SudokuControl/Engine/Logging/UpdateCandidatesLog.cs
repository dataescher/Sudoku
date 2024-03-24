using System.Collections.Generic;

namespace Sudoku.Engine.Logging {
	namespace Sudoku.Engine.Logging {
		public class UpdateCandidatesLog : LogItem {
			private readonly Dictionary<Unit, Dictionary<Cell, List<Cell>>> _candidatesEliminated;
			public UpdateCandidatesLog(Grid grid, Dictionary<Unit, Dictionary<Cell, List<Cell>>> candidatesEliminated) : base(grid) {
				if (candidatesEliminated == null) {
					_candidatesEliminated = null;
				} else {
					_candidatesEliminated = [];
					foreach (KeyValuePair<Unit, Dictionary<Cell, List<Cell>>> thisCandidateEliminated in candidatesEliminated) {
						Dictionary<Cell, List<Cell>> thisNewCandidateEliminated = [];
						foreach (KeyValuePair<Cell, List<Cell>> thisElimination in thisCandidateEliminated.Value) {
							List<Cell> thisNewElimination = [];
							foreach (Cell thisCell in thisElimination.Value) {
								thisNewElimination.Add(_grid.Cells[thisCell.Index]);
							}
							thisNewCandidateEliminated.Add(_grid.Cells[thisElimination.Key.Index], thisNewElimination);
						}
						_candidatesEliminated.Add(_grid.Units[thisCandidateEliminated.Key.UnitIndex - 1], thisNewCandidateEliminated);
					}
				}
			}
			public override LogDetail Details {
				get {
					LogDetail details;
					if (_candidatesEliminated is null) {
						details = new(this, "No candidates eliminated");
					} else {
						details = new(this, $"Candidates eliminated from {_candidatesEliminated.Count} units");
						foreach (KeyValuePair<Unit, Dictionary<Cell, List<Cell>>> thisCandidateEliminated in _candidatesEliminated) {
							LogDetail thisUnitDetails = new UnitDetail(this, thisCandidateEliminated.Key, GameColors.LightYellow);
							details.SubDetails.Add(thisUnitDetails);
							foreach (KeyValuePair<Cell, List<Cell>> thisElimination in thisCandidateEliminated.Value) {
								foreach (Cell thisCell in thisElimination.Value) {
									LogDetail thisEliminationDetails = new CellCandidateEliminationDetail(this, thisCandidateEliminated.Key, thisCell, thisElimination.Key);
									thisUnitDetails.SubDetails.Add(thisEliminationDetails);
								}
							}
						}
					}
					return details;
				}
			}

			public override void ApplyColoring() {
				// First, highlight just the affected units
				foreach (KeyValuePair<Unit, Dictionary<Cell, List<Cell>>> thisCandidateEliminated in _candidatesEliminated) {
					foreach (Cell thisCell in thisCandidateEliminated.Key.Cells) {
						thisCell.Color = GameColors.LightYellow;
					}
				}
				foreach (KeyValuePair<Unit, Dictionary<Cell, List<Cell>>> thisCandidateEliminated in _candidatesEliminated) {
					foreach (KeyValuePair<Cell, List<Cell>> thisElimination in thisCandidateEliminated.Value) {
						thisElimination.Key.Color = GameColors.LightGreen;
						foreach (Cell thisCell in thisElimination.Value) {
							thisCell.SetCandidateColor(thisElimination.Key.Value, GameColors.DarkRed);
						}
					}
				}
			}
		}
	}
}
