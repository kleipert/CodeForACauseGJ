using System;
using UnityEngine;
using Random = System.Random;

namespace Enemies
{
    public class ModelPicker : MonoBehaviour
    {
        public GameObject[] models;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            Random rand = new Random(Guid.NewGuid().GetHashCode());
            int chosenModel = rand.Next(0, models.Length-1);
            for (int i = 0; i < models.Length; i++)
            {
                models[i].SetActive(i == chosenModel);
            }
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
