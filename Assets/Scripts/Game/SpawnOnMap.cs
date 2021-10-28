using System;
using UnityEngine;

namespace Mapbox.AddedStuff
{
    using UnityEngine;
    using Mapbox.Utils;
    using Mapbox.Unity.Map;
    using Mapbox.Unity.Utilities;
    using System.Collections.Generic;

    public class SpawnOnMap : MonoBehaviour
    {

        API api;
        LocationsRC.LocationData locationData;
        GameObject LocationPopUpPortrait;
        GameObject LocationPopUpLandscape;
        LocationsRC.Questions questions;

        [SerializeField]
        AbstractMap _map;

        [SerializeField]
        [Geocode]
        string locationString;

        [SerializeField]
        float _spawnScale = 1f;

        //[SerializeField]
        //GameObject _markerPrefab;

        List<GameObject> _spawnedObjects;

        Transform parent;

        Vector2d _location;

        bool SetupFinished = false;

        // map, locationdata, API, LocationPopUp
        public void SetValues(AbstractMap imap, LocationsRC.LocationData _locationData, LocationsRC.Questions _questions, API _api, GameObject _LocationPopUpPortrait, GameObject _LocationPopUpLandscape)
        {
            //Debug.Log(_locationData.location);
            _map = imap;

            api = _api;

            locationData = _locationData;

            LocationPopUpPortrait = _LocationPopUpPortrait;
            LocationPopUpLandscape = _LocationPopUpLandscape;

            locationString = locationData.location;

            questions = _questions;

            _spawnScale = locationData.scale;

            Setup();
        }

        public void Setup()
        {
            parent = transform;
            _spawnedObjects = new List<GameObject>();

            _location = Conversions.StringToLatLon(locationString);


            //Replace with AssetBundelSpawner
            //var instance = Instantiate(_markerPrefab);

            //Replacement
            api.GetBundleObject(locationData.item, OnContentLoaded, transform);
        }

        void OnContentLoaded(AssetBundle bundle)
        {
            //do something cool here

            string rootAssetPath = bundle.GetAllAssetNames()[0];
            GameObject content = Instantiate(bundle.LoadAsset(rootAssetPath) as GameObject, transform);
            bundle.Unload(false);

            content.transform.localPosition = _map.GeoToWorldPosition(_location, true);
            content.transform.parent = parent;
            content.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
            //transform.localPosition = new Vector3(content.transform.localPosition.x, (float)0.35 * _spawnScale, content.transform.localPosition.y);
            //Debug.Log(new Vector3(transform.localPosition.x, (float)0.35 * _spawnScale, transform.localPosition.y));

            _spawnedObjects.Add(content);

            content.AddComponent<MarkerController>().SetUpMarker(locationData, questions, LocationPopUpPortrait, LocationPopUpLandscape);

            SetupFinished = true;
        }

        private void Update()
        {
            if (SetupFinished)
            {
                var spawnedObject = _spawnedObjects[0];
                spawnedObject.transform.localPosition = _map.GeoToWorldPosition(_location, true);
            }
        }
    }
}