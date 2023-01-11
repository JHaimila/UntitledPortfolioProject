using System.Collections;
using System.Collections.Generic;
using RPG.Quests;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace RPG.UI.Quests
{
    public class QuestUI : MonoBehaviour
    {
        [SerializeField] private GameObject _questPanel;
        [SerializeField] private Transform _questListParent, _objListParent, _rewardListParent;
        [SerializeField] private GameObject _questItemPrefab, _objItemPrefab;
        [SerializeField] private Button _rewardTab, _objTab;
        [SerializeField] private TextMeshProUGUI _title;
        private bool _isObjective = true;
        private Button _currentBtn;
        private QuestList _questList;
        

        private void Start() {
            _questList = GameObject.FindGameObjectWithTag("Player").GetComponent<QuestList>();
            _questList.OnUpdate += UpdateList;
        }
        private void OnDestroy() {
            if(_questList != null)
            {
                _questList.OnUpdate -= UpdateList;
            }
        }
        private Quest _currentQuest;
        public void ToggleUI()
        {
            _questPanel.SetActive(!_questPanel.activeSelf);
            _isObjective = true;
            _rewardTab.interactable = true;
            _objTab.interactable = false;
            _title.text = "Objectives";
            if(_questPanel.activeSelf)
            {
                ClearList(_questListParent);
                ClearList(_objListParent);
                ClearList(_rewardListParent);
                UpdateList();
            }
            else
            {
                CloseUI();
            }
        }
        public void UpdateList()
        {
            QuestList _questList = GameObject.FindGameObjectWithTag("Player").GetComponent<QuestList>();
            ClearList(_questListParent);
            foreach(var status in _questList.GetStatuses())
            {
                GameObject newItem = Instantiate(_questItemPrefab, _questListParent);
                newItem.GetComponent<Button>().onClick.AddListener(delegate {PopulateQuestInfo(status.GetQuest());});
                newItem.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = status.GetQuest().name;
                newItem.GetComponent<QuestItemUI>().Setup(status);
            }

            if(_currentQuest != null)
            {
                PopulateQuestInfo();
            }
        }
        public void PopulateQuestInfo()
        {
            ClearList(_objListParent);
            ClearList(_rewardListParent);
            if(_currentQuest == null){return;}
            if(_isObjective)
            {
                foreach(var objective in _currentQuest.GetObjectives())
                {
                    GameObject newItem = Instantiate(_objItemPrefab, _objListParent);
                    newItem.GetComponent<TextMeshProUGUI>().text = objective.GetDescription();
                    foreach(var status in _questList.GetStatuses())
                    {
                        if(status.GetQuest() != _currentQuest){continue;}
                        if(status.GetCompletedCount() == 0){continue;}
                        if(status.GetCompleteObjectives().Contains(objective.GetReference()))
                        {
                            newItem.GetComponent<TextMeshProUGUI>().fontStyle = FontStyles.Strikethrough;
                        }
                    }
                }
            }
            else
            {
                foreach(var reward in _currentQuest.GetRewards())
                {
                    GameObject newItem = Instantiate(_objItemPrefab, _objListParent);
                    newItem.GetComponent<TextMeshProUGUI>().text = reward.item.GetDisplayName();
                }
            }
            
        }
        public void SwitchTab()
        {
            _isObjective = !_isObjective;
            if(_isObjective)
            {
                _rewardTab.interactable = true;
                _objTab.interactable = false;
                _title.text = "Objectives";
            }
            else
            {
                _rewardTab.interactable = false;
                _objTab.interactable = true;
                _title.text = "Rewards";
            }
            PopulateQuestInfo();
        }
        private void PopulateQuestInfo(Quest quest)
        {
            _currentQuest = quest;
            PopulateQuestInfo();
        }
        private void CloseUI()
        {
            ClearList(_questListParent);
            ClearList(_objListParent);
            ClearList(_rewardListParent);
            _currentQuest = null;
        }
        private void ClearList(Transform parent)
        {
            int childCount = parent.childCount;
            if(childCount > 0)
            {
                for(int i = 0; i < childCount; i++)
                {
                    Destroy(parent.GetChild(i).gameObject);
                }
            }
        }
    }
}