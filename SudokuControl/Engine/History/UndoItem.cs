using System;
using System.Collections.Generic;

namespace Sudoku.Engine.History {
	public class UndoItem {
		internal readonly List<Action> _actions;
		public UndoItem() {
			_actions = new();
		}
		public void Add(Action action) {
			_actions.Add(action);
		}
		public Int32 Count => _actions.Count;
		public void Undo() {
			// Perform the undo actions in reverse order
			for (Int32 undoItemIdx = _actions.Count - 1; undoItemIdx >= 0; undoItemIdx--) {
				_actions[undoItemIdx].Undo();
			}
		}
		public void Redo() {
			for (Int32 undoItemIdx = 0; undoItemIdx < _actions.Count; undoItemIdx++) {
				_actions[undoItemIdx].Undo();
			}
		}
	}
}