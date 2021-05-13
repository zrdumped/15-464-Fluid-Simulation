using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simulator : MonoBehaviour
{
    public int N = 16;
    public float diff = 0.1f;

    public bool use3D = false;

    public float densityAdd = 30;
    public float velocityMul = 100;

    public float velocityBounce = -1;

    public bool clearVelocity = true;

    public bool useBoundary = false;
    public bool multiDensities = false;

    private Vector2[] velocity, velocity_prev, velocity_input;
    private Vector3[] velocity3, velocity_prev3, velocity_input3;
    private float[] density, density_prev;
    private float[] density2, density_prev2;

    public float g = 0.1f;

    private int[] boundary;

    private int size;
    private int N2_2;

    private int randHeight;

    private bool mouseClicked = false;
    private bool mouseDragging = false;

    private float previousU = 0, previousV = 0;

    int ID(int i, int j) 
    {
        return i + (N + 2) * j;
    }

    int ID(int x, int y, int z)
    {
        return x + (N + 2) * y + N2_2 * z;
    }

    int toInt (bool b)
    {
        if (b == false) return 0;
        else return 1;
    }

    void Swap (ref int a, ref int b)
    {
        int tmp = a;
        a = b;
        b = tmp;
    }

    // Start is called before the first frame update
    void Start()
    {
        N2_2 = (N + 2) * (N + 2);

        if (use3D)
        {
            size = (N + 2) * (N + 2) * (N + 2);

            velocity3 = new Vector3[size];
            velocity_prev3 = new Vector3[size];
            velocity_input3 = new Vector3[size];

            boundary = new int[size];

            randHeight = N / 2;

            // Initial velocities
            for (int i = 1; i < N + 1; i++)
            {
                for (int k = 1; k < N + 1; k++)
                {
                    // Swirl vector field
                    //Vector3 direction = new Vector3(i, 0, k) - new Vector3(N / 2, 0, N / 2);
                    //if (direction.magnitude > N / 4)
                    //{
                    //    for (int j = 1; j < N + 1; j++)
                    //    {
                    //        velocity_input3[ID(i, j, k)] = Vector3.Cross(direction, new Vector3(0, 1, 0)).normalized;
                    //    }
                    //}

                    for (int j = 1; j < N + 1; j++)
                    {
                        velocity_input3[ID(i, j, k)] = new Vector3(0, 5, 0);
                    }
                }
            }

            // Initial boudnary
            // Sphere
            //int r = N / 4;
            //Vector3 center = new Vector3(N / 2, N / 2, N / 2);
            //for (int i = 2; i < N - 2 ; i++)
            //{
            //    for (int j = 0; j < N + 2; j++)
            //    {
            //        for (int k = 0; k < N + 2; k++)
            //        {
            //            //Vector3 conn = new Vector3(i, j, k) - center;
            //            //if (conn.magnitude <= r)
            //            //{
            //            //    boundary[ID(i, j, k)] = 1;

            //            //    boundary[ID(i - 1, j, k)] = (boundary[ID(i - 1, j, k)] == 0) ? 2 : 1;
            //            //    boundary[ID(i, j - 1, k)] = (boundary[ID(i, j - 1, k)] == 0) ? 2 : 1;
            //            //    boundary[ID(i + 1, j, k)] = (boundary[ID(i + 1, j, k)] == 0) ? 2 : 1;
            //            //    boundary[ID(i, j + 1, k)] = (boundary[ID(i, j + 1, k)] == 0) ? 2 : 1;
            //            //    boundary[ID(i, j, k + 1)] = (boundary[ID(i, j, k + 1)] == 0) ? 2 : 1;
            //            //    boundary[ID(i, j, k - 1)] = (boundary[ID(i, j, k - 1)] == 0) ? 2 : 1;
            //            //}

            //            // square
            //            if (k > N * 0.25 && k < N * 0.75 && j < N * 0.5 && j > N * 0.25)
            //            {
            //                boundary[ID(i, j, k)] = 1;

            //                boundary[ID(i - 1, j, k)] = (boundary[ID(i - 1, j, k)] == 0) ? 2 : 1;
            //                boundary[ID(i, j - 1, k)] = (boundary[ID(i, j - 1, k)] == 0) ? 2 : 1;
            //                boundary[ID(i + 1, j, k)] = (boundary[ID(i + 1, j, k)] == 0) ? 2 : 1;
            //                boundary[ID(i, j + 1, k)] = (boundary[ID(i, j + 1, k)] == 0) ? 2 : 1;
            //                boundary[ID(i, j, k + 1)] = (boundary[ID(i, j, k + 1)] == 0) ? 2 : 1;
            //                boundary[ID(i, j, k - 1)] = (boundary[ID(i, j, k - 1)] == 0) ? 2 : 1;
            //            }
            //        }
            //    }
            //}



            density = new float[size];
            density_prev = new float[size];

            

            // density_prev[ID(N / 2, 0, N / 2)] = 0.1f;
        }
        else
        {
            size = (N + 2) * (N + 2);

            velocity = new Vector2[size];
            velocity_prev = new Vector2[size];
            velocity_input = new Vector2[size];

            density = new float[size];
            density2 = new float[size];
            density_prev = new float[size];
            density_prev2 = new float[size];

            boundary = new int[size];

            // Square boundary
            //for (int i = 0; i < N + 2; i++)
            //{
            //    for (int j = 0; j < N + 2; j++)
            //    {
            //        int left = (int)(0.4 * N);
            //        int right = (int)(0.6 * N);
            //        if (toInt(i == left) + toInt(i == right) + toInt(j == left) + toInt(j == right) == 2)
            //        {
            //            boundary[ID(i, j)] = 2;
            //        }
            //        else if (i >= left && i <= right && j >= left && j <= right)
            //        {
            //            boundary[ID(i, j)] = 1;
            //        }
            //        else
            //        {
            //            boundary[ID(i, j)] = 0;
            //        }
            //    }
            //}


            // Circle boundary
            //int r = N / 4;
            //Vector2 center = new Vector2(N / 2, N / 2);
            //for (int i = 0; i < N + 2; i++)
            //{
            //    for (int j = 0; j < N + 2; j++)
            //    {
            //        Vector2 conn = new Vector2(i, j) - center;
            //        if (conn.magnitude <= r)
            //        {
            //            boundary[ID(i, j)] = 1;

            //            boundary[ID(i - 1, j)] = (boundary[ID(i - 1, j)] == 0) ? 2 : 1;
            //            boundary[ID(i, j - 1)] = (boundary[ID(i, j - 1)] == 0) ? 2 : 1;
            //            boundary[ID(i + 1, j)] = (boundary[ID(i + 1, j)] == 0) ? 2 : 1;
            //            boundary[ID(i, j + 1)] = (boundary[ID(i, j + 1)] == 0) ? 2 : 1;
            //        }
            //    }
            //}


            //for (int i = 10; i < N - 10; i++)
            //{
            //    for (int j = 10; j < N - 10; j++)
            //    {
            //        velocity_input[ID(i, j)] = new Vector2(0, 1f);
            //    }
            //}
        }

        //for (int i = 0; i < size; i++)
        //{
        //    velocity_input[i] = new Vector2(1, 1);
        //}

        //density[0] = 1;
        //density_prev[0] = 2;
        //float[] tmp = density;
        //density = density_prev;
        //density_prev = tmp;
        //Debug.Log(density[0]);
        //Debug.Log(density_prev[0]);
    }

    // Add source to the density
    void AddDensity(float[] d, float[] source, float dt)
    {
        for (int i = 0; i < N + 2; i++)
        {
            for (int j = 0; j < N + 2; j++)
            {
                d[ID(i, j)] += dt * source[ID(i, j)];
            }
        }
    }

    void AddDensity3D(float[] d, float[] source, float dt)
    {
        for (int i = 0; i < N + 2; i++)
        {
            for (int j = 0; j < N + 2; j++)
            {
                for (int k = 0; k < N + 2; k++)
                {
                    d[ID(i, j, k)] += dt * source[ID(i, j, k)];
                }
            }
        }
    }

    void AddVelocity(Vector2[] v, Vector2[] source, float dt)
    {
        for (int i = 0; i < N + 2; i++)
        {
            for (int j = 0; j < N + 2; j++)
            {
                v[ID(i, j)] += dt * (source[ID(i, j)] + new Vector2(0, g));
            }
        }
    }

    void AddVelocity3D(Vector3[] v, Vector3[] source, float dt)
    {
        for (int i = 0; i < N + 2; i++)
        {
            for (int j = 0; j < N + 2; j++)
            {
                for (int k = 0; k < N + 2; k++)
                {
                    v[ID(i, j, k)] += dt * source[ID(i, j, k)] + new Vector3(0, -g, 0); 
                }
            }
        }
    }

    // Exchange density with neighbors
    void Diffuse(float[] d, float[] d_prev, float dt)
    {
        float a = dt * diff * N * N;

        // Large a will oscillate. Unstable.
        //for (int i = 1; i < N + 1; i++)
        //{
        //    for (int j = 1; j < N + 1; j++)
        //    {
        //        d[ID(i, j)] = d_prev[ID(i, j)] + a * (d_prev[ID(i - 1, j)] + d_prev[ID(i + 1, j)] + d_prev[ID(i, j - 1)] + d_prev[ID(i, j + 1)] - 4 * d_prev[ID(i, j)]);
        //    }
        //}
        //SetBoundary(d);

        // Gaussian-Seidel relaxation
        for (int k = 0; k < 20; k++)
        {
            for (int i = 1; i < N + 1; i++)
            {
                for (int j = 1; j < N + 1; j++)
                {
                    d[ID(i, j)] = (d_prev[ID(i, j)] + a * (d_prev[ID(i - 1, j)] + d_prev[ID(i + 1, j)] + d_prev[ID(i, j - 1)] + d_prev[ID(i, j + 1)])) / (1 + 4 * a);
                }
            }
            SetBoundary(d);
        }
    }

    void Diffuse3D(float[] d, float[] d_prev, float dt)
    {
        float a = dt * diff * N * N * N;

        // Large a will oscillate. Unstable.
        //for (int i = 1; i < N + 1; i++)
        //{
        //    for (int j = 1; j < N + 1; j++)
        //    {
        //        d[i, j] = d_prev[i, j] + a * (d_prev[i - 1, j] + d_prev[i + 1, j] + d_prev[i, j - 1] + d_prev[i, j + 1] - 4 * d_prev[i, j]);
        //    }
        //}
        //SetBoundary();

        // Gaussian-Seidel relaxation
        for (int l = 0; l < 20; l++)
        {
            for (int i = 1; i < N + 1; i++)
            {
                for (int j = 1; j < N + 1; j++)
                {
                    for (int k = 1; k < N + 1; k++)
                    {
                        d[ID(i, j, k)] = (d_prev[ID(i, j, k)] + a * (d_prev[ID(i - 1, j, k)] + d_prev[ID(i + 1, j, k)] + d_prev[ID(i, j - 1, k)] + d_prev[ID(i, j + 1, k)]
                            + d_prev[ID(i, j, k - 1)] + d_prev[ID(i, j, k + 1)])) / (1 + 6 * a);
                    }
                }
            }
            SetBoundary3D(d);
        }
    }

    void Diffuse(Vector2[] d, Vector2[] d_prev, float dt)
    {
        float a = dt * diff * N * N;

        // Gaussian-Seidel relaxation
        for (int k = 0; k < 20; k++)
        {
            for (int i = 1; i < N + 1; i++)
            {
                for (int j = 1; j < N + 1; j++)
                {
                    // if(boundary[ID(i,j)] == 0)
                        d[ID(i, j)] = (d_prev[ID(i, j)] + a * (d_prev[ID(i - 1, j)] + d_prev[ID(i + 1, j)] + d_prev[ID(i, j - 1)] + d_prev[ID(i, j + 1)])) / (1 + 4 * a);
                }
            }
            SetBoundary(d);
        }
    }

    void Diffuse3D(Vector3[] d, Vector3[] d_prev, float dt)
    {
        float a = dt * diff * N * N * N;

        // Gaussian-Seidel relaxation
        for (int l = 0; l < 20; l++)
        {
            for (int i = 1; i < N + 1; i++)
            {
                for (int j = 1; j < N + 1; j++)
                {
                    for (int k = 1; k < N + 1; k++)
                    {
                        d[ID(i, j, k)] = (d_prev[ID(i, j, k)] + a * (d_prev[ID(i - 1, j, k)] + d_prev[ID(i + 1, j, k)] + d_prev[ID(i, j - 1, k)] + d_prev[ID(i, j + 1, k)]
                            + d_prev[ID(i, j, k - 1)] + d_prev[ID(i, j, k + 1)])) / (1 + 6 * a);
                    }
                }
            }
            SetBoundary3D(d);
        }

    }


    void SetBoundary(float[] field)
    {
        for (int i = 1; i < N + 1; i++)
        {
            field[ID(0, i)] = field[ID(1, i)];
            field[ID(N + 1, i)] = field[ID(N, i)];
            field[ID(i, 0)] = field[ID(i, 1)];
            field[ID(i, N + 1)] = field[ID(i, N)];
        }

        field[ID(0, 0)] = 0.5f * (field[ID(1, 0)] + field[ID(0, 1)]);
        field[ID(0, N + 1)] = 0.5f * (field[ID(1, N + 1)] + field[ID(0, N)]);
        field[ID(N + 1, 0)] = 0.5f * (field[ID(N + 1, 1)] + field[ID(N, 0)]);
        field[ID(N + 1, N + 1)] = 0.5f * (field[ID(N + 1, N)] + field[ID(N, N + 1)]);

        if (!useBoundary) return;
        for (int i = 1; i < N + 1; i++)
        {
            for (int j = 1; j < N + 1; j++)
            {
                if (boundary[ID(i, j)] == 2)
                {
                    int NeighborNum = 0;
                    float density = 0;
                    if (boundary[ID(i - 1, j)] == 0)
                    {
                        NeighborNum++;
                        density += field[ID(i - 1, j)];
                    }
                    if (boundary[ID(i, j - 1)] == 0)
                    {
                        NeighborNum++;
                        density += field[ID(i, j - 1)];
                    }
                    if (boundary[ID(i + 1, j)] == 0)
                    {
                        NeighborNum++;
                        density += field[ID(i + 1, j)];
                    }
                    if (boundary[ID(i, j + 1)] == 0)
                    {
                        NeighborNum++;
                        density += field[ID(i, j + 1)];
                    }
                }
                else if (boundary[ID(i, j)] == 1)
                {
                    field[ID(i, j)] = 0;
                }
            }
        }
    }

    void SetBoundary3D(float[] field)
    {
        // Surface
        for (int i = 1; i < N + 1; i++)
        {
            for (int j = 1; j < N + 1; j++)
            {
                field[ID(0, i, j)] = field[ID(1, i, j)];
                field[ID(i, 0, j)] = field[ID(i, 1, j)];
                field[ID(i, j, 0)] = field[ID(i, j, 1)];

                field[ID(N + 1, i, j)] = field[ID(N, i, j)];
                field[ID(i, N + 1, j)] = field[ID(i, N, j)];
                field[ID(i, j, N + 1)] = field[ID(i, j, N)];
            }
        }

        // Edge
        for (int i = 1; i < N + 1; i++)
        {
            field[ID(0, 0, i)] = 0.5f * (field[ID(0, 1, i)] + field[ID(1, 0, i)]);
            field[ID(0, i, 0)] = 0.5f * (field[ID(0, i, 1)] + field[ID(1, i, 0)]);
            field[ID(i, 0, 0)] = 0.5f * (field[ID(i, 0, 1)] + field[ID(i, 1, 0)]);

            field[ID(N + 1, N + 1, i)] = 0.5f * (field[ID(N, N + 1, i)] + field[ID(N + 1, N, i)]);
            field[ID(N + 1, i, N + 1)] = 0.5f * (field[ID(N, i, N + 1)] + field[ID(N + 1, i, N)]);
            field[ID(i, N + 1, N + 1)] = 0.5f * (field[ID(i, N, N + 1)] + field[ID(i, N + 1, N)]);
        }

        // Vertex
        field[ID(0, 0, 0)] = 0.333f * (field[ID(0, 0, 1)] + field[ID(0, 1, 0)] + field[ID(1, 0, 0)]);
        field[ID(0, 0, N+1)] = 0.333f * (field[ID(0, 0, N)] + field[ID(1, 0, N + 1)] + field[ID(0, 1, N + 1)]);
        field[ID(0, N + 1, 0)] = 0.333f * (field[ID(0, N, 0)] + field[ID(1, N + 1, 0)] + field[ID(0, N + 1, 1)]);
        field[ID(N + 1, 0, 0)] = 0.333f * (field[ID(N, 0, 0)] + field[ID(N + 1, 1, 0)] + field[ID(N + 1, 0, 1)]);
        field[ID(0, N + 1, N + 1)] = 0.333f * (field[ID(1, N + 1, N + 1)] + field[ID(0, N + 1, N)] + field[ID(0, N, N + 1)]);
        field[ID(N + 1, 0, N + 1)] = 0.333f * (field[ID(N + 1, 1, N + 1)] + field[ID(N, 0, N + 1)] + field[ID(N + 1, 0, N)]);
        field[ID(N + 1, N + 1, 0)] = 0.333f * (field[ID(N, N + 1, 0)] + field[ID(N + 1, N, 0)] + field[ID(N + 1, N + 1, 1)]);
        field[ID(N + 1, N + 1, N + 1)] = 0.333f * (field[ID(N, N + 1, N + 1)] + field[ID(N + 1, N, N + 1)] + field[ID(N + 1, N + 1, N)]);

        // Collision
        if (!useBoundary) return;
        for (int i = 1; i < N + 1; i++)
        {
            for (int j = 1; j < N + 1; j++)
            {
                for (int k = 1; k < N + 1; k++)
                {
                    if (boundary[ID(i, j, k)] == 2)
                    {
                        int NeighborNum = 0;
                        float density = 0;
                        if (boundary[ID(i - 1, j, k)] == 0)
                        {
                            NeighborNum++;
                            density += field[ID(i - 1, j, k)];
                        }
                        if (boundary[ID(i, j - 1, k)] == 0)
                        {
                            NeighborNum++;
                            density += field[ID(i, j - 1, k)];
                        }
                        if (boundary[ID(i + 1, j, k)] == 0)
                        {
                            NeighborNum++;
                            density += field[ID(i + 1, j, k)];
                        }
                        if (boundary[ID(i, j + 1, k)] == 0)
                        {
                            NeighborNum++;
                            density += field[ID(i, j + 1, k)];
                        }                        
                        if (boundary[ID(i, j, k - 1)] == 0)
                        {
                            NeighborNum++;
                            density += field[ID(i, j, k - 1)];
                        }                        
                        if (boundary[ID(i, j, k + 1)] == 0)
                        {
                            NeighborNum++;
                            density += field[ID(i, j, k + 1)];
                        }
                    }
                    else if (boundary[ID(i, j, k)] == 1)
                    {
                        field[ID(i, j)] = 0;
                    }
                }
            }
        }
    }

    void SetBoundary(Vector2[] field)
    {
        for (int i = 1; i < N + 1; i++)
        {
            field[ID(0, i)] = field[ID(1, i)];
            field[ID(N + 1, i)] = field[ID(N, i)];
            field[ID(i, 0)] = field[ID(i, 1)];
            field[ID(i, N + 1)] = field[ID(i, N)];

            field[ID(0, i)][0] *= velocityBounce;
            field[ID(N + 1, i)][0] *= velocityBounce;
            field[ID(i, 0)][1] *= velocityBounce;
            field[ID(i, N + 1)][1] *= velocityBounce;
            //field[ID(0, i)][0] *= 0;
            //field[ID(N + 1, i)][0] *= 0;
            //field[ID(i, 0)][1] *= 0;
            //field[ID(i, N + 1)][1] *= 0;
        }

        field[ID(0, 0)] = 0.5f * (field[ID(1, 0)] + field[ID(0, 1)]);
        field[ID(0, N + 1)] = 0.5f * (field[ID(1, N + 1)] + field[ID(0, N)]);
        field[ID(N + 1, 0)] = 0.5f * (field[ID(N + 1, 1)] + field[ID(N, 0)]);
        field[ID(N + 1, N + 1)] = 0.5f * (field[ID(N + 1, N)] + field[ID(N, N + 1)]);

        if (!useBoundary) return;
        for (int i = 1; i < N + 1; i++)
        {
            for (int j = 1; j < N + 1; j++)
            {
                if (boundary[ID(i, j)] != 0)
                {
                    field[ID(i, j)] = new Vector2(0, 0);
                }
            }
        }

        //if (!useBoundary) return;

        ////Vector2 center = new Vector2(N / 2, N / 2);

        //for (int i = 1; i < N + 1; i++)
        //{
        //    for (int j = 1; j < N + 1; j++)
        //    {
        //        if (boundary[ID(i, j)] == 1)
        //        {
        //            field[ID(i, j)] = new Vector2(0, 0);
        //        } 
        //        else if (boundary[ID(i, j)] == 2)
        //        {
        //            // field[ID(i, j)] = new Vector2(0, 0);

        //            Vector2 n = new Vector2(i - N / 2, j - N / 2).normalized;

        //            //Debug.Log(field[ID(i, j)]);
        //            //Vector2 a = field[ID(i, j)].normalized;
        //            //Debug.Log(field[ID(i, j)]);


        //            if (field[ID(i, j)].magnitude > 0)
        //                field[ID(i, j)] = (field[ID(i, j)].normalized + n * 2) * field[ID(i, j)].magnitude * 0.1f;

        //            // Debug.Log(field[ID(i, j)]);
        //        }
        //    }
        //}
    }

    void SetBoundary3D(Vector3[] field)
    {
        // Surface
        for (int i = 1; i < N + 1; i++)
        {
            for (int j = 1; j < N + 1; j++)
            {
                field[ID(0, i, j)] = field[ID(1, i, j)];
                field[ID(i, 0, j)] = field[ID(i, 1, j)];
                field[ID(i, j, 0)] = field[ID(i, j, 1)];

                field[ID(N + 1, i, j)] = field[ID(N, i, j)];
                field[ID(i, N + 1, j)] = field[ID(i, N, j)];
                field[ID(i, j, N + 1)] = field[ID(i, j, N)];

                field[ID(0, i, j)][0] *= -1;
                field[ID(i, 0, j)][1] *= -1;
                field[ID(i, j, 0)][2] *= -1;

                field[ID(N + 1, i, j)][0] *= -1;
                field[ID(i, N + 1, j)][1] *= -1;
                field[ID(i, j, N + 1)][2] *= -1;
            }
        }

        // Edge
        for (int i = 1; i < N + 1; i++)
        {
            field[ID(0, 0, i)] = 0.5f * (field[ID(0, 1, i)] + field[ID(1, 0, i)]);
            field[ID(0, i, 0)] = 0.5f * (field[ID(0, i, 1)] + field[ID(1, i, 0)]);
            field[ID(i, 0, 0)] = 0.5f * (field[ID(i, 0, 1)] + field[ID(i, 1, 0)]);

            field[ID(N + 1, N + 1, i)] = 0.5f * (field[ID(N, N + 1, i)] + field[ID(N + 1, N, i)]);
            field[ID(N + 1, i, N + 1)] = 0.5f * (field[ID(N, i, N + 1)] + field[ID(N + 1, i, N)]);
            field[ID(i, N + 1, N + 1)] = 0.5f * (field[ID(i, N, N + 1)] + field[ID(i, N + 1, N)]);
        }

        // Vertex
        field[ID(0, 0, 0)] = 0.333f * (field[ID(0, 0, 1)] + field[ID(0, 1, 0)] + field[ID(1, 0, 0)]);
        field[ID(0, 0, N + 1)] = 0.333f * (field[ID(0, 0, N)] + field[ID(1, 0, N + 1)] + field[ID(0, 1, N + 1)]);
        field[ID(0, N + 1, 0)] = 0.333f * (field[ID(0, N, 0)] + field[ID(1, N + 1, 0)] + field[ID(0, N + 1, 1)]);
        field[ID(N + 1, 0, 0)] = 0.333f * (field[ID(N, 0, 0)] + field[ID(N + 1, 1, 0)] + field[ID(N + 1, 0, 1)]);
        field[ID(0, N + 1, N + 1)] = 0.333f * (field[ID(1, N + 1, N + 1)] + field[ID(0, N + 1, N)] + field[ID(0, N, N + 1)]);
        field[ID(N + 1, 0, N + 1)] = 0.333f * (field[ID(N + 1, 1, N + 1)] + field[ID(N, 0, N + 1)] + field[ID(N + 1, 0, N)]);
        field[ID(N + 1, N + 1, 0)] = 0.333f * (field[ID(N, N + 1, 0)] + field[ID(N + 1, N, 0)] + field[ID(N + 1, N + 1, 1)]);
        field[ID(N + 1, N + 1, N + 1)] = 0.333f * (field[ID(N, N + 1, N + 1)] + field[ID(N + 1, N, N + 1)] + field[ID(N + 1, N + 1, N)]);

        if (!useBoundary) return;
        for (int i = 1; i < N + 1; i++)
        {
            for (int j = 1; j < N + 1; j++)
            {
                for (int k = 1; k < N + 1; k++)
                {
                    if (boundary[ID(i, j, k)] != 0)
                    {
                        field[ID(i, j, k)] = new Vector3(0, 0, 0);
                    }
                }
            }
        }
    }


    // Simple linear backtrace
    void Advect (float[] d, float[] d_prev, Vector2[] v, float dt)
    {
        float dt0 = dt * N;

        for (int i = 1; i < N + 1; i++)
        {
            for (int j = 1; j < N + 1; j++)
            {
                float x = Mathf.Clamp(i - dt0 * v[ID(i, j)][0], 0.5f, N + 0.5f);
                float y = Mathf.Clamp(j - dt0 * v[ID(i, j)][1], 0.5f, N + 0.5f);

                int i0 = (int)x;
                int i1 = i0 + 1;

                int j0 = (int)y;
                int j1 = j0 + 1;

                float s1 = x - i0;
                float s0 = 1 - s1;
                float t1 = y - j0;
                float t0 = 1 - t1;

                d[ID(i, j)] = s0 * (t0 * d_prev[ID(i0, j0)] + t1 * d_prev[ID(i0, j1)])
                            + s1 * (t0 * d_prev[ID(i1, j0)] + t1 * d_prev[ID(i1, j1)]);

            }
        }

        SetBoundary(d);
    }

    void Advect3D(float[] d, float[] d_prev, Vector3[] v, float dt)
    {
        float dt0 = dt * N;

        for (int i = 1; i < N + 1; i++)
        {
            for (int j = 1; j < N + 1; j++)
            {
                for (int k = 1; k < N + 1; k++)
                {
                    float x = Mathf.Clamp(i - dt0 * v[ID(i, j, k)][0], 0.5f, N + 0.5f);
                    float y = Mathf.Clamp(j - dt0 * v[ID(i, j, k)][1], 0.5f, N + 0.5f);
                    float z = Mathf.Clamp(k - dt0 * v[ID(i, j, k)][2], 0.5f, N + 0.5f);

                    int i0 = (int)x;
                    int i1 = i0 + 1;

                    int j0 = (int)y;
                    int j1 = j0 + 1;

                    int k0 = (int)z;
                    int k1 = k0 + 1;

                    float s1 = x - i0;
                    float s0 = 1 - s1;
                    float t1 = y - j0;
                    float t0 = 1 - t1;
                    float u1 = z - k0;
                    float u0 = 1 - u1;

                    d[ID(i, j, k)] = s0 * (t0 * (u0 * d_prev[ID(i0, j0, k0)] + u1 * d_prev[ID(i0, j0, k1)])
                                         + t1 * (u0 * d_prev[ID(i0, j1, k0)] + u1 * d_prev[ID(i0, j1, k1)]))
                                   + s1 * (t0 * (u0 * d_prev[ID(i1, j0, k0)] + u1 * d_prev[ID(i1, j0, k1)])
                                         + t1 * (u0 * d_prev[ID(i1, j1, k0)] + u1 * d_prev[ID(i1, j1, k1)]));
                }
            }
        }

        SetBoundary3D(d);
    }

    // Simple linear backtrace
    void Advect(Vector2[] d, Vector2[] d_prev, Vector2[] v, float dt)
    {
        float dt0 = dt * N;

        for (int i = 1; i < N + 1; i++)
        {
            for (int j = 1; j < N + 1; j++)
            {

                float x = Mathf.Clamp(i - dt0 * v[ID(i, j)][0], 0.5f, N + 0.5f);
                float y = Mathf.Clamp(j - dt0 * v[ID(i, j)][1], 0.5f, N + 0.5f);

                int i0 = (int)x;
                int i1 = i0 + 1;

                int j0 = (int)y;
                int j1 = j0 + 1;

                float s1 = x - i0;
                float s0 = 1 - s1;
                float t1 = y - j0;
                float t0 = 1 - t1;

                d[ID(i, j)] = s0 * (t0 * d_prev[ID(i0, j0)] + t1 * d_prev[ID(i0, j1)])
                            + s1 * (t0 * d_prev[ID(i1, j0)] + t1 * d_prev[ID(i1, j1)]);

            }
        }

        SetBoundary(d);
    }

    void Advect3D(Vector3[] d, Vector3[] d_prev, Vector3[] v, float dt)
    {
        float dt0 = dt * N;

        for (int i = 1; i < N + 1; i++)
        {
            for (int j = 1; j < N + 1; j++)
            {
                for (int k = 1; k < N + 1; k++)
                {
                    float x = Mathf.Clamp(i - dt0 * v[ID(i, j, k)][0], 0.5f, N + 0.5f);
                    float y = Mathf.Clamp(j - dt0 * v[ID(i, j, k)][1], 0.5f, N + 0.5f);
                    float z = Mathf.Clamp(k - dt0 * v[ID(i, j, k)][2], 0.5f, N + 0.5f);

                    int i0 = (int)x;
                    int i1 = i0 + 1;

                    int j0 = (int)y;
                    int j1 = j0 + 1;

                    int k0 = (int)z;
                    int k1 = k0 + 1;

                    float s1 = x - i0;
                    float s0 = 1 - s1;
                    float t1 = y - j0;
                    float t0 = 1 - t1;
                    float u1 = z - k0;
                    float u0 = 1 - u1;

                    d[ID(i, j, k)] = s0 * (t0 * (u0 * d_prev[ID(i0, j0, k0)] + u1 * d_prev[ID(i0, j0, k1)])
                                         + t1 * (u0 * d_prev[ID(i0, j1, k0)] + u1 * d_prev[ID(i0, j1, k1)]))
                                   + s1 * (t0 * (u0 * d_prev[ID(i1, j0, k0)] + u1 * d_prev[ID(i1, j0, k1)])
                                         + t1 * (u0 * d_prev[ID(i1, j1, k0)] + u1 * d_prev[ID(i1, j1, k1)]));
                }
            }
        }

        SetBoundary3D(d);
    }

    void Project(Vector2[] v)
    {
        float h = 1 / (float)N;

        float[] div = new float[size];
        float[] p_prev = new float[size];
        float[] p = new float[size];

        for (int i = 1; i < N + 1; i++)
        {
            for (int j = 1; j < N + 1; j++)
            {
                // Divergence
                div[ID(i, j)] = -0.5f * h * (v[ID(i + 1, j)][0] - v[ID(i - 1, j)][0] + v[ID(i, j + 1)][1] - v[ID(i, j - 1)][1]);
            }
        }

        SetBoundary(div);


        for (int k = 0; k < 10; k++)
        {
            for (int i = 1; i < N + 1; i++)
            {
                for (int j = 1; j < N + 1; j++)
                {
                    p_prev[ID(i, j)] = (div[ID(i, j)] + p[ID(i - 1, j)] + p[ID(i + 1, j)]
                            + p[ID(i, j - 1)] + p[ID(i, j + 1)]) / 4.0f;
                }
            }
            SetBoundary(p_prev);

            for (int i = 1; i < N + 1; i++)
            {
                for (int j = 1; j < N + 1; j++)
                {
                    p[ID(i, j)] = (div[ID(i, j)] + p_prev[ID(i - 1, j)] + p_prev[ID(i + 1, j)]
                            + p_prev[ID(i, j - 1)] + p_prev[ID(i, j + 1)]) / 4.0f;
                }
            }
            SetBoundary(p);
        }

        for (int i = 1; i < N + 1; i++)
        {
            for (int j = 1; j < N + 1; j++)
            {
                v[ID(i, j)][0] -= 0.5f * (p[ID(i + 1, j)] - p[ID(i - 1, j)]) / h;
                v[ID(i, j)][1] -= 0.5f * (p[ID(i, j + 1)] - p[ID(i, j - 1)]) / h;
            }
        }
        SetBoundary(v);
    }

    void Project3D(Vector3[] v)
    {
        float h = 1 / (float)N;

        float[] div = new float[size];
        float[] p = new float[size];
        float[] p_prev = new float[size];

        for (int i = 1; i < N + 1; i++)
        {
            for (int j = 1; j < N + 1; j++)
            {
                for (int k = 1; k < N + 1; k++)
                {
                    // Divergence
                    div[ID(i, j, k)] = -0.5f * h * (v[ID(i + 1, j, k)][0] - v[ID(i - 1, j, k)][0] + v[ID(i, j + 1, k)][1] - v[ID(i, j - 1, k)][1] 
                        + v[ID(i, j, k + 1)][2] - v[ID(i, j, k - 1)][2]);
                }
            }
        }

        SetBoundary3D(div);

        for (int l = 0; l < 20; l++)
        {
            for (int i = 1; i < N + 1; i++)
            {
                for (int j = 1; j < N + 1; j++)
                {
                    for (int k = 1; k < N + 1; k++)
                    {
                        p_prev[ID(i, j, k)] = (div[ID(i, j, k)] + p[ID(i - 1, j, k)] + p[ID(i + 1, j, k)]
                            + p[ID(i, j - 1, k)] + p[ID(i, j + 1, k)] + p[ID(i, j, k - 1)] + p[ID(i, j, k + 1)]) / 6.0f;
                    }
                }
            }
            SetBoundary3D(p_prev);

            for (int i = 1; i < N + 1; i++)
            {
                for (int j = 1; j < N + 1; j++)
                {
                    for (int k = 1; k < N + 1; k++)
                    {
                        p[ID(i, j, k)] = (div[ID(i, j, k)] + p_prev[ID(i - 1, j, k)] + p_prev[ID(i + 1, j, k)]
                            + p_prev[ID(i, j - 1, k)] + p_prev[ID(i, j + 1, k)] + p_prev[ID(i, j, k - 1)] + p_prev[ID(i, j, k + 1)]) / 6.0f;
                    }
                }
            }
            SetBoundary3D(p);
        }

        for (int i = 1; i < N + 1; i++)
        {
            for (int j = 1; j < N + 1; j++)
            {
                for (int k = 1; k < N + 1; k++)
                {
                    v[ID(i, j, k)][0] -= 0.5f * (p[ID(i + 1, j, k)] - p[ID(i - 1, j, k)]) / h;
                    v[ID(i, j, k)][1] -= 0.5f * (p[ID(i, j + 1, k)] - p[ID(i, j - 1, k)]) / h;
                    v[ID(i, j, k)][2] -= 0.5f * (p[ID(i, j, k + 1)] - p[ID(i, j, k - 1)]) / h;
                }
            }
        }
        SetBoundary3D(v);
    }

    void DensityStep(float[] d, float[] d_prev, float dt)
    {
        AddDensity(d, d_prev, dt);
        Diffuse(d_prev, d, dt);
        Advect(d, d_prev, velocity, dt);
    }
    
    void DensityStep3D(float dt)
    {
        AddDensity3D(density, density_prev, dt);
        Diffuse3D(density_prev, density, dt);
        Advect3D(density, density_prev, velocity3, dt);
    }

    void VelocityStep(float dt)
    {
        AddVelocity(velocity, velocity_input, dt);
        Diffuse(velocity_prev, velocity, dt);
        Project(velocity_prev);
        Advect(velocity, velocity_prev, velocity_prev, dt);
        Project(velocity);
    }
    
    void VelocityStep3D(float dt)
    {
        AddVelocity3D(velocity3, velocity_input3, dt);
        Diffuse3D(velocity_prev3, velocity3, dt);
        Project3D(velocity_prev3);
        Advect3D(velocity3, velocity_prev3, velocity_prev3, dt);
        Project3D(velocity3);
    }

    bool density1 = true;
    void UpdateSource()
    {
        //velocity_prev = new Vector2[size];
        density_prev = new float[size];
        if(multiDensities)
            density_prev2 = new float[size];

        // density_prev[ID(N / 2, N / 2)] = 30;


        if (!use3D && clearVelocity)
            velocity_input = new Vector2[size];
        
        //else
            //velocity_input3 = new Vector3[size];

        //if (mouseClicked)
        //{
        //    RaycastHit hit;
        //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //    if (Physics.Raycast(ray, out hit, 100.0f))
        //    {
        //        float u = 1 - (hit.point.x + 5) / 10.0f;
        //        float v = 1 - (hit.point.z + 5) / 10.0f;
        //        int id = (use3D) ? ID((int)(u * N), N / 2, (int)(v * N)) : ID((int)(u * N), (int)(v * N));
        //        density_prev[id] = 10;
        //        //velocity_input3[id] = new Vector3(Random.Range(-0.1f, 0.1f), 10.0f, Random.Range(-0.1f, 0.1f)) * 0.1f;
        //        // Debug.Log(id);

        //        previousU = u;
        //        previousV = v;
        //    }
        //    mouseClicked = false;


        //}


        if (mouseClicked)
        {
            if (use3D)
                randHeight = 2;// Random.Range(2, N - 1);
            
            density1 = !density1;

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                previousU = 1 - (hit.point.x + 5) / 10.0f;
                previousV = 1 - (hit.point.z + 5) / 10.0f;
            }

            mouseClicked = false;
        }

        if (mouseDragging)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                float u = 1 - (hit.point.x + 5) / 10.0f;
                float v = 1 - (hit.point.z + 5) / 10.0f;
                
                if (use3D)
                {
                    int id = ID((int)(u * N), randHeight, (int)(v * N));
                    density_prev[id] = densityAdd;
                    //velocity_input3[id] = new Vector3(u, 0, v) - new Vector3(previousU, 0, previousV);
                    //velocity_input3[id] *= velocityMul;
                    velocity_input3[id] = new Vector3(0, 1, 0) * velocityMul;
                }
                else
                {
                    int id = ID((int)(u * N), (int)(v * N));
                    if ((multiDensities && density1) || !multiDensities)
                    {
                        density_prev[id] = densityAdd;
                    }
                    else if (multiDensities && !density1)
                    {
                        density_prev2[id] = densityAdd;
                    }
                    velocity_input[id] = new Vector2(u, v) - new Vector2(previousU, previousV);
                    velocity_input[id] *= velocityMul;
                }

                // Debug.Log(id);

                previousU = u;
                previousV = v;
            }
            
        }
    }

    bool creating = false;
    int startX = 0, startY = 0;
    void CreateBoundary()
    {
        if (Input.GetMouseButtonDown(1))
        {
            
            if (!creating)
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, 100.0f))
                {
                    creating = true;

                    float u = 1 - (hit.point.x + 5) / 10.0f;
                    float v = 1 - (hit.point.z + 5) / 10.0f;

                    startX = (int)(u * N);
                    startY = (int)(v * N);

                    Debug.Log(startX);
                }
            }
            else
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, 100.0f))
                {
                    creating = false;

                    float u = 1 - (hit.point.x + 5) / 10.0f;
                    float v = 1 - (hit.point.z + 5) / 10.0f;

                    int curX = (int)(u * N);
                    int curY = (int)(v * N);

                    Debug.Log(curX);

                    // Square boundary
                    if (startX > curX) Swap(ref startX, ref curX);
                    if (startY > curY) Swap(ref startY, ref curY);
                    for (int i = startX; i < curX; i++)
                    {
                        for (int j = startY; j < curY; j++)
                        {
                            boundary[ID(i, j)] = 1;

                            boundary[ID(i - 1, j)] = (boundary[ID(i - 1, j)] == 0) ? 2 : 1;
                            boundary[ID(i, j - 1)] = (boundary[ID(i, j - 1)] == 0) ? 2 : 1;
                            boundary[ID(i + 1, j)] = (boundary[ID(i + 1, j)] == 0) ? 2 : 1;
                            boundary[ID(i, j + 1)] = (boundary[ID(i, j + 1)] == 0) ? 2 : 1;
                        
                        }
                    }
                }
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        float dt = Time.deltaTime;

        UpdateSource();

        if(useBoundary)
            CreateBoundary();

        if (use3D)
        {
            VelocityStep3D(dt);
            DensityStep3D(dt);

            this.GetComponent<Visualizer>().DrawDensity3D(N, density, boundary);
        }
            
        else
        {

            VelocityStep(dt);
            DensityStep(density, density_prev, dt);


            if (multiDensities)
            {
                VelocityStep(dt);
                DensityStep(density2, density_prev2, dt);
                this.GetComponent<Visualizer>().DrawDensity(N, density, density2, boundary);
            }
            else
            {
                this.GetComponent<Visualizer>().DrawDensity(N, density, boundary);
            }
        }
        //this.GetComponent<Visualizer>().DrawVelocity(N, velocity_prev);
    }

    public void MouseClicked()
    {
        mouseClicked = true;
    }

    public void MouseDraged(bool dragging)
    {
        mouseDragging = dragging;
    }
}
