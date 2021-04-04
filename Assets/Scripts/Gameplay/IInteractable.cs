namespace Game {
	public interface IInteractable {
		bool CanInteract();

		void Interact();

		void StopInteraction();
	}
}
