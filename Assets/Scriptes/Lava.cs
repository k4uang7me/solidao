using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{

    public float speedY = 3.5f;
    public float speedX = 3.5f;

    MeshRenderer rend;
    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        rend.material.mainTextureOffset = new Vector2(speedX * Time.timeSinceLevelLoad, speedY * Time.timeSinceLevelLoad);
    }
}
