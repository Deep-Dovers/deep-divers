using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

/// <summary>
/// Input controller/reader etc.
/// Reference: 
/// https://docs.unity3d.com/Packages/com.unity.inputsystem@1.5/manual/PlayerInput.html
/// https://docs.unity3d.com/Packages/com.unity.inputsystem@1.5/manual/Interactions.html
/// </summary>
[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    [Header("Identification")]
    [field: SerializeField, ReadOnly]
    public int PlayerIndex { get; private set; } = -1;
    private string m_uId = string.Empty;
    public string UniqueId => m_uId;

    //! so i can test movement
    [Header("Character")]
    [SerializeField]
    public PlayerCharacter m_character;
    public PlayerAnimationState m_charAnim;

    //event system/input
    [Header("Input")]
    [SerializeField, ReadOnly]
    private PlayerInput m_pInputControls;
    private MultiplayerEventSystem m_eventSystem;

    private UIRelicInventoryWindow m_inventoryWindow;

    private void OnValidate()
    {
        m_pInputControls = GetComponentInChildren<PlayerInput>();
    }

    private void Awake()
    {
        m_pInputControls = GetComponentInChildren<PlayerInput>();
        ToggleInputActive(true);
    }

    public void AssignId(int id)
    {
        Debug.Log($"Player id changed from {PlayerIndex} to {id}");
        PlayerIndex = id;
    }

    public void AssignUniqueId(string id)
    {
        m_uId = id;
    }

    #region Input
    #region Input Map Stuff
    private void SwitchInputMap(string map)
    {
        m_pInputControls.SwitchCurrentActionMap(map);
    }

    /// <summary>
    /// Use at your own risk
    /// </summary>
    /// <param name="active"></param>
    private void ToggleInputActive(bool active = true)
    {
        if (active)
        {
            m_pInputControls.ActivateInput();
        }
        else
        {
            m_pInputControls.DeactivateInput();
        }
    }
    #endregion

    #region Event Listeners - Start
    public void OnStart()
    {
        print("Joined!");
        SwitchInputMap(PlayerConsts.PlayerInputMap_Gameplay);
    }
    #endregion

    #region Event Listeners - Gameplay
    public void OnJump(InputValue value)
    {
        bool isButtonPressed = value.Get<float>() > 0f;
        m_character.OnJumpInput(isButtonPressed);
    }

    public void OnMove(InputValue value)
    {
        Vector2 move = value.Get<Vector2>();
        m_character.OnMoveInput(move);

        m_charAnim.TransitToAnimationState(move != Vector2.zero ? PlayerAnimationState.AnimationState.Walk :
            PlayerAnimationState.AnimationState.Idle);
    }

    public void OnDash()
    {
        print("DASH!!!");
        m_character.OnDashInput();

        m_charAnim.TransitToAnimationState(PlayerAnimationState.AnimationState.Dash);
    }

    public void OnAttack()
    {
        print("attack!!!");
        m_character.OnAttackInput();

        m_charAnim.TransitToAnimationState(PlayerAnimationState.AnimationState.AttackDown);
    }
    public void OnAbility1Triggered()
    {
        print("a1!!!");
        //m_character.OnAttackInput();
    }
    public void OnAbility2Triggered()
    {
        print("a2!!!");
        //m_character.OnAttackInput();
    }
    public void OnAbility3Triggered()
    {
        print("a3!!!");
        //m_character.OnAttackInput();
    }

    public void OnOpenInventory()
    {
        if (!m_inventoryWindow)
            m_inventoryWindow = FindFirstObjectByType<UIRelicInventoryWindow>(FindObjectsInactive.Include);

        m_inventoryWindow.ToggleWindow();
    }
    #endregion

    #region Event System (UI)
    #endregion
    #endregion

    public void SetCharacter(PlayerCharacter character)
    {
        m_character = character;
        character.SetOwner(this);

        m_charAnim = m_character.GetComponent<PlayerAnimationState>();
    }
}
