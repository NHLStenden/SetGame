using System;
using System.Linq;
using Backend.Models;
using MoreLinq.Extensions;

namespace Backend.Services
{
    public interface ISetService
    {
        public SetResult Check(Card[] cards);
    }

    public class SetService : ISetService
    {
        public SetResult Check(Card[] cards)
        {
            if (cards.Length != 3)
                throw new ArgumentException();

            var firstCard = cards.First();
            bool colorSame = cards.All(x => x.Color == firstCard.Color);
            bool shapeSame = cards.All(x => x.Shape == firstCard.Shape);
            bool fillSame = cards.All(x => x.Fill == firstCard.Fill);
            bool nrOfShapeSame = cards.All(x => x.NrOfShapes == firstCard.NrOfShapes);
            
            bool colorDifferent = cards.DistinctBy(x => x.Color).Count() == 3;
            bool shapeDifferent = cards.DistinctBy(x => x.Shape).Count() == 3;
            bool fillDifferent = cards.DistinctBy(x => x.Fill).Count() == 3;
            bool nrOfShapeDifferent = cards.DistinctBy(x => x.NrOfShapes).Count() == 3;
            
            var result = new SetResult()
            {
                ColorsCorrect = colorSame || colorDifferent,
                ColorSame = colorSame,
                ColorDifferent = colorDifferent,
                
                ShapeCorrect = shapeSame || shapeDifferent,
                ShapeSame = shapeSame,
                ShapeDifferent = shapeDifferent,
                
                FillCorrect = fillSame || fillDifferent,
                FillSame = fillSame,
                FillDifferent = fillDifferent,
                
                NrOfShapeCorrect = nrOfShapeSame || nrOfShapeDifferent,
                NrOfShapeSame = nrOfShapeSame,
                NrOfShapeDifferent = nrOfShapeDifferent
            };
            
            result.CorrectSet = result.ColorsCorrect && result.ShapeCorrect && result.FillCorrect && result.NrOfShapeCorrect;
            
            return result;
        }
    }

    public class SetResult
    {
        public bool CorrectSet { get; set; }
        
        public bool ColorsCorrect { get; set; }
        public bool ShapeCorrect { get; set; }
        public bool FillCorrect { get; set; }
        public bool NrOfShapeCorrect { get; set; }
        
        public bool ColorSame { get; set; }
        public bool ColorDifferent { get; set; }
        public bool ShapeSame { get; set; }
        public bool ShapeDifferent { get; set; }
        public bool FillSame { get; set; }
        public bool FillDifferent { get; set; }
        public bool NrOfShapeSame { get; set; }
        public bool NrOfShapeDifferent { get; set; }
    }
}