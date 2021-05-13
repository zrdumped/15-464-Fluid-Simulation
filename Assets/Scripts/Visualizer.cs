using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Visualizer : MonoBehaviour
{
    float maxDisplayDensity = 0;

    public GameObject volume;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [MenuItem("CreateExamples/3DTexture")]
    static void CreateTexture3D()
    {
        // Configure the texture
        int size = 32;
        TextureFormat format = TextureFormat.RGBA32;
        TextureWrapMode wrapMode = TextureWrapMode.Clamp;

        // Create the texture and apply the configuration
        Texture3D texture = new Texture3D(size, size, size, format, false);
        texture.wrapMode = wrapMode;

        // Create a 3-dimensional array to store color data
        Color[] colors = new Color[size * size * size];

        // Populate the array so that the x, y, and z values of the texture will map to red, blue, and green colors
        float inverseResolution = 1.0f / (size - 1.0f);
        for (int z = 0; z < size; z++)
        {
            int zOffset = z * size * size;
            for (int y = 0; y < size; y++)
            {
                int yOffset = y * size;
                for (int x = 0; x < size; x++)
                {
                    //colors[x + yOffset + zOffset] = new Color(x * inverseResolution,
                    //    y * inverseResolution, z * inverseResolution, 1.0f);
                    //if (Vector3.Magnitude(new Vector3(x - 16, y - 16, z - 16)) < 8)
                    //    colors[x + yOffset + zOffset] = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                    //else
                    //    colors[x + yOffset + zOffset] = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                    if (x == 0)
                        colors[x + yOffset + zOffset] = new Color(1.0f, 0.0f, 0.0f, 1.0f);
                    else if (y == 0)
                        colors[x + yOffset + zOffset] = new Color(0.0f, 1.0f, 0.0f, 1.0f);
                    else if (z == 0)
                        colors[x + yOffset + zOffset] = new Color(0.0f, 0.0f, 1.0f, 1.0f);
                    else
                        colors[x + yOffset + zOffset] = new Color(0.0f, 0.0f, 1.0f, 0.0f);
                }
            }
        }


        // Copy the color values to the texture
        texture.SetPixels(colors);

        // Apply the changes to the texture and upload the updated texture to the GPU
        texture.Apply();

        // Save the texture to your Unity Project
        AssetDatabase.CreateAsset(texture, "Assets/Example3DTexture3.asset");
    }

    public void DrawDensity(int N, float[] density, int[] boundary)
    {
        Texture2D texture = new Texture2D(N, N);
        GetComponent<Renderer>().material.mainTexture = texture;

        //maxDisplayDensity = (Mathf.Max(density) > maxDisplayDensity) ? Mathf.Max(density) : maxDisplayDensity;

        // Debug.Log(maxDisplayDensity);

        for (int y = 0; y < N; y++)
        {
            for (int x = 0; x < N; x++)
            {
                int id = x + (N + 2) * y;
                if (boundary[id] == 2)
                {
                    Color color = new Color(1, 0, 0);
                    texture.SetPixel(x, y, color);
                }
                else
                {
                    float grayValue1 = density[id];
                    Color color = new Color(grayValue1, grayValue1, grayValue1);
                    texture.SetPixel(x, y, color);
                }
            }
        }
        texture.Apply();
    }

    public void DrawDensity(int N, float[] density1, float[] density2, int[] boundary)
    {
        Texture2D texture = new Texture2D(N, N);
        GetComponent<Renderer>().material.mainTexture = texture;

        //maxDisplayDensity = (Mathf.Max(density) > maxDisplayDensity) ? Mathf.Max(density) : maxDisplayDensity;

        // Debug.Log(maxDisplayDensity);

        for (int y = 0; y < N; y++)
        {
            for (int x = 0; x < N; x++)
            {
                int id = x + (N + 2) * y;
                if (boundary[id] == 2)
                {
                    Color color = new Color(1, 0, 0);
                    texture.SetPixel(x, y, color);
                }else { 
                    float grayValue1 = density1[id] ;
                    float grayValue2 = density2[id] ;
                    Color color = new Color(grayValue1, grayValue2, 0);
                    texture.SetPixel(x, y, color);
                }
            }
        }
        texture.Apply();
    }


    public void DrawDensity3D(int N, float[] density, int[] boundary)
    {
        // Configure the texture
        TextureFormat format = TextureFormat.RGBA32;
        TextureWrapMode wrapMode = TextureWrapMode.Clamp;

        // Create the texture and apply the configuration
        Texture3D texture = new Texture3D(N, N, N, format, false);
        texture.wrapMode = wrapMode;

        maxDisplayDensity = 1f;// (Mathf.Max(density) > maxDisplayDensity) ? Mathf.Max(density) : maxDisplayDensity;

        // Create a 3-dimensional array to store color data
        Color[] colors = new Color[N * N * N];

        // Populate the array so that the x, y, and z values of the texture will map to red, blue, and green colors
        int N2 = N * N;
        int N2_2 = (N + 2) * (N + 2);
        //for (int y = 0; y < N; y++)
        //{
        //    int yOffset = y * N2;
        //    for (int x = 0; x < N; x++)
        //    {
        //        float grayValue = density[x + y * (N + 2)] / maxDisplayDensity;
        //        int xOffset = x;
        //        for (int z = 0; z < N; z++)
        //        {
        //            int zOffset = z * N;
        //            colors[xOffset + yOffset + zOffset] = new Color(1.0f, 1.0f, 1.0f, grayValue);
        //        }
        //    }
        //}

        for (int z = 0; z < N; z++)
        {
            int zOffset = z * N2;
            for (int y = 0; y < N; y++)
            {
                int yOffset = y * N;
                for (int x = 0; x < N; x++)
                {
                    if (boundary[x + y * (N + 2) + z * N2_2] == 2)
                    {
                        colors[x + yOffset + zOffset] = new Color(1, 0, 0, 1);
                    }
                    else
                    {
                        float grayValue = density[x + y * (N + 2) + z * N2_2] / maxDisplayDensity;
                        colors[x + yOffset + zOffset] = new Color(1.0f, 1.0f, 1.0f, grayValue);
                        //colors[x + yOffset + zOffset] = new Color(grayValue, grayValue, grayValue, 1);
                    }
                }
            }
        }

        // Copy the color values to the texture
        texture.SetPixels(colors);

        // Apply the changes to the texture and upload the updated texture to the GPU
        texture.Apply();

        volume.GetComponent<Renderer>().material.mainTexture = texture;
    }

    public void DrawVelocity(int N, Vector2[] velocity)
    {
        Texture2D texture = new Texture2D(N, N);
        GetComponent<Renderer>().material.mainTexture = texture;

        // maxDisplayDensity = (Mathf.Max(density) > maxDisplayDensity) ? Mathf.Max(density) : maxDisplayDensity;

        for (int y = 0; y < N; y++)
        {
            for (int x = 0; x < N; x++)
            {
                Color color = new Color(velocity[x + (N + 2) * y][0], velocity[x + (N + 2) * y][1], 1);
                Debug.Log(color);
                texture.SetPixel(x, y, color);
            }
        }

        //Debug.Log(maxDisplayDensity);

        for (int y = 0; y < N; y++)
        {
            for (int x = 0; x < N; x++)
            {
                Color color = new Color(velocity[x + (N + 2) * y][0], velocity[x + (N + 2) * y][1], 1);
                Debug.Log(color);
                texture.SetPixel(x, y, color);
            }
        }
        texture.Apply();
    }

    private void OnMouseDown()
    {
        GetComponent<Simulator>().MouseClicked();
    }

    private void OnMouseDrag()
    {
        GetComponent<Simulator>().MouseDraged(true);
    }

    private void OnMouseUp()
    {
        GetComponent<Simulator>().MouseDraged(false);
    }
}
