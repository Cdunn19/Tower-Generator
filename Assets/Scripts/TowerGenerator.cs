using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerGenerator : MonoBehaviour
{
    public GameObject Tower;
    public GameObject cubePrefab;
    public GameObject walls;
    public GameObject glass;

    public int glassOffset = -1;
    public int floors = -1;
    public int height = 0;
    public int baseWidth = -1;
    public int baseLength = -1;
    public int floorHeightMod = -1;

    public float wallColorR = -1f;
    public float wallColorG = -1f;
    public float wallColorB = -1f;
    public float wallColorA = 1f;

    public float glassColorR = -1f;
    public float glassColorG = -1f;
    public float glassColorB = -1f;
    public float glassColorA = -1f;

    public Color wallColor;
    public Color glassColor; 

    public Renderer cubeRenderer;

    public Shader transparentShader;

    public class cube
    {
        public Color cubeColor;
        public Shader cubeShader;
        public bool isRendered = false;
        public bool isGlass = false;


    }

    public float rollValue(float value, float lower, float upper){
        if(value < 0)
        {
            return Random.Range(lower, upper);
        }
        else
        {
            return value;
        }
    }

    void Start()
    {
        transparentShader = Shader.Find("Transparent/Diffuse");

        wallColorR = rollValue(wallColorR, 0f, 1f);
        wallColorG = rollValue(wallColorG, 0f, 1f);
        wallColorB = rollValue(wallColorB, 0f, 1f);

        glassColorR = rollValue(glassColorR, 0f, 1f);
        glassColorG = rollValue(glassColorG, 0f, 1f);
        glassColorB = rollValue(glassColorB, 0f, 1f);
        if(glassColorA < 0)
        {
            glassColorA = Random.Range(.05f, .5f);
        }

        wallColor = new Color(wallColorR, wallColorG, wallColorB, wallColorA);
        glassColor = new Color(glassColorR, glassColorG, glassColorB, glassColorA);

        if (floorHeightMod < 2)
        {
            floorHeightMod = Random.Range(2, 5);
        }
        if (floors < 1)
        {
            floors = (Random.Range(2, 40));
        }
        height = ((floors * (floorHeightMod + 1)) + 1);
        Debug.Log("floors = " + floors + ", floorHeightMod = " + floorHeightMod + ", height = " + height);
        if (baseWidth < 1)
        {
            baseWidth = Random.Range(25, 50);
        }
        Debug.Log("baseWidth = " + baseWidth);
        if (baseLength < 1)
        {
            baseLength = Random.Range(25, 50);
        }
        Debug.Log("baseLength = " + baseLength);
        if (glassOffset < 0)
        {
            glassOffset = Random.Range(0, 2);
        }

        cube[,,] cubeGrid = new cube[height, baseWidth, baseLength];

        for (int heightCount = 0; heightCount < height; heightCount++)
        {
            for (int widthCount = 0; widthCount < baseWidth; widthCount++)
            {
                for (int lengthCount = 0; lengthCount < baseLength; lengthCount++)
                {

                    cubeGrid[heightCount, widthCount, lengthCount] = new cube();

                }
            }

        }

        for (int heightCount = 0; heightCount < height; heightCount++)
        {
            for (int widthCount = 0; widthCount < baseWidth; widthCount++)
            {
                for (int lengthCount = 0; lengthCount < baseLength; lengthCount++)
                {
                    if ((heightCount == 0 || heightCount == height - 1) || (lengthCount == 0 || lengthCount == baseLength - 1) || (widthCount == 0 || widthCount == baseWidth - 1)) //only outside blocks(including top & bottom)
                    {
                        if(heightCount%(floorHeightMod + 1) == 0) { //Render solid Floors according to floorHeightMod, set current cubeColor to wallColor
                            Tower = Instantiate(cubePrefab, new Vector3(widthCount, heightCount, lengthCount), Quaternion.identity, walls.transform);
                            cubeGrid[heightCount, widthCount, lengthCount].cubeColor = wallColor;
                            Tower.GetComponent<Renderer>().material.color = wallColor;
                        }
                        else
                        {   //Render Windows between floors (everything on the outside that isn't considered to be part of a floor), sets current cubeColor to defaultGlassColor & sets cubeShader to transparentShader
                            Tower = Instantiate(cubePrefab, new Vector3(widthCount, heightCount, lengthCount), Quaternion.identity, walls.transform);
                            cubeGrid[heightCount, widthCount, lengthCount].cubeColor = glassColor;
                            Tower.GetComponent<Renderer>().material.color = glassColor;
                            cubeGrid[heightCount, widthCount, lengthCount].cubeShader = transparentShader;
                            Tower.GetComponent<Renderer>().material.shader = transparentShader;
                        }
                        cubeGrid[heightCount, widthCount, lengthCount].isRendered = true;
                    }

                }

            }

        } 

    } 

}
