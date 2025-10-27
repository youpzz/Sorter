using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    public static PlayerScore Instance;
    [SerializeField] private int Score = 0;


    void Awake()
    {
        Instance = this;
    }

    public void AddScore(int value) => Score += value;
}
