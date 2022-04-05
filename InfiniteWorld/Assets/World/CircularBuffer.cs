using UnityEngine;
public class CircularBuffer<T>
{
  T[] buffer;
  int head;

  public T[] All => buffer;

  public CircularBuffer(int length)
  {
    buffer = new T[length];
    head = 0;
  }

  public void Add(T o)
  {
    buffer[head] = o;
    head = (head+1) % buffer.Length;
  }

  public void Reset() {
      head = 0;
  }

  public T RotateCW()
  {
    var aux = buffer[head];
    head = (head+1) % buffer.Length;
    return aux;
  }

  public T RotateCCW()
  {
    head--;
    if (head < 0) head = buffer.Length - 1;
    return buffer[head];
  }

  public T Get(int idx) {
      return buffer[(idx + head) % buffer.Length];
  }

  public T First() {
      return buffer[head];
  }

  public T Last() {
      return buffer[head > 0 ? head - 1 : buffer.Length - 1];
  }
}