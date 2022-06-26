using UnityEngine;
using UnityEngine.UI;

public class GeneticAlgorithmView : MonoBehaviour
{
    public Text GenerationIdText;
    public Text BestFitnessScoreText;
    public Button PlayBestGenomeButton;
    public Button RestartButton;

    public void ShowUI()
    {
        this.PlayBestGenomeButton.gameObject.SetActive(true);
        this.RestartButton.gameObject.SetActive(true);
    }

    public void HideUI()
    {
        this.PlayBestGenomeButton.gameObject.SetActive(false);
        this.RestartButton.gameObject.SetActive(false);
    }

    public void OnStartGeneration(Population generation, bool isLast)
    {
        this.GenerationIdText.text = $"Generation {generation.GenerationId}";
    }

    public void OnEndGeneration(Population generation, bool isLast)
    {
        if (isLast)
            this.ShowUI();
    }

    public void UpdateBestFitnessScore(Genome bestGenome)
    {
        this.BestFitnessScoreText.text = $"Best fitness score:\n{bestGenome.Fitness.Value}";
    }
}
