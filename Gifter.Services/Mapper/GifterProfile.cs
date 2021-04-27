using AutoMapper;
using Gifter.DataAccess.Models;
using Gifter.Services.DTOS.Wish;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;

namespace Gifter.Services.Mapper
{
    public class GifterProfile : Profile
    {
        public GifterProfile()
        {
            CreateMap<JsonPatchDocument<UpdateWishDTO>, JsonPatchDocument<Wish>>();
            CreateMap<Operation<UpdateWishDTO>, Operation<Wish>>();
        }
    }
}
