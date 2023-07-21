using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine.Networking;
using UnityEngine.UIElements;

[System.Serializable]
public class ApiResponse
{
    public Info info;
    public List<Character> results;
}

[System.Serializable]
public class Info
{
    public int count;
    public int pages;
    public string next;
    public string prev;
}

[System.Serializable]
public class Character
{
    public int id;
    public string name;
    public string status;
    public string species;
    public string image;
    // Otros campos de datos según la API de Rick and Morty
}

public class APIManager : MonoBehaviour
{
    private string apiUrl = "https://rickandmortyapi.com/api"; // URL de la API

    public IEnumerator GetRequest(string endpoint, System.Action<ApiResponse> callback)
    {
        string requestUrl = apiUrl + endpoint;
    
        UnityWebRequest webRequest = UnityWebRequest.Get(requestUrl);
        yield return webRequest.SendWebRequest();
    
        if (webRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error en la solicitud: " + webRequest.error);
        }
        else
        {
            string responseText = webRequest.downloadHandler.text;
            ApiResponse response = JsonConvert.DeserializeObject<ApiResponse>(responseText);
            callback?.Invoke(response);
        }
    }
    // Puedes implementar otros métodos de solicitud como POST, PUT, DELETE según tus necesidades.
}