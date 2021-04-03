using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Game.Events;
using SMGCore.EventSys;

using DG.Tweening;

namespace Game {
	public class BreachController : MonoBehaviour {
		public float BreachSpawnInterval = 15f;
		[Range(0,100)]
		public int BreachSpawnChance = 40;

		public Transform BreachParent = null;

		public List<Transform> BreachSpawnPoints = new List<Transform>();
		public GameObject BreachPrefab = null;

		float _lastSpawnTime = 0f;
		bool _isBreachActive = false;
		void Start() {
			EventManager.Subscribe<Breach_Sealed>(this, OnBreachSealed);
			EventManager.Subscribe<Breach_Broken>(this, OnBreachBroken);
		}

		private void OnDestroy() {
			EventManager.Unsubscribe<Breach_Sealed>(OnBreachSealed);
			EventManager.Unsubscribe<Breach_Broken>(OnBreachBroken);
		}

		private void Update() {
			var ct = GameState.Instance.TimeController.CurrentTime;

			if ( ct - _lastSpawnTime > BreachSpawnInterval && !_isBreachActive ) {
				_lastSpawnTime = ct;
				RollSpawnBreach();
			}
		}

		void RollSpawnBreach() {

			if ( Random.Range(0, 100) < BreachSpawnChance) {
				SpawnBreach();
			}
		}

		void SpawnBreach() {
			if ( BreachSpawnPoints.Count == 0 ) {
				return;
			}
			var index = Random.Range(0, BreachSpawnPoints.Count);
			var spawnPoint = BreachSpawnPoints[index];
			BreachSpawnPoints.RemoveAt(index);

			var inst = Instantiate(BreachPrefab, BreachParent, false);
			inst.transform.position = spawnPoint.position;
			inst.gameObject.SetActive(true);
			_isBreachActive = true;
		}

		void OnBreachSealed(Breach_Sealed e ) {
			_isBreachActive = false;
		}

		void OnBreachBroken(Breach_Broken e) {
			var seq = DOTween.Sequence();
			seq.AppendInterval(5f);

			seq.AppendCallback(() => {
				EventManager.Fire(new Game_Ended(false, GameResult.BreachBroken));
			});
		}
	}
}

