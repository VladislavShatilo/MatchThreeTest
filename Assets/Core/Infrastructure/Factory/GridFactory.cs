using System;
using VContainer;

public class GridFactory : IGridFactory
{
    private ITileTypeGenerator generator;

    [Inject]
    private void Construct(ITileTypeGenerator generator)
    {
        this.generator = generator ?? throw new ArgumentNullException(nameof(generator)); 
    }

    public GridModel Create(int width, int height)
    {
        var grid = new GridModel(width, height);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                grid.Set(x, y, new Cell(generator.GetRandom()));
            }
        }

        return grid;
    }
}