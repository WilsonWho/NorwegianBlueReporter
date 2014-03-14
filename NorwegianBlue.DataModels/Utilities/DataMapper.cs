using AutoMapper;

namespace NorwegianBlue.DataModels.Utilities
{
    public static class DataMapper
    {
        public static TR Map<T, TR>(T obj)
        {
            Mapper.CreateMap<T, TR>();

            return Mapper.Map<TR>(obj);
        }
    }
}