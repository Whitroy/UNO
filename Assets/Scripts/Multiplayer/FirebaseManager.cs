using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using Firebase.Firestore;
using System.Collections.Generic;
using Firebase.Storage;
using System.Threading.Tasks;

public class FirebaseManager : MonoBehaviour
{
    private FirebaseAuth auth = null;
    private FirebaseUser currentUser = null;
    private FirebaseFirestore db = null;
    private FirebaseStorage storage = null;

    private static FirebaseManager _instance = null;

    public static FirebaseManager Instance { get => _instance;private  set => _instance = value; }

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(Instance);
        }

        Instance = this;
    }

    public void SaveData(string _username,string _email, string _password)
    {
        DocumentReference docRef = db.Collection("Users").Document(_username);
        Dictionary<string, object> user = new Dictionary<string, object>()
        {
            {"Username",_username },
            {"Email Id",_email },
            {"password",_password}
        };


        Debug.Log("start writing");
        docRef.SetAsync(user).ContinueWithOnMainThread(
                task =>
                {
                    if (task.IsFaulted)
                    {
                        Debug.Log("Faulted");
                    }

                    if (task.IsCanceled)
                    {
                        Debug.Log("Cancelled");
                    }
                    Debug.Log($"<color=green>Added data to the {_username} document" +
                        $" in the Users Collection. </color>");
                }
            );
        Debug.Log("Stop Writing");
    }

    void demo()
    {
        /*if (Input.GetKeyDown(KeyCode.Return))
        {
            StorageReference storageReference = storage.GetReferenceFromUrl("gs://fir-project-a0120.appspot.com");
            StorageReference up = storageReference.Child("s.jpg");
            up.PutFileAsync("C:\\Users\\techn\\OneDrive\\Pictures\\Screenshots\\Screenshot (1).png").ContinueWith(
                (Task<StorageMetadata> task) =>
                {
                    if (task.IsFaulted || task.IsCanceled)
                    {
                        Debug.Log(task.Exception.ToString());
                        // Uh-oh, an error occurred!
                    }
                    else
                    {
                        // Metadata contains file metadata such as size, content-type, and download URL.
                        StorageMetadata metadata = task.Result;
                        string md5Hash = metadata.Md5Hash;
                        Debug.Log("Finished uploading...");
                        Debug.Log("md5 hash = " + md5Hash);
                    }
                }
                
                    );



        }*/
    }

    private void Update()
    {
        demo();
    }

    private void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
        db = FirebaseFirestore.DefaultInstance;
        storage = FirebaseStorage.DefaultInstance;
    }

    private void AuthStateChanged(object sender,System.EventArgs eventArgs)
    {
        if(auth.CurrentUser != currentUser)
        {
            
        }
    }

    private void LogExeception(FirebaseException exception)
    {
        Debug.Log($"<color=red>{exception.Message}</color>");
    }
    private void IntializeFirebase()
    {
        auth = FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
    }

    public bool SignInUser(string _email,string _password)
    {
        if (_email == "" || _password.Length < 6)
        {
            Debug.Log($"<color=red>Not valid Data! Email :{_email} | Password :{_password}</color>");
            return false;
        }

        if (auth.SignInWithEmailAndPasswordAsync(_email, _password).ContinueWith(
            task =>
            {
                if (task.IsCanceled)
                {
                    Debug.Log($"<color=green>Task Got Cancelled.");
                    return;
                }

                if (task.IsFaulted)
                {
                    FirebaseException exception = task.Exception.InnerExceptions[0]
                                                        as FirebaseException;

                    LogExeception(exception);

                }

                if (task.IsCompleted)
                {
                    Debug.Log($"<color=green>User Successfully SignIn!</color>");
                    Firebase.Auth.FirebaseUser newUser = task.Result;
                    Debug.LogFormat("Firebase user signin successfully: {0} ({1})",
                        newUser.DisplayName, newUser.UserId);
                    return;
                }

            }).IsCompleted)
            return true;

        return false;
    }

    public bool SignUpNewUser(string _email,string _password) 
    {
        if(_email == "" || _password.Length < 6)
        {
            Debug.Log($"<color=red>Not valid Data! Email :{_email} | Password :{_password}</color>");
            return false;
        }

        if (auth.CreateUserWithEmailAndPasswordAsync(_email, _password).ContinueWith(
                task =>
                {
                    if (task.IsCanceled)
                    {
                        Debug.Log($"<color=green>Task Got Cancelled.");
                        return;
                    }

                    if (task.IsFaulted)
                    {
                        FirebaseException exception = task.Exception.InnerExceptions[0]
                                                            as FirebaseException;

                        LogExeception(exception);

                    }

                    if (task.IsCompleted)
                    {
                        Debug.Log($"<color=green>User Successfully Created!</color>");
                        Firebase.Auth.FirebaseUser newUser = task.Result;
                        Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                            newUser.DisplayName, newUser.UserId);
                        return;
                    }
                }
            ).IsCompleted)
            return true;  


        return false;
    }
}
