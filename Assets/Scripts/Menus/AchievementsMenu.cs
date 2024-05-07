/**
 * File: AchievementsMenu.cs
 * --------------------------
 * This script is responsible for displaying the achievements in the achievements menu.
 * It reads the achievements from the children of the game object and displays them in the
 * achievements menu.
 *
 * Author: William Fridh
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AchievementsMenu : MonoBehaviour
{

    [Tooltip("Achievement prefab.")]
    [SerializeField] GameObject AchievementPrefab;

    [Tooltip("Achievement container.")]
    [SerializeField] GameObject AchievementContainer;

    [Tooltip("The name of the scene to load when the player wants to quit.")]
    [SerializeField] string QuiteMenuSceneName;

    [Tooltip("Achievement holder.")]
    [SerializeField] GameObject AchievementHolder;

    // Start is called before the first frame update
    void Start()
    {

        if (AchievementPrefab == null)
        {
            Debug.LogError("Achievement prefab is not set.");
            Destroy(this);
        }

        if (AchievementContainer == null)
        {
            Debug.LogError("Achievement container is not set.");
            Destroy(this);
        }

        PrintAchievements();

        Debug.Log(Storage.GetPath());

    }

    // Print array of achievements.
    void PrintAchievements()
    {

        foreach (Transform Achievement in AchievementHolder.transform)
        {

            AchievementAbstract AchievementObject = Achievement.gameObject.GetComponent<AchievementAbstract>();

            // Create the prefab.
            GameObject AchievementElement = Instantiate(AchievementPrefab, AchievementContainer.transform);

            // Select text and sprite elements.
            TMPro.TextMeshProUGUI Title =
                AchievementElement.transform
                .Find("Right Column/Title")
                .gameObject.GetComponent<TMPro.TextMeshProUGUI>();

            TMPro.TextMeshProUGUI Description =
                AchievementElement.transform
                .Find("Right Column/Description")
                .gameObject.GetComponent<TMPro.TextMeshProUGUI>();

            TMPro.TextMeshProUGUI Progress =
                AchievementElement.transform
                .Find("Right Column/Progress Bar/Progress")
                .gameObject.GetComponent<TMPro.TextMeshProUGUI>();

            UnityEngine.UI.Image icon =
                AchievementElement.transform
                .Find("Icon")
                .gameObject.GetComponent<UnityEngine.UI.Image>();

            RectTransform bar =
                AchievementElement.transform
                .Find("Right Column/Progress Bar/Bar")
                .gameObject.GetComponent<RectTransform>();

            // Set progress bar fill amount.
            float progress = bar.sizeDelta.x * (AchievementObject.GetProgress() / AchievementObject.GetMaxProgress());
            bar.sizeDelta = new Vector2(progress * bar.sizeDelta.y, bar.sizeDelta.y);

            // Set text and sprite element contents.
            Title.text = AchievementObject.GetTitle();
            Description.text = AchievementObject.GetDescription();
            Progress.text = AchievementObject.GetProgress() + "/" + AchievementObject.GetMaxProgress();

            string SpritePath = AchievementObject.GetSpritePath();

            Sprite spriteResources = Resources.Load<Sprite>(SpritePath);
            if (spriteResources == null) {
                Debug.LogError("PrintMessage: Sprite located at \"" + SpritePath + "\" could not be found. Message won't be printed.");
                Destroy(AchievementElement);
                return;
            }

            icon.sprite = spriteResources;

        }

    }

    public void cancel() {
        SceneManager.LoadScene(QuiteMenuSceneName);
    }

}
