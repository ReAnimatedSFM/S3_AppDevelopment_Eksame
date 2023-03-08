using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class FruitManager : MonoBehaviour
{
    [Header("Object Prefab")]
    [SerializeField] GameObject fruitPrefab;

    public int fruitAmount;

    private GameObject[] fruitArray;

    private FruitObject[] fruits;

    private FruitRepository fruitRepository = new FruitRepository();

    int fruitIndex = 0;
    int x = 0, z = 0;

    bool finishedInitialization = false;

    private float xIndex;

    private void Awake()
    {
    }

    private void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
       
        if (finishedInitialization)
        {
            GameObject curFruit = fruitArray[fruitIndex];

            if (curFruit == null)
                Debug.Log("curFruit is null");
            else if (curFruit.GetComponent<FruitObject>().isDead)
            {
                MoveFruit(curFruit);

                curFruit.GetComponent<FruitObject>().isDead = false;
                fruitIndex = (fruitIndex + 1) % fruitAmount;
            }
        }
    }

    private void Initialize()
    {
        fruits = new FruitObject[fruitAmount];

        FruitRepository.Instance.RunGetFunc(this, fruitAmount);

        fruits = FruitRepository.Instance.fruitObjects;

        fruitArray = new GameObject[fruitAmount];

        
            for (int i = 0; i < fruitAmount; i++)
            {
                Debug.Log("Instansiating fruitArray");

                GameObject fruitGo = SetFruit(fruits[i].thisFruit);

                fruitArray[i] = fruitGo;

                if (i < 5)
                {
                    fruitGo.transform.position = new Vector3(x, 0, z);
                    z += 2;
                    continue;
                }

                MoveFruit(fruitArray[i]);
            }

        finishedInitialization = true;


    }

    /*
    private void Initialize()
    {
        FruitObject[] fruits = new FruitObject[fruitAmount];
        fruits = fruitRepository.GetFruitPrefabArray(fruitAmount);

        fruitArray = new GameObject[fruitAmount];

        for (int i = 0; i < fruitAmount; i++)
        {
            currentFruit = fruits[i];
            currentFruit.StateChanged += FruitStateChanged;
            FruitStateChanged(this, null);

            GameObject fruitPrefabInstance = fruitPrefab;
            fruitPrefabInstance.GetComponent<FruitObject>().thisFruit = currentFruit.thisFruit;

            GameObject fruitGo = Instantiate(fruitPrefabInstance, new Vector3(0, -4, 0), new Quaternion());

            fruitArray[i] = fruitGo;

            if (i < 5)
            {
                fruitGo.transform.position = new Vector3(x, 0, z);
                z += 2;
            }

            MoveFruit(fruitArray[i]);
        }
    }
    */

    void MoveFruit(GameObject gameObj)
    {
        gameObj.transform.position = new Vector3(x, 0, z);

        if (Random.value < 0.5)
            x += 2;
        else
            z += 2;
    }

    private GameObject SetFruit(Fruit fruit)
    {
        GameObject fruitGo = Instantiate(fruitPrefab, new Vector3(0, -4, 0), new Quaternion());
        fruitGo.GetComponent<FruitObject>().isDead = false;
        fruitGo.GetComponent<FruitObject>().thisFruit = fruit;

        switch(fruit.Name)
        {
            case "Banana":
                fruitGo.GetComponent<FruitObject>().thisRenderer.material = fruitPrefab.GetComponent<FruitObject>().Materials[1];
                break;
            case "Pineapple":
                fruitGo.GetComponent<FruitObject>().thisRenderer.material = fruitPrefab.GetComponent<FruitObject>().Materials[3];
                break;
            case "Apple":
                fruitGo.GetComponent<FruitObject>().thisRenderer.material = fruitPrefab.GetComponent<FruitObject>().Materials[0];
                break;
            case "Watermelon":
                fruitPrefab.GetComponent<FruitObject>().thisRenderer.material = fruitPrefab.GetComponent<FruitObject>().Materials[4];
                break;
        }
        return fruitGo;
    }
}
