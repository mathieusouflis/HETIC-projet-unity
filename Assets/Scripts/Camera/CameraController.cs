using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public bool canTurn = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        if (!canTurn) return;
        float newY = -Input.GetAxis("Mouse Y") * 10f;
        float newX = Input.GetAxis("Mouse X") * 10f;
        float currentX = transform.localRotation.eulerAngles.x;
        if (currentX > 180f) currentX -= 360f;

        if (currentX + newY <= 55f && currentX + newY >= -72f)
        {
            RotateX(newY);
        }

        RotateY(newX);
    }

    public void RotateX(float newY)
    {
        if(canTurn) GameObject.Find("Game Values").GetComponent<WS>().SendMessage("rotate", new Dictionary<string, object> {{"axe", "x"}, {"newCord", newY}});
        transform.Rotate(newY, 0f, 0f); // Rotation sur l'axe X
    }
    
    public void RotateY(float newX)
    {
        if(canTurn) GameObject.Find("Game Values").GetComponent<WS>().SendMessage("rotate", new Dictionary<string, object> {{"axe", "y"}, {"newCord", newX}});
        transform.parent.Rotate(0f, newX, 0f); // Rotation sur l'axe Y
    }
}
