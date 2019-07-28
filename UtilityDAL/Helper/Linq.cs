using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilityDAL.Helper
{
    public class Linq
    {

        public static IEnumerable<(T left, T right)> FullOuterJoin<T, R>(
            IEnumerable<T> left,
            IEnumerable<T> right,
            Func<T, R> keySelector) => (from l in left
                                        join r in right on keySelector(l) equals keySelector(r) into g
                                        from r in g.DefaultIfEmpty()
                                        where r == null
                                        select (l, r)).Concat(from r in right
                                                              join sc in left on keySelector(r) equals keySelector(sc) into g
                                                              from l in g.DefaultIfEmpty()
                                                              where l == null
                                                              select (l, r));

    }
}
