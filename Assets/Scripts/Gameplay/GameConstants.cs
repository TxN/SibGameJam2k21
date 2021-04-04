using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game {
	public class LevelDescription {
		public float LevelTime = 150;
		public int InitialExclusions = 0;
		public int PlannedCount = 10;
		public List<VisitorTrait> BannedTraits = new List<VisitorTrait>();
		public bool ExclusionsEnabled = false;
		public int MaxCalls = 0;
	}
	public static class GameConstants  {
		public static List<LevelDescription> Levels = new List<LevelDescription> {
			new LevelDescription {
				LevelTime = 200,
				InitialExclusions = 1,
				PlannedCount = 12,
				ExclusionsEnabled = true,
				MaxCalls = 1,
				BannedTraits = new List<VisitorTrait> { VisitorTrait.Human, VisitorTrait.Mechanical }
			},
			new LevelDescription {
				LevelTime = 160f,
				InitialExclusions = 0,
				PlannedCount = 15,
				ExclusionsEnabled = true,
				MaxCalls = 2,
				BannedTraits = new List<VisitorTrait> { VisitorTrait.Human, VisitorTrait.Mechanical, VisitorTrait.Fedora, VisitorTrait.Crown, VisitorTrait.Plunger}
			},
			new LevelDescription {
				LevelTime = 140f,
				InitialExclusions = 1,
				PlannedCount = 17,
				ExclusionsEnabled = true,
				MaxCalls = 2,
				BannedTraits = new List<VisitorTrait> { VisitorTrait.Human, VisitorTrait.Mechanical, VisitorTrait.Demonic, VisitorTrait.Plunger, VisitorTrait.Crown, VisitorTrait.Fedora}
			}
		};
	}
}
