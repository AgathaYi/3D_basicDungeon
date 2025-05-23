using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    private static CharacterManager _instance;
    public static CharacterManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject("CharacterManager").AddComponent<CharacterManager>();
            }
            return _instance;
        }
    }

    public Player _player;
    public Player Player
    {
        get { return _player; }
        set { _player = value; }
    }

    private void Awake()
    {
        // 싱글톤 패턴
        // 싱글톤 인스턴스 or DonDestroyOnLoad 중, 오류 발생하면 catch 블록 실행
        try
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (_instance == this)
            {
                Destroy(gameObject);
            }
        }
        catch (System.Exception exception)
        {
            Debug.LogError("CharacterManager Awake 실패");
            Debug.LogException(exception);
        }
    }
}
