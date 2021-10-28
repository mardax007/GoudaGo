using UnityEngine;
using Unity.RemoteConfig;
using Mapbox.Unity.Map;
using System.Collections.Generic;
using Mapbox.AddedStuff;
using UnityEngine.UI;
using System;

public class LocationsRC : MonoBehaviour
{
    public string LocationsEnv = "3bb5bcdc-d402-4c47-b3bf-7d2931b9fba6";
    [SerializeField] AbstractMap map;
    [SerializeField] Transform Player;
    [SerializeField] LookAround Cam;

    [SerializeField] GameObject LocationPopUpPortrait;
    [SerializeField] GameObject LocationPopUpLandscape;

    public bool LocationsPlaced = false;
    public int locationsCount = -1;

    public struct UserAttributes { }
    public struct AppAttributes { }

    private void Awake()
    {
        SetRemoteConfig();
    }

    private void SetRemoteConfig()
    {
        ConfigManager.SetEnvironmentID(LocationsEnv);
        // Add a listener to apply settings when successfully retrieved:
        ConfigManager.FetchCompleted += ApplyRemoteSettings;

        // Fetch configuration setting from the remote service:
        ConfigManager.FetchConfigs<UserAttributes, AppAttributes>(new UserAttributes(), new AppAttributes());
    }

    void ApplyRemoteSettings(ConfigResponse configResponse)
    {
        // Conditionally update settings, depending on the response's origin:
        switch (configResponse.requestOrigin)
        {
            case ConfigOrigin.Default:
                //Debug.Log("No settings loaded this session; using default values.");
                break;
            case ConfigOrigin.Cached:
                //Debug.Log("No settings loaded this session; using cached values from a previous session.");
                break;
            case ConfigOrigin.Remote:
                //Debug.Log("New settings loaded this session; update values accordingly.");
                GetAndSetValues();
                break;
        }
    }

    void GetAndSetValues()
    {
        string locationsInfo = ConfigManager.appConfig.GetJson("locationsInfo");

        RemoteConfigInfo remoteConfigInfo = JsonUtility.FromJson<RemoteConfigInfo>(locationsInfo);

        locationsCount = remoteConfigInfo.locations.Length;

        if (remoteConfigInfo.enabled)
        {
            int locationsCount = remoteConfigInfo.locations.Length;

            for (int i = 0; i < locationsCount; i++)
            {
                string locationID = remoteConfigInfo.locations[i];

                LocationData locationData = JsonUtility.FromJson<LocationData>(ConfigManager.appConfig.GetJson(locationID));

                Questions questionInfo = JsonUtility.FromJson<Questions>(ConfigManager.appConfig.GetJson(locationData.questionInfo));

                //print(locationData.location);

                // Instantiate thing
                GameObject newLocation = new GameObject(locationData.name);

                newLocation.transform.SetParent(transform);

                // map, locationdata, API, LocationPopUp
                newLocation.AddComponent<SpawnOnMap>().SetValues(map, locationData, questionInfo, GetComponent<API>(), LocationPopUpPortrait, LocationPopUpLandscape);

                //Debug.Log($"Created object {locationData.name} at real world location {locationData.location} with marker {locationData.item} at scale {locationData.scale} succesfully.");
            }

            LocationsPlaced = true;
        }
    }

    class RemoteConfigInfo
    {
        public bool enabled;
        public float PlayerScale;
        public float Zoom;
        public float StandardSensitivity;

        public string[] locations;
    }

    public class LocationData
    {
        public string location;
        public string name;
        public string item;
        public float scale;
        public string questionInfo;
        public int MaxDistance;
    }

    public class Questions
    {
        public string[] question;
        public int[] anwser;
        public string imageURL;
        public bool preserveAspect;
        public int points;
    }
}