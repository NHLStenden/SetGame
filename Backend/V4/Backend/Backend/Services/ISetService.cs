using System.Collections.Generic;
using Backend.Models;

namespace Backend.Services
{
    public interface ISetService
    {
        public SetResult Check(IList<Card> cards);

        public List<IList<Card>> FindAllSets(IEnumerable<Card> cards);

        public int CalculateComplexity(IEnumerable<Card> cards);
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