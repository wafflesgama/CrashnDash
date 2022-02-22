using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class NPCDialogue : MonoBehaviour, Interactable
{

    [Header("Dialogue")]
    [Multiline] public string[] dialogue;

    private UEventHandler eventHandler = new UEventHandler();

    public List<Renderer> GetInteractableMeshes()
    {
        return null;
    }

    public void Interact()
    {
        gameObject.tag = "Uninteractable";
        PlayerUIManager.OnStartedDialogue.TryInvoke(transform, dialogue);
        PlayerUIManager.OnFinishedDialogue.Subscribe(eventHandler, FinishedDialogue);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private async void FinishedDialogue()
    {
        PlayerUIManager.OnFinishedDialogue.Unsubscribe(FinishedDialogue);
        await Task.Delay(1000);
        gameObject.tag = "Interactable";
    }
}
