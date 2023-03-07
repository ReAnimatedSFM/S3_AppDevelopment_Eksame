using Firebase.Firestore;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class UserRepository : GenericRepository<User>
{
    private string UserId;

    public IEnumerable<User> GetUser(string id, List<Dictionary<string, object>> keyValuePairs)
    {
        if (keyValuePairs == null)
        {
            yield return null;
            yield break;
        }

        foreach (var item in keyValuePairs)
        {
            if (item.TryGetValue("UserId", out object obj))
            {
                if (obj.ToString() == id)
                {
                    var firestore = FirebaseFirestore.DefaultInstance;
                    DocumentReference docRef = firestore.Collection("Users").Document(id);

                    Task<DocumentSnapshot> task = Task.Run(async () =>
                    {
                        DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
                        return snapshot;
                    });

                    if (task.IsCanceled || task.IsFaulted)
                    {
                        Debug.Log(task.Exception);
                        yield break;
                    }

                    User user = task.Result.ConvertTo<User>();

                    yield return user;
                }
            }
        }
    }

    public IEnumerable<Dictionary<string, object>> Get(string userId, List<Dictionary<string, object>> keyValuePairs)
    {
        if (keyValuePairs != null)
        {
            foreach (var item in GenericList)
            {
                if (item.TryGetValue("UserId", out object obj))
                {
                    if (userId.Equals(obj.ToString()))
                    {
                        yield return item;
                        break;
                    }
                }
            }
        }
    }

    public string GetId(string userId, List<Dictionary<string, object>> keyValuePairs)
    {
        Debug.Log("Entered ID method");

        if (keyValuePairs == null)
        {
            Debug.Log("keyValuePairs is null");
            return null;
        }

        else
        {
            Debug.Log("KeyValuePair was not null" + keyValuePairs.Count);

            foreach (var item in keyValuePairs)
            {
                Debug.Log("Checking Dictionairy from list");
                if (item.TryGetValue("UserId", out object obj))
                {
                    Debug.Log("Dictionairy contains value for UserId");
                    if (obj.ToString() == userId)
                    {
                        Debug.Log("Success");
                        break;
                    }
                }
            }
            return userId;
        }

    }
}
