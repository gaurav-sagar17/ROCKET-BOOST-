using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement ; 

public class CollisionHandler : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] float LevelDelayTime = 1f;
    [SerializeField] AudioClip CrashAudio;
    [SerializeField] AudioClip SuccessAudio;
    // [SerializeField] AudioClip ThrustAudio; 
    [SerializeField] ParticleSystem CrashParticles;  
    [SerializeField] ParticleSystem SuccessParticles;  


    AudioSource audioSource;

    bool IsControlable = true;
    bool isCollideable = true;  
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();  
    }

    // Update is called once per frame
    void Update()
    {
        RespondToDebugKeys(); 
    }

    void RespondToDebugKeys()
    {
        if (Keyboard.current.lKey.wasPressedThisFrame)
        {
            LoadNextLevel();
        }
        else if (Keyboard.current.cKey.wasPressedThisFrame)
        {
            if (isCollideable)
            {
                isCollideable = false;
            }
            else
            {
                isCollideable = true;
            }
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        if (!IsControlable || !isCollideable)
        {
            return;  
        }

        switch (other.gameObject.tag)
        {
            case "FRIENDLY":
                Debug.Log("this is friendly , no issues   .");
                break;
            case "FINISH":
                // Debug.Log("this is ending / finidhing  ") ;   
                // load the next level  ; 

                // to do some audio / animations  / particle systems  , etc before loading the enxt scene  
                StartFinishSequence();
                // LoadNextLevel() ; 
                break;

            default:
                // Debug.Log("u crashed dummy  !! ") ;   
                //  we want our level to retart . load the same scene form the beginning 
                StartCrashSequence();
                // ReloadLevel() ; 
                break;
        }
    }

    void StartFinishSequence()
    {
        if (!IsControlable)
        {
            return;
        }
        
        GetComponent<RocketMovement>().enabled = false;
        IsControlable = false; 
        audioSource.PlayOneShot(SuccessAudio); 
        SuccessParticles.Play(); 
        
        Invoke("LoadNextLevel", LevelDelayTime); 
    }

    void StartCrashSequence()
    {
        // if the palyer is not controllable -->  something has already ahppened  ie eitehr it crashed or it has cleared th level  
        if (!IsControlable)
        {
            // retyrhning waiting for the next scene to load  '
            return;
        }
        
        GetComponent<RocketMovement>().enabled = false;
        IsControlable = false;
        audioSource.PlayOneShot(CrashAudio);  
        CrashParticles.Play(); 
        
        Invoke("ReloadLevel", LevelDelayTime); 
    }
    void LoadNextLevel()
    {

        int currentLevel = SceneManager.GetActiveScene().buildIndex;
        int nextLevel = currentLevel + 1;
        int total = SceneManager.sceneCountInBuildSettings;
        if (nextLevel == total)
        {
            nextLevel = 0;
        }
        
        SceneManager.LoadScene(nextLevel); 
    }


    void ReloadLevel() {
        int currentScene = SceneManager.GetActiveScene().buildIndex ;
        
        SceneManager.LoadScene(currentScene) ; 
    }
}
