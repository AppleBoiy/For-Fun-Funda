using UnityEngine;

public class BGMScript : MonoBehaviour
{
    public AudioSource musicSource;

    // Start is called before the first frame update
    void Start()
    {
        musicSource.Play(); 
    }
}