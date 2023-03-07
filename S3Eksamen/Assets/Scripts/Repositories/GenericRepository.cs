using Firebase.Firestore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class GenericRepository<TEntity>
{
    public static List<Dictionary<string, object>> GenericList;

    public IEnumerator Add(TEntity entity, string collection, string entityId)
    {
        var firestore = FirebaseFirestore.DefaultInstance;

        Task task = Task.Run(async () =>
        {
            await firestore.Collection(collection).Document(entityId).SetAsync(entity);
        });

        while (!task.IsCompleted)
        {
            yield return null;
        }
    }

    protected IEnumerator<List<Dictionary<string, object>>> LoadAllDocuments(string get)
    {
        var firestore = FirebaseFirestore.DefaultInstance;

        List<Dictionary<string, object>> results = new List<Dictionary<string, object>>();

        CollectionReference listRef = firestore.Collection(get);

        Task task = Task.Run(async () =>
        {
            QuerySnapshot snapshots = await listRef.GetSnapshotAsync();

            foreach (DocumentSnapshot document in snapshots.Documents)
            {
                results.Add(document.ToDictionary());
            }

            return results;
        });

        while (!task.IsCompleted)
            yield return null;
    }

    public IEnumerator<List<Dictionary<string, object>>> GetEntityList(string collection)
    {
        var firestore = FirebaseFirestore.DefaultInstance;

        CollectionReference listRef = firestore.Collection(collection);


        Debug.Log("Starting task");


        Task<QuerySnapshot> snapshotTask = Task.Run(async () =>
        {
            QuerySnapshot querySnapshot = await listRef.GetSnapshotAsync();
            return querySnapshot;
        });

        while (!snapshotTask.IsCompleted)
            yield return null;

        List<Dictionary<string, object>> results = new List<Dictionary<string, object>>();

        foreach (DocumentSnapshot document in snapshotTask.Result.Documents)
        {
            results.Add(document.ToDictionary());
        }

        yield return results;
    }

    public async Task<List<Dictionary<string, object>>> GetEntityListAsync(string collection)
    {
        var firestore = FirebaseFirestore.DefaultInstance;

        CollectionReference listRef = firestore.Collection(collection);

        Debug.Log("Starting task");

        QuerySnapshot snapshots = await listRef.GetSnapshotAsync();

        List<Dictionary<string, object>> results = new List<Dictionary<string, object>>();

        foreach (DocumentSnapshot document in snapshots.Documents)
        {
            results.Add(document.ToDictionary());
        }
        return results;

    }

}