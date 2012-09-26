using System.Collections.Generic;

namespace Tools.OneToMany
{
    public interface IOneToMany
    {
        bool AddOneToOne(int index1, int index2);
        bool AlreadyExists(int index1, int index2);
        bool RemoveOneToOne(int index1, int index2);
        IEnumerable<int> GetManyFromOne(int origin);
        int HowManyToOne(int index);
        int HowManyFromOne(int origin);
        IEnumerable<int> Origins { get; }
        bool RemoveOne(int origin);
    }
}