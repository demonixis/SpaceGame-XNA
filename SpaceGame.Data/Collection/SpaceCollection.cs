using System;
using System.Collections.Generic;
using SpaceGame.Data.Description;

namespace SpaceGame.Data.Collection
{
    public class SpaceCollection<T>
    {
        public Dictionary<int, T> Collection;

        public int Count
        {
            get { return Collection.Count; }
        }

        public T this[int index]
        {
            get { return Collection[index]; }
            set { Collection[index] = value; }
        }

        public SpaceCollection()
        {
            Collection = new Dictionary<int, T>();
        }

        public SpaceCollection(int capacity = 0)
        {
            Collection = new Dictionary<int, T>(capacity);
        }

        public void Add(int identifier, T description)
        {
            Collection.Add(identifier, description);
        }

        public T Get(int identifier)
        {
            return Collection[identifier];
        }

        public void Remove(int identifier)
        {
            Collection.Remove(identifier);
        }

        public void Clear()
        {
            Collection.Clear();
        }
    }
}
