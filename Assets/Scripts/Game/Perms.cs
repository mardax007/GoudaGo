using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

public class Perms : MonoBehaviour
{
    bool acces = false;

    [SerializeField] bool OVERRIDE;
    [SerializeField] LocationsRC locationsRC;
    [SerializeField] GameObject Spawner;
    [SerializeField] GameObject MapView;
    [SerializeField] GameObject Map;
    [SerializeField] GameObject LoadView;
    [SerializeField] GameObject LoadingAnimation;
    [SerializeField] Text logText;

    private void Start()
    {
        if(Application.platform == RuntimePlatform.Android && !Input.location.isEnabledByUser)
        {
            Permission.RequestUserPermission(Permission.FineLocation);
        }
    }

    private void Update()
    {
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            acces = true;
        }
        if (((acces && Input.location.isEnabledByUser) || OVERRIDE) && !MapView.activeInHierarchy)
        {
            LoadingAnimation.SetActive(true);
            MapView.SetActive(true);
            logText.text = "Map aan het inladen";
        }

        if (Spawner.transform.childCount == locationsRC.locationsCount && Map.transform.childCount > 9)
        {
            LoadView.SetActive(false);
        }
    }
}
