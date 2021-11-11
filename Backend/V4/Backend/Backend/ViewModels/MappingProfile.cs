using System.Linq;
using AutoMapper;
using Backend.Controllers;
using Backend.Models;

namespace Backend.ViewModels
{
    public class MappingProfile : Profile {
        public MappingProfile()
        {
            CreateMap<Player, PlayerViewModel>();
            CreateMap<PlayerCreateViewModel, Player>();
            CreateMap<PlayerUpdateViewModel, Player>();

            CreateMap<Game, GameViewModel>()
                .ForMember(x => x.Complexity,
                    act =>
                        act.MapFrom(g => g.Deck.Complexity))
                .ForMember(x => x.PlayerName,
                    act =>
                        act.MapFrom(g => g.Player.Name))
                .ForMember(x => x.CardsOnTable, act => 
                    act.MapFrom(g => g.CardsOnTable.OrderBy(w => w.Order).Select(w => w.Card)));
        }
    }
}