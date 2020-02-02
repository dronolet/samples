using AutoMapper;
using System.Collections.Generic;

namespace apitest.Classes
{
    /// <summary>
    /// AutoMapper моделей
    /// </summary>
    public static class MapHelper
    {
        public static M MapObjects<T, M>(T obj)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<T, M>()).CreateMapper();
            return mapper.Map<T, M>(obj);
        }

        public static IEnumerable<M> MapListObjects<T, M>(IEnumerable<T> obj)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<T, M>()).CreateMapper();
            return mapper.Map<IEnumerable<T>, IEnumerable<M>>(obj);
        }
    }
}
