using System;
using System.Collections.Generic;
using System.Linq;
using Backend.Models;
using MoreLinq.Extensions;

namespace Backend.Services
{
    public class SetService : ISetService
    {
        public SetResult Check(IList<Card> cards)
        {
            if (cards.Count != 3)
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

        public List<IList<Card>> FindAllSets(IList<Card> cards)
        {
            var result = new List<IList<Card>>();
            
            var subsets = cards.Subsets(3);
            foreach (var subset in subsets)
            {
                var setResult = Check(subset);
                if (setResult.CorrectSet)
                {
                    result.Add(subset);
                }
            }

            return result;
        }
    }
}