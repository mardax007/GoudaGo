using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnwserController : MonoBehaviour
{
    int Awnser;
    int _InputAwnser;
    int points;
    
    [SerializeField] GameObject CorrectText;
    [SerializeField] GameObject FalseText;
    [SerializeField] List<Image> Buttons;

    public LocationPopupController locationPopupController;

    public void InputAwnser(int _Awnser)
    {
        _InputAwnser = _Awnser;
        if (_Awnser == Awnser)
        {
            Debug.Log("Correct");
            SaveManager.instance.Load();
            SaveManager.instance.points += points;
            SaveManager.instance.UpdateScore();
            SaveManager.instance.Save();

            CorrectText.SetActive(true);

            CorrectText.GetComponent<Animator>().SetTrigger("Correct");

            StartCoroutine(WaitThenNextC());
        }
        else
        {
            Debug.Log("False");

            FalseText.SetActive(true);

            FalseText.GetComponent<Animator>().SetTrigger("False");

            Buttons[_Awnser].color = Color.red;

            StartCoroutine(WaitThenNextF());
        }

        Buttons[Awnser].color = Color.green;

        if (!SaveManager.instance.visitedLocations.Contains(locationPopupController.locationData.name))
        {
            SaveManager.instance.AddVistedLocations(locationPopupController.locationData.name);
            SaveManager.instance.Save();
        }
    }

    public void SetCorrectAwnser(int _correctAwnser, int _points)
    {
        Awnser = _correctAwnser;
        points = _points;
    }

    IEnumerator WaitThenNextC()
    {
        yield return new WaitForSeconds(1);
        CorrectText.SetActive(false);

        Buttons[Awnser].color = Color.white;
        locationPopupController.NextQuestion();
    }

    IEnumerator WaitThenNextF()
    {
        yield return new WaitForSeconds(1);
        FalseText.SetActive(false);

        Buttons[_InputAwnser].color = Color.white;
        Buttons[Awnser].color = Color.white;
        locationPopupController.NextQuestion();
    }
}
