using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class LobbyManager : MonoBehaviour
{
    public string currentRoomCode;
    public GameObject waitingPanel;
    public TMP_Text roomCodeText;
    public TMP_InputField joinInput;
    public static LobbyManager Instance;
    public TMP_Text player1Text;
    public TMP_Text player2Text;

    public GameObject startButton;

    private void Awake()
    {
        Instance = this;
    }

    public async void CreateRoom()
    {
        currentRoomCode = GenerateRoomCode();

        Debug.Log("Created Room Code: " + currentRoomCode);

        bool success = await NetworkManager.Instance.StartGame(
            Fusion.GameMode.Host,
            currentRoomCode
        );

        if (success)
        {
            Debug.Log("Room created successfully!");
            waitingPanel.SetActive(true);
            roomCodeText.text = "Room Code: " + currentRoomCode;
            startButton.GetComponent<Button>().interactable = false;
        }
    }

    public void PlayerJoined(PlayerRef player)
    {
        if (player == PlayerRef.None)
            return;

        if (player.PlayerId == 1)
        {
            player1Text.text = "Player 1: Connected";
        }
        else if (player.PlayerId == 2)
        {
            player2Text.text = "Player 2: Connected";
            if (NetworkManager.Instance.Runner.IsServer)
            {
                startButton.GetComponent<Button>().interactable = true;
            }
        }
    }

    public async void JoinRoom()
    {
        string code = joinInput.text.Trim();

        if (string.IsNullOrEmpty(code))
        {
            Debug.Log("No room code entered");
            return;
        }


        bool success = await NetworkManager.Instance.StartGame(
            Fusion.GameMode.Client,
            code
        );
        waitingPanel.SetActive(true);
        roomCodeText.text = "";

        Debug.Log("Join result: " + success);
    }

    private string GenerateRoomCode()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        string code = "";

        for (int i = 0; i < 6; i++)
        {
            code += chars[Random.Range(0, chars.Length)];
        }

        return code;
    }
}