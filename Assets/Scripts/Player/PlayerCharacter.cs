using NaughtyAttributes;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    [field: SerializeField, ReadOnly]
    public PlayerController Owner { get; private set; }

    public void OnAttackInput(bool isAttackPressed = true)
    {
        print("I am attacking");
    }

    public void OnJumpInput(bool isJumpPressed = true)
    {
        print("I am jumping");
    }

    public void OnMoveInput(Vector2 value)
    {
        //THIS IS VERY PLACEHOLDER
        transform.position += new Vector3(value.x, value.y, 0f)/* * Time.deltaTime*/;
    }

    public void SetOwner(PlayerController owner)
    {
        print("My new owner is player index " + owner.PlayerIndex);
        Owner = owner;
    }
}
