using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TETRIS
{
    //Step 3
    static class ShapesHandler
    {
        private static Shape[] shapesArray;

        // Static constructor : No need to manually initialize

        static ShapesHandler()
        {
            // Create shapes add into the array
            shapesArray = new Shape[]
            {
                new Shape
                {
                    Widht = 2,
                    Height = 2,
                    Dots = new int[,]
                    {
                        { 1, 1 },
                        { 1, 1 }
                    }
                },
                new Shape
                {
                    Widht = 1,
                    Height = 4,
                    Dots = new int[,]
                    {
                        { 1 },
                        { 1 },
                        { 1 },
                        { 1 }
                    }
                },
                new Shape
                {
                    Widht = 3,
                    Height = 2,
                    Dots = new int[,]
                    {
                        { 0, 0, 1 },
                        { 1, 1, 1 }
                    }
                },
                new Shape
                {
                    Widht = 3,
                    Height = 2,
                    Dots = new int[,]
                  {
                      { 1, 0, 0 },
                      { 1, 1, 1 }
                  }
                },
                new Shape
                {
                    Widht = 3,
                    Height = 2,
                    Dots = new int[,]
               {
                   { 1, 1, 0 },
                   { 0, 1, 1 }
               }
                },
                new Shape
                {
                    Widht = 3,
                    Height = 2,
                    Dots = new int[,]
                {
                    { 0, 1, 1 },
                    { 1, 1, 0 }
                }
                }
                // New shapes can be added here
            };
        }
        // Get a shape from the array in a random basis
        public static Shape GetRandomShape()
        {
            var shape = shapesArray[new Random().Next(shapesArray.Length)];
            return shape;
        }
    }
}
