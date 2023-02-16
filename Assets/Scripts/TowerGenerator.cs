using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerGenerator : MonoBehaviour
{
    public GameObject Tower;
    public GameObject cubePrefab;
    public GameObject walls;
    public GameObject glass;

    public int glassOffset;
    public int floors;
    public int towerHeight;
    public int baseWidth;
    public int baseLength;
    public int floorHeight;

    public Color wallColor;
    public Color glassColor; 

    public Renderer cubeRenderer;

    public Shader transparentShader;

    /*
     * TODO:
     * 
     * Utilize Cube class to cut down some of the heavy looping, using xyz coords
     * 
     * Fix floors not rendering -- issue with order of if statements (and rendering in general needs rewrite).
     */



    private class Cube
    {
        public Color cubeColor;
        public Shader cubeShader;
        public bool isRendered = false;
        public bool isGlass = false;
    }

    void Start()
    {
        transparentShader = Shader.Find("Transparent/Diffuse");

        wallColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f); 
        glassColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(.05f, .5f)); 

        floorHeight = Random.Range(2, 5);
        floors = (Random.Range(2, 40));
        towerHeight = ((floors * (floorHeight + 1)) + 1);

        Debug.Log("floors = " + floors + ", floorHeight = " + floorHeight + ", height = " + towerHeight);

        baseWidth = Random.Range(25, 50);
        baseLength = Random.Range(25, 50);

        Debug.Log("baseWidth = " + baseWidth + ", baseLength = " + baseLength);

        Cube[,,] cubeGrid = new Cube[towerHeight, baseWidth, baseLength];

        for (int heightCount = 0; heightCount < towerHeight; heightCount++) //initialize all possible cubes of the tower, including interior
        {
            for (int widthCount = 0; widthCount < baseWidth; widthCount++)
            {
                for (int lengthCount = 0; lengthCount < baseLength; lengthCount++)
                {
                    cubeGrid[heightCount, widthCount, lengthCount] = new Cube();
                }
            }
        }

        for (int heightCount = 0; heightCount < towerHeight; heightCount++)
        {
            for (int widthCount = 0; widthCount < baseWidth; widthCount++)
            {
                for (int lengthCount = 0; lengthCount < baseLength; lengthCount++)
                {
                    if ((heightCount == 0 || heightCount == towerHeight - 1) || //Ignores all cubes not on the outside of the array
                        (lengthCount == 0 || lengthCount == baseLength - 1) ||
                        (widthCount == 0 || widthCount == baseWidth - 1)) 
                    {
                        //This if statement needs to be before the previous if statement to function, on to-do list
                        if(heightCount%(floorHeight + 1) == 0) { //Render solid Floors according to floorHeight, set current cubeColor to wallColor
                            Tower = Instantiate(cubePrefab, new Vector3(widthCount, heightCount, lengthCount), Quaternion.identity, walls.transform);
                            cubeGrid[heightCount, widthCount, lengthCount].cubeColor = wallColor;
                            Tower.GetComponent<Renderer>().material.color = wallColor;
                        }
                        else
                        {   //Render Windows between floors (everything on the outside that isn't considered to be part of a floor)
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
