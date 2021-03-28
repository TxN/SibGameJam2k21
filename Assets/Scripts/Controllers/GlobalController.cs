using System.Collections.Generic;
using UnityEngine;

using SMGCore;

namespace Game {
	public sealed class GlobalController : MonoSingleton<GlobalController> {
		protected override void Awake() {
			base.Awake();
			DontDestroyOnLoad(this.gameObject);
		}

		public void StartGame() {

		}

		public void Quit() {

		}
	}
}
