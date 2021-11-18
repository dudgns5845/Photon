using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListPanel : MonoBehaviour
{
    public Button [] listPanels;
    public Button Prew, Next;
    List<string> roomindex = new List<string> { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11","12","13","14","15" };
    int currentPage = 1, maxPage, multiple;

    private void Start()
    {
        maxPage = (roomindex.Count % listPanels.Length == 0) ? roomindex.Count % listPanels.Length : roomindex.Count % listPanels.Length + 1;

        //���� ������ư �ʱ�ȭ
        Prew.interactable = (currentPage <= 1) ? false : true;
        Next.interactable = (currentPage >= maxPage) ? false : true;

        multiple = (currentPage - 1) * listPanels.Length; //�ش� �������� ù��° �ε���

        for(int i = 0; i < listPanels.Length; i++)
        {
            listPanels[i].interactable = (multiple + i < roomindex.Count) ? true : false;
            listPanels[i].GetComponentInChildren<Text>().text = (multiple + i < roomindex.Count) ? roomindex[multiple + i] : "";

        }
    }

    public void BtnClick(int index)
    {
        if (index == -2) currentPage--;
        else if (index == -1) currentPage++;
        else print(roomindex[multiple+ index]);

        Start();
    }
}
