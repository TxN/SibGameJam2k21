using SMGCore;

namespace Game {
	public sealed class GamePersistence : PersistentDataHolder {
		public bool IsWin = false;
		public GameResult Result;
		public int LevelIndex = 0;
	}
}
