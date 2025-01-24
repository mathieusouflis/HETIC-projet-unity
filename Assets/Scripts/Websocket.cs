using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using WebSocketSharp;

public class Websocket : MonoBehaviour
{
    //Replace http by ws !!!!
    public string url;
    // Start is called before the first frame update
    void Start()
    {
        WebSocket ws = new WebSocket(url);
        ws.ConnectAsync();
        ws.Send("I'm connected");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
