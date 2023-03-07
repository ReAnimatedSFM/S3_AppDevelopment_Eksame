using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class FruitManager : MonoBehaviour
{
    [Header("Object Prefab")]
    public GameObject ApplePrefab, WatermelonPrefab, PineapplePrefab, BananaPrefab;
    [SerializeField] private int fruitAmount;

    private FruitObject[] fruitArray;

    private FruitRepository fruitRepository = new FruitRepository();

    int fruitIndex = 0;
    int x = 0, z = 0;

    private float xIndex;

    private void Awake()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        FruitObject curFruit = fruitArray[fruitIndex];

        if (curFruit.GetComponent<FruitObject>().isDead)
        {
            MoveFruit(curFruit);

            curFruit.GetComponent<FruitObject>().isDead = false;
            fruitIndex = (fruitIndex + 1) % fruitAmount;
        }
    }

    private void Initialize()
    {
        FruitObject[] fruits = fruitRepository.GetFruitPrefabArray(fruitAmount);

        fruitArray = new FruitObject[fruitAmount];

        for (int i = 0; i < fruitAmount; i++)
        {
            FruitObject fruitGo = Instantiate(fruits[i], new Vector3(0, -4, 0), new Quaternion());

            fruitArray[i] = fruitGo;

            if (i < 5)
            {
                fruitGo.transform.position = new Vector3(x, 0, z);
                z += 2;
            }

            MoveFruit(fruitArray[i]);
        }
    }

    void MoveFruit(FruitObject gameObj)
    {
        gameObj.transform.position = new Vector3(x, 0, z);

        if (Random.value < 0.5)
            x += 2;
        else
            z += 2;
    }
}
