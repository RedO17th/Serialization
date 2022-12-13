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
