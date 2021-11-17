using System.Linq;
using AutoMapper;
using Backend.Models;

namespace Backend.DTOs
{
    public class MappingProfile : Profile {
        public MappingProfile()
        {
            CreateMap<Player, PlayerDto>();
            CreateMap<PlayerCreateDto, Player>();
            CreateMap<PlayerUpdateDto, Player>();

            CreateMap<PlayerCreateDto, ApiUser>()
                .ForMember(x => x.UserName, act =>
                    act.MapFrom(x => x.Email)
                );

            CreateMap<Game, GameDto>()
                .ForMember(x => x.Complexity,
                    act =>
                        act.MapFrom(g => g.Deck.Complexity))
                .ForMember(x => x.PlayerName,
                    act =>
                        act.MapFrom(g => g.Player.Name))
                .ForMember(x => x.CardsOnTable, act => 
                    act.MapFrom(g => 
                        g.CardsOnTable.OrderBy(w => w.Order)
                            .Select(w => w.Card)));
        }
    }
}