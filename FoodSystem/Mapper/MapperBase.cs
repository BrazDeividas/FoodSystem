namespace Mapper 
{
    public abstract class MapperBase<TFirst, TSecond>
    {
        public abstract TFirst Map(TSecond second);
        public abstract TSecond Map(TFirst first);

        public List<TFirst> Map(List<TSecond> seconds, Action<TFirst>? callback = null)
        {
            if(callback != null)
            {
                return seconds.Select(second =>
                {
                    var first = Map(second);
                    callback(first);
                    return first;
                }).ToList();
            }
            else
            {
                return seconds.Select(Map).ToList();
            }
        }

        public List<TSecond> Map(List<TFirst> firsts, Action<TSecond>? callback = null)
        {
            if(callback != null)
            {
                return firsts.Select(first =>
                {
                    var second = Map(first);
                    callback(second);
                    return second;
                }).ToList();
            }
            else
            {
                return firsts.Select(Map).ToList();
            }
        }
    }
}