using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR.Interaction.Toolkit;

[System.Serializable]
public class Haptic
{
    [Range(0, 1)]
    public float hapticIntensity;
    public float hapticDuration;

    public void TriggerHaptic(BaseInteractionEventArgs args)
    {
        if (args.interactorObject is XRBaseControllerInteractor controllerInteractor)
        {
            TriggerHaptic(controllerInteractor.xrController);
        }
    }

    public void TriggerHaptic(XRBaseController controller)
    {
        if (hapticIntensity > 0)
        {
            controller.SendHapticImpulse(hapticIntensity, hapticDuration);
        }

    }
}

public class HapticInteractable : MonoBehaviour
{
    public Haptic hapticOnActivated;
    public Haptic hapticHoverEntered;
    public Haptic hapticHoverExited;
    public Haptic hapticSelectEntered;
    public Haptic hapticSelectExited;

    public Haptic hapticClick;
    public static XRBaseController defaultRightController;
    public static XRBaseController defaultLeftController;

    void Start()
    {
        defaultRightController = GameObject.Find("Right Controller").GetComponent<XRBaseController>();
        defaultLeftController = GameObject.Find("Left Controller").GetComponent<XRBaseController>();

        XRBaseInteractable interactable = GetComponent<XRBaseInteractable>();
        // if interactable exists, add listeners to the events
        if (interactable == null) return;

        interactable.activated.AddListener(hapticOnActivated.TriggerHaptic);
        interactable.hoverEntered.AddListener(hapticHoverEntered.TriggerHaptic);
        interactable.hoverExited.AddListener(hapticHoverExited.TriggerHaptic);
        interactable.selectEntered.AddListener(hapticSelectEntered.TriggerHaptic);
        interactable.selectExited.AddListener(hapticSelectExited.TriggerHaptic);
    }

    public void OnClickTriggerHaptic()
    {
        SetHapticImpulse(hapticClick.hapticIntensity, hapticClick.hapticDuration);
    }

    public static void SetHapticImpulse(float intensity, float duration)
    {
        defaultRightController.SendHapticImpulse(intensity, duration);
        defaultLeftController.SendHapticImpulse(intensity, duration);
    }
}
