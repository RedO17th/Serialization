using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

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
        ListNode temp = Head;
        int number = Count;

        for (int i = 0; i < number; i++)
        {
            if (temp.Next == null)
            {
                temp.Next = newNode;
                newNode.Prev = temp;

                Tail = newNode;

                Count++;
            }
            else
                temp = temp.Next;
        }
    }
    private void AddHeadElement(ListNode newNode)
    {
        Head = newNode;
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
        int id = -1;

        using StreamWriter sw = new StreamWriter(s);

        for (ListNode currentNode = Head; currentNode != null; currentNode = currentNode.Next)
        {
            ++id;
            int prevId = -1;
            int nextId = -1;
            int randId = -1;

            if (currentNode.Prev != null) 
                prevId = GetID(currentNode.Prev);

            if (currentNode.Next != null) 
                nextId = GetID(currentNode.Next);

            if (currentNode.Rand != null)
                randId = GetID(currentNode.Rand);

            sw.WriteLine($"{id}:{prevId}:{nextId}:{randId}:{currentNode.Data}");
        }
    }

    private int GetID(ListNode element)
    {
        int id = -1;

        for (ListNode currentNode = Head; currentNode != null; currentNode = currentNode.Next)
        {
            id++;

            if (currentNode == element)
                break;    
        }

        return id;
    }

    public void Clear()
    {
        if(Head == null)
            return;

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

        Head = null;
        Tail = null;
        Count = -1;
    }

    public void Deserialize(FileStream s) 
    {
        using StreamReader sr = new StreamReader(s);

        List<int[]> elements = new List<int[]>();

        string resultLine = string.Empty;
        while ((resultLine = sr.ReadLine()) != null)
        {
            var dataInLine = resultLine.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
            
            var data = new int[dataInLine.Length - 1];

            for (int i = 1; i < dataInLine.Length; i++)
                data[i - 1] = Convert.ToInt32(dataInLine[i]);

            elements.Add(data);
        }

        if(elements.Count == 0)
            return;

        const int nextFieldID = 1;
        const int randFieldID = 2;
        const int dataFieldID = 3;
        const int defaultValue = -1;

        ListNode currentItem = null;

        //Инициализация прямого порядка списка 
        for (int i = 0; i < elements.Count; i++)
        {
            if (Head == null)
            {
                Head = new ListNode(elements[i][dataFieldID].ToString());
                Head.Prev = null; 
                Head.Next = null;
                Head.Rand = null;

                currentItem = Head;
            }

            if (elements[i][nextFieldID] != defaultValue)
            {
                var newItem = new ListNode(elements[i + 1][dataFieldID].ToString());
                    newItem.Prev = currentItem;
                    newItem.Next = null;
                    newItem.Rand = null;

                currentItem.Next = newItem;
                currentItem = newItem; 
            }
        }

        Tail = currentItem;
        Count = elements.Count;

        //Инициализация ссылок на рандомные объекты
        for (int i = 0; i < elements.Count; i++)
        {
            if (elements[i][randFieldID] != defaultValue)
            {
                var randomItemID = elements[i][randFieldID];

                var randomItem = GetByIndex(randomItemID);

                var parentItem = GetByIndex(i);
                    parentItem.Rand = randomItem;
            }    
        }

        elements.Clear();
    }
}
