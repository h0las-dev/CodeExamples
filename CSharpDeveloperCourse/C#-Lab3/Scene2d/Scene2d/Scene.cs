namespace Scene2d
{
    using System;
    using System.Collections.Generic;
    using Exceptions;
    using Figures;

    public class Scene
    {
        private Dictionary<string, IFigure> figures =
            new Dictionary<string, IFigure>();

        private Dictionary<string, ICompositeFigure> compositeFigures =
            new Dictionary<string, ICompositeFigure>();

        public void AddFigure(string name, IFigure figure)
        {
            if (this.compositeFigures.ContainsKey(name) || this.figures.ContainsKey(name))
            {
                throw new NameDoesAlreadyExistExeption($"name {name} does already exist");
            }

            this.figures.Add(name, figure);
            Console.WriteLine("Figure {0} successfully added!", name);
        }

        public void CreateCompositeFigure(string name, List<string> childFigures)
        {
            if (this.figures.ContainsKey(name) || this.compositeFigures.ContainsKey(name))
            {
                throw new NameDoesAlreadyExistExeption($"name {name} does already exist");
            }
            else
            {
                var newCompositeFigure = new CompositeFigure();

                foreach (var figureName in childFigures)
                {
                    if (!this.figures.ContainsKey(figureName) && !this.compositeFigures.ContainsKey(figureName))
                    {
                        throw new BadNameExeption($"bad {name} name"); 
                    }

                    if (this.figures.ContainsKey(figureName))
                    {
                        IFigure figure;
                        this.figures.TryGetValue(figureName, out figure);
                        newCompositeFigure.AddFigure(figure);
                        this.figures.Remove(figureName);
                    }
                    else if (this.compositeFigures.ContainsKey(figureName))
                    {
                        IList<IFigure> tmpChildFigures = null;
                        ICompositeFigure tmpCompositeFigure;

                        this.compositeFigures.TryGetValue(figureName, out tmpCompositeFigure);
                        if (tmpCompositeFigure != null)
                        {
                            tmpChildFigures = tmpCompositeFigure.ChildFigures;
                        }

                        if (tmpChildFigures != null)
                        {
                            foreach (var figure in tmpChildFigures)
                            {
                                newCompositeFigure.AddFigure(figure);
                            }
                        }

                        this.compositeFigures.Remove(figureName);
                    }
                }

                this.compositeFigures.Add(name, newCompositeFigure);

                Console.WriteLine("Composite Figure {0} successfully added!", name);
            }
        }

        public void DeleteFigure(string name)
        {
            if (this.figures.ContainsKey(name))
            {
                this.figures.Remove(name);
            }
            else if (this.compositeFigures.ContainsKey(name))
            {
                this.compositeFigures.Remove(name);
            }
            else
            {
                throw new BadNameExeption($"bad {name} name");
            }

            Console.WriteLine("Figure {0} successfully deleted!", name);
        }

        public void DeleteScene()
        {
            this.figures.Clear();
            this.compositeFigures.Clear();

            Console.WriteLine("Scene successfully deleted!");
        }

        public void PrintSceneArea()
        {
            var s = this.CalculateSceneArea();

            Console.WriteLine("Area for {0} = {1}", "scene", s);
        }

        public double CalculateSceneArea()
        {
            var s = 0.0;

            foreach (var figure in this.figures)
            {
                s += figure.Value.CalulateArea();
            }

            foreach (var compositeFigure in this.compositeFigures)
            {
                var childFigures = new List<IFigure>(compositeFigure.Value.ChildFigures);

                foreach (var figure in childFigures)
                {
                    s += figure.CalulateArea();
                }
            }

            return s;
        }

        public void PrintFigureArea(string name)
        {
            var s = this.CalculateFigureArea(name);

            Console.WriteLine("Area for {0} = {1}", name, s);
        }

        public double CalculateFigureArea(string name)
        {
            if (this.figures.ContainsKey(name))
            {
                IFigure figure;
                this.figures.TryGetValue(name, out figure);

                if (figure != null)
                {
                    return figure.CalulateArea();
                }
            }

            if (this.compositeFigures.ContainsKey(name))
            {
                ICompositeFigure tmpCompositeFigure;
                this.compositeFigures.TryGetValue(name, out tmpCompositeFigure);

                if (tmpCompositeFigure != null)
                {
                    return tmpCompositeFigure.CalulateArea();
                }
            }
            else
            {
                throw new BadNameExeption($"bad {name} name");
            }

            return 0;
        }

        public void PrintCircumscribingSceneRectangle()
        {
            var circRectangle = this.CalculateCircumscribingSceneRectangle();

            Console.WriteLine("Circumscribing Rectangle for scene: ({0}, {1}) ({2}, {3})", circRectangle.Vertex1.X, circRectangle.Vertex1.Y, circRectangle.Vertex2.X, circRectangle.Vertex2.Y);
        }

        public Rectangle CalculateCircumscribingSceneRectangle()
        {
            var sceneCompositeFigure = new CompositeFigure();

            if (this.figures.Count == 0 && this.compositeFigures.Count == 0)
            {
                throw new BadFormatException("Scene is empty");
            }

            foreach (var figure in this.figures)
            {
                sceneCompositeFigure.AddFigure(figure.Value);
            }

            foreach (var figure in this.compositeFigures)
            {
                var childFugures = figure.Value.ChildFigures;
                foreach (var childFigure in childFugures)
                {
                    sceneCompositeFigure.AddFigure(childFigure);
                }
            }

            return sceneCompositeFigure.CalculateCircumscribingRectangle();
        }

        public void PrintCircumscribingRectangle(string name)
        {
            var circRectangle = this.CalculateCircumscribingRectangle(name);

            Console.WriteLine("Circumscribing Rectangle for {0}: ({1}, {2}) ({3}, {4})", name, circRectangle.Vertex1.X, circRectangle.Vertex1.Y, circRectangle.Vertex2.X, circRectangle.Vertex2.Y);
        }

        public Rectangle CalculateCircumscribingRectangle(string name)
        {
            var circRectangle = new Rectangle();

            if (this.figures.ContainsKey(name))
            {
                IFigure figure = null;

                this.figures.TryGetValue(name, out figure);
                if (figure != null)
                {
                    circRectangle = figure.CalculateCircumscribingRectangle();
                }
            }
            else if (this.compositeFigures.ContainsKey(name))
            {
                ICompositeFigure tmpCompositeFigure;
                this.compositeFigures.TryGetValue(name, out tmpCompositeFigure);

                if (tmpCompositeFigure != null)
                {
                    circRectangle = tmpCompositeFigure.CalculateCircumscribingRectangle();
                }
            }
            else
            {
                throw new BadNameExeption($"bad {name} name");
            }

            return circRectangle;
        }

        public void Move(string name, Point vector)
        {
            if (this.figures.ContainsKey(name))
            {
                IFigure figure;

                this.figures.TryGetValue(name, out figure);
                figure?.Move(vector);
            }
            else if (this.compositeFigures.ContainsKey(name))
            {
                ICompositeFigure tmpCompositeFigure;

                this.compositeFigures.TryGetValue(name, out tmpCompositeFigure);
                tmpCompositeFigure?.Move(vector);
            }
            else
            {
                throw new BadNameExeption($"bad {name} name");
            }

            Console.WriteLine("Figure {0} successfully moved at ({1}, {2})!", name, vector.X, vector.Y);
        }

        public void MoveScene(Point vector)
        {
            foreach (var figure in this.figures)
            {
                figure.Value.Move(vector);
            }

            foreach (var figure in this.compositeFigures)
            {
                figure.Value.Move(vector);
            }

            Console.WriteLine("Scene successfully moved at ({0}, {1})!", vector.X, vector.Y);
        }

        public void RotateFigure(string name, double angle)
        {
            if (this.figures.ContainsKey(name))
            {
                IFigure figure;

                this.figures.TryGetValue(name, out figure);
                figure?.Rotate(angle);
            }
            else if (this.compositeFigures.ContainsKey(name))
            {
                ICompositeFigure tmpCompositeFigure;

                this.compositeFigures.TryGetValue(name, out tmpCompositeFigure);
                tmpCompositeFigure?.Rotate(angle);
            }
            else
            {
                throw new BadNameExeption($"bad {name} name");
            }

            Console.WriteLine("Figure {0} successfully rotated on {1}", name, angle);
        }

        public void RotateScene(double angle)
        {
            foreach (var figure in this.figures)
            {
                figure.Value.Rotate(angle);
            }

            foreach (var figure in this.compositeFigures)
            {
                figure.Value.Rotate(angle);
            }

            Console.WriteLine("Scene successfully rotated on {0}", angle);
        }

        public void CopyFigure(string name, string copyName)
        {
            if (this.figures.ContainsKey(name) && !this.figures.ContainsKey(copyName))
            {
                IFigure copyFigure;

                this.figures.TryGetValue(name, out copyFigure);
                if (copyFigure != null)
                {
                    this.figures.Add(copyName, (IFigure)copyFigure.Clone());
                }
            }
            else if (this.compositeFigures.ContainsKey(name) && !this.compositeFigures.ContainsKey(copyName))
            {
                ICompositeFigure copyFigure;

                this.compositeFigures.TryGetValue(name, out copyFigure);
                if (copyFigure != null)
                {
                    this.compositeFigures.Add(copyName, (ICompositeFigure)copyFigure.Clone());
                }
            }
            else
            {
                throw new BadNameExeption($"bad {name} or {copyName} name");
            }

            Console.WriteLine("Figure {0} successfully copy in {1}", name, copyName);
        }

        public void CopyScene(string copyName)
        {
            if (!this.compositeFigures.ContainsKey(copyName))
            {
                var copyScene = new CompositeFigure();

                foreach (var figure in this.figures)
                {
                    copyScene.AddFigure(figure.Value);
                }

                foreach (var figure in this.compositeFigures)
                {
                    var childFigures = new List<IFigure>(figure.Value.ChildFigures);

                    foreach (var childFigure in childFigures)
                    {
                        copyScene.AddFigure(childFigure);
                    }
                }

                this.compositeFigures.Add(copyName, copyScene);

                Console.WriteLine("Scene successfully copy in {0}", copyName);
            }
            else
            {
                throw new BadNameExeption($"bad {copyName} name");
            }
        }

        public void ReflectFigure(string name, string direction)
        {
            if (this.figures.ContainsKey(name))
            {
                IFigure figure;

                this.figures.TryGetValue(name, out figure);

                if (direction == "vertically")
                {
                    figure?.Reflect(direction);
                }
                else if (direction == "horizontally")
                {
                    figure?.Reflect(direction);
                }
                else
                {
                    throw new BadFormatException("bad rotate direction");
                }
            }
            else if (this.compositeFigures.ContainsKey(name))
            {
                ICompositeFigure tmpCompositeFigure;

                this.compositeFigures.TryGetValue(name, out tmpCompositeFigure);

                if (direction == "vertically")
                {
                    tmpCompositeFigure?.Reflect(direction);
                }
                else if (direction == "horizontally")
                {
                    tmpCompositeFigure?.Reflect(direction);
                }
                else
                {
                    throw new BadFormatException("bad rotate direction");
                }
            }
            else
            {
                throw new BadNameExeption($"bad {name} name");
            }

            Console.WriteLine("Figure {0} successfully reflect from {1}", name, direction);
        }

        public void ReflectScene(string direction)
        {
            foreach (var figure in this.figures)
            {
                figure.Value.Reflect(direction);
            }

            foreach (var figure in this.compositeFigures)
            {
                figure.Value.Reflect(direction);
            }

            Console.WriteLine("Scene successfully reflect from {0}", direction);
        }
    }
}