using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public GameObject foreGroundObject;
    public GameObject backGroundObject;
    private Material fgmat;
    private Material bgmat;

    public float foreGroundSpeed;
    public float backGroundSpeed;
    public float skyboxSpeed;

    public void Start()
    {
        //if (foreGroundObject != null)
        //{
            fgmat = foreGroundObject.GetComponent<Renderer>().material;
            bgmat = backGroundObject.GetComponent<Renderer>().material;
        //}
    }

    // Update is called once per frame
    void Update()
    {
        //if (foreGroundObject != null)
        //{
            fgmat.mainTextureOffset += new Vector2(foreGroundSpeed, 0);
            bgmat.mainTextureOffset += new Vector2(backGroundSpeed, 0);
            RenderSettings.skybox.SetFloat("_Rotation", Time.time * skyboxSpeed);
        //}
    }
}
