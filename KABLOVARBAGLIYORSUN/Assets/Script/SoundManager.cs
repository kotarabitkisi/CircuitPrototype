using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip[] sounds;//0:wrong 1:true 2:Clock 3:Sayfa
    public AudioSource source;
    public void Playsound(int id)
    {
        source.PlayOneShot(sounds[id]);
    }
}
