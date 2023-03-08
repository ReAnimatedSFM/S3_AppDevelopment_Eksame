using Firebase.Extensions;
using Firebase.Firestore;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class FruitRepository : GenericRepository<Fruit>
{
    public static FruitRepository Instance
    {
        get { return instance; }
        set 
        { 
            if (instance == null) 
            { 
                value = new FruitRepository();
                instance = value;
            }
        }
    }

    private static FruitRepository instance;

    public static Fruit[] Fruits = new Fruit[4];

    public FruitObject[] fruitObjects;

    private static int counter = 0;

    /// <summary>
    /// Adds fruits from firestore to List: Fruits
    /// </summary>
    public IEnumerator<Fruit[]> InitializeFruits()
    {
        var firestore = FirebaseFirestore.DefaultInstance;

        Query fruitQuery = firestore.Collection("Fruits");

        Task<Fruit[]> returnTask = Task.Run(() =>
            fruitQuery.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            Debug.Log("Initializing fruits");
            QuerySnapshot querySnapshot = task.Result;
            foreach (DocumentSnapshot documentSnapshot in querySnapshot.Documents)
            {
                Fruit fruit = documentSnapshot.ConvertTo<Fruit>();
                Fruits[counter] = fruit;
                counter++;
            }
            return Fruits;
        }));

        yield return returnTask.Result;
    }

    /// <summary>
    /// Returns array with a specified amount, with random fruits from firestore
    /// </summary>
    /// <param name="amount"></param>
    /// <returns></returns>
    public Fruit[] GetFruitArrayBySpecifiedAmount(int amount)
    {
        if (Fruits.Length == 0)
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

    public IEnumerator RunGetFunc(MonoBehaviour owner, int amount)
    {
        CoroutineWithData cd = new CoroutineWithData(owner, InitializeFruits());

        yield return cd.coroutine;

        fruitObjects = GetFruitPrefabArray(amount, (Fruit[])cd.result);
    }

    public FruitObject[] GetFruitPrefabArray(int amount, Fruit[] frtArray)
    {
        var fruitPrefabArray = new FruitObject[amount];

        for (int i = 0; i < amount; i++)
        {
            int index = Random.Range(0, 3);
            Fruit rndFruit = frtArray[index];

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
