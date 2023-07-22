using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.Networking;

public class CharacterGridGenerator : MonoBehaviour
{
    public GameObject characterPrefab; // El prefab que contiene la imagen y los textos
    public APIManager apiManager;
    public Transform parentPanel;

    public void GenerateData()
    {
        StartCoroutine(apiManager.GetRequest("/character", HandleResponse));
    }

    private void HandleResponse(ApiResponse response)
    {
        List<Character> characters = response.results;
        foreach (Character character in characters)
        {
            GameObject characterComponent = Instantiate(characterPrefab, parentPanel);
            Image imageComponent = characterComponent.GetComponent<Image>();
            imageComponent.sprite = null; // Establecer una imagen vacía temporalmente
            
            StartCoroutine(LoadImageFromURL(character.image, imageComponent));

            // Obtener referencias a los objetos de texto dentro del prefab
            TextMeshProUGUI[] texts = characterComponent.GetComponentsInChildren<TextMeshProUGUI>();
            foreach (TextMeshProUGUI text in texts)
            {
                if (text.gameObject.name == "Name Text")
                {
                    text.text = character.name;
                }
                else if (text.gameObject.name == "ID Text")
                {
                    text.text = "ID: " + character.id;
                }
            }
        }
    }
    
    private IEnumerator LoadImageFromURL(string imageUrl, Image imageComponent)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(imageUrl);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(www);
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
            imageComponent.sprite = sprite;
        }
        else
        {
            Debug.LogError("Error al cargar la imagen desde la URL: " + www.error);
        }
    }

    
}