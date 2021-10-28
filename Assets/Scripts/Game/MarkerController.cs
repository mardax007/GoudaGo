using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MarkerController : MonoBehaviour
{
    //public string imageURL = "https://flxt.tmsimg.com/assets/p8214163_p_v8_ag.jpg";

    public GameObject LocationPopUpPortrait;
    public GameObject LocationPopUpLandscape;

    LocationsRC.LocationData locationData;
    LocationsRC.Questions questions;


    void OnMouseDown()
    {
        //Do thing


        float distance = Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position);

        if (distance <= locationData.MaxDistance)
        {
            if (SaveManager.instance.visitedLocations.Contains(locationData.name))
            {
                gameObject.SetActive(false);
            }
            
            //Open Popup
            if(Screen.orientation == ScreenOrientation.LandscapeLeft || Screen.orientation == ScreenOrientation.Landscape || Screen.orientation == ScreenOrientation.LandscapeRight)
            {
                LocationPopUpLandscape.SetActive(true);
                LocationPopUpLandscape.GetComponent<LocationPopupController>().SetInformation(questions, locationData);
            }
            else
            {
                LocationPopUpPortrait.SetActive(true);
                LocationPopUpPortrait.GetComponent<LocationPopupController>().SetInformation(questions, locationData);
            }

            if (SaveManager.instance.visitedLocations.Contains(locationData.name))
            {
                gameObject.SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < 40; i++)
            {
                transform.localRotation.Set(0, transform.localRotation.y + 1, 0, 0);
            }
        }
    }

    public void SetUpMarker(LocationsRC.LocationData _locationData, LocationsRC.Questions _questions, GameObject iLocationPopUpPortrait, GameObject iLocationPopUpLandscape)
    {

        LocationPopUpPortrait = iLocationPopUpPortrait;
        LocationPopUpLandscape = iLocationPopUpLandscape;

        locationData = _locationData;

        questions = _questions;

        if (SaveManager.instance.visitedLocations.Contains(locationData.name))
        {
            gameObject.SetActive(false);
        }


        if (locationData.MaxDistance == 0)
        {
            locationData.MaxDistance = 100;
        }

        StartCoroutine(KeepUpToDate());
    }



    IEnumerator KeepUpToDate()
    {
        while(true)
        {
            if (SaveManager.instance.visitedLocations.Contains(locationData.name))
            {
                gameObject.SetActive(false);
            }

            yield return new WaitForSeconds(1);
        }
    }


}
