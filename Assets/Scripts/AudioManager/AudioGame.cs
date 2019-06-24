using System.Collections;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(UIManager))]
public class AudioGame : MonoBehaviour
{
    public AudioClip[] Bgms;
    private int index = 0;
    private int bgmCount;
    public AudioSource Speaker;
    [Tooltip("脚步声专用音源")]
    public AudioSource PlayerMoveSpeaker;
    public float PlayerMoveVolume = 0.2f;
    [Tooltip("脚步声速率")]
    public float PlayerMoveSoundPitch = 1.5f;
    [Tooltip("最大音量，0到1之间")]
    public float BgmMaxVolume = 0.2f;
    [Tooltip("Bgm播放多久后到达最大音量（音量渐强）")]
    public float BgmVolumeMaxTime = 1.5f;
    private float BgmVolumeTimeDelta = 0;
    private bool existBgm = false;

    public AudioClip GunFireSound;
    public AudioClip GunNoAmmoSound;
    public AudioClip MonsterAttackSound;
    public AudioClip PlayerMoveSound;
    public AudioClip PlayerHittedSound;
    private UIManager uiGame;



    // Start is called before the first frame update
    void Start()
    {
        uiGame = gameObject.GetComponent<UIManager>();
        bgmCount = Bgms.Length;
        existBgm = bgmCount > 0;

        uiGame.pauseBtn.onClick.Add(() => { Speaker.Pause(); });
        uiGame.continueBtn.onClick.Add(() => { Speaker.UnPause(); });



        PlayerMoveSpeaker.clip = PlayerMoveSound;
        PlayerMoveSpeaker.loop = true;
        PlayerMoveSpeaker.volume = PlayerMoveVolume;
        PlayerMoveSpeaker.pitch = PlayerMoveSoundPitch;
        PlayerMoveSpeaker.Play();
        PlayerMoveSpeaker.Pause();
    }

    // Update is called once per frame
    void Update()
    {

        if (!Speaker.isPlaying && existBgm && Time.timeScale > 0)
        {
            NextBgm();
        }


        if (Input.GetAxisRaw("Horizontal") != 0 && Time.timeScale > 0)
        {
            //应当有脚步音效
            if (!PlayerMoveSpeaker.isPlaying)
                PlayerMoveSpeaker.UnPause();

            print(PlayerMoveSpeaker.isPlaying);
        }
        else
        {
            //不应当有脚步音效
            if (PlayerMoveSpeaker.isPlaying)
                PlayerMoveSpeaker.Pause();

        }
    }

    /// <summary>
    /// 切歌
    /// </summary>
    private void NextBgm()
    {
        ++index;
        index %= bgmCount;
        Speaker.clip = Bgms[index];
        Speaker.loop = false;
        StartCoroutine(VolumeUp());
        Speaker.Play();
    }



    private IEnumerator VolumeUp()
    {

        BgmVolumeTimeDelta = 0;
        while (BgmVolumeTimeDelta < BgmVolumeMaxTime)
        {
            BgmVolumeTimeDelta += Time.deltaTime;
            //线性增加bgm音量
            Speaker.volume = BgmMaxVolume * BgmVolumeTimeDelta / BgmVolumeMaxTime;
            yield return null;
        }
    }

    public void PlayerSound(SoundEnum sound)
    {
        switch (sound)
        {
            case SoundEnum.Fire:
                Speaker.PlayOneShot(GunFireSound);
                break;
            case SoundEnum.MonsterAttack:
                Speaker.PlayOneShot(MonsterAttackSound);
                break;
            case SoundEnum.NoAmmo:
                Speaker.PlayOneShot(GunNoAmmoSound);
                break;
            case SoundEnum.PlayerHitted:
                Speaker.PlayOneShot(PlayerHittedSound);
                break;
        }
    }



}

public enum SoundEnum
{
    /// <summary>
    /// 玩家开火
    /// </summary>
    Fire,
    /// <summary>
    /// 玩家没子弹
    /// </summary>
    NoAmmo,
    /// <summary>
    /// 怪物发动攻击
    /// </summary>
    MonsterAttack,
    /// <summary>
    /// 玩家被攻击
    /// </summary>
    PlayerHitted
}
