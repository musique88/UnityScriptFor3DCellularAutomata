using System;
using UnityEngine;
using Random = System.Random;

public class GameOfLifeComponent : MonoBehaviour
{

    public int lowerThanKill;
    public int higherThanKill;
    public int generateNumber;

    public bool randomInitialisation;
    public int ruleset;

    private static int width =  20;
    private static int height = 20;
    private static int depth =  20;
    
    private byte[,,] matrix = new Byte[width,height,depth];
    private GameObject[,,] cubes = new GameObject[width,height,depth];
    
    void Awake()
    {
        for (int i = 1; i < width; i++)
        {
            for (int j = 1; j < height; j++)
            {
                for (int k = 1; k < depth; k++)
                {
                    matrix[i, j, k] = 0;
                }
            }
        }

        if (randomInitialisation)
            randomize();
    }
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 1; i < width; i++)
        {
            for (int j = 1; j < height; j++)
            {
                for (int k = 1; k < depth; k++)
                {
                    cubes[i,j,k] = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cubes[i,j,k].transform.localPosition = new Vector3(i,j,k);
                    cubes[i, j, k].GetComponent<BoxCollider>().enabled = false;
                }
            }
        }
        
        Show();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey)
        {
            Step();
            Show();
        }
    }

    void Show()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                for (int k = 0; k < depth; k++)
                {
                    cubes[i, j, k].GetComponent<MeshRenderer>().enabled = matrix[i,j,k] == 1;
                }
            }
        }
    }

    void Step()
    {
        byte[,,] nextStep = new Byte[width,height,depth];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                for (int k = 0; k < depth; k++)
                {
                    if (k * j * i == 0 || i == width - 1 || j == height - 1 || k == depth - 1)
                        nextStep[i, j, k] = 0;
                    else
                    {
                        int neighbours = new int();
                        switch (ruleset)
                        {
                            case 1:
                                neighbours += matrix[i - 1, j - 1, k - 1] +
                                              matrix[i - 1, j - 1, k] +
                                              matrix[i - 1, j - 1, k + 1] +
                                              matrix[i - 1, j, k - 1] +
                                              matrix[i - 1, j, k] +
                                              matrix[i - 1, j, k + 1] +
                                              matrix[i - 1, j + 1, k - 1] +
                                              matrix[i - 1, j + 1, k] +
                                              matrix[i - 1, j + 1, k + 1] +

                                              matrix[i, j - 1, k - 1] +
                                              matrix[i, j - 1, k] +
                                              matrix[i, j - 1, k + 1] +
                                              matrix[i, j, k - 1] +
                                              //not counting self
                                              matrix[i, j, k + 1] +
                                              matrix[i, j + 1, k - 1] +
                                              matrix[i, j + 1, k] +
                                              matrix[i, j + 1, k + 1] +

                                              matrix[i + 1, j - 1, k - 1] +
                                              matrix[i + 1, j - 1, k] +
                                              matrix[i + 1, j - 1, k + 1] +
                                              matrix[i + 1, j, k - 1] +
                                              matrix[i + 1, j, k] +
                                              matrix[i + 1, j, k + 1] +
                                              matrix[i + 1, j + 1, k - 1] +
                                              matrix[i + 1, j + 1, k] +
                                              matrix[i + 1, j + 1, k + 1];
                                break;
                            case 2:
                                neighbours += matrix[i - 1, j, k] +

                                              matrix[i, j - 1, k] +
                                              matrix[i, j, k - 1] +
                                              matrix[i, j, k + 1] +
                                              matrix[i, j + 1, k] +

                                              matrix[i + 1, j, k];
                                break;
                        }

                        if (neighbours == generateNumber)
                            nextStep[i, j, k] = 1;
                        else if (neighbours < lowerThanKill || neighbours > higherThanKill)
                            nextStep[i, j, k] = 0;
                        else
                            nextStep[i, j, k] = matrix[i, j, k];
                    }
                }
            }
        }

        matrix = nextStep;
    }

    private void randomize()
    {
        Random rnd = new Random();
        for (int i = 1; i < width; i++)
        {
            for (int j = 1; j < height; j++)
            {
                for (int k = 1; k < depth; k++)
                {
                    matrix[i, j, k] = rnd.Next(3) > 1 ? (byte)1 : (byte)0;
                }
            }
        }
    }
}
