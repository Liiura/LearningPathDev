using AutoMapper;
using LearningPathDev.Models;
using LearningPathDev.Models.DTO;

namespace LearningPathDev.Mapper
{
    public class Mappers : Profile
    {
        public Mappers()
        {
            CreateMap<Product, ProductDTO>().ReverseMap();
        }
    }
}
