using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LocationPopupController : MonoBehaviour
{
    public LocationsRC.LocationData locationData;
    LocationsRC.Questions questions;

    public GameObject mapView;

    public int questionCount = 0;

    public Text TitleText;
    public Text QuestionText;
    public Image img;
    public AnwserController anwserController;

    public void NextQuestion()
    {
        questionCount++;
        SetInformation(questions, locationData);
    }


    public void SetInformation(LocationsRC.Questions _questionInfo, LocationsRC.LocationData _locationData)
    {

        locationData = _locationData;

        questions = _questionInfo;

        if (questions.question.Length == questionCount)
        {
            // reset text
            TitleText.text = "";
            QuestionText.text = "";

            mapView.SetActive(true);
            gameObject.SetActive(false);
        }
        else
        {
            TitleText.text = locationData.name;
            QuestionText.text = questions.question[questionCount];


            anwserController.SetCorrectAwnser(questions.anwser[questionCount], questions.points);

            if (questionCount < 1)
            {
                StartLoadingPhoto(questions.imageURL, questions.preserveAspect);
            }
        }
    }

    public void StartLoadingPhoto(string imgURL, bool preserveAspect)
    {
        IEnumerator coroutine = LoadPhoto(imgURL, preserveAspect);
        StartCoroutine(coroutine);
    }

    IEnumerator LoadPhoto(string imgURL, bool preserveAspect)
    {
        Texture2D tex;
        tex = new Texture2D(4, 4, TextureFormat.DXT1, false);
        WWW www = new WWW(imgURL);
        yield return www;
        www.LoadImageIntoTexture(tex);
        Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0, 0));
        img.sprite = sprite;
        img.preserveAspect = preserveAspect;
    }
}