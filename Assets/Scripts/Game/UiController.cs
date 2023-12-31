using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UiController : MonoBehaviour
{
    [SerializeField] List<Image> statsPanels;
    [SerializeField] Text diceResult;
    // Roll Dice Button
    [SerializeField] Button diceButton;
    // Done Button;
    [SerializeField] Button doneButton;
    // CourseBox
    [SerializeField] Text courseBoxTitle;
    [SerializeField] Transform course1;
    [SerializeField] Transform course2;
    // Info Box
    [SerializeField] Text infoBoxTitle;
    [SerializeField] Text infoContent;
    // Platform Transform
    [SerializeField] Transform platformTransform;
    // Game Executor
    [SerializeField] Transform executorTransform;

    List<Player> playerList;
    Animator animator;
    PlatformHelper platformHelper;
    GameExecutor gameExecutor;

    // Start is called before the first frame update
    void Start()
    {
        playerList = GameStats.GetPlayerList();
        playerList.Add(new Player("Anya", "Image/anya"));
        playerList.Add(new Player("Saugy1", "Image/saugy"));
        playerList.Add(new Player("Saugy2", "Image/saugy"));
        playerList.Add(new Player("Saugy3", "Image/saugy"));

        for (int i = 0; i < playerList.Count; i++)
        {
            InitializePlayerCard(playerList[i], statsPanels[i]);
        }

        animator = GetComponent<Animator>();
        platformHelper = platformTransform.GetComponent<PlatformHelper>();
        gameExecutor = executorTransform.GetComponent<GameExecutor>();
    }

    // Update is called once per frame
    void Update()
    {
        // test
        diceResult.text = GameStats.UI.DiceResult.ToString();

        // Player Stats Value
        for (int i = 0; i < playerList.Count; i++) UpdatePlayerCardValue(playerList[i], statsPanels[i]);

        // Roll Dice Button
        if (GameStats.currentState == GameState.ROLL_DICE) diceButton.gameObject.SetActive(true);
        else diceButton.gameObject.SetActive(false);

        // Done Button
        if (GameStats.currentState == GameState.NEXT_PLAYER) doneButton.gameObject.SetActive(true);
        else doneButton.gameObject.SetActive(false);

        // Open MessageBox
        if (GameStats.currentState == GameState.EXECUTE_FEATURE)
        {
            Player currentPlayer = GetCurrentPlayer();
            Block block = GetCurrentBlock();
            if (block.Type == "Department")
            {
                SetupCourseBox(block);
                animator.SetBool("CourseBoxOpen", true);
            } else
            {
                SetupInfoBox(block);
                animator.SetBool("InfoBoxOpen", true);
            }
        } else
        {
            animator.SetBool("CourseBoxOpen", false);
            animator.SetBool("InfoBoxOpen", false);
        }
    }

    public void CloseCourseBox()
    {
        GameStats.currentState = GameState.NEXT_PLAYER;
        animator.SetBool("CourseBoxOpen", false);
    }

    public void CloseInfoBox()
    {
        Player currentPlayer = GetCurrentPlayer();
        Block block = GetCurrentBlock();
        if (block.specialEvent != null) gameExecutor.ExecuteRewards(currentPlayer, block.specialEvent.Rewards);
        GameStats.currentState = GameState.NEXT_PLAYER;
        animator.SetBool("InfoBoxOpen", false);
    }

    public void GoToNextCamera()
    {
        CameraDirection curr = GameSettings.cameraDirection;
        switch (curr)
        {
            case CameraDirection.PLAYER: 
                GameSettings.cameraDirection = CameraDirection.CLOSE_PLAYER;
                break;
            case CameraDirection.CLOSE_PLAYER:
                GameSettings.cameraDirection = CameraDirection.DORM;
                break;
            case CameraDirection.DORM:
                GameSettings.cameraDirection = CameraDirection.HOSTPITAL;
                break;
            case CameraDirection.HOSTPITAL:
                GameSettings.cameraDirection = CameraDirection.LIBRARY;
                break;
            case CameraDirection.LIBRARY:
                GameSettings.cameraDirection = CameraDirection.PARK;
                break;
            case CameraDirection.PARK:
                GameSettings.cameraDirection = CameraDirection.PLAYER;
                break;
            default:
                GameSettings.cameraDirection = CameraDirection.PLAYER;
                break;
        }
    }

    private void SetupCourseBox(Block block)
    {
        courseBoxTitle.text = block.Name;
        LoadCourse(1, block);
        LoadCourse(2, block);
    }

    private void LoadCourse(int courseNumber, Block block)
    {
        Transform courseTrans = (courseNumber == 1 ? course1 : course2);
        int courseIndex = (courseNumber == 1 ? block.course1 : block.course2);
        Course course = block.Courses[courseIndex];

        // find text
        Text name = courseTrans.Find("name").GetComponent<Text>();
        Text type = courseTrans.Find("type").Find("content").GetComponent<Text>();
        Text credit = courseTrans.Find("credit").Find("content").GetComponent<Text>();
        Text owner = courseTrans.Find("owner").Find("content").GetComponent<Text>();
        Text condition = courseTrans.Find("condition").Find("content").GetComponent<Text>();
        Text select = courseTrans.Find("selectButton").Find("selectText").GetComponent<Text>();
        Text selectCost = courseTrans.Find("selectButton").Find("costText").GetComponent<Text>();

        // set value
        name.text = course.Name;
        type.text = course.Type;
        credit.text = course.Credit.ToString();
        owner.text = (course.Owner == null ? "�L" : course.Owner);
        condition.text = "�L";
        select.text = (course.Owner == null ? "���" : "�R��");
        selectCost.text = "��O -10";
    }

    private void SetupInfoBox(Block block)
    {
        infoBoxTitle.text = block.Name;
        if (block.Message != null)
        {
            SpecialEvent spEvent = null;
            if (block.Type == "Chance") spEvent = platformHelper.GetRandomChance();
            else if (block.Type == "Destiny") spEvent = platformHelper.GetRandomDestiny();
            block.specialEvent = spEvent;
            infoContent.text = CreateInfoMessage(block);
        }
    }

    private string CreateInfoMessage(Block block)
    {
        SpecialEvent spEvent = block.specialEvent;
        StringBuilder sb = new StringBuilder();
        sb.AppendLine(block.Message);
        if (spEvent != null)
        {
            sb.AppendLine("-------------------------------------------------");
            sb.AppendLine(spEvent.Title);
            sb.AppendLine("  " + spEvent.Message);
            foreach (Reward reward in spEvent.Rewards)
            {
                sb.Append(" - ");
                if (reward.Emotion != 0)
                {
                    sb.Append("�߱� " + string.Format("{0:+#;-#}", reward.Emotion));
                }
                sb.AppendLine();
                sb.Append(" - ");
                if (reward.Power != 0)
                {
                    sb.Append("��O " + string.Format("{0:+#;-#}", reward.Power));
                }
                sb.AppendLine();
            }
        }
        return sb.ToString();
    }

    private Player GetCurrentPlayer()
    {
        return playerList[GameStats.CurrentPlayerIndex];
    }

    private Block GetCurrentBlock()
    {
        Player currentPlayer = GetCurrentPlayer();
        return platformHelper.GetBlock(currentPlayer.StandingPos);
    }

    private void InitializePlayerCard(Player player, Image panel)
    {
        Text textName = panel.transform.Find("Name").GetComponent<Text>();
        Image iconImage = panel.transform.Find("Image").GetComponent<Image>();

        textName.text = player.Name;
        iconImage.sprite = Resources.Load<Sprite>(player.ImagePath);

        UpdatePlayerCardValue(player, panel);
    }

    private void UpdatePlayerCardValue(Player player, Image panel)
    {
        Text valueCredit = panel.transform.Find("ValueCredit").GetComponent<Text>();
        Text valueEmotion = panel.transform.Find("ValueEmotion").GetComponent<Text>();
        Text valuePower = panel.transform.Find("ValuePower").GetComponent<Text>();
        valueCredit.text = $"{player.Credit}/{GameSettings.TARGET_CREDIT}";
        valueEmotion.text = $"{player.Emotion}/{GameSettings.MAX_EMOTION}";
        valuePower.text = $"{player.Power}/{GameSettings.MAX_POWER}";
    }

}
