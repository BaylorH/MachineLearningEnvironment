using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreen : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;
    // Start is called before the first frame update

    // From Start scene, if select 'Binary classification'
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

    // From Start scene, if select 'K-Nearest Neighbors'
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

    // From Start scene, if select 'K-Means'
    public void On_KMeans_algorithm()
    {
        StartCoroutine(LoadKMeans());
    }
    IEnumerator LoadKMeans()
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene("KMeansPenguinScene");
        DynamicGI.UpdateEnvironment();
    }

    // From Start scene, if select 'K-Means++'
    public void On_KMeansPlusPlus_algorithm()
    {
        StartCoroutine(LoadKMeansPlusPlus());
    }
    IEnumerator LoadKMeansPlusPlus()
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene("KMeans++PenguinScene");
        DynamicGI.UpdateEnvironment();
    }

    // From BinaryScene, if select 'Iris dataset'
    public void On_BINARYIRIS_dataset ()
    {
        StartCoroutine(LoadBinaryIRIS());
    }
    IEnumerator LoadBinaryIRIS()
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene("BinaryIrisScene");
        DynamicGI.UpdateEnvironment();
    }

    // From KNNScene, if select 'Iris dataset'
    public void On_KNNIRIS_dataset ()
    {
        StartCoroutine(LoadKNNIRIS());
    }
    IEnumerator LoadKNNIRIS()
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene("KNNIrisScene");
        DynamicGI.UpdateEnvironment();
    }

    // From BinaryScene, if select 'Diabetes dataset'
    public void On_BINARYDIABETES_dataset ()
    {
        StartCoroutine(LoadBINARYDIABETES());
    }
    IEnumerator LoadBINARYDIABETES()
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene("BinaryDiabetesScene");
        DynamicGI.UpdateEnvironment();
    }

    // From KNNScene, if select 'Diabetes dataset'
    public void On_KNNDIABETES_dataset ()
    {
        StartCoroutine(LoadKNNDIABETES());
    }
    IEnumerator LoadKNNDIABETES()
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene("KNNDiabetesScene");
        DynamicGI.UpdateEnvironment();
    }

    // If 'Main menu' button is selected
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
