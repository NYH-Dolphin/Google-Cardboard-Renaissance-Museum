using System.Collections;
using Environment;
using SoundEffects;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class PlayerController : MonoBehaviour
{
    public TextMesh uiCoinNum;
    public GameObject uiLosePanel;

    private int _iCoinNum;
    [HideInInspector] public bool bLose;


    private static float[] _carPos = { -3f, 0f, 3f };
    private int _iPos = 1;
    private const float fMoveSpeed = 20f;
    private const float fRotSpeed = 120;
    private InputController _inputs;
    private IEnumerator _coroutine;
    private readonly Quaternion _rRight = Quaternion.Euler(new Vector3(0, 20, 0));
    private readonly Quaternion _rLeft = Quaternion.Euler(new Vector3(0, -20, 0));
    private Quaternion _origin;

    public static PlayerController Instance;

    private void Awake()
    {
        Instance = this;
        uiLosePanel.SetActive(false);
    }


    private void Start()
    {
        _origin = transform.rotation;
    }

    public void OnLose()
    {
        bLose = true;
        _inputs.ParkourGame.Disable();
        uiLosePanel.SetActive(true);
        ObjectGenerator.fSpeed = 0f;
        StartCoroutine(LoseCountDown());
    }

    IEnumerator LoseCountDown()
    {
        yield return new WaitForSeconds(3f);
        string sceneName = SceneManager.GetActiveScene().name;
        ObjectGenerator.fSpeed = 5f;
        SceneManager.LoadScene(sceneName);
    }


    private void OnEnable()
    {
        if (_inputs == null)
        {
            _inputs = new InputController();
        }

        _inputs.ParkourGame.Enable();
        _inputs.ParkourGame.Left.performed += OnMoveLeftPerformed;
        _inputs.ParkourGame.Right.performed += OnMoveRightPerformed;
        _inputs.ParkourGame.Click.performed += OnClickPerformed;


        // Development only
        // _inputs.Control.Enable();
        // _inputs.Control.Rotation.performed += OnRotationPerformed;
        // _inputs.Control.Rotation.canceled += OnRotationCanceled;
    }


    #region TapAndClick

    private bool _bClick;

    private void OnClickPerformed(InputAction.CallbackContext value)
    {
        if (_bClick)
        {
            Debug.Log("Click");
            OnMoveRight();
            _bClick = false;
        }
        else
        {
            StartCoroutine(ClickCountDown(0.25f));
        }
    }

    IEnumerator ClickCountDown(float time)
    {
        _bClick = true;
        yield return new WaitForSeconds(time);
        if (_bClick)
        {
            _bClick = false;
            OnMoveLeft();
        }
    }

    #endregion


    [SerializeField] private float fInputRotSpeed = 100f;
    private Vector3 _vecRotDir;
    private readonly float _fRotDamping = 0.1f;

    private void OnRotationPerformed(InputAction.CallbackContext value)
    {
        Vector2 vec2 = value.ReadValue<Vector2>();
        _vecRotDir = new Vector3(vec2.x, 0f, vec2.y);
    }

    private void OnRotationCanceled(InputAction.CallbackContext value)
    {
        _vecRotDir = Vector3.zero;
    }

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject camera;

    private void Update()
    {
        RotationUpdate();
    }

    private void RotationUpdate()
    {
        // Calculate the target rotation for the player
        Quaternion playerTargetRotation = Quaternion.Euler(0, _vecRotDir.x * fInputRotSpeed * Time.deltaTime, 0);

        // Use RotateTowards to smoothly interpolate between current and target rotation for the player
        player.transform.rotation = Quaternion.RotateTowards(player.transform.rotation,
            player.transform.rotation * playerTargetRotation, _fRotDamping * Mathf.Rad2Deg);

        // Calculate the target rotation for the camera
        Quaternion cameraTargetRotation = Quaternion.Euler(-1 * _vecRotDir.z * fInputRotSpeed * Time.deltaTime, 0, 0);

        // Use RotateTowards to smoothly interpolate between current and target rotation for the camera
        camera.transform.rotation = Quaternion.RotateTowards(camera.transform.rotation,
            camera.transform.rotation * cameraTargetRotation, _fRotDamping * Mathf.Rad2Deg);
    }

    private void OnMoveLeftPerformed(InputAction.CallbackContext value)
    {
        OnMoveLeft();
    }


    public void OnMoveLeft()
    {
        if (_iPos > 0)
        {
            _iPos -= 1;
            SoundManager.Instance.move.Play();
        }

        Vector3 pos = transform.position;
        pos.x = _carPos[_iPos];
        Vector3 midPos = (transform.position + pos) / 2;
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            transform.rotation = _origin;
        }

        _coroutine = CarTranslation(pos, midPos);
        StartCoroutine(_coroutine);
    }

    private void OnMoveRightPerformed(InputAction.CallbackContext value)
    {
        OnMoveRight();
    }

    public void OnMoveRight()
    {
        if (_iPos < 2)
        {
            _iPos += 1;
            SoundManager.Instance.move.Play();
        }

        Vector3 pos = transform.position;
        pos.x = _carPos[_iPos];
        Vector3 midPos = (transform.position + pos) / 2;
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }


        _coroutine = CarTranslation(pos, midPos);

        StartCoroutine(_coroutine);
    }


    IEnumerator CarTranslation(Vector3 target, Vector3 mid)
    {
        while (true)
        {
            transform.position = Vector3.Lerp(transform.position, target, fMoveSpeed * Time.deltaTime);

            if (transform.position.x > target.x)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, _rLeft, Time.deltaTime * fRotSpeed);
            }

            if (transform.position.x < target.x)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, _rRight, Time.deltaTime * fRotSpeed);
            }


            if (Vector3.Distance(transform.position, target) < 0.01f)
            {
                transform.position = target;
                transform.rotation = Quaternion.Euler(Vector3.zero);
                break;
            }

            yield return null;
        }
    }

    public void AddOneCoin()
    {
        _iCoinNum += 1;
        uiCoinNum.text = "" + _iCoinNum;
    }
}