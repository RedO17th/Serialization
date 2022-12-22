using System;
using System.Collections.Generic;
using System.IO;

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
            if (result.Next != null)
                result = result.Next;
        }

        return result;
    }
    public ListNode GetFirst() => Head;
    public ListNode GetLast() => Tail;

    #endregion

    #region Serialization

    public void Serialize(FileStream s)
    {
        int[] indexes = GenerateArrayOfRandomIndexes();

        WriteData(s, indexes);

        //Сan do it like this, but will it be clear
        // WriteData(s, GenerateArrayOfRandomIndexes());
    }

    private int[] GenerateArrayOfRandomIndexes()
    {
        var indexes = new int[Count];

        var currentItem = Head;
        var randomItem = Head;

        int unknownID = -1;

        for (int i = 0, j = 0; j < Count; i++)
        {
            if (currentItem?.Rand == null)
            {
                indexes[j] = unknownID;

                currentItem = currentItem.Next;

                j++;
            }

            if (currentItem?.Rand == randomItem)
            {
                indexes[j] = i;

                currentItem = currentItem.Next;

                randomItem = Head;

                i = unknownID;
                j++;

                continue;
            }

            randomItem = randomItem.Next;
        }

        return indexes;
    }

    private void WriteData(FileStream s, int[] indexes)
    {
        using StreamWriter sw = new StreamWriter(s);

        sw.WriteLine(Count.ToString());

        var currentItem = Head;

        for (int i = 0; i < Count; i++)
        {
            sw.WriteLine(currentItem.Data);
            sw.WriteLine(indexes[i].ToString());

            currentItem = currentItem.Next;
        }
    }

    #endregion

    #region ClearingList

    public void Clear()
    {
        if (Head == null)
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

    #endregion

    #region Deserialization

    public void Deserialize(FileStream s)
    {
        var indexes = GenerateMainSequanceFromFile(s);

        InitializeRandomItemsInSequance(indexes);
    }

    //Return array of random items
    private int[] GenerateMainSequanceFromFile(FileStream s)
    {
        using StreamReader sw = new StreamReader(s);

        Count = Convert.ToInt32(sw.ReadLine());

        var indexes = new int[Count];

        Head = new ListNode();

        var currentItem = Head;

        for (int i = 0; i < Count; i++)
        {
            if (i == 0)
            {
                currentItem.Data = sw.ReadLine();
                currentItem.Prev = null;
                currentItem.Next = new ListNode();

                indexes[i] = Convert.ToInt32(sw.ReadLine());
            }
            else
            {
                currentItem.Next.Prev = currentItem;
                currentItem = currentItem.Next;
                currentItem.Data = sw.ReadLine();
                if (i != Count - 1)
                    currentItem.Next = new ListNode();
                else
                    Tail = currentItem;

                indexes[i] = Convert.ToInt32(sw.ReadLine());
            }
        }

        return indexes;
    }

    private void InitializeRandomItemsInSequance(int[] indexes)
    {
        var currentItem = Head;
        var randomItem = Head;
        int unknownID = -1;

        for (int i = 0, j = 0; j < Count; i++)
        {
            if (indexes[j] == unknownID)
            {
                currentItem = currentItem.Next;

                i = unknownID;
                j++;

                continue;
            }

            if (indexes[j] == i)
            {
                currentItem.Rand = randomItem;
                currentItem = currentItem.Next;

                randomItem = Head;

                i = unknownID;
                j++;

                continue;
            }

            randomItem = randomItem.Next;
        }
    }

    #endregion
}