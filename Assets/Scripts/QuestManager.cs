using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class QuestManager : MonoBehaviour
{
    private Dictionary<int, string> questScript;
    private Dictionary<string, Dictionary<int, string>> questDictionary; 

    private Dictionary<string, Dictionary<int, string>> playerQuestDictionary;
    private List<string> completedQuest;    // 이미 완료한 퀘스트 이름 리스트
    
    [SerializeField] private TextMeshProUGUI textBox;
    [SerializeField] private GameObject textLetterBox;
    private string currentQuestName;
    private int textIndex;
    
    private static QuestManager instance;
    
    public static QuestManager Instance
    {
        get
        {
            if (instance == null)
                return null;
            
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        questScript = new Dictionary<int, string>();
        questDictionary = new Dictionary<string, Dictionary<int, string>>();

        playerQuestDictionary = new Dictionary<string, Dictionary<int, string>>();
        completedQuest = new List<string>();

        textLetterBox.SetActive(false);
        textBox.gameObject.SetActive(false);

        currentQuestName = "";
        textIndex = 0;
        
        AddQuestInData();
    }

    public void LetterBoxInteraction()
    {
        if (currentQuestName != "")
        {
            InteractWithQuestText(currentQuestName);
            textIndex++;
        }
    }
    
    private void AddQuestInData()
    {
        
        // 퀘스트 내용 기술
        questScript.Add(0, "튜토리얼 퀘스트 대사 1번입니다.");
        questScript.Add(1, "튜토리얼 퀘스트 대사 2번입니다.");
        questScript.Add(2, "튜토리얼 퀘스트 대사 3번입니다.");
        
        // 퀘스트 사전에 삽입
        questDictionary.Add("튜토리얼 퀘스트", new Dictionary<int, string>(questScript));
        
        // 퀘스트 내용 버퍼 삭제
        questScript.Clear();
        
        questScript.Add(0, "튜토리얼 퀘스트2 대사 1번입니다.");
        questScript.Add(1, "튜토리얼 퀘스트2 대사 2번입니다.");
        questScript.Add(2, "튜토리얼 퀘스트2 대사 3번입니다.");
        
        questDictionary.Add("튜토리얼 퀘스트2", new Dictionary<int, string>(questScript));
        
        questScript.Clear();
        
        Debug.Log(currentQuestName);

        foreach (var quest in playerQuestDictionary)
        {
            var questName = quest.Key;

            Debug.Log("퀘스트 이름 : " + questName);

            foreach (var script in quest.Value)
            {
                Debug.Log("인덱스 넘버 : " + script.Key);
                Debug.Log("키 값 : " + script.Value);
            }
        }
    }
    
    void AddQuestInPlayerQuestDictionary(string acceptedQuestName)
    {
        foreach (var quest in questDictionary)
        {
            var questName = quest.Key;
            
            if (questName == acceptedQuestName)
            {
                if (playerQuestDictionary.ContainsKey(questName)) return;
                if (completedQuest.Contains(questName)) return;
                
                playerQuestDictionary.Add(quest.Key, quest.Value);
                Debug.Log("추가된 Quest : " + questName);
                currentQuestName = questName;
                break;
            }
        }
        
        textLetterBox.SetActive(true);
        Time.timeScale = 0.0f;
    }

    void CompleteQuest(string acceptedQuestName)
    {
        foreach (var quest in playerQuestDictionary)
        {
            var questName = quest.Key;
            if (questName == acceptedQuestName)
            {
                if (playerQuestDictionary.ContainsKey(questName) == false) return;
                
                playerQuestDictionary.Remove(questName);
                completedQuest.Add(questName);
                Debug.Log("삭제된 Quest : " + questName);
                break;
            }
        }
    }

    void InteractWithQuestText(string acceptedQuestName)
    {
        Time.timeScale = 0.0f;
        textBox.gameObject.SetActive(true);

        var scripts = playerQuestDictionary[acceptedQuestName];

        if (textIndex == scripts.Count)
        {
            currentQuestName = "";
            textIndex = 0;
            textBox.gameObject.SetActive(false);
            textLetterBox.SetActive(false);
            Time.timeScale = 1.0f;
        }
        
        else      
            textBox.text = scripts[textIndex];

    }
}
