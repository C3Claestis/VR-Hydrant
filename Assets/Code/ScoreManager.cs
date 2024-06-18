using System.Collections;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class ScoreManager : MonoBehaviourPun
{
    #region Serialized Fields

    [Header("Members Of General Score")]
    [SerializeField] private GameObject[] correct = new GameObject[6]; // Array of correct objects
    [SerializeField] private TextMeshProUGUI score; // Text element to display the score
    [SerializeField] private TextMeshProUGUI timer; // Text element to display the timer
    [SerializeField] private float countdownTime; // Countdown time in minutes

    [Header("Members Of Finish")]
    [SerializeField] private TextMeshProUGUI score_finish; // Text element to display the final score
    [SerializeField] private GameObject canvas_finish; // Canvas to show when the game finishes

    [Header("Members Of Dead")]
    [SerializeField] private GameObject canvas_gameover; // Canvas to show when the player is dead
    [SerializeField] private GameObject fire_dead; // Game object to show fire effect when the player is dead

    #endregion

    #region Private Variables

    private bool isDead = false; // Indicates if the player is dead
    private int score_count; // Current score
    private bool isUpgrade; // Indicates if the score should be upgraded
    private bool canDecreaseScorePillar = true; // Flag to allow score decrease for pillar
    private bool canDecreaseScoreSelang = true; // Flag to allow score decrease for selang
    private bool stopCountdown = false; // Variable to stop countdown

    // Array for score values corresponding to the correct objects
    private int[] scoreValues = { 10, 15, 20, 25, 30, 50 };
    // Array to track if the score has been added
    private bool[] isScored;

    #endregion

    #region Unity Methods

    void Start()
    {
        countdownTime *= 60; // Convert countdownTime from minutes to seconds
        score.text = score_count.ToString();
        StartCoroutine(CountdownRoutine());
        isScored = new bool[correct.Length]; // Initialize isScored array with the same size as correct array
    }

    void Update()
    {
        if (!isDead)
        {
            // Ensure ScoreFunction is called only if isUpgrade is true and this client owns the object
            if (isUpgrade && photonView.IsMine)
            {
                // Check if score has not been added and correct element is active
                for (int i = 0; i < correct.Length; i++)
                {
                    ScoreFunction(i, scoreValues[i]);
                }
            }

            // Check if countdown has finished
            if (countdownTime < 0)
            {
                photonView.RPC("DeadRPC", RpcTarget.All, true);
            }

            // Check if the last correct object is active
            if (correct[5].gameObject.activeSelf)
            {
                canvas_finish.SetActive(true);
                photonView.RPC("FinishManager", RpcTarget.All);
                photonView.RPC("StopCountdownRPC", RpcTarget.All); // Stop countdown
            }
        }
    }

    #endregion

    #region Custom Methods

    private IEnumerator CountdownRoutine()
    {
        float remainingTime = countdownTime;

        while (remainingTime > 0 && !stopCountdown) // Check stopCountdown
        {
            int minutes = Mathf.FloorToInt(remainingTime / 60);
            int seconds = Mathf.FloorToInt(remainingTime % 60);

            timer.text = string.Format("{0:00}:{1:00}", minutes, seconds);

            yield return null;

            remainingTime -= Time.deltaTime;

            if (photonView.IsMine)
            {
                photonView.RPC("SyncCountdown", RpcTarget.All, remainingTime);
            }
        }

        if (!stopCountdown) // Check stopCountdown
        {
            timer.text = "00:00";
        }
    }

    void ScoreFunction(int valuecorrect, int value)
    {
        // Add score only if correct[valuecorrect] is active and score has not been added
        if (correct[valuecorrect].activeSelf && !isScored[valuecorrect])
        {
            score_count += value;
            score.text = score_count.ToString();
            isScored[valuecorrect] = true; // Set score added status

            photonView.RPC("SyncCorrectStatus", RpcTarget.All, valuecorrect);

            isUpgrade = false;

            // Call RPC to synchronize score and isUpgrade
            photonView.RPC("SyncScoreAndUpgrade", RpcTarget.All, score_count, isUpgrade);
        }
    }

    public void DecreaseScoreColider(int value)
    {
        if (photonView.IsMine)
        {
            photonView.RPC("RPCDecreaseScore", RpcTarget.All, value);
        }
    }

    public void DecreaseScorePillar(int value, bool index)
    {
        if (index && canDecreaseScorePillar)
        {
            if (photonView.IsMine)
            {
                photonView.RPC("RPCDecreaseScore", RpcTarget.All, value);
                photonView.RPC("RPCSetCanDecreaseScorePillar", RpcTarget.All, false);
                index = false;
            }
        }
    }

    public void DecreaseScoreSelang(int value, bool index)
    {
        if (index && canDecreaseScoreSelang)
        {
            if (photonView.IsMine)
            {
                photonView.RPC("RPCDecreaseScore", RpcTarget.All, value);
                photonView.RPC("RPCSetCanDecreaseScoreSelang", RpcTarget.All, false);
                index = false;
            }
        }
    }

    public void SetIsUpgrade(bool value)
    {
        isUpgrade = value;

        // Call RPC to synchronize isUpgrade
        photonView.RPC("SyncIsUpgrade", RpcTarget.All, value);
    }

    #endregion

    #region PunRPC Methods

    [PunRPC]
    void RPCDecreaseScore(int value)
    {
        score_count -= value;
        if (score_count < 0) score_count = 0; // Prevent negative score
        score.text = score_count.ToString();
    }

    [PunRPC]
    void FinishManager()
    {
        score_finish.text = "Score : " + score.text;
    }

    [PunRPC]
    void RPCSetCanDecreaseScorePillar(bool value)
    {
        canDecreaseScorePillar = value;
    }

    [PunRPC]
    void RPCSetCanDecreaseScoreSelang(bool value)
    {
        canDecreaseScoreSelang = value;
    }

    [PunRPC]
    void DeadRPC(bool value)
    {
        isDead = value;
        if (value) // Activate death UI only if isDead is true
        {
            fire_dead.SetActive(true);
            canvas_gameover.SetActive(true);
            gameObject.SetActive(false);
            Debug.Log("DEAD PLAYER>>>>>>>>>>>>>>");
        }
    }

    [PunRPC]
    void SyncCorrectStatus(int correctIndex)
    {
        // Set the correct status to true on all clients
        correct[correctIndex].SetActive(true);
    }

    [PunRPC]
    void SyncCountdown(float syncedTime)
    {
        countdownTime = syncedTime;
        int minutes = Mathf.FloorToInt(countdownTime / 60);
        int seconds = Mathf.FloorToInt(countdownTime % 60);
        timer.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    [PunRPC]
    void SyncScoreAndUpgrade(int syncedScore, bool syncedIsUpgrade)
    {
        score_count = syncedScore;
        score.text = score_count.ToString();
        isUpgrade = syncedIsUpgrade;
    }

    [PunRPC]
    void SyncIsUpgrade(bool syncedValue)
    {
        // Synchronize isUpgrade value on all clients
        isUpgrade = syncedValue;
    }

    [PunRPC]
    void StopCountdownRPC()
    {
        stopCountdown = true; // Set stopCountdown to true to stop countdown
    }

    #endregion

    #region TODO

    // TODO: Add more detailed logging for debugging purposes.
    // TODO: Implement additional feedback for the player when score is increased or decreased.
    // TODO: Add UI animations for transitions between different game states (e.g., game over, finish).

    #endregion
}
