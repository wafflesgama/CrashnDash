using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionDisplayer : MonoBehaviour
{
    [SerializeField]
    private Outline.Mode outlineMode;

    [SerializeField]
    [ColorUsage(true, true)]
    private Color outlineColor = Color.white;

    [SerializeField, Range(0f, 10f)]
    private float outlineWidth = 2f;

    private List<Outline> objsOutlined = new List<Outline>();
    private UEventHandler eventHandler= new UEventHandler();
    void Start()
    {
        PlayerInteractionHandler.OnInteractableAppeared.Subscribe(eventHandler, DisplayInteractable);
        PlayerInteractionHandler.OnInteractableDisappeared.Subscribe(eventHandler, ClearInteractable);
    }

    private void OnDestroy()
    {
        eventHandler.UnsubcribeAll();
    }

    private void DisplayInteractable(Interactable interactable, Transform origin)
    {
        objsOutlined.Clear();
        interactable.GetInteractableMeshes().ForEach(x =>
        {
            var outlineComp = x.gameObject.AddComponent<Outline>();
            objsOutlined.Add(outlineComp);
            outlineComp.OutlineColor = outlineColor;
            outlineComp.OutlineWidth = outlineWidth;
            outlineComp.OutlineMode = outlineMode;
        });

    }

    private void ClearInteractable()
    {
        objsOutlined.ForEach(x =>
        {
            Destroy(x);
        });
        objsOutlined.Clear();
    }
}
