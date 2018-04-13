﻿using System.IO;
using UnityEngine;

public class LoadUtility : MonoBehaviour {

    static public LoadUtility instance { get; private set; }

    public string fileName = "savedFile.json";
    public string fileLocation = "SavedFiles/";

    BaseComponentData data;

    void Awake() {
        instance = (LoadUtility)Singleton.Setup(this, instance);
    }

    public void LoadFromFile() {
        try {
            ReadDataContainer();
            ClearCurrentSimulation();
            InstantiateComponents();
        }
        catch (FileNotFoundException) {
            Debug.LogError("File " + fileLocation+fileName + " could not be found.");
        }
    }

    void ReadDataContainer() {
        StreamReader file = new StreamReader(fileLocation + fileName, System.Text.Encoding.UTF8);
        data = JsonUtility.FromJson<BaseComponentData>(file.ReadToEnd());
        file.Close();
    }

    void ClearCurrentSimulation() {
        SimulationPanel.instance.ClearEntireSimulation();
    }

    void InstantiateComponents() {
        for (int i = 0; i < data.components.Length; ++i) {
            var newComponent = Instantiate(ComponentLibrary.instance.nameToPrefab[data.components[i]]);
            newComponent.transform.position = data.positions[i];
            newComponent.name = newComponent.name.Replace("(Clone)", "");
        }
    }
}
