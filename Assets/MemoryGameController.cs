using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using TMPro;

public class MemoryGameController : MonoBehaviour
{
    public TMP_Text resultText; // Reference to the TMP text object for displaying game result
    public TMP_Text timerText; // Reference to the TMP text object for displaying time remaining
    public float timeLimit = 30f; // Time limit for the player to guess the lights
    public GameObject resetButton; // Reference to the reset button

    public bool CanInteract { get; private set; }

   
    public List<GameObject> lightsList; 
    public int lightsToRememberCount = 3;
    public bool manualLightsCount = false;

    private List<GameObject> lightsToRemember = new List<GameObject>();
    private int incorrectAttempts = 0;
    private float remainingTime;


    void Start()
    {
        CanInteract = false;
        remainingTime = timeLimit;
        resultText.text = string.Empty;
        timerText.text = string.Empty;

        // Hide the reset button
        resetButton.SetActive(false);

        StartCoroutine(StartGame());
    }

    private IEnumerator StartGame()
    {
        yield return new WaitForSeconds(2f);
        StartRound();
    }

    private void StartRound()
    {
        CanInteract = false;
        lightsToRemember = GetRandomLights();
        StartCoroutine(TurnOnRememberedLights());
        StartCoroutine(StartCountdown());
    }

    private IEnumerator TurnOnRememberedLights()
    {


        // Turn on only the lights that the player needs to remember
        foreach (var light in lightsToRemember)
        {
            light.GetComponent<LightController>().ToggleLight(true);
        }

        yield return new WaitForSeconds(1f);

        // Turn off all lights, including the ones the player needs to remember
        foreach (var light in lightsList)
        {
            light.GetComponent<LightController>().ToggleLight(false);
        }

        yield return new WaitForSeconds(0.5f);

        CanInteract = true;
    }

    private IEnumerator StartCountdown()
    {
        while (remainingTime > 0)
        {
            yield return new WaitForSeconds(1f);
            remainingTime -= 1f;

            // Update UI with remaining time
            timerText.text = "Time: " + Mathf.RoundToInt(remainingTime).ToString();
        }

        // Time's up
        Debug.Log("Time's up!");
        
        EndGame();
    }

    private List<GameObject> GetRandomLights()
    {
        List<GameObject> lights = new List<GameObject>();

        int totalLights = lightsList.Count;

        if (!manualLightsCount)
        {
            lightsToRememberCount = Random.Range(1, totalLights + 1);
        }

        for (int i = 0; i < lightsToRememberCount; i++)
        {
            int randomIndex = Random.Range(0, totalLights);
            GameObject randomLight = lightsList[randomIndex];
            lights.Add(randomLight);
        }

        return lights;
    }

    public void OnLightClicked(GameObject clickedLight)
    {
        if (CanInteract)
        {
            // Toggle the light to be active
            clickedLight.GetComponent<LightController>().ToggleLight(true);

            // Check if the clicked light is one of the lights to remember
            if (lightsToRemember.Contains(clickedLight))
            {
                Debug.Log("Correct selection!");

                // Remove the clicked light from the lights to remember
                lightsToRemember.Remove(clickedLight);

                // If all lights are correctly selected
                if (lightsToRemember.Count == 0)
                {
                    Debug.Log("Player wins!");
                    

                    // end the game 
                    EndGame();
                }
            }
            else
            {
                Debug.Log("Incorrect selection!");
                clickedLight.GetComponent<LightController>().CrackLight();

                // Increment incorrect attempts count
                incorrectAttempts++;

                // If player exceeds the maximum allowed incorrect attempts
                if (incorrectAttempts >= 3)
                {
                    Debug.Log("Player loses!");
                    

                    // end the game 
                    EndGame();
                }
            }
        }
    }

    public void ResetGame()
    {
        //reset Game State
        StopAllCoroutines();
        CanInteract = false;
        incorrectAttempts = 0;
        remainingTime = timeLimit;
        resultText.text = string.Empty;
        timerText.text = string.Empty;

        // Hide the reset button
        resetButton.SetActive(false);


        TurnOffAllLights();
        lightsToRemember.Clear();


        // Start a new round
        StartCoroutine(StartGame());
    }

    private void EndGame()
    {
        CanInteract = false;
        StopAllCoroutines();

        if (lightsToRemember.Count == 0)
        {
            resultText.text = "Player wins!";
        }
        else
        {
            resultText.text = "Player loses!";
        }

        // Show the reset button
        resetButton.SetActive(true);
    }

    
    void TurnOffAllLights()
    {
        
        foreach (var light in lightsList)
        {
            light.GetComponent<LightController>().ToggleLight(false);
        }
    }
}
