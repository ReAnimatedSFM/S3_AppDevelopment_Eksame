using Firebase.Extensions;
using Firebase.Firestore;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class FruitRepository : GenericRepository<Fruit>
{
    public List<Fruit> Fruits;

    public GameObject BananaPrefab, PineapplePrefab, WatermelonPrefab,
        ApplePrefab;

    /// <summary>
    /// Adds fruits from firestore to List: Fruits
    /// </summary>
    public void InitializeFruits()
    {
        var firestore = FirebaseFirestore.DefaultInstance;

        Query fruitQuery = firestore.Collection("Fruits");

        fruitQuery.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            QuerySnapshot querySnapshot = task.Result;
            foreach (DocumentSnapshot documentSnapshot in querySnapshot.Documents)
            {
                Fruit fruit = documentSnapshot.ConvertTo<Fruit>();
                Fruits.Add(fruit);
            }
        });
    }

    /// <summary>
    /// Returns array with a specified amount, with random fruits from firestore
    /// </summary>
    /// <param name="amount"></param>
    /// <returns></returns>
    public Fruit[] GetFruitArrayBySpecifiedAmount(int amount)
    {
        if (Fruits.Count <= 0)
            InitializeFruits();

        Fruit[] fruitsToReturn = new Fruit[amount];

        for (int i = 0; i < amount; i++)
        {
            int index = Random.Range(0, 3);
            Fruit rndFruit = Fruits[index];
            fruitsToReturn[i] = rndFruit;
        }

        return fruitsToReturn;
    }

    public FruitObject[] GetFruitPrefabArray(int amount)
    {
        if (Fruits.Count <= 0)
            InitializeFruits();

        var fruitPrefabArray = new FruitObject[amount];

        for (int i = 0; i < amount; i++)
        {
            int index = Random.Range(0, 3);
            Fruit rndFruit = Fruits[index];
            switch (rndFruit.Name)
            {
                case "Banana":
                    FruitObject banana = new FruitObject();
                    banana.thisFruit = rndFruit;
                    fruitPrefabArray[i] = banana;
                    break;

                case "Pinapple":
                    FruitObject pineapple = new FruitObject();
                    pineapple.thisFruit = rndFruit;
                    fruitPrefabArray[i] = pineapple;
                    break;

                case "Apple":
                    FruitObject apple = new FruitObject();
                    apple.thisFruit = rndFruit;
                    fruitPrefabArray[i] = apple;
                    break;

                case "Watermelon":
                    FruitObject watermelon = new FruitObject();
                    watermelon.thisFruit = rndFruit;
                    fruitPrefabArray[i] = watermelon;
                    break;

                default:
                    break;
            }

        }
        return fruitPrefabArray;
    }
}
