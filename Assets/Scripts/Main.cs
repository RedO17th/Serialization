using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Main : MonoBehaviour
{
    [SerializeField] private int numberOfElement = 5;


    void Start()
    {
        ListRand list = new ListRand();

        int value = 0;
        for (int i = 0; i < numberOfElement; i++)
            list.Add(new ListNode(Convert.ToString(++value)));

        var secondNode = list.GetByIndex(1);
            secondNode.SetRand(secondNode);

        var thirdNode = list.GetByIndex(2);
            thirdNode.SetRand(list.GetLast());

        var fourthNode = list.GetByIndex(3);
            fourthNode.SetRand(list.GetFirst());
    }
}

class ListNode
{
    public ListNode Prev;
    public ListNode Next;
    public ListNode Rand; // произвольный элемент внутри списка
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

    public void SetPrev(ListNode previous) => Prev = previous;

    public void SetNext(ListNode next) => Next = next;

    public void SetRand(ListNode rand) => Rand = rand;
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
                temp.SetNext(newNode);
                newNode.SetPrev(temp);

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

    public ListNode GetByIndex(int index = 0)
    {
        if (index < 0 && index >= Count) return null;

        //TODO: Refactoring
        if (Head == null) return null;

        if (index == 0) return Head;
        //

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

    //Test
    public void Show()
    {
        var item = Head;
        for (int i = 0; i < Count; i++)
        {
            Debug.Log($"Index = { i }, value = { item?.Data }");

            if (item.Next == null)
                break;

            item = item.Next;
        }
    }

    public void Serialize(FileStream s) { }

    public void Deserialize(FileStream s) { }
}
