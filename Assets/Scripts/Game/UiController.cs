using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class UiController : MonoBehaviour
{
    [SerializeField] List<Image> statsPanels;
    [SerializeField] Text diceResult;
    // Roll Dice Button
    [SerializeField] Button diceButton;
    // Done Button
    [SerializeField] Button doneButton;
    // CourseBox
    [SerializeField] Text courseBoxTitle;
    [SerializeField] Transform course1;
    [SerializeField] Transform course2;
    // Info Box
    [SerializeField] Text infoBoxTitle;
    [SerializeField] Text infoContent;
    // Summary Box
    [SerializeField] Transform summaryPanel;
    // Platform Transform
    [SerializeField] Transform platformTransform;
    // Game Executor
    [SerializeField] Transform executorTransform;
    // Player Controller
    [SerializeField] Transform playerControllerTransform;

    List<Player> playerList;
    Animator animator;
    PlatformHelper platformHelper;
    GameExecutor gameExecutor;
    PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        playerList = GameStats.GetPlayerList();
        playerList.Add(new Player("Dock", "Image/dock"));
        playerList.Add(new Player("Mr. Egg", "Image/egg_head"));
        playerList.Add(new Player("Saugy", "Image/saugy"));
        playerList.Add(new Player("Traffic Cone", "Image/cones"));

        for (int i = 0; i < playerList.Count; i++)
        {
            InitializePlayerCard(playerList[i], statsPanels[i]);
        }

        animator = GetComponent<Animator>();
        platformHelper = platformTransform.GetComponent<PlatformHelper>();
        gameExecutor = executorTransform.GetComponent<GameExecutor>();
        playerController = playerControllerTransform.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        // test
        diceResult.text = GameStats.UI.DiceResult.ToString();

        // Player Stats Value
        for (int i = 0; i < playerList.Count; i++) UpdatePlayerCardValue(playerList[i], statsPanels[i]);

        // Roll Dice Button
        if (GameStats.currentState == GameState.WAIT_ROLL_DICE) diceButton.gameObject.SetActive(true);
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
                if (animator.GetBool("CourseBoxOpen") == false)
                {
                    block.RefreshUnselectedCourse();
                }
                SetupCourseBox(block, currentPlayer);
                animator.SetBool("CourseBoxOpen", true);
            } else
            {
                if (animator.GetBool("InfoBoxOpen") == false)
                {
                    SetupInfoBox(block);
                }
                animator.SetBool("InfoBoxOpen", true);
            }
        } else
        {
            animator.SetBool("CourseBoxOpen", false);
            animator.SetBool("InfoBoxOpen", false);
        }

        // Pass Starting point
        if (GameStats.currentState == GameState.PASS_START)
        {
            // TODO: display summary form
            Player currentPlayer = GetCurrentPlayer();
            SetupSummaryBox(currentPlayer);
            animator.SetBool("SummaryBoxOpen", true);
        } else
        {
            animator.SetBool("SummaryBoxOpen", false);
        }

        // check if player is EMO or whatever
        if (GameStats.currentState == GameState.CHECK)
        {
            Player currentPlayer = GetCurrentPlayer();
            if (currentPlayer.IsEMO())
            {
                // TODO: display check form to notificate player and execute the movement
                animator.SetBool("CheckBoxOpen", true);
            }
            else
            {
                GameStats.currentState = GameState.WAIT_ROLL_DICE;
            }
        } else
        {
            animator.SetBool("CheckBoxOpen", false);
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
        GameStats.currentState = GameState.NEXT_PLAYER;
        if (block.specialEvent != null) gameExecutor.ExecuteRewards(currentPlayer, block.specialEvent.Rewards);
        animator.SetBool("InfoBoxOpen", false);
    }

    public void CloseSummaryBox()
    {
        Player currentPlayer = GetCurrentPlayer();
        gameExecutor.OnPlayerPassStart(currentPlayer);
        animator.SetBool("SummaryBoxOpen", false);
        GameStats.currentState = GameState.MOVE; // keep moving
    }

    public void CloseCheckBox()
    {
        Player currentPlayer = GetCurrentPlayer();
        animator.SetBool("CheckBoxOpen", false);
        playerController.TeleportPlayerToCorner(GameStats.CurrentPlayerIndex, CornerBlock.DORM);
        gameExecutor.ExecuteEmoPenalty(currentPlayer);
        GameStats.currentState = GameState.NEXT_PLAYER;
    }

    public void GoToNextCamera()
    {
        CameraDirection curr = GameSettings.cameraDirection;
        if (curr == CameraDirection.DICE) return;
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

    private void SetupCourseBox(Block block, Player currentPlayer)
    {
        courseBoxTitle.text = block.Name;
        LoadCourse(1, block, currentPlayer);
        LoadCourse(2, block, currentPlayer);
    }

    private void LoadCourse(int courseNumber, Block block, Player currentPlayer)
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
        owner.text = course.Owner ?? "無";
        condition.text = "無";
        select.text = (course.Owner == null ? "選課" : (course.Owner == currentPlayer.Name ? "已選" : "搶課"));
        selectCost.text = (course.Owner == null ? $"體力 -{GameSettings.SELECT_COURSE_POWER_COST}" : (course.Owner == currentPlayer.Name ? "" : $"體力 -{currentPlayer.GetGrabCost()}"));
    }

    private void SetupInfoBox(Block block)
    {
        infoBoxTitle.text = block.Name;
        if (block.Message != null)
        {
            SpecialEvent spEvent = null;
            if (block.Type == "Chance") spEvent = platformHelper.GetRandomChance();
            else if (block.Type == "Destiny") spEvent = platformHelper.GetRandomDestiny();
            else if (block.Type.StartsWith("Event"))
            {
                Debug.Log("Special Event");
                WalkingEventType walkEvent = WalkingEventType.BG_FRIEND;
                if (block.Type.EndsWith("bfgf")) walkEvent = WalkingEventType.BG_FRIEND;
                else if (block.Type.EndsWith("construct")) walkEvent = WalkingEventType.CONSTRUCTION;
                else if (block.Type.EndsWith("bike")) walkEvent = WalkingEventType.BIKE_ACCIDENT;
                else if (block.Type.EndsWith("dog")) walkEvent = WalkingEventType.DOG_BITE;
                spEvent = platformHelper.GetSpecialWalkingEvent(walkEvent);
            }
            Debug.Log($"spEvent: null? {spEvent == null}");
            block.specialEvent = spEvent;
            infoContent.text = CreateInfoBoxMessage(block);
        }
    }

    private string CreateInfoBoxMessage(Block block)
    {
        SpecialEvent spEvent = block.specialEvent;
        StringBuilder sb = new StringBuilder();
        sb.AppendLine(block.Message);
        if (spEvent != null)
        {
            sb.AppendLine("-------------------------------------------------");
            sb.AppendLine(spEvent.Title);
            sb.AppendLine("  " + spEvent.Message);
            sb.AppendLine();
            foreach (Reward reward in spEvent.Rewards)
            {
                if (reward.Mode == "Add")
                {
                    if (reward.Credit != 0)
                    {
                        sb.Append(" - ");
                        sb.Append("學分 " + string.Format("{0:+#;-#}", reward.Credit));
                        sb.AppendLine();
                    }

                    if (reward.Emotion != 0)
                    {
                        sb.Append(" - ");
                        sb.Append("心情 " + string.Format("{0:+#;-#}", reward.Emotion));
                        sb.AppendLine();
                    }

                    if (reward.Power != 0)
                    {
                        sb.Append(" - ");
                        sb.Append("體力 " + string.Format("{0:+#;-#}", reward.Power));
                        sb.AppendLine();
                    }
                }
                else if (reward.Mode == "Divide")
                {
                    if (reward.Credit != 0)
                    {
                        sb.Append(" - ");
                        sb.Append("學分 " + string.Format("除以{0}", reward.Credit));
                        sb.AppendLine();
                    }

                    if (reward.Emotion != 0)
                    {
                        sb.Append(" - ");
                        sb.Append("心情 " + string.Format("除以{0}", reward.Emotion));
                        sb.AppendLine();
                    }

                    if (reward.Power != 0)
                    {
                        sb.Append(" - ");
                        sb.Append("體力 " + string.Format("除以{0}", reward.Power));
                        sb.AppendLine();
                    }
                }
                else if (reward.Mode == "All_Add")
                {
                    if (reward.Credit != 0)
                    {
                        sb.Append(" - ");
                        sb.Append("所有玩家 學分 " + string.Format("{0:+#;-#}", reward.Credit));
                        sb.AppendLine();
                    }

                    if (reward.Emotion != 0)
                    {
                        sb.Append(" - ");
                        sb.Append("所有玩家 心情 " + string.Format("{0:+#;-#}", reward.Emotion));
                        sb.AppendLine();
                    }

                    if (reward.Power != 0)
                    {
                        sb.Append(" - ");
                        sb.Append("所有玩家 體力 " + string.Format("{0:+#;-#}", reward.Power));
                        sb.AppendLine();
                    }
                }
                else if (reward.Mode == "RollAgain")
                {
                    sb.Append(" - ");
                    sb.Append("重骰一次!");
                    sb.AppendLine();
                }
                else if (reward.Mode == "Teleport")
                {
                    sb.Append($" - 被移動至 {MyTools.TranslateCorner(reward.TpTarget)}");
                    sb.AppendLine();
                }
            }
        }
        return sb.ToString();
    }

    private void SetupSummaryBox(Player player)
    {
        Image iconImage = summaryPanel.Find("PlayerImage").GetComponent<Image>();
        Text name = summaryPanel.Find("PlayerName").GetComponent<Text>();
        Text info = summaryPanel.Find("PlayerInfo").GetComponent<Text>();
        Text infoDelta = summaryPanel.Find("InfoDelta").GetComponent<Text>();
        Text courses = summaryPanel.Find("SelectedCourses").Find("Viewport").Find("Content").GetComponent<Text>();

        iconImage.sprite = Resources.Load<Sprite>(player.ImagePath);
        name.text = player.Name;
        info.text = CreatePlayerInfoText(player);
        infoDelta.text = CreateInfoDeltaText(player);
        courses.text = player.GetCurrentCoursesText();
    }

    private string CreatePlayerInfoText(Player player)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"學分：{player.Credit}");
        sb.AppendLine();
        sb.AppendLine($"心情：{player.Emotion}");
        sb.AppendLine();
        sb.AppendLine($"體力：{player.Power}");
        return sb.ToString();
    }

    private string CreateInfoDeltaText(Player player)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"(+{player.CurrentCourse.Sum(c => c.Credit)})");
        sb.AppendLine();
        sb.AppendLine("(+5)");
        sb.AppendLine();
        sb.AppendLine("(100)");
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
