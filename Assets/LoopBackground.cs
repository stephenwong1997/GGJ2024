using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopBackground : MonoBehaviour
{
    public float scrollSpeed = 1.0f;
    public GameObject backgroundPrefab;
    public int numberOfBackgrounds = 2;
    public float backgroundWidth;

    private Transform[] backgrounds;

    void Start()
    {
        // Initialize the backgrounds array
        backgrounds = new Transform[numberOfBackgrounds];

        // Instantiate and position background elements
        for (int i = 0; i < numberOfBackgrounds; i++)
        {
            GameObject background = Instantiate(backgroundPrefab, new Vector3(i * backgroundWidth, 0, 0), Quaternion.identity);
            backgrounds[i] = background.transform;
        }
    }

    void Update()
    {
        // Move the backgrounds based on scrollSpeed (change sign to move right)
        for (int i = 0; i < numberOfBackgrounds; i++)
        {
            backgrounds[i].position += new Vector3(scrollSpeed * Time.deltaTime, 0, 0);

            // Check if the background has moved out of view
            if (backgrounds[i].position.x > backgroundWidth * (numberOfBackgrounds - 1))
            {
                // Reposition the background to the left to create a loop
                backgrounds[i].position -= new Vector3(numberOfBackgrounds * backgroundWidth, 0, 0);
            }
        }
    }
}
