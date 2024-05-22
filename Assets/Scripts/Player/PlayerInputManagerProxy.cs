using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// This script handles business logic with the unity component 
/// PlayerInputManager, ie what to do when joining
/// 
/// NOTE: Local only!
/// </summary>
[RequireComponent(typeof(PlayerInputManager))]
public class PlayerInputManagerProxy : MonoBehaviour
{
    private PlayerInputManager m_mgrCmp;

    [Header("Character Prefab")]
    [SerializeField]
    private PlayerCharacter m_charPrefab;

    [Header("Player UI")]
    [SerializeField]
    private UIGameplay m_gameplayUIPrefab;
    //temp
    private UIGameplay m_gameplayUI;

    [Header("Cinemachine Camera")]
    [SerializeField]
    private CinemachineTargetGroup m_mainTargetGrp;

    

    //temp
    private static int pCount = 0;

    private void OnValidate()
    {
        m_mgrCmp = GetComponent<PlayerInputManager>();
    }

    private void Awake()
    {
        //if doing local testing with controllers etc
        m_mgrCmp = GetComponent<PlayerInputManager>();

        if (m_mgrCmp)
        {
            m_mgrCmp.onPlayerJoined += OnPlayerJoined;
            m_mgrCmp.onPlayerLeft += OnPlayerLeft;
        }
    }

    private void OnDestroy()
    {
        //handle local testing
        if (m_mgrCmp)
        {
            m_mgrCmp.onPlayerJoined -= OnPlayerJoined;
            m_mgrCmp.onPlayerLeft -= OnPlayerLeft;
        }
    }

    private void OnPlayerJoined(PlayerInput obj)
    {
        var pc = obj.GetComponent<PlayerController>();

        SetUpPlayer(pc);
    }

    private void OnPlayerLeft(PlayerInput obj)
    {
        print("Player left");
    }

    private void SetUpPlayer(PlayerController controller)
    {
        controller.AssignId(pCount++);

        PlayerCharacter character = SpawnAndSetCharacter(ref controller);

        //do other stuff here
        if (!m_gameplayUI)
            m_gameplayUI = Instantiate(m_gameplayUIPrefab);
        else
            m_gameplayUI = GameObject.FindAnyObjectByType<UIGameplay>(FindObjectsInactive.Include);

        m_gameplayUI.Setup(character);

        m_mainTargetGrp.AddMember(character.transform, 1f / (float)pCount, 0.25f);

        //need to loop and reset all target group radius and weight
        foreach(var elem in m_mainTargetGrp.Targets)
            elem.Weight = 1f / (float)pCount;

        //change control scheme
        controller.OnStart();
    }

    private PlayerCharacter SpawnAndSetCharacter(ref PlayerController pc)
    {
        PlayerCharacter ch = Instantiate(m_charPrefab, Vector3.right, Quaternion.identity);

        pc.SetCharacter(ch);

        return ch;
    }
}
