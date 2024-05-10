using System.Collections;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;

public class DDNetworkManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        InitUnityServices();
    }

    public async void InitUnityServices()
    {
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }
    private async void CreateRoom()
    {
        try
        {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(3);
            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
        }
        catch(RelayServiceException e)
        {
            Debug.Log(e);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
