using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirebaseManager : AuthManager
{
    public static FirebaseManager Instance;

    public static UserRepository userRepository;

    private void Awake()
    {
        if (Instance == null && Instance != this)
        {
            Instance = this;

            InitializeComponents();
        }
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        userRepository = new UserRepository();
    }

    public IEnumerator SetUserData()
    {
        CoroutineWithData cd = new CoroutineWithData(this, userRepository.GetEntityList("Users"));

        List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();

        yield return cd.coroutine;

        foreach (var user in list)
        {
            if (user.TryGetValue("UserId", out object obj))
            {
                if (firebaseUser.UserId == obj.ToString())
                {
                    UserData.ThisUser = (User)userRepository.GetUser(firebaseUser.UserId, list);
                    break;
                }
            }
        }
    }

    public void SetFirebaseData()
    {
        FirebaseData.ThisFirebaseAuth = auth;
        FirebaseData.ThisFirebaseUser = firebaseUser;
        FirebaseData.ThisDependecyStatus = dependencyStatus;
        FirebaseData.Instance = Instance;
    }
}
