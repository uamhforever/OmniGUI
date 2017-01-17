namespace OmniGui
{
    public abstract class Layout : IChild
    {
        protected Layout()
        {
            Children = new OwnedList<Layout>(this);
        }

        public Color Background { get; set; } = Color.Transparent;
        public object Parent { get; set; }
        public Size RequestedSize { get; set; } = Size.Empty;
        public Size DesiredSize { get; set; }
        protected OwnedList<Layout> Children { get; }
        public Rect Bounds { get; set; }

        public Rect VisualBounds
        {
            get
            {
                if (Parent == null)
                {
                    return Bounds;
                }
                else
                {
                    var parent = (StackPanel)Parent;
                    var offset = parent.VisualBounds.Point.Offset(Bounds.Point);
                    return new Rect(offset, Bounds.Size);
                }
            }
        }

        public Layout AddChild(Layout child)
        {
            Children.Add(child);
            return this;
        }

        public void Measure(Size availableSize)
        {
            var desiredSize = MeasureOverride(availableSize);
            DesiredSize = desiredSize;
        }

        protected abstract Size MeasureOverride(Size availableSize);

        public void Arrange(Size finalSize)
        {
            ArrangeOverride(new Rect(Point.Zero, finalSize));
        }

        public void Arrange(Rect rect)
        {
            ArrangeOverride(rect);
        }

        protected abstract Size ArrangeOverride(Rect rect);
        
        public void Render(IDrawingContext drawingContext)
        {
            drawingContext.DrawRectangle(VisualBounds, Background);
            foreach (var child in Children)
            {
                child.Render(drawingContext);
            }
        }
    }
}