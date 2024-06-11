using AutoMapper;

namespace Ordering.Application.Common.Mapping;

public interface IMapForm<T> 
{
    void Mapping(Profile profile) =>
        profile.CreateMap(typeof(T), GetType());
}
