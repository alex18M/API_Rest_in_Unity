using System.Collections;
using TMPro;
using UnityEngine;
using Unity.Collections;
using UnityEngine.UI;

public class RestAPI : MonoBehaviour
{
    private APIManager apiManager;

    private void Start()
    {
        apiManager = GetComponent<APIManager>();
        StartCoroutine(apiManager.GetRequest("/character", HandleResponse));
    }

    private void HandleResponse(ApiResponse response)
    {
        foreach (var character in response.results)
        {
            Debug.Log("ID: " + character.id + ", Nombre: " + character.name);
        }
    }
}
