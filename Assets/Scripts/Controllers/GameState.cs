using UnityEngine;

using SMGCore;
using SMGCore.EventSys;

using Game.Events;

namespace Game {
	public sealed class GameState : MonoSingleton<GameState> {
		public readonly TimeController TimeController = new TimeController();

		public VisitorMechanic VisitorMechanic;
		public BreachController BreachController;
		public TimeLimit TimeLimiter;

		public bool IsDebug {
			get {
				return true; //Should be set to 'false' for release build.
			}
		}

		protected override void Awake() {
			base.Awake();
			if ( ScenePersistence.Instance.Data == null ) {
				ScenePersistence.Instance.SetupHolder(new GamePersistence());
			}
			EventManager.Subscribe<Game_Ended>(this, OnGameEnded);
		}

		private void Start() {
			var p = ScenePersistence.Get<GamePersistence>();
			var levelIndex = p.LevelIndex;
			var levelInfo = GameConstants.Levels[levelIndex];
			VisitorMechanic.Setup(Mathf.CeilToInt( levelInfo.PlannedCount * 1.5f), levelInfo.InitialExclusions, levelInfo.PlannedCount, levelInfo.BannedTraits);
			TimeLimiter.Setup(levelInfo.LevelTime);
		}

		void Update() {
			TimeController.Update(Time.deltaTime);
			HandleInput();
		}

		private void OnDestroy() {
			EventManager.Unsubscribe<Game_Ended>(OnGameEnded);
		}

		public void EndGame(bool win) {
			var pers = ScenePersistence.Get<GamePersistence>();
			pers.IsWin = win;
			var nextLevel = pers.LevelIndex + 1;

			var willStartNextLevel = win && GameConstants.Levels.Count > nextLevel;
			if ( win && willStartNextLevel ) {
				pers.LevelIndex = nextLevel;
				Invoke("LoadGameScene", 1f);
				return;
			}
			Invoke("LoadFinishScene", 1f);
		}

		void LoadFinishScene() {
			GlobalController.Instance.OpenFinalScene();
		}

		void LoadGameScene() {
			GlobalController.Instance.StartGame(false);
		}

		void HandleInput() {
			if ( IsDebug ) {
				if ( Input.GetKeyDown(KeyCode.P) ) {
					var pauseFlag = TimeController.AddOrRemovePause(this);
					Debug.LogFormat("Pause cheat used, new pause state is: {0}", pauseFlag);
				}
				if ( Input.GetKeyDown(KeyCode.U) ) {
					EventManager.Fire<Game_Ended>(new Game_Ended(true, GameResult.Win));
				}
				if ( Input.GetKeyDown(KeyCode.I) ) {
					EventManager.Fire<Game_Ended>(new Game_Ended(false, GameResult.WronglyYeeted));
				}
			}
		}

		void OnGameEnded(Game_Ended e) {
			EndGame(e.Win);
		}
	}
}
