using System.Collections.Generic;
using UnityEngine;

namespace BFL
{
    public static class ListExtensions
    {
        /// <summary>
        ///     Returns a list of random numbers. Count is the number of random numbers to generate and max is the upper bound of the random numbers.
        /// </summary>
        /// <param name="count"></param>
        /// <param name="max"></param>
        /// <returns>Random number list</returns>
        public static List<int> GetRandomNumbers(int count, int max)
        {
            List<int> randomNumbers = new();
            for (int i = 0; i < count; i++)
            {
                randomNumbers.Add(Random.Range(0, max));
            }
            return randomNumbers;
        }
        
        /// <summary>
        ///     Returns a list of random numbers. This method take seed as an parameter. Count is the number of random numbers to generate and max is the upper bound of the random numbers.
        /// </summary>
        /// <param name="count"></param>
        /// <param name="max"></param>
        /// <param name="seed"></param>>
        /// <returns>Random number list</returns>
        public static List<int> GetRandomNumbers(int count, int max, int seed)
        {
            Random.InitState(seed);
            List<int> randomNumbers = new();
            for (int i = 0; i < count; i++)
            {
                randomNumbers.Add(Random.Range(0, max));
            }
            return randomNumbers;
        }
        
        /// <summary>
        ///     Returns a list of non-repeating random numbers. Count is the number of random numbers to generate and max is the upper bound of the random numbers.
        /// </summary>
        /// <param name="count"></param>
        /// <param name="max"></param>
        /// <returns>Non-repeating random number list</returns>
        public static List<int> GetNonRepeatingRandomNumbers(int count, int max)
        {
            List<int> randomNumbers = new();
            for (int i = 0; i < count; i++)
            {
                int randomNumber = Random.Range(0, max);
                while (randomNumbers.Contains(randomNumber))
                {
                    randomNumber = Random.Range(0, max);
                }
                randomNumbers.Add(randomNumber);
            }
            return randomNumbers;
        }
        
        /// <summary>
        ///     Returns a list of non-repeating random numbers with exclude integers. Count is the number of random numbers to generate, max is the upper bound of the random numbers, and exclude is a list of numbers to exclude.
        /// </summary>
        /// <param name="count"></param>
        /// <param name="max"></param>
        /// <param name="exclude"></param>
        /// <returns>Non-repeating random number list</returns>
        public static List<int> GetNonRepeatingRandomNumbers(int count, int max, List<int> exclude)
        {
            List<int> randomNumbers = new();
            for (int i = 0; i < count; i++)
            {
                int randomNumber = Random.Range(0, max);
                while (randomNumbers.Contains(randomNumber) || exclude.Contains(randomNumber))
                {
                    randomNumber = Random.Range(0, max);
                }
                randomNumbers.Add(randomNumber);
            }
            return randomNumbers;
        }
        
        /// <summary>
        ///     Returns a list of non-repeating random numbers. This method take seed as an parameter. Count is the number of random numbers to generate and max is the upper bound of the random numbers.
        /// </summary>
        /// <param name="count"></param>
        /// <param name="max"></param>
        /// <param name="seed"></param>
        /// <returns>Non-repeating random number list</returns>
        public static List<int> GetNonRepeatingRandomNumbers(int count, int max, int seed)
        {
            Random.InitState(seed);
            List<int> randomNumbers = new();
            for (int i = 0; i < count; i++)
            {
                int randomNumber = Random.Range(0, max);
                while (randomNumbers.Contains(randomNumber))
                {
                    randomNumber = Random.Range(0, max);
                }
                randomNumbers.Add(randomNumber);
            }
            return randomNumbers;
        }
        
        /// <summary>
        ///     Returns a list of non-repeating random numbers with exclude integers. This method take seed as an parameter. Count is the number of random numbers to generate, max is the upper bound of the random numbers, and exclude is a list of numbers to exclude.
        /// </summary>
        /// <param name="count"></param>
        /// <param name="max"></param>
        /// <param name="exclude"></param>
        /// <returns>Non-repeating random number list</returns>
        public static List<int> GetNonRepeatingRandomNumbers(int count, int max, List<int> exclude, int seed)
        {
            Random.InitState(seed);
            List<int> randomNumbers = new();
            for (int i = 0; i < count; i++)
            {
                int randomNumber = Random.Range(0, max);
                while (randomNumbers.Contains(randomNumber) || exclude.Contains(randomNumber))
                {
                    randomNumber = Random.Range(0, max);
                }
                randomNumbers.Add(randomNumber);
            }
            return randomNumbers;
        }
        
        private static System.Random rng = new System.Random();

        /// <summary>
        ///     Shuffles a list.
        /// </summary>
        /// <param name="list"></param>
        /// <typeparam name="T"></typeparam>
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                var k = rng.Next(n + 1);
                (list[k], list[n]) = (list[n], list[k]);
            }
        }
        
    }
}