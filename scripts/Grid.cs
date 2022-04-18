using Godot;
using System;
using System.Collections.Generic;

public class Grid : Node2D
{

    #region Properties

    [Export] public int Columns { get; private set; } = 16;
    [Export] public int Rows { get; private set; } = 16;

    [Export] public int CellWidth { get; private set; } = 20;
    [Export] public int CellHeight { get; private set; } = 20;

    [Export] public Color GridColor { get; private set; } = Colors.White;
    [Export] public int LineWidth { get; private set; } = 2;

    #endregion // Properties



    #region Fields

    private readonly List<Line2D> m_lines;

    #endregion // Fields



    #region Constructors

    public Grid ()
    {
        m_lines = new List<Line2D>();
    }

    #endregion // Constructors



    #region Godot methods

    public override void _Ready ()
    {
        CreateGrid();
    }

    #endregion // Godot methods



    #region Public methods

    public void SetGridSize (int columns, int rows)
    {
        ClearGrid();
        Columns = columns;
        Rows = rows;
    }
    public void SetColumns (int columns)
    {
        SetGridSize(columns, Rows);
    }
    public void SetRows (int rows)
    {
        SetGridSize(Columns, rows);
    }

    public void SetGridCellSize (int width, int height)
    {
        ClearGrid();
        CellWidth = width;
        CellHeight = height;
    }
    public void SetCellWidth (int width)
    {
        SetGridCellSize(width, CellHeight);
    }
    public void SetCellHeight (int height)
    {
        SetGridCellSize(CellWidth, height);
    }

    public void SetGridColor (float r, float g, float b, float a)
    {
        GridColor = new Color(r, g, b, a);
        m_lines.ForEach(_line => _line.SelfModulate = GridColor);
    }
    public void SetGridColor (Color color)
    {
        SetGridColor(color.r, color.g, color.b, color.a);
    }
    public void SetGridColorRed (float red)
    {
        SetGridColor(red, GridColor.g, GridColor.b, GridColor.a);
    }
    public void SetGridColorGreen (float green)
    {
        SetGridColor(GridColor.r, green, GridColor.b, GridColor.a);
    }
    public void SetGridColorBlue (float blue)
    {
        SetGridColor(GridColor.r, GridColor.g, blue, GridColor.a);
    }
    public void SetGridColorAlpha (float alpha)
    {
        SetGridColor(GridColor.r, GridColor.g, GridColor.b, alpha);
    }

    public void SetLineWidth (int lineWidth)
    {
        LineWidth = lineWidth;
        m_lines.ForEach(_line => _line.Width = LineWidth);
    }

    public void SetGridVisible (bool visible)
    {
        Visible = visible;
    }
    public void ToggleGridVisible ()
    {
        Visible = !Visible;
    }

    public void CreateGrid ()
    {
        ClearGrid();

        for (int column = 0; column <= Columns; column++)
        {
            AddLine(column * CellWidth, 0, 0, Rows * CellHeight);
        }

        for (int row = 0; row <= Rows; row++)
        {
            AddLine(0, row * CellHeight, Columns * CellWidth, 0);
        }
    }

    #endregion // Public methods



    #region Private methods

    private void ClearGrid ()
    {
        m_lines.ForEach(_line => _line.QueueFree());
        m_lines.Clear();
    }

    private void AddLine (int x1, int y1, int x2, int y2)
    {
        Line2D line = new Line2D();
        AddChild(line);

        line.Position = new Vector2(x1, y1);
        line.DefaultColor = GridColor;
        line.Width = LineWidth;
        line.Points = new Vector2[] { new Vector2(0, 0), new Vector2(x2, y2) };

        m_lines.Add(line);
    }

    #endregion // Private methods

}
