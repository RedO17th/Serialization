# Serialization
 Serialization of a doubly linked list
 
Реализуйте функции сериализации и десериализации двусвязного списка.

class ListNode
{

     public ListNode Prev;
     public ListNode Next;
     public ListNode Rand;
     public string Data;
}

class ListRand
{

     public ListNode Head;
     public ListNode Tail;
     public int Count;

     public void Serialize(FileStream s) { }
     public void Deserialize(FileStream s) { }
}

Примечание: сериализация подразумевает сохранение и восстановление полной структуры списка, включая взаимное соотношение его элементов между собой — в том числе ссылок на Rand элементы.
- Алгоритмическая сложность решения должна быть меньше квадратичной.
- Нельзя добавлять новые поля в исходные классы ListNode, ListRand
- Для выполнения задания можно использовать любой общеиспользуемый язык.
- Тест нужно выполнить без использования библиотек/стандартных средств сериализации.

![Безымянный](https://user-images.githubusercontent.com/69148778/210084901-6ca6c51b-2cb5-4015-9587-bb11991bd7e9.jpg)

В репозитории присутствует два решения, одно в ветке Main, другое в ветке feature/sarat/ReWrite
