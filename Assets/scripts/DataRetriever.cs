using Firebase;
using Firebase.Database;
using UnityEngine;

public class DataRetriever : MonoBehaviour
{
    DatabaseReference reference;
    public static int retrievedNumber; // Int variable to store the retrieved number
    public static DataRetriever instance;
    void Start()
    {
        instance = this;
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            reference = FirebaseDatabase.DefaultInstance.RootReference;

            // Call a method to retrieve the number
            RetrieveNumber();
        });
    }

    void RetrieveNumber()
    {
        DatabaseReference numberRef = reference.Child("images"); // "number" is the key for the number you want to retrieve

        numberRef.GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Failed to retrieve the number: " + task.Exception);
                return;
            }

            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;

                // Ensure the snapshot exists and has a value
                if (snapshot != null && snapshot.Exists)
                {
                    // Assign the retrieved number to the int variable
                    retrievedNumber = (int)(long)snapshot.Value;
                    Debug.Log("Retrieved number: " + retrievedNumber);
                    

                    // Now you have the number, you can use it in your game
                }
                else
                {
                    Debug.LogError("No data found.");
                }
            }
        });
    }

    // Example method to demonstrate using the retrieved number
    void UseRetrievedNumber()
    {
        // Example usage of the retrieved number
        int doubledNumber = retrievedNumber * 2;
        Debug.Log("Doubled number: " + doubledNumber);
    }
}
