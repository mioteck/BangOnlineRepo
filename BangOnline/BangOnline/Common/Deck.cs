﻿using System;
using System.Collections.Generic;

namespace BangOnline.Common
{
    public class Deck<T> : List<T>
    {
        /// <summary>
        /// Shuffle the list
        /// </summary>
        public void Shuffle()
        {
            int length = this.Count;

            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    int index = j;

                    while (j == index)
                    {
                        Random rand = new Random();
                        index = rand.Next(0, length);
                    }

                    this.Switch(j, index);
                }
            }
        }

        /// <summary>
        /// Switch 2 elements
        /// </summary>
        /// <param name="i1">The first index</param>
        /// <param name="i2">The second index</param>
        public void Switch(int i1, int i2)
        {
            T tmp = this[i1];
            this[i1] = this[i2];
            this[i2] = tmp;
        }

        public T PopFirstElement()
        {
            T element = this[0];
            this.RemoveAt(0);
            return element;
        }

        public Deck<T> PopFirstElement(int size)
        {
            Deck<T> elements = new Deck<T>();

            for(int i = 0; i < size; i++)
            {
                elements.Add(this[i]);
            }

            RemoveRange(0, size);

            return elements;
        }

        public T GetRandomElement(bool deleteElement = false)
        {
            Random rand = new Random();

            int index = rand.Next(0, Count-1);
            T toReturn = this[index];
            if (deleteElement)
            {
                Remove(toReturn);
            }
            return toReturn;
        }

        public T GetRandomElement(Random rand, bool deleteElement = false)
        {
            int index = rand.Next(0, Count-1);
            T toReturn = this[index];
            if (deleteElement)
            {
                Remove(toReturn);
            }
            return toReturn;
        }
    }
}