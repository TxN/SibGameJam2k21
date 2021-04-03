namespace Game.Events {
	public struct VisitorPlace_Freed {

	}

	public struct Visitor_Entered {
		public VisitorDescription Description;

		public Visitor_Entered(VisitorDescription desc) {
			Description = desc;
		}
	}

	public struct Visitor_Spawned {
		public VisitorDescription Description;

		public Visitor_Spawned(VisitorDescription desc) {
			Description = desc;
		}
	}

	public struct Stamp_Taken {
		public IInteractable Object;
	}

	public struct Stamp_Dropped {
		public IInteractable Object;
	}

	public struct Document_Stamped {
		public StampType StampType;
		public VisitorDescription VisitorDesc;

		public Document_Stamped(StampType stamp, VisitorDescription desc) {
			StampType = stamp;
			VisitorDesc = desc;
		}
	}


	//страшный костыль
	public struct Document_Stamped_Pre {
		public StampType StampType;
		public VisitorDescription VisitorDesc;

		public Document_Stamped_Pre(StampType stamp, VisitorDescription desc) {
			StampType = stamp;
			VisitorDesc = desc;
		}
	}

	public struct Game_Ended {
		public bool Win;

		public GameResult Result;
		public Game_Ended(bool isWin, GameResult result) {
			Win = isWin;
			Result = result;
		}
	}

	public struct Breach_Sealed {

	}

	public struct Breach_Broken {

	}

	public struct Tape_Taken {

	}

	public struct Tape_Dropped {

	}
}
