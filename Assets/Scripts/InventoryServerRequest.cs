using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class InventoryServerRequest : MonoBehaviour
{
    private string apiUrl = "https://wadahub.manerai.com/api/inventory/status"; 
    private string token = "kPERnYcWAY46xaSy8CEzanosAgsWM84Nx7SKM4QBSqPq6c7StWfGxzhxPfDh8MaP";  

    public void SendInventoryEvent(string itemId, string eventType)
    {
        StartCoroutine(SendRequest(itemId, eventType));
    }

    private IEnumerator SendRequest(string itemId, string eventType)
    {
        InventoryEventData data = new InventoryEventData()
        {
            item_id = itemId,
            event_type = eventType
        };

        string jsonData = JsonUtility.ToJson(data);

        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Authorization", "Bearer " + token);
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Response response = JsonUtility.FromJson<Response>(request.downloadHandler.text);
            if (response.response == "success")
            {
                Debug.Log("Data successfully received by server: " + response.data_submitted);
            }
        }
        else
        {
            Debug.LogError("Error sending request: " + request.error);
        }
    }

    [System.Serializable]
    public class InventoryEventData
    {
        public string item_id;
        public string event_type;
    }

    [System.Serializable]
    public class Response
    {
        public string response;
        public string status;
        public string data_submitted;
    }
}
