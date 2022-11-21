using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private Vector2 sourcePosition;
    private Vector2 anchorPosition;
    private Vector2 secondAnchorPosition;
    private Vector2 targetPosition;

    private readonly int speed = 8;

    [SerializeField]
    private AudioClip beatAudio = null;
    [SerializeField]
    private AudioClip spawnAudio = null;

    [SerializeField]
    private GameObject starParticlesObject = null;

    private void OnEnable()
    {
        NewGame();
    }

    private void Update()
    {
        if (GameManager.instance.gameInProgress && Time.timeScale > 0)
        {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN

            if (Camera.main.ScreenToWorldPoint(Input.mousePosition).y <= 3)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    sourcePosition = transform.position;
                    targetPosition = Vector2.zero;
                    anchorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                }
                else if (Input.GetMouseButton(0))
                {
                    secondAnchorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    targetPosition = new Vector2(secondAnchorPosition.x - anchorPosition.x, secondAnchorPosition.y - anchorPosition.y);
                }
            }

#elif UNITY_ANDROID || UNITY_IOS

            if (Input.touchCount > 0)
            {
                if (Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position).y <= 3)
                {
                    if (Input.GetTouch(0).phase == TouchPhase.Began)
                    {
                        sourcePosition = transform.position;
                        anchorPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                    }
                    else if (Input.GetTouch(0).phase == TouchPhase.Moved)
                    {
                        secondAnchorPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                        targetPosition = new Vector2(secondAnchorPosition.x - anchorPosition.x, secondAnchorPosition.y - anchorPosition.y);
                    }
                }
            }

#endif

            transform.position = Vector2.Lerp(transform.position, new Vector2(Mathf.Clamp(sourcePosition.x + targetPosition.x,
                -SpawnManager.instance.starMaxPositionX, SpawnManager.instance.starMaxPositionX), -2), speed * Time.deltaTime);
            
            Animation();
        }
    }

    private void Animation()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + 1), GameManager.instance.gravity * 120 * Time.deltaTime);
    }

    private void NewGame()
    {
        sourcePosition = transform.position;
        targetPosition = Vector2.zero;

        SongsManager.instance.audioSource.PlayOneShot(spawnAudio, 0.5f);
        starParticlesObject.SetActive(true);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameManager.instance.GameOver();
        SongsManager.instance.audioSource.PlayOneShot(beatAudio);
        transform.parent = collision.transform;
        starParticlesObject.SetActive(false);
    }
}