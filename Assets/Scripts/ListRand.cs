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
            if(result.Next != null)
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

        //[COMMIT] Сan do it like this, but will it be clear
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
            sw.WriteLine(indexes[i].ToString());
            sw.WriteLine(currentItem.Data);

            currentItem = currentItem.Next;
        }
    }

    #endregion

    #region ClearingList

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

    #endregion

    #region Deserialization

    public void Deserialize(FileStream s) 
    {
        //List<int[]> items = GenerateItemsFromFile(s);

        //if (items.Count == 0)
        //    return;

        //ListNode lastItem = InitializeHeadItem();

        //GenerateMainSequance(ref lastItem, items);

        //Tail = lastItem;
        //Count = items.Count;

        //AddRandomItemsToSequenceItems(items);

        //items.Clear();
    }
    #endregion

    #region Green
    //private List<int[]> GenerateItemsFromFile(FileStream s)
    //{
    //    var items = new List<int[]>();

    //    char splitter = ':';
    //    string resultLine = string.Empty;

    //    using StreamReader sr = new StreamReader(s);

    //    while ((resultLine = sr.ReadLine()) != null)
    //    {
    //        string[] dataInLine = resultLine.Split(new char[] { splitter }, StringSplitOptions.RemoveEmptyEntries);

    //        items.Add(GenerateArrayOfIndexes(dataInLine));
    //    }

    //    return items;
    //}

    //private int[] GenerateArrayOfIndexes(string[] dataInLine)
    //{
    //    if (dataInLine.Length == 0)
    //        return new int[0];

    //    var result = new int[dataInLine.Length];

    //    for (int i = 0; i < dataInLine.Length; i++)
    //        result[i] = Convert.ToInt32(dataInLine[i]);

    //    return result;
    //}

    //private ListNode InitializeHeadItem()
    //{
    //    Head = new ListNode();
    //    Head.Prev = null;
    //    Head.Next = null;

    //    return Head;
    //}

    //private void GenerateMainSequance(ref ListNode currentItem, List<int[]> items)
    //{
    //    ListNode nextItem = null;

    //    int dataPositionIndex = 3;

    //    for (int i = 0; i < items.Count; i++)
    //    {
    //        currentItem.Data = items[i][dataPositionIndex].ToString();
    //        currentItem.Rand = null;

    //        if (i != (items.Count - 1))
    //        {
    //            nextItem = new ListNode();

    //            currentItem.Next = nextItem;
    //            nextItem.Prev = currentItem;

    //            currentItem = nextItem;
    //        }
    //    }
    //}

    //private void AddRandomItemsToSequenceItems(List<int[]> items)
    //{
    //    int unknownIndex = -1;
    //    int referencePositionIndex = 2;

    //    for (int i = 0; i < items.Count; i++)
    //    {
    //        var fieldsData = items[i];

    //        if (fieldsData[referencePositionIndex] != unknownIndex)
    //        {
    //            var randomItemID = fieldsData[referencePositionIndex];

    //            var randomItem = GetByIndex(randomItemID);

    //            var parentItem = GetByIndex(i);
    //                parentItem.Rand = randomItem;
    //        }
    //    }
    //}

    #endregion
}
