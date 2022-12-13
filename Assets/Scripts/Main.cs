using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using static UnityEditor.Progress;

public class Main : MonoBehaviour
{
    [SerializeField] private int _numberOfElement = 5;
    [SerializeField] private string _fileName = "note.txt";

    private ListRand _list = new ListRand();

    private string _path = string.Empty;

    void Start()
    {
        _path = Path.Combine(Application.dataPath, _fileName);

        InitializeList();

        using (FileStream fstream = new FileStream(_path, FileMode.Create))
        {
            _list.Serialize(fstream);
        }

        _list.Clear();

        using (FileStream fstream = File.OpenRead(_path))
        {
            _list.Deserialize(fstream);
        }
    }

    private void InitializeList()
    {
        _list = new ListRand();

        int value = 0;
        for (int i = 0; i < _numberOfElement; i++)
            _list.Add(new ListNode(Convert.ToString(++value)));

        var secondNode = _list.GetByIndex(1);
            secondNode.Rand = secondNode;

        var thirdNode = _list.GetByIndex(2);
            thirdNode.Rand = _list.GetLast();

        var fourthNode = _list.GetByIndex(3);
            fourthNode.Rand = _list.GetFirst();
    }
}

class ListNode
{
    public ListNode Prev;
    public ListNode Next;
    public ListNode Rand;
    public string Data;

    public ListNode()
    {
        Prev = null;
        Next = null;
        Rand = null;
        Data = string.Empty;
    }
    public ListNode(string value)
    {
        Prev = null;
        Next = null;
        Rand = null;
        Data = value;
    }
}

class ListRand
{
    public ListNode Head;
    public ListNode Tail;
    public int Count;

    public ListRand()
    {
        Head = null;
        Tail = null;
        Count = 0;
    }

    #region Add element
    public void Add(ListNode newNode)
    {
        if (newNode == null)
            return;

        if (Head != null)
            AddNewElement(newNode);
        else
            AddHeadElement(newNode);
    }
    private void AddNewElement(ListNode newNode)
    {
        int number = Count;
        ListNode item = Head;

        for (int i = 0; i < number; i++)
        {
            if (item.Next == null)
            {
                item.Next = newNode;
                newNode.Prev = item;

                Tail = newNode;

                Count++;
            }
            else
                item = item.Next;
        }
    }
    private void AddHeadElement(ListNode newNode)
    {
        Head = newNode;
        Tail = Head;
        Count++;
    }
    #endregion

    #region GetElement
    public ListNode GetByIndex(int index = 0)
    {
        if (index < 0 && index >= Count) return null;

        if (Head == null) return null;

        if (index == 0) return Head;

        ListNode result = Head;

        for (int i = 0; i < index; i++)
        {
            if(result.Next != null)
                result = result.Next;
        }

        return result;
    }
    public ListNode GetFirst() => Head;
    public ListNode GetLast() => Tail;

    #endregion

    public void Serialize(FileStream s)
    {
        using StreamWriter sw = new StreamWriter(s);

        for (ListNode currentNode = Head; currentNode != null; currentNode = currentNode.Next)
        {
            sw.WriteLine($"{ GetID(currentNode.Prev) }:{ GetID(currentNode.Next) }:{ GetID(currentNode.Rand) }:{ currentNode.Data }");
        }
    }

    private int GetID(ListNode element)
    {
        int unknownID = -1;

        if(element == null) return unknownID;

        for (ListNode currentNode = Head; currentNode != null; currentNode = currentNode.Next)
        {
            unknownID++;

            if (currentNode == element)
                break;    
        }

        return unknownID;
    }

    public void Clear()
    {
        if(Head == null)
            return;

        ClearMainSequance();

        Head = null;
        Tail = null;
        Count = 0;
    }

    private void ClearMainSequance()
    {
        ListNode nextElement = null;

        for (ListNode currentNode = Head; currentNode != null;)
        {
            nextElement = currentNode.Next;

            currentNode.Next = null;
            currentNode.Prev = null;
            currentNode.Rand = null;
            currentNode.Data = string.Empty;

            if (nextElement == null)
                break;

            currentNode = nextElement;
        }
    }

    public void Deserialize(FileStream s) 
    {
        //Обдумать название метода,или попробовать избавиться от него
        List<int[]> items = ReadFile(s);

        if (items.Count == 0)
            return;

        Head = new ListNode();
        Head.Prev = null;
        Head.Next = null;

        var currentItem = Head;
        ListNode nextItem = null;

        int dataPositionID = 3;

        for (int i = 0; i < items.Count; i++)
        {
            currentItem.Data = items[i][dataPositionID].ToString();
            currentItem.Rand = null;

            if (i != (items.Count - 1))
            {
                nextItem = new ListNode();

                currentItem.Next = nextItem;
                nextItem.Prev = currentItem;

                currentItem = nextItem;
            }
        }

        Tail = currentItem;
        Count = items.Count;

        int unknownID = -1;
        int referencePositionID = 2;

        for (int i = 0; i < items.Count; i++)
        {
            var seq = items[i];

            if (seq[referencePositionID] != unknownID)
            {
                var randomItemID = seq[referencePositionID];

                var randomItem = GetByIndex(randomItemID);

                var parentItem = GetByIndex(i);
                parentItem.Rand = randomItem;
            }
        }

        items.Clear();
    }

    private List<int[]> ReadFile(FileStream s)
    {
        var items = new List<int[]>();

        char splitter = ':';
        string resultLine = string.Empty;

        using StreamReader sr = new StreamReader(s);

        while ((resultLine = sr.ReadLine()) != null)
        {
            string[] dataInLine = resultLine.Split(new char[] { splitter }, StringSplitOptions.RemoveEmptyEntries);

            items.Add(GenerateArrayOfIndexes(dataInLine));
        }

        return items;
    }

    private int[] GenerateArrayOfIndexes(string[] dataInLine)
    {
        if (dataInLine.Length == 0)
            return new int[0];

        var result = new int[dataInLine.Length];

        for (int i = 0; i < dataInLine.Length; i++)
            result[i] = Convert.ToInt32(dataInLine[i]);

        return result;
    }
}

#region Green

//ListNode currentItem = null;
//for (int i = 0; i < items.Count; i++)
//{
//    if (Head == null)
//    {
//        Head = new ListNode(items[i][dataFieldID].ToString());
//        Head.Prev = null;
//        Head.Next = null;
//        Head.Rand = null;

//        currentItem = Head;
//    }

//    if (items[i][nextFieldID] != unknownID)
//    {
//        var newItem = new ListNode(items[i + 1][dataFieldID].ToString());
//        newItem.Prev = currentItem;
//        newItem.Next = null;
//        newItem.Rand = null;

//        currentItem.Next = newItem;
//        currentItem = newItem;
//    }
//}

//Tail = currentItem;
//Count = items.Count;

////Инициализация ссылок на рандомные объекты
///
//int randFieldID = 2;
//for (int i = 0; i < items.Count; i++)
//{
//    if (items[i][randFieldID] != unknownID)
//    {
//        var randomItemID = items[i][randFieldID];

//        var randomItem = GetByIndex(randomItemID);

//        var parentItem = GetByIndex(i);
//        parentItem.Rand = randomItem;
//    }
//}

//items.Clear();

#endregion
