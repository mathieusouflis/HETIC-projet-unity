
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text.Json;
using PimDeWitte.UnityMainThreadDispatcher;
using UnityEngine;
using SocketIOClient;
using Unity.VisualScripting;

public class WS : MonoBehaviour
{
    private SocketIOClient.SocketIO client;
    private bool test = true;

    async void Start()
    {
        client = new SocketIOClient.SocketIO("https://test.mathieusouflis.com/");

        client.OnConnected += async (sender, e) =>
        {
            Debug.Log("Connected to server.");
            await client.EmitAsync("newUser", "Unity");
        };
        
        
        client.On("newUser", response =>
        {
            string player = response.GetValue<string>();
            if (player == "Player2")
            {
                UnityMainThreadDispatcher.Instance().Enqueue(() =>
                {
                    var player2 = GameObject.Find("Player2");
                    var player = GameObject.Find("Player");
                    if (player2 != null)
                    {
                        Vector3 player2Position = player2.transform.position;
                        GameObject.Find("Player2").transform.position = player.transform.position;
                        GameObject.Find("Player").transform.position = player2Position;
                    }
                    else
                    {
                        Debug.LogError("Player2 GameObject not found.");
                    }
                });
            }
        });
        
        client.On("messageUnity", response =>
        {
            try
            {
                var responseString = response.ToString();
                //Debug.Log(responseString);
                var jsonDocument = JsonDocument.Parse(responseString);
                var jsonArray = jsonDocument.RootElement.EnumerateArray();

                if (jsonArray.MoveNext())
                {
                    var jsonObject = jsonArray.Current;
                    if (jsonObject.TryGetProperty("event", out JsonElement eventSent))
                    {
                        string evt = eventSent.GetString();
                        //Debug.Log(evt);

                        if (evt == "rotate")
                        {
                            UnityMainThreadDispatcher.Instance().Enqueue(() =>
                            {
                                var player = GameObject.Find("Player2");
                                if (player != null)
                                {
                                    var camera = player.transform.Find("Camera");
                                    if (camera != null)
                                    {
                                        var cameraController = camera.GetComponent<CameraController>();
                                        if (cameraController != null)
                                        {
                                            if (jsonObject.TryGetProperty("data", out JsonElement dataSent))
                                            {
                                                if (dataSent.TryGetProperty("axe", out JsonElement axeSent))
                                                {
                                                    if (dataSent.TryGetProperty("newCord", out JsonElement newCordSent))
                                                    {
                                                        //Debug.Log(axeSent.GetString());
                                                        if (axeSent.GetString() == "x")
                                                        {
                                                            //Debug.Log("x");

                                                            cameraController.RotateX(newCordSent.GetSingle());
                                                        }
                                                        else if (axeSent.GetString() == "y")
                                                        {
                                                            //Debug.Log("y");
                                                            cameraController.RotateY(newCordSent.GetSingle());
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            Debug.LogError("CameraController component not found on Camera.");
                                        }
                                    }
                                    else
                                    {
                                        Debug.LogError("Camera GameObject not found under Player2.");
                                    }
                                }
                                else
                                {
                                    Debug.LogError("Player2 GameObject not found.");
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
                                        playerController.CheckHp();
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
                                if (dataSent.TryGetProperty("x", out JsonElement axisSent))
                                {
                                    if (dataSent.TryGetProperty("y", out JsonElement axisSentY))
                                    {
                                        if (dataSent.TryGetProperty("z", out JsonElement axisSentZ))
                                        {
                                    UnityMainThreadDispatcher.Instance().Enqueue(() =>
                                    {
                                        var player = GameObject.Find("Player2");
                                        if (player != null)
                                        {
                                            var playerController = player.GetComponent<PlayerControler>();
                                            if (playerController != null)
                                            {
                                                playerController.MoveFromPosition(new Vector3(axisSent.GetSingle(), axisSentY.GetSingle(), axisSentZ.GetSingle()));
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
        try
        {
            await client.EmitAsync("messageUnity", new Dictionary<string, object> { { "event", evt }, { "data", data } });
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }

    private void OnDestroy()
    {
        if (client != null)
        {
            client.DisconnectAsync();
        }
    }
}