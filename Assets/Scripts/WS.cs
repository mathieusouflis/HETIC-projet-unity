
using System;
using System.Collections.Generic;
using System.Text.Json;
using PimDeWitte.UnityMainThreadDispatcher;
using UnityEngine;
using SocketIOClient;

public class WS : MonoBehaviour
{
    private SocketIOClient.SocketIO client;
    private bool test = true;

    async void Start()
    {
        client = new SocketIOClient.SocketIO("https://test.mathieusouflis.com/");

        client.OnConnected += (sender, e) => { Debug.Log("Connected to server."); };

        client.On("messageUnity", response =>
        {
            try
            {
                var responseString = response.ToString();
                Debug.Log(responseString);
                var jsonDocument = JsonDocument.Parse(responseString);
                var jsonArray = jsonDocument.RootElement.EnumerateArray();

                if (jsonArray.MoveNext())
                {
                    var jsonObject = jsonArray.Current;
                    if (jsonObject.TryGetProperty("event", out JsonElement eventSent))
                    {
                        string evt = eventSent.GetString();
                        Debug.Log(evt);

                        if (evt == "rotate")
                        {
                            UnityMainThreadDispatcher.Instance().Enqueue(() =>
                            {
                                var player = GameObject.Find("Player2");
                                if (player != null)
                                {
                                    if (jsonObject.TryGetProperty("data", out JsonElement dataSent))
                                    {
                                        if (dataSent.TryGetProperty("axe", out JsonElement axeSent))
                                        {
                                            if (dataSent.TryGetProperty("newCord", out JsonElement newCordSent))
                                            {
                                                var cameraController = player.transform.Find("Camera")
                                                    .GetComponent<CameraController>();
                                                if (axeSent.GetString() == "x")
                                                {
                                                    cameraController.RotateX(newCordSent.GetSingle());
                                                }
                                                else if (axeSent.GetString() == "y")
                                                {
                                                    cameraController.RotateY(newCordSent.GetSingle());
                                                }
                                            }
                                        }
                                    }
                                }
                            });
                        }else if (evt == "hit")
                        {
                            UnityMainThreadDispatcher.Instance().Enqueue(() =>
                            {
                                var player = GameObject.Find("Player");
                                if (player != null)
                                {
                                    var playerController = player.GetComponent<PlayerControler>();
                                    if (playerController != null)
                                    {
                                        playerController.playerHealth -= 10;
                                    }
                                    else
                                    {
                                        Debug.LogError("PlayerControler component not found on Player2.");
                                    }
                                }
                                else
                                {
                                    Debug.LogError("Player2 GameObject not found.");
                                }
                            });
                        }else if (evt == "shoot")
                        {
                                UnityMainThreadDispatcher.Instance().Enqueue(() =>
                                {
                                    var player = GameObject.Find("Player2");
                                    if (player != null)
                                    {
                                        var playerController = player.GetComponent<PlayerControler>();
                                        if (playerController != null)
                                        {
                                            GameObject.Find("Player2").transform.Find("Camera").transform.Find("Gun").GetComponent<Gun>().Shoot();
                                        }
                                        else
                                        {
                                            Debug.LogError("PlayerControler component not found on Player2.");
                                        }
                                    }
                                    else
                                    {
                                        Debug.LogError("Player2 GameObject not found.");
                                    }
                                });
                        }else if (evt == "jump")
                        {
                                UnityMainThreadDispatcher.Instance().Enqueue(() =>
                                {
                                    var player = GameObject.Find("Player2");
                                    if (player != null)
                                    {
                                        var playerController = player.GetComponent<PlayerControler>();
                                        if (playerController != null)
                                        {
                                            playerController.JumpPlayer();
                                        }
                                        else
                                        {
                                            Debug.LogError("PlayerControler component not found on Player2.");
                                        }
                                    }
                                    else
                                    {
                                        Debug.LogError("Player2 GameObject not found.");
                                    }
                                });
                        }else if (evt == "move")
                        {
                            if (jsonObject.TryGetProperty("data", out JsonElement dataSent))
                            {
                                if (dataSent.TryGetProperty("axis", out JsonElement axisSent))
                                {
                                    string message = axisSent.GetString();
                                    UnityMainThreadDispatcher.Instance().Enqueue(() =>
                                    {
                                        var player = GameObject.Find("Player2");
                                        if (player != null)
                                        {
                                            var playerController = player.GetComponent<PlayerControler>();
                                            if (playerController != null)
                                            {
                                                switch (message)
                                                {
                                                    case "z":
                                                        playerController.MovePlayer("z");
                                                        break;
                                                    case "q":
                                                        playerController.MovePlayer("q");
                                                        break;
                                                    case "s":
                                                        playerController.MovePlayer("s");
                                                        break;
                                                    case "d":
                                                        playerController.MovePlayer("d");
                                                        break;
                                                    case "fire":
                                                        player.transform.Find("Main Camera").transform.Find("Gun").GetComponent<Gun>().Shoot();
                                                        break;
                                                }
                                            }
                                            else
                                            {
                                                Debug.LogError("PlayerControler component not found on Player2.");
                                            }
                                        }
                                        else
                                        {
                                            Debug.LogError("Player2 GameObject not found.");
                                        }
                                    });
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        });

        await client.ConnectAsync();
    }

    public async void SendMessage(string evt, Dictionary<string, object> data)
    {
        Debug.Log("Sending " + evt);
        await client.EmitAsync("messageUnity", new Dictionary<string, object> { { "event", evt }, { "data", data } });
    }

    private void OnDestroy()
    {
        if (client != null)
        {
            client.DisconnectAsync();
        }
    }
}