using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using static SoundUtils;

public class DoorSwitch : MonoBehaviour, Interactable
{
    public List<Renderer> renderers;
    public List<Renderer> GetInteractableMeshes() => renderers;

    public AudioSource audioSource;

    public Sound pressSound;
    public Sound rejectedSound;

    public Door door;

    public async void Interact()
    {
        audioSource.PlaySound(pressSound);
        var success = await door.OpenClose();

        await Task.Delay(250);
        if (!success)
            audioSource.PlaySound(rejectedSound);
    }
}
