using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreen : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;
    // Start is called before the first frame update
    public void On_BINARY_algorithm()
    {
        StartCoroutine(LoadBINARY());
    }
    IEnumerator LoadBINARY()
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene("BinaryScene");
        DynamicGI.UpdateEnvironment();
    }

    public void On_KNN_algorithm()
    {
        StartCoroutine(LoadKNN());
    }
    IEnumerator LoadKNN()
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene("KNNScene");
        DynamicGI.UpdateEnvironment();
    }

    public void On_IRIS_dataset ()
    {
        StartCoroutine(LoadIRIS());
    }

    IEnumerator LoadIRIS()
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene("graphScene");
        DynamicGI.UpdateEnvironment();
    }

    public void On_OTHER_dataset ()
    {
        StartCoroutine(LoadOTHER());
    }

    IEnumerator LoadOTHER()
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene("otherScene");
        DynamicGI.UpdateEnvironment();
    }

    public void onMAIN ()
    {
        StartCoroutine(LoadMAIN());
    }

    IEnumerator LoadMAIN()
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene("Start");
        DynamicGI.UpdateEnvironment();
    }
}
