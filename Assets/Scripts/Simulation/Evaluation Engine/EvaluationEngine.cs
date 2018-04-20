﻿using System;
using System.Collections.Generic;
using UnityEngine;
using EvaluationExceptions;

public class EvaluationEngine : MonoBehaviour {

    public GameObject MessageWindowPrefab;

    BaseComponent[] activeComponents;

    public void EvaluateCurrentSimulation() {
        try {
            SimulationInput.instance.gameObject.SetActive(false);
            activeComponents = SimulationPanel.instance.GetActiveComponents();
            CheckComponentAmount();
            CheckConnectors();
            CheckContacts();
            SimulationPanel.instance.EnterSimulationMode();
        }
        catch (EvaluationException exception) {
            var window = Instantiate(MessageWindowPrefab).GetComponent<ListMessageWindow>();
            window.listItems = exception.GetProblemList();
            window.windowTitle = exception.Message;
        }
        catch (Exception exception) {
            Debug.Log(exception.Message);
            SimulationInput.instance.gameObject.SetActive(true);
        }
    }

    void CheckComponentAmount() {
        if (activeComponents.Length == 0)
            throw new Exception("Nenhum componente para simular");
    }

    void CheckConnectors() {
        List<BaseComponent> unconnectedComponents = new List<BaseComponent>();
        foreach (var component in activeComponents)
            foreach (var connector in component.GetComponent<ComponentConnections>().connectorList)
                if (connector.connectedObjects.Count == 0 && !unconnectedComponents.Contains(component))
                    unconnectedComponents.Add(component);
        if (unconnectedComponents.Count > 0)
            throw new OpenConnectorException(unconnectedComponents.ToArray());
    }

    void CheckContacts() {
        List<Contact> looseContacts = new List<Contact>();
        foreach (var component in activeComponents) {
            Contact contact = component.GetComponent<Contact>();
            if (contact) {
                if (!(contact as IConfigurable).IsConfigured())
                    looseContacts.Add(contact);
            }
        }
        if (looseContacts.Count > 0)
            throw new UnassignedContactException(looseContacts.ToArray());
    }
}
