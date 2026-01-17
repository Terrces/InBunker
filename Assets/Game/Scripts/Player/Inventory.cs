using UnityEngine;
using UnityEngine.InputSystem;

public class Inventory : MonoBehaviour
{
    public GameObject[] startItems = new GameObject[9];
    [SerializeField] bool InEnvironment = true;
    GameObject[] items = new GameObject[9];
    Interaction interaction => GetComponent<Interaction>();

    InputAction previous => InputSystem.actions.FindAction("Previous");
    InputAction next => InputSystem.actions.FindAction("Next");

    private int previousIndex = 1;
    private int currentIndex = 0;

    void Start()
    {
        if(startItems.Length == 0) return;
        for (int i = 0; i < startItems.Length; i++)
        {
            GameObject item = null;
            if(startItems[i] == null) continue;

            if (InEnvironment) item = startItems[i];
            else item = Instantiate(
                startItems[i], 
                interaction.GetPoint().position, 
                interaction.GetPoint().rotation, 
                null);

            interaction.GetItem(item.GetComponent<InteractiveObject>());
            AddItem(item);
            choiseItem(i);
        }
    }

    void Update()
    {
        if (previous.WasPressedThisFrame())
        {
            choiseItem(0);
        }
        else if (next.WasPressedThisFrame())
        {
            choiseItem(1);
        }
    }

    void changeIndex(int index)
    {
        previousIndex = currentIndex;
        currentIndex = index;
    }

    void choiseItem(int Current)
    {
        changeIndex(Current);
        if(Current == previousIndex) return;
        if(interaction.carriedObject) interaction.SetInactive();

        if(GetItem(previousIndex) != null)
        {
            GetItem(previousIndex).SetActive(false);
            interaction.SetInactive();
            interaction.Store();
            
            interaction.carriedObject = null;
        }
        if(GetItem(currentIndex) != null)
        {
            GetItem(currentIndex).gameObject.transform.position = interaction.GetPoint().position;
            GetItem(currentIndex).SetActive(true);
            interaction.GetItem(GetItem(currentIndex).GetComponent<InteractiveObject>());
        }
        else interaction.carriedObject = null;
    }

    public bool AddItem(GameObject obj)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] != null) continue;

            items[i] = obj;
            return true;
        }
        return false;
    }

    public GameObject GetItem(int index)
    {
        if (index < 0 || index >= items.Length)
            return null;

        return items[index];
    }

    public void RemoveItem(GameObject obj)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == obj)
                items[i] = null;
        }
    }
}
