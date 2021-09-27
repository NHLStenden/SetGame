using System;
using Backend.Models;
using Backend.Services;
using FluentAssertions;
using Xunit;

namespace TestBackend
{
    public class SetServiceTests
    {
        [Fact]
        public void CheckSet_IncorrectNumberOfCards_ThrowsArgumentException()
        {
            var sut = new SetService();

            sut.Invoking(x => x.Check(new[]
            {
                new Card() {Color = Color.Red, Fill = Fill.Hollow, Shape = Shape.Diamond, NrOfShapes = 1},
                new Card() {Color = Color.Red, Fill = Fill.Hollow, Shape = Shape.Diamond, NrOfShapes = 2},
            })).Should().Throw<ArgumentException>();
        }
        
        [Fact]
        public void CheckSet_CorrectSetDifferentNrOfShapes_ValidSet()
        {
            var sut = new SetService();

            SetResult result =  sut.Check(new[]
            {
                new Card() {Color = Color.Red, Fill = Fill.Hollow, Shape = Shape.Diamond, NrOfShapes = 1},
                new Card() {Color = Color.Red, Fill = Fill.Hollow, Shape = Shape.Diamond, NrOfShapes = 2},
                new Card() {Color = Color.Red, Fill = Fill.Hollow, Shape = Shape.Diamond, NrOfShapes = 3},
            });

            var expected = new SetResult()
            {
                ColorDifferent = false, ColorSame = true, ColorsCorrect = true,
                FillDifferent = false, FillSame = true, FillCorrect = true,
                ShapeDifferent = false, ShapeSame = true, ShapeCorrect = true,

                NrOfShapeDifferent = true, NrOfShapeSame = false, NrOfShapeCorrect = true,

                CorrectSet = true
            };

            result.Should().BeEquivalentTo(expected);
        }
        
        [Fact]
        public void CheckSet_CorrectSetDifferentShapes_ValidSet()
        {
            var sut = new SetService();

            SetResult result =  sut.Check(new[]
            {
                new Card() {Color = Color.Red, Fill = Fill.Hollow, Shape = Shape.Diamond, NrOfShapes = 1},
                new Card() {Color = Color.Red, Fill = Fill.Hollow, Shape = Shape.Pill, NrOfShapes = 1},
                new Card() {Color = Color.Red, Fill = Fill.Hollow, Shape = Shape.Wave, NrOfShapes = 1},
            });

            var expected = new SetResult()
            {
                ColorDifferent = false, ColorSame = true, ColorsCorrect = true,
                FillDifferent = false, FillSame = true, FillCorrect = true,
                NrOfShapeDifferent = false, NrOfShapeSame = true, NrOfShapeCorrect = true,
                
                ShapeDifferent = true, ShapeSame = false, ShapeCorrect = true,
                
                CorrectSet = true
            };

            result.Should().BeEquivalentTo(expected);
        }
        
        [Fact]
        public void CheckSet_CorrectSetDifferentFill_ValidSet()
        {
            var sut = new SetService();

            SetResult result =  sut.Check(new[]
            {
                new Card() {Color = Color.Red, Fill = Fill.Hollow, Shape = Shape.Diamond, NrOfShapes = 1},
                new Card() {Color = Color.Red, Fill = Fill.Solid, Shape = Shape.Diamond, NrOfShapes = 1},
                new Card() {Color = Color.Red, Fill = Fill.Striped, Shape = Shape.Diamond, NrOfShapes = 1},
            });

            var expected = new SetResult()
            {
                ColorDifferent = false, ColorSame = true, ColorsCorrect = true,
                NrOfShapeDifferent = false, NrOfShapeSame = true, NrOfShapeCorrect = true,
                ShapeDifferent = false, ShapeSame = true, ShapeCorrect = true,
                
                FillDifferent = true, FillSame = false, FillCorrect = true,
                
                CorrectSet = true
            };

            result.Should().BeEquivalentTo(expected);
        }
        
        [Fact]
        public void CheckSet_CorrectSetDifferentColor_ValidSet()
        {
            var sut = new SetService();

            SetResult result =  sut.Check(new[]
            {
                new Card() {Color = Color.Red, Fill = Fill.Hollow, Shape = Shape.Diamond, NrOfShapes = 1},
                new Card() {Color = Color.Green, Fill = Fill.Hollow, Shape = Shape.Diamond, NrOfShapes = 1},
                new Card() {Color = Color.Violet, Fill = Fill.Hollow, Shape = Shape.Diamond, NrOfShapes = 1},
            });

            var expected = new SetResult()
            {
                NrOfShapeDifferent = false, NrOfShapeSame = true, NrOfShapeCorrect = true,
                ShapeDifferent = false, ShapeSame = true, ShapeCorrect = true,
                FillDifferent = false, FillSame = true, FillCorrect = true,
                
                ColorDifferent = true, ColorSame = false, ColorsCorrect = true,
                
                CorrectSet = true
            };

            result.Should().BeEquivalentTo(expected);
        }
        
        [Fact]
        public void CheckSet_IncorrectNrOfShapes_InValidSet()
        {
            var sut = new SetService();

            SetResult result =  sut.Check(new[]
            {
                new Card() {Color = Color.Red, Fill = Fill.Hollow, Shape = Shape.Diamond, NrOfShapes = 1},
                new Card() {Color = Color.Green, Fill = Fill.Hollow, Shape = Shape.Diamond, NrOfShapes = 2},
                new Card() {Color = Color.Violet, Fill = Fill.Hollow, Shape = Shape.Diamond, NrOfShapes = 1},
            });

            var expected = new SetResult()
            {
                ShapeDifferent = false, ShapeSame = true, ShapeCorrect = true,
                FillDifferent = false, FillSame = true, FillCorrect = true,
                ColorDifferent = true, ColorSame = false, ColorsCorrect = true,
                
                NrOfShapeDifferent = false, NrOfShapeSame = false, NrOfShapeCorrect = false,
                
                CorrectSet = false
            };

            result.Should().BeEquivalentTo(expected);
        }
    }
}