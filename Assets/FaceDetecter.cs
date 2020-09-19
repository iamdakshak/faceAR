using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using IBM.Watson.DeveloperCloud.Services.VisualRecognition.v3;
using IBM.Watson.DeveloperCloud.Logging;
using IBM.Watson.DeveloperCloud.Utilities;
using IBM.Watson.DeveloperCloud.Connection;


public class FaceDetecter : MonoBehaviour
{
    public Text dataOutput;
    private VisualRecognition _visualRecognition;
    private string path = "C:\\Users\\Dakshak\\Desktop\\Dakshak.jpg";

    public string _serviceUrl = "https://gateway.watsonplatform.net/visual-recognition/api";
    public string _iamApikey = "CyyIooMWMPdQNJ2_cYL3-BFcTsh_Y57xtA70U6Kh5aik";

    void Start()
    {
        LogSystem.InstallDefaultReactors();
        Runnable.Run(CreateService());
    }

    private IEnumerator CreateService()
    {
        if (string.IsNullOrEmpty(_iamApikey))
        {
            throw new WatsonException("Please provide IAM ApiKey for the service.");
        }

        Credentials credentials = null;

        TokenOptions tokenOptions = new TokenOptions()
        {
            IamApiKey = _iamApikey
        };

        credentials = new Credentials(tokenOptions, _serviceUrl);

        //wait for token
        while (!credentials.HasIamTokenData())
            yield return null;

        //create credentials
        _visualRecognition = new VisualRecognition(credentials);
        _visualRecognition.VersionDate = "2019-02-26";
    }

    public void DetectFaces(string path)
    {
        //classify using image url
        // if (!_visualRecognition.DetectFaces(picURL, OnDetectFaces, OnFail))
        //     Log.Debug("ExampleVisualRecognition.DetectFaces()", "Detect faces failed!");

        //classify using image path
        if (!_visualRecognition.DetectFaces(OnDetectFaces, OnFail, path))
        {
            Debug.Log("VisualRecognition.DetectFaces(): Detect faces failed!");
        }
        else
        {
            Debug.Log("Calling Watson");
            dataOutput.text = "";
        }
    }

    private void OnDetectFaces(DetectedFaces multipleImages, Dictionary<string, object> customData)
    {
        var data = multipleImages.images[0].faces[0]; //assume 1
        dataOutput.text = "Age : " + data.age.min + "-" +
            data.age.max + " PROBABILITY: " + data.age.score + "\n" +
            "Gender : " + data.gender.Gender + "\n" ;
        Debug.Log("VisualRecognition.OnDetectFaces(): Detect faces result: " + customData["json"].ToString());
        /*Log.Debug("ExampleFaceDetection", "First face age: {0}", multipleImages.images[0].faces[0].age);
        Log.Debug("ExampleFaceDetection", "First face gender: {0}", multipleImages.images[0].faces[0].gender);*/
    }


    private void OnFail(RESTConnector.Error error, Dictionary<string, object> customData)
    {
        Debug.LogError("ExampleVisualRecognition.OnFail(): Error received: " + error.ToString());
    }
}