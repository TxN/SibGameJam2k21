using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game {
	[System.Serializable]
	public sealed class VisitorDescription {
		public string Name;
		public List<VisitorTrait> Traits         = new List<VisitorTrait>();
		public Archetype Archetype;

		public override string ToString() {
			return $"{Name} {Archetype}, Hat: {Traits.Contains(VisitorTrait.Crown) || Traits.Contains(VisitorTrait.Fedora) || Traits.Contains(VisitorTrait.Plunger) }, Mismatch: {Traits.Contains(VisitorTrait.IdentityMismatch)} ";
		}
	}

	public sealed class Visitor : MonoBehaviour {
		public VisitorDescription Description;
		public Sprite             MainSprite = null;
		public Transform          HatAttachment;

		RectTransform _rt = null;

		public RectTransform RectTransform {
			get {
				if ( !_rt ) {
					_rt = GetComponent<RectTransform>();
				}
				return _rt;
			}
		}

		public void SetupAttachement(VisitorTrait trait, GameObject attachmentGO) {
			if ( trait.IsHatTrait() ) {
				attachmentGO.transform.SetParent(HatAttachment, false);
			} else {
				attachmentGO.transform.SetParent(transform, false);
			}			
		}

	}
}

