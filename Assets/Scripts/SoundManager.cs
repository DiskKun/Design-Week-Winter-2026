using UnityEngine;
using UnityEngine.InputSystem;

public class SoundManager : MonoBehaviour
{
    private InputAction blowAction;
    
    public AudioSource wind;
    public AudioSource TrumpetSound;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        blowAction = InputSystem.actions.FindAction("Blow");
    }

    // Update is called once per frame
    void Update()
    {
        if (blowAction.WasPressedThisFrame())
        {
            wind.Play();
        }
        if (blowAction.WasReleasedThisFrame())
        {
            wind.Stop();
        }
    }

    public void PlayTrumpet()
    {
        TrumpetSound.Play();
    }
}
