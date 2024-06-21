using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    public PlayerInputAction action {  get; private set; }
    void Start()
    {
        action = new PlayerInputAction();
        action.Player.Enable();
    }

    public Vector2 GetMoveInputVector()
    {
        return action.Player.Move.ReadValue<Vector2>().normalized;
    }
}
