using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    bool gameEnded = false;
    public bool gameResult; // make accessible for announcement
    public ParticleSystem Confetti;
    ParticleSystem confetti_ps;

    void Awake(){
        Time.timeScale = 1.0f;
    }

    void Start(){
        confetti_ps = Confetti.GetComponent<ParticleSystem>();
        confetti_ps.Play();
        Debug.Log(confetti_ps.isPlaying);
    }

    public void EndGame(){
        if (gameEnded == false){
            gameEnded = true;
        }
    }

    public void Continue(){
        if (gameResult){
            if (SceneManager.GetActiveScene().buildIndex < 3) {
                NextLevel();
            } else {
                Celebrate();
            }
        } else {
            Restart();
        }
    }

    public void Celebrate(){ // remeber to set particle system's time to unscaled
        if (!confetti_ps.isPlaying){
            confetti_ps.Play();
        } else {
            confetti_ps.Clear();
            confetti_ps.Play();
        }
        Debug.Log(confetti_ps.isPlaying);
    }

    public void Restart(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextLevel(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }   

    public void MainMenu(){
        SceneManager.LoadScene(0);
    }

    public void Determine(bool result){
        gameResult = result;
        Time.timeScale = 0f;
    } 
}
