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
        // �̱��� ����
        // �̱��� �ν��Ͻ� or DonDestroyOnLoad ��, ���� �߻��ϸ� catch ��� ����
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
            Debug.LogError("CharacterManager Awake ����");
            Debug.LogException(exception);
        }
    }
}
