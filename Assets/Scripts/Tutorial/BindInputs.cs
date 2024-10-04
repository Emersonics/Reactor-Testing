using System.Collections.Generic;
using UnityEngine;
using KS.Reactor;
using KS.Reactor.Client.Unity;

// Bind inputs with Reactor
public class BindInputs : MonoBehaviour
{
    // Run when the game starts.
    void Start()
    {
        // Bind Unity input to Reactor input.
        ksReactor.InputManager.BindAxis(Axes.X, "Horizontal");
        ksReactor.InputManager.BindAxis(Axes.Y, "Vertical");
        ksReactor.InputManager.BindButton(Buttons.JUMP, "Jump");
        ksReactor.InputManager.BindButton(Buttons.SHOOT, "Fire1");
    }
}