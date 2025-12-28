public interface Iinteractable
{
    public enum GameObjectTypes {Object, Item, Other}
    GameObjectTypes objectType {get; set;}

    public void Interact(Interaction interaction){}
}
