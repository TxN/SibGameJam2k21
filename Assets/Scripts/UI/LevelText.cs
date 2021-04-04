using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using DG.Tweening;
using SMGCore;

namespace Game {
	public class LevelText : MonoBehaviour {
		public TMP_Text Text = null;

		private void Start() {
			var p = ScenePersistence.Get<GamePersistence>();
			var levelText = $"Уровень {p.LevelIndex +1}";
			
			Text.text = levelText;

			var seq = DOTween.Sequence();
			var initScale = Text.transform.localScale;
			Text.transform.localScale = Vector3.zero;
			seq.Append(Text.transform.DOScale(initScale, 0.5f));
			seq.AppendInterval(1.7f);
			seq.Append(Text.transform.DOScale(Vector3.zero, 0.5f));
			seq.AppendCallback(() => {
				Text.gameObject.SetActive(false);
			});

		}
	}

}
