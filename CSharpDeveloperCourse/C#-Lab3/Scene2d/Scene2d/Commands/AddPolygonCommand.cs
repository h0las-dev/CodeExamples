namespace Scene2d.Commands
{
    using System.Collections.Generic;
    using Figures;

    public class AddPolygonCommand : ICommand
    {
        public const double Eps = 1E-9;
        private PolygonFigure polygon;
        
        public AddPolygonCommand(string name, List<Point> points)
        {
            this.polygon = new PolygonFigure(name, points);

            this.polygon.CheckPolygon();
        }

        public void Apply(Scene scene)
        {
            scene.AddFigure(this.polygon.Name, this.polygon);
        }
    }
}