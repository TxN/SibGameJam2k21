using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

namespace Game {
	public sealed class ExclusionDisplay : MonoBehaviour {
		public List<Transform> ExclusionParents = new List<Transform>();

		public void SetupExclusion(Visitor visitorFab) {
			visitorFab.SetupForDisplay();
			var parent = ExclusionParents[0];
			foreach ( var item in ExclusionParents ) {
				if ( item.transform.childCount == 0 ) {
					parent = item;
					break;
				}				
			}
			visitorFab.transform.SetParent(parent, false);
			visitorFab.transform.localPosition = Vector3.zero;
			var initScale = parent.localScale;
			parent.localScale = Vector3.zero;
			parent.DOScale(initScale, 0.5f);
		}

		public bool HasFreePlaces() {
			foreach ( var item in ExclusionParents ) {
				if ( item.transform.childCount == 0 ) {
					return true;
				}
			}
			return false;
		}
	}
}
