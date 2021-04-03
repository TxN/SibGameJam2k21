using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Game.Events;

using SMGCore.EventSys;

namespace Game {
	public sealed class DocumentCollider : MonoBehaviour, IStampable {
		public DocumentController Owner;

		public void Stamp(StampType type) {
			Owner.Stamp(type);
		}
	}
}

