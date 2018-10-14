//地图格子的定义类

public class Tile
{
    public int X;
    public int Y;
    public bool CanHold; //是否可以防止塔
    public object Data; //格子所保存的数据

    public Tile(int x, int y)
    {
        this.X = x;
        this.Y = y;
    }

    //重写ToString(虽然没什么可重写的)
    public string ToString()
    {
        return string.Format("[X:{},Y:{},CanHold:{}]", this.X, this.Y, this.CanHold);
    }
}