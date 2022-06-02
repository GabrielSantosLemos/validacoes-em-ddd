namespace WebAPI._Core
{
    public static class CollectionExtension
    {
        public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> source, bool iniciaEmZero)
        {
            return source.Select((item, index) =>
            {
                if (iniciaEmZero)
                    index++;

                return (item, index);
            });
        }
    }
}
