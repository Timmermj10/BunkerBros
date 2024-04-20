using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public AudioSource soundPrefab;

    public AudioClip zombieAttack;
    public AudioClip zombieDeath;
    public AudioClip zombieDamaged;
    public AudioClip missileExplosion;
    public AudioClip nukeExplosion;
    public AudioClip airdropLandedSound;
    public AudioClip knifeSwingSound;
    public AudioClip shootingSound;

    private GameObject player;

    void Start()
    {
        player = GameObject.Find("player");

        EventBus.Subscribe<PlayerRespawnEvent>(playerRespawn);
        EventBus.Subscribe<ObjectDestroyedEvent>(zombieDeathSound);
        EventBus.Subscribe<zombieDamagedEvent>(zombieDamagedSound);
        EventBus.Subscribe<AirdropLandedEvent>(explosionNoises);
        EventBus.Subscribe<KnifeAttackSoundEvent>(knifeSwing);
        EventBus.Subscribe<ShootEvent>(gunSound);
    }

    private void playerRespawn(PlayerRespawnEvent e)
    {
        player = e.activePlayer;
    }

    private void zombieDamagedSound(zombieDamagedEvent e)
    {
        PlaySoundAtLocation(zombieDamaged, e.position, 0.5f, 8);
    }

    private void knifeSwing(KnifeAttackSoundEvent e)
    {
        PlaySoundAtLocation(knifeSwingSound, player.transform.position, 1f, 3);
    }

    private void gunSound(ShootEvent e)
    {
        PlaySoundAtLocation(shootingSound, player.transform.position, 1f, 3);
    }

    private void explosionNoises(AirdropLandedEvent e)
    {
        switch (e.itemID)
        {
            case 4:
                PlaySoundAtLocation(nukeExplosion, e.itemLocation, 0.8f, 100);
                break;
            case 5:
                //missile
                PlaySoundAtLocation(missileExplosion, e.itemLocation, 0.8f, 25);
                break;
            default:
                PlaySoundAtLocation(airdropLandedSound, e.itemLocation, 20f, 10);
                break;
        }

    }

    private void zombieDeathSound(ObjectDestroyedEvent e)
    {
        if (e.tag is "Enemy") PlaySoundAtLocation(zombieDeath, e.deathCoordinates, 0.5f, 8);
    }

    private void PlaySoundAtLocation(AudioClip clip, Vector3 position, float volume, float maxSoundDistance)
    {
        if (player != null)
        {
            AudioSource soundInstance = Instantiate(soundPrefab, position, Quaternion.identity);
            soundInstance.clip = clip;
            soundInstance.volume = volume;

            soundInstance.spatialBlend = 1f; // Set to 3D spatialization
            soundInstance.minDistance = 1.5f; // Minimum distance at which the sound is heard at full volume
            soundInstance.maxDistance = maxSoundDistance;
            soundInstance.Play();
            Destroy(soundInstance.gameObject, clip.length); // Destroy the AudioSource after the sound finishes playing
        }
    }
}
