using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    #region Variables

    enum states { Alive , Dying , Transcending };
    int CurrentLevel;
    int TotalLevel;
    states State;

    Rigidbody rigidBody;
    AudioSource audioSource;

    public ParticleSystem Thrust_P;
    public ParticleSystem Explosion_P;
    public ParticleSystem Won_P;

    public float thrustValue ;
    public float rotateValue ;

    public AudioClip Thrust_S;
    public AudioClip Explosion_S;
    public AudioClip Won_S;

    #endregion

    private void Start()
    {
        TotalLevel = SceneManager.sceneCountInBuildSettings;
             
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        CurrentLevel = SceneManager.GetActiveScene().buildIndex;

        Rotating();
    }
    private void FixedUpdate()
    {
        Thrusting();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (State == states.Dying || State == states.Transcending)
        { return; }

        string Tag = collision.gameObject.tag;
        
        if(Tag == "Start")
        {
            
        }
        else if(Tag == "Finish")
        {
            Thrust_P.Stop();
            State = states.Transcending;
            transcending();
        }
        else
        {
            Thrust_P.Stop();
            State = states.Dying;
            dying();
        }
    }
    void transcending()
    {
        audioSource.Stop();
        audioSource.PlayOneShot(Won_S);
        Won_P.Play();
        Invoke("LoadNextLevel", 2f);
    }
    void dying()
    {
        audioSource.Stop();
        audioSource.PlayOneShot(Explosion_S);
        Explosion_P.Play();
        Invoke("LoadSameLevel", 2f);
    }
    private void LoadNextLevel()
    {
        Won_P.Stop();
        audioSource.Stop();
        if(CurrentLevel == TotalLevel - 1) 
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(CurrentLevel + 1);
        }
    }
    private void LoadSameLevel()
    {
        Explosion_P.Stop();
        audioSource.Stop();
        SceneManager.LoadScene(CurrentLevel);
    }

    void Thrusting()
    {
        if(State == states.Transcending || State == states.Dying)
        { return;  }

        float Thrust = thrustValue * Time.fixedDeltaTime * 100f;

        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up * Thrust);

            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(Thrust_S);
            }
            Thrust_P.Play();
        }
        else
        {
            audioSource.Stop();
            Thrust_P.Stop();
        }
    }
    void Rotating()
    {
        if (State == states.Transcending || State == states.Dying)
        { return; }

        rigidBody.freezeRotation = true;
        float rcsThrust = rotateValue * Time.fixedDeltaTime * 10;

        if (Input.GetKey(KeyCode.A))
        {
            
            transform.Rotate(Vector3.forward * rcsThrust);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rcsThrust);
        }

        rigidBody.freezeRotation = false;
    }
}