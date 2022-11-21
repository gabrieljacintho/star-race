using UnityEngine;

public class StarParticlesControl : MonoBehaviour
{
    [SerializeField]
    private GameObject playerObject = null;

    public void Update()
    {
        transform.position = playerObject.transform.position;
    }
}
