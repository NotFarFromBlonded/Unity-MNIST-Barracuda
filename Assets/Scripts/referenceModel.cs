using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Barracuda;
using UnityEngine;
using UnityEngine.UI;

public class referenceModel : MonoBehaviour
{
    public NNModel modelSource;
    private Model rmodel;
    private IWorker engine;
    public Texture2D texture;
    public Text predictedValue;
    public Text predictedValueScore;

    [Serializable]
    public struct Prediction
    {
        public int predValue;
        public float[] preds;
        public void setPrediction(Tensor t)
        {
            preds = t.AsFloats();
            predValue = Array.IndexOf(preds, preds.Max());
            //Debug.Log($"Predicted {predValue}");
        }
    }

    public Prediction pred;

    // Start is called before the first frame update
    void Awake()
    {
        rmodel = ModelLoader.Load(modelSource);
        engine = WorkerFactory.CreateWorker(rmodel, WorkerFactory.Device.GPU);
        pred = new Prediction();
    }

    private void OnDestroy()
    {
        engine.Dispose();
    }

    public void Predict()
    {
        var channelCount = 1; // you can treat input pixels as 1 (grayscale), 3 (color) or 4 (color with alpha) channels
        var input = new Tensor(texture, channelCount);

        Tensor output = engine.Execute(input).PeekOutput();
        input.Dispose();
        pred.setPrediction(output);
        predictedValue.text = pred.predValue.ToString();
        predictedValueScore.text = pred.preds.Max().ToString();
    }
}
