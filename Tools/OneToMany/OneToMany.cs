using System.Collections.Generic;
using System.Diagnostics;

namespace Tools.OneToMany
{
    public class OneToMany : IOneToMany
    {
        /// <summary>
        /// Contains, for a given a index i, the set of all indices that can be reached from i.
        /// </summary>
        private readonly Dictionary<int, HashSet<int>> _dictionary = new Dictionary<int, HashSet<int>>();

        /// <summary>
        /// Contains, for a given index j, the number of links leading to j.
        /// </summary>
        private readonly Dictionary<int, int> _nbOfLinkGoingTo = new Dictionary<int, int>();

        public int HowManyToOne(int index)
        {
            int nb;
            return _nbOfLinkGoingTo.TryGetValue(index, out nb) ? nb : 0;
        }

        private void IncrementLinkNbGoingTo(int index)
        {
            _nbOfLinkGoingTo[index] = HowManyToOne(index) + 1;
        }

        private void DecrementLinkNbGoingTo(int index)
        {
            _nbOfLinkGoingTo[index] = HowManyToOne(index) - 1;
            Debug.Assert(_nbOfLinkGoingTo[index] >= 0);
        }

        private OneToMany()
        {
        }

        public static IOneToMany Create()
        {
            return new OneToMany();
        }

        public bool AddOneToOne(int index1, int index2)
        {
            HashSet<int> set;
            if (!_dictionary.TryGetValue(index1, out set))
            {
                set = new HashSet<int>();
                _dictionary[index1] = set;
            }

            var result = set.Add(index2);

            if(result)
                IncrementLinkNbGoingTo(index2);

            return result;
        }

        public bool AlreadyExists(int index1, int index2)
        {
            HashSet<int> set;
            return _dictionary.TryGetValue(index1, out set) && set.Contains(index2);
        }

        public bool RemoveOneToOne(int index1, int index2)
        {
            HashSet<int> set;
            var result = _dictionary.TryGetValue(index1, out set) && set.Remove(index2);

            if(result)
                DecrementLinkNbGoingTo(index2);

            return result;
        }

        public IEnumerable<int> GetManyFromOne(int origin)
        {
            HashSet<int> result;
            return _dictionary.TryGetValue(origin, out result) ? result : emptyList;
        }

        public int HowManyFromOne(int origin)
        {
            HashSet<int> result;
            return _dictionary.TryGetValue(origin, out result) ? result.Count : 0;
        }

        private static readonly IEnumerable<int> emptyList = new int[0];

        public IEnumerable<int> Origins
        {
            get { return _dictionary.Keys; }
        }

        public bool RemoveOne(int origin)
        {
            return _dictionary.Remove(origin);
        }
    }
}
