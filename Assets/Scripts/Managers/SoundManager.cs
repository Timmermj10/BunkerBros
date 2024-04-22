using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class SoundManager : MonoBehaviour
{

    public AudioSource soundPrefab;


    public AudioClip titleScreenMusicTrack;
    public AudioClip mainMusicTrack;
    public AudioClip finalWaveMusicTrack;
    public AudioClip victoryMusicTrack;
    public AudioClip deathMusicTrack;
    private AudioSource musicInstance;

    public AudioClip unlockingChestSound;
    public AudioClip fixingBunkerSound;
    public AudioClip activatingKeypad;
    public AudioClip pickingUpItemSound;
    private AudioSource interactSoundPrefabInstance;
    private bool isInteracting = false;

    public AudioClip managerCodeCorrect;
    public AudioClip managerCodeWrong;
    public AudioClip managerButtonPress;
    public AudioClip playerRadioTowerActivateSound;
    public AudioClip bombFalling;

    public AudioClip coinPickupSound;
    public AudioClip drawKnifeSound;
    public AudioClip drawGunSound;
    public AudioClip emptyAmmoSound;
    public AudioClip reloadSound;

    public AudioClip siloLoadedSound;
    public AudioClip nukeLanchedSound;
    public AudioClip ItemSelectSound;

    public AudioClip chestOpeningSound;
    public AudioClip zombieAttack;
    public AudioClip zombieDeath;
    public AudioClip zombieDamaged;
    public AudioClip missileExplosion;
    public AudioClip nukeExplosion;
    public AudioClip airdropLandedSound;
    public AudioClip knifeSwingSound;
    public AudioClip shootingSound;
    public AudioClip turretFire;
    public AudioClip bunkerAlarmNoise;
    private float bunkerAlarmTimer = 0;
    private float bunkerLastCallTime = 0;

    public AudioClip footstepSound;
    private AudioSource footstepPrefabInstance;

    public AudioClip playerDamaged;

    private GameObject player;


    static SoundManager instance;

    void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        player = GameObject.Find("player");

        EventBus.Subscribe<PlayerRespawnEvent>(playerRespawn);
        EventBus.Subscribe<ObjectDestroyedEvent>(zombieDeathSound);
        EventBus.Subscribe<zombieDamagedEvent>(zombieDamagedSound);
        EventBus.Subscribe<AirdropLandedEvent>(explosionNoises);
        EventBus.Subscribe<ItemUseEvent>(MissileOrNukeWhistle);
        EventBus.Subscribe<KnifeAttackSoundEvent>(knifeSwing);
        EventBus.Subscribe<TurretShootingEvent>(turretShooting);
        EventBus.Subscribe<ShootEvent>(gunSound);
        EventBus.Subscribe<ObjectiveDamagedEvent>(bunkerAlarm);
        EventBus.Subscribe<PlayerMovingEvent>(footstepSounds);
        EventBus.Subscribe<PlayerDamagedEvent>(metalDamageNoise);
        EventBus.Subscribe<LastWaveEvent>(finalWaveMusic);
        EventBus.Subscribe<LastWaveOverEvent>(stopMusic);
        EventBus.Subscribe<CoinCollect>(coinsCollected);
        EventBus.Subscribe<GameplayStartEvent>(startMainMusic);
        EventBus.Subscribe<EmptyAmmo>(noAmmoNoise);
        EventBus.Subscribe<SiloLoadedEvent>(loadSilo);
        EventBus.Subscribe<SiloUnloadedEvent>(nukeLaunched);
        EventBus.Subscribe<ManagerButtonClickEvent>(ManagerItemSelect);
        EventBus.Subscribe<WeaponSwapEvent>(swapWeapons);
        EventBus.Subscribe<VictoryMusicEvent>(playVictoryMusic);
        EventBus.Subscribe<DeathMusicEvent>(playDeathMusic);
        EventBus.Subscribe<ReloadEvent>(reload);

        EventBus.Subscribe<InteractTimerStartedEvent>(playInteractSound);
        EventBus.Subscribe<InteractTimerEndedEvent>(stopInteractSound);

        EventBus.Subscribe<ManagerButtonPress>(buttonPressed);
        EventBus.Subscribe<ManagerIncorrectAnswer>(incorrectCode);
        EventBus.Subscribe<RadioTowerActivatedManagerEvent>(correctCode);
        EventBus.Subscribe<RadioTowerActivatedPlayerEvent>(playerActivateTower);



        musicInstance = Instantiate(soundPrefab, Vector3.zero, Quaternion.identity);
        musicInstance.clip = titleScreenMusicTrack;
        musicInstance.volume = 0.15f;
        musicInstance.spatialBlend = 0;
        musicInstance.loop = true;
        musicInstance.Play();
    }

    private void swapWeapons(WeaponSwapEvent e)
    {
        if (e.trueIsKnife)
        {
            PlaySoundAtLocation(drawKnifeSound, player.transform.position, 0.7f, 3);
        }
        else
        {
            PlaySoundAtLocation(drawGunSound, player.transform.position, 0.7f, 3);
        }
        
    }

    private void stopInteractSound(InteractTimerEndedEvent e)
    {
        isInteracting = false;
        interactSoundPrefabInstance.Stop();
    }

    private void playInteractSound(InteractTimerStartedEvent e)
    {
        if (player == null) return;

        isInteracting = true;

        if (e.item.name is "ChestPack" || e.item.name is "ChestPack(Clone)")
        {
            interactSoundPrefabInstance = Instantiate(soundPrefab, player.transform.position, Quaternion.identity);
            interactSoundPrefabInstance.clip = unlockingChestSound;
            interactSoundPrefabInstance.volume = 1f;
            interactSoundPrefabInstance.loop = true;
            interactSoundPrefabInstance.Play();
        }
        else if (e.item.name is "Objective")
        {
            interactSoundPrefabInstance = Instantiate(soundPrefab, player.transform.position, Quaternion.identity);
            interactSoundPrefabInstance.clip = fixingBunkerSound;
            interactSoundPrefabInstance.volume = 0.4f;
            interactSoundPrefabInstance.loop = true;
            interactSoundPrefabInstance.Play();
        }
        else if (e.item.name is "ControlPanel")
        {
            interactSoundPrefabInstance = Instantiate(soundPrefab, player.transform.position, Quaternion.identity);
            interactSoundPrefabInstance.clip = activatingKeypad;
            interactSoundPrefabInstance.volume = 0.6f;
            interactSoundPrefabInstance.loop = true;
            interactSoundPrefabInstance.Play();
        }
        else
        {
            interactSoundPrefabInstance = Instantiate(soundPrefab, player.transform.position, Quaternion.identity);
            interactSoundPrefabInstance.clip = pickingUpItemSound;
            interactSoundPrefabInstance.volume = 0.6f;
            interactSoundPrefabInstance.loop = true;
            interactSoundPrefabInstance.Play();
        }
    }

    private void buttonPressed(ManagerButtonPress e)
    {
        PlaySoundAtLocation(managerButtonPress, Vector3.zero, 0.6f, 15, true);
    }

    private void reload(ReloadEvent e)
    {
        PlaySoundAtLocation(reloadSound, player.transform.position, 0.6f, 3);
    }

    private void ManagerItemSelect(ManagerButtonClickEvent e)
    {
        PlaySoundAtLocation(ItemSelectSound, Vector3.zero, 1f, 15, true);
    }

    private void loadSilo(SiloLoadedEvent e)
    {
        PlaySoundAtLocation(siloLoadedSound, e.position, 1f, 15);
    }

    private void nukeLaunched(SiloUnloadedEvent e)
    {
        PlaySoundAtLocation(nukeLanchedSound, e.position, 1f, 15);
    }

    private void incorrectCode(ManagerIncorrectAnswer e)
    {
        PlaySoundAtLocation(managerCodeWrong, Vector3.zero, 0.6f, 15, true);
    }

    private void correctCode(RadioTowerActivatedManagerEvent e)
    {
        PlaySoundAtLocation(managerCodeCorrect, Vector3.zero, 1f, 15, true);
    }

    private void playerActivateTower(RadioTowerActivatedPlayerEvent e)
    {
        PlaySoundAtLocation(playerRadioTowerActivateSound, Vector3.zero, 1f, 15, true);
    }

    private void playerRespawn(PlayerRespawnEvent e)
    {
        player = e.activePlayer;
    }

    private void finalWaveMusic(LastWaveEvent e)
    {
        Destroy(musicInstance);

        musicInstance = Instantiate(soundPrefab, Vector3.zero, Quaternion.identity);
        musicInstance.clip = finalWaveMusicTrack;
        musicInstance.volume = 0.08f;
        musicInstance.spatialBlend = 0;
        musicInstance.loop = true;
        musicInstance.Play();
    }

    private void playVictoryMusic(VictoryMusicEvent e)
    {
        musicInstance = Instantiate(soundPrefab, Vector3.zero, Quaternion.identity);
        musicInstance.clip = victoryMusicTrack;
        musicInstance.volume = 0.1f;
        musicInstance.spatialBlend = 0;
        musicInstance.loop = true;
        musicInstance.Play();
    }
    private void playDeathMusic(DeathMusicEvent e)
    {
        musicInstance = Instantiate(soundPrefab, Vector3.zero, Quaternion.identity);
        musicInstance.clip = deathMusicTrack;
        musicInstance.volume = 0.1f;
        musicInstance.spatialBlend = 0;
        musicInstance.loop = true;
        musicInstance.Play();
    }


    private void startMainMusic(GameplayStartEvent e)
    {
        musicInstance = Instantiate(soundPrefab, Vector3.zero, Quaternion.identity);
        musicInstance.clip = mainMusicTrack;
        musicInstance.volume = 0.05f;
        musicInstance.spatialBlend = 0;
        musicInstance.loop = true;
        musicInstance.Play();
    }

    private void stopMusic(LastWaveOverEvent e)
    {
        musicInstance.Stop();
        Destroy(musicInstance);
    }

    private void coinsCollected(CoinCollect e)
    {
        if (e.value > 100)
        {
            PlaySoundAtLocation(chestOpeningSound, player.transform.position, 0.6f, 5);
        }
        else if (e.value > 0)
        {
            PlaySoundAtLocation(coinPickupSound, player.transform.position, 0.6f, 5);
        }
    }


    private void footstepSounds(PlayerMovingEvent e)
    {
        //Debug.Log($"footstep noises");
        if (footstepPrefabInstance == null && e.movementValue != Vector2.zero)
        {
            footstepPrefabInstance = Instantiate(soundPrefab, player.transform.position, Quaternion.identity);
            footstepPrefabInstance.clip = footstepSound;
            footstepPrefabInstance.spatialBlend = 0;
            footstepPrefabInstance.loop = true;

            footstepPrefabInstance.Play();
        }
        else if (footstepPrefabInstance != null && e.movementValue != Vector2.zero)
        {
            if (e.isSprinting)
            {
                footstepPrefabInstance.pitch = 1.2f;
                footstepPrefabInstance.volume = 1f;
            }
            else if (e.movementValue.magnitude > 0.7f)
            {
                footstepPrefabInstance.pitch = 0.8f;
                footstepPrefabInstance.volume = 0.9f;
            }
            else
            {
                footstepPrefabInstance.pitch = 0.5f;
                footstepPrefabInstance.volume = 0.75f;
            }
        }
        else if (e.movementValue == Vector2.zero && footstepPrefabInstance != null)
        {
            Destroy(footstepPrefabInstance.gameObject);
        }
    }

    private void turretShooting(TurretShootingEvent e)
    {
        PlaySoundAtLocation(turretFire, e.position, 0.6f, 15);
    }

    private void noAmmoNoise(EmptyAmmo e)
    {
        PlaySoundAtLocation(emptyAmmoSound, player.transform.position, 0.6f, 15);
    }

    private void metalDamageNoise(PlayerDamagedEvent e)
    {
        PlaySoundAtLocation(playerDamaged, player.transform.position, 0.6f, 3, true);
    }

    private void bunkerAlarm(ObjectiveDamagedEvent e)
    {
        float timePassed = Time.time - bunkerLastCallTime;
        bunkerLastCallTime = Time.time;
        bunkerAlarmTimer += timePassed;
        //Debug.Log($"bunkerAlarmTimer = {bunkerAlarmTimer}, length = {bunkerAlarmNoise.length}");
        if (bunkerAlarmTimer > (bunkerAlarmNoise.length - 0.09f))
        {
            bunkerAlarmTimer = 0;
            PlaySoundAtLocation(bunkerAlarmNoise, new Vector3(0, 1, 0), 0.05f, 100, true);
        }
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

    private void MissileOrNukeWhistle(ItemUseEvent e)
    {
        switch (e.itemID)
        {
            case 4:
                //nuke
                PlaySoundAtLocation(bombFalling, e.itemLocation, 0.8f, 15);
                break;
            case 5:
                //missile
                PlaySoundAtLocation(bombFalling, e.itemLocation, 0.8f, 15);
                break;
            default:
                break;
        }

    }

    private void zombieDeathSound(ObjectDestroyedEvent e)
    {
        if (e.tag is "Enemy") PlaySoundAtLocation(zombieDeath, e.deathCoordinates, 0.5f, 8);
    }

    private void PlaySoundAtLocation(AudioClip clip, Vector3 position, float volume, float maxSoundDistance, bool canHearSameVolumeEverywhere = false)
    {
        if (player != null)
        {
            AudioSource soundInstance = Instantiate(soundPrefab, position, Quaternion.identity);
            soundInstance.clip = clip;
            soundInstance.volume = volume;

            if (!canHearSameVolumeEverywhere)
            {
                soundInstance.spatialBlend = 1f; // Set to 3D spatialization
                soundInstance.minDistance = 1.5f; // Minimum distance at which the sound is heard at full volume
                soundInstance.maxDistance = maxSoundDistance;
            }
            else
            {
                soundInstance.spatialBlend = 0f; // Set to 3D spatialization
            }


            soundInstance.Play();
            Destroy(soundInstance.gameObject, clip.length); // Destroy the AudioSource after the sound finishes playing
        }
    }
}
