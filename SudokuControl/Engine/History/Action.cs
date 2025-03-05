namespace Sudoku.Engine.History {
	public abstract class Action {
		public abstract void Undo();
		public abstract void Redo();

	}
}