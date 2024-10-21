using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Win32;
using Paintly.Enums;

namespace Paintly;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private SolidColorBrush _currentBrush = new SolidColorBrush(Colors.Black);
    private Point _startPoint;
    private Shape _currentShape;
    private Point _originalSize;
    private Ellipse _lastResizeHadler;

    
    private double _drawThickness = 1;
    private Tools _currentTool = Tools.Pencil;
    private BrushType _brushType = BrushType.Classic;
    
    // flags
    private bool _isDrawing = false;
    private bool _isResizing = false;
    private bool _isResizingHandle = false;

    public MainWindow() => 
        InitializeComponent();
    
    // Tool selection
    #region Tools
 
    private void SetToolsColor(Button button)
    {
        var сolor = Brushes.DarkGray;
        
        PencilButton.BorderBrush = сolor;
        BrushButton.BorderBrush = сolor;
        RectangleButton.BorderBrush = сolor;
        CircleButton.BorderBrush = сolor;
        EraserButton.BorderBrush = сolor;

        button.BorderBrush = Brushes.Cyan; // highlight current
    }
    
    private void SetTool(Tools tool, Button button)
    {
        _currentTool = tool;
        SetToolsColor(button);
    }
    
    private void PencilTool_Click(object sender, RoutedEventArgs e) 
        => SetTool(Tools.Pencil, PencilButton);

    private void BrushTool_Click(object sender, RoutedEventArgs e) 
        => SetTool(Tools.Brush, BrushButton);

    private void RectangleTool_Click(object sender, RoutedEventArgs e) 
        => SetTool(Tools.Rectangle, RectangleButton);

    private void CircleTool_Click(object sender, RoutedEventArgs e) 
        => SetTool(Tools.Circle, CircleButton);

    private void EraserTool_Click(object sender, RoutedEventArgs e) 
        => SetTool(Tools.Eraser, EraserButton);

    #endregion
    
    // Color selection
    #region Colors

    private void BlackColor_Click(object sender, RoutedEventArgs e) => 
        _currentBrush = new SolidColorBrush(Colors.Black);

    private void WhiteColor_Click(object sender, RoutedEventArgs e) => 
        _currentBrush = new SolidColorBrush(Colors.White);

    private void RedColor_Click(object sender, RoutedEventArgs e) => 
        _currentBrush = new SolidColorBrush(Colors.Red);

    private void GreenColor_Click(object sender, RoutedEventArgs e) => 
        _currentBrush = new SolidColorBrush(Colors.Green);

    private void BlueColor_Click(object sender, RoutedEventArgs e) => 
        _currentBrush = new SolidColorBrush(Colors.Blue);

    private void ColorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
    {
        if (e.NewValue.HasValue)
        {
            var color = e.NewValue.Value;
            _currentBrush = new SolidColorBrush(color);
        }
    }

    #endregion

    // Drawing settings
    #region DrawingToolsSettings

    // Thickness selection
    private void ThicknessSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) => 
        _drawThickness = e.NewValue;

    // Brush type selection
    private void BrushTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        _currentTool = Tools.Brush;
        
        if (BrushTypeComboBox.SelectedItem is ComboBoxItem selectedItem && selectedItem.Tag is BrushType brushType)
            _brushType = brushType;
    }

    #endregion
    
    // Drawing logic
    #region Drawing

    // Start Drawing
    private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
        {
            if (_isResizing)
                _isResizingHandle = true; 
            
            else
            {
                drawingCanvas.Children.Remove(_lastResizeHadler);
                _isDrawing = true;
                _startPoint = e.GetPosition(drawingCanvas);

                if (_currentTool == Tools.Rectangle || _currentTool == Tools.Circle)
                {
                    if (_currentTool == Tools.Rectangle)
                        _currentShape = new Rectangle { Stroke = _currentBrush, StrokeThickness = _drawThickness};
                    
                    else if (_currentTool == Tools.Circle)
                        _currentShape = new Ellipse { Stroke = _currentBrush, StrokeThickness = _drawThickness };

                    drawingCanvas.Children.Add(_currentShape);
                    Canvas.SetLeft(_currentShape, _startPoint.X);
                    Canvas.SetTop(_currentShape, _startPoint.Y);
                }
            }
        }
    }

    // Drawing
    private void Canvas_MouseMove(object sender, MouseEventArgs e)
    {
        Point currentPoint = e.GetPosition(drawingCanvas);

        if (_isDrawing && !_isResizingHandle) 
        {
            if (_currentTool == Tools.Pencil || _currentTool == Tools.Brush || _currentTool == Tools.Eraser)
            {
                if (_currentTool == Tools.Eraser)
                    Erase(currentPoint);
                else if (_currentTool == Tools.Brush)
                    DrawBrush(currentPoint);
                else if (_currentTool == Tools.Pencil) 
                    DrawPencil(currentPoint);
                _startPoint = currentPoint;
            }
            else if (_currentTool == Tools.Rectangle || _currentTool == Tools.Circle) 
                SetShapeSizeAndPosition(_currentShape, _startPoint, currentPoint);
        }
    }

    // Stop Drawing
    private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
    {
        _isDrawing = false;
        _isResizingHandle = false;
        
        if (_currentTool == Tools.Rectangle || _currentTool == Tools.Circle) 
            AddResizeHandles();
    }

    // Erase tool
    private void Erase(Point currentPoint)
    {
        var line = new Line
        {
            X1 = _startPoint.X,
            Y1 = _startPoint.Y,
            X2 = currentPoint.X,
            Y2 = currentPoint.Y,
            Stroke = Brushes.White,
            StrokeThickness = _drawThickness
        };
        drawingCanvas.Children.Add(line);
    }

    // Pencil tool
    private void DrawPencil(Point currentPoint)
    {
        var line = new Line
        {
            X1 = _startPoint.X,
            Y1 = _startPoint.Y,
            X2 = currentPoint.X,
            Y2 = currentPoint.Y,
            Stroke = _currentBrush,
            StrokeThickness = _drawThickness
        };
        drawingCanvas.Children.Add(line);
    }

    // Brush tool
    private void DrawBrush(Point currentPoint)
    {
        if (_brushType == BrushType.Spray)
            DrawSpray(currentPoint);
        else
            DrawLine(currentPoint, _drawThickness * 2);
    }
    
    // Spray brush
    private void DrawSpray(Point currentPoint)
    {
        Random random = new Random();
        int numberOfDots = 2 * (int)_drawThickness;

        for (int i = 0; i < numberOfDots; i++)
        {
            double offsetX = random.NextDouble() * _drawThickness;
            double offsetY = random.NextDouble() * _drawThickness;

            var dot = new Ellipse
            {
                Fill = _currentBrush,
                Width = 2,
                Height = 2
            };

            Canvas.SetLeft(dot, currentPoint.X + offsetX);
            Canvas.SetTop(dot, currentPoint.Y + offsetY);
            drawingCanvas.Children.Add(dot);
        }
    }
    
    // Classic brush
    private void DrawLine(Point currentPoint, double thickness)
    {
        int numberOfCircles = 2;

        for (int i = 0; i < numberOfCircles; i++)
        {
            var random = new Random();
            double offsetX = random.NextDouble() * _drawThickness - _drawThickness / 2;
            double offsetY = random.NextDouble() * _drawThickness - _drawThickness / 2;

            var circle = new Ellipse
            {
                Fill = _currentBrush,
                Width = _drawThickness,
                Height = _drawThickness
            };

            Canvas.SetLeft(circle, currentPoint.X + offsetX);
            Canvas.SetTop(circle, currentPoint.Y + offsetY);

            drawingCanvas.Children.Add(circle);
        }
    }
    
    // Shape drawing
    private void SetShapeSizeAndPosition(Shape shape, Point startPoint, Point currentPoint)
    {
        double width = Math.Abs(currentPoint.X - startPoint.X);
        double height = Math.Abs(currentPoint.Y - startPoint.Y);
    
        shape.Width = width;
        shape.Height = height;
    
        Canvas.SetLeft(shape, Math.Min(startPoint.X, currentPoint.X));
        Canvas.SetTop(shape, Math.Min(startPoint.Y, currentPoint.Y));
    }

    // Add Resize Handles
    private void AddResizeHandles()
    {
        if (_currentShape != null)
        {
            var resizeHandle = CreateResizeHandle();
            
            _lastResizeHadler = resizeHandle;
            PositionResizeHandle(resizeHandle, _currentShape);
            
            drawingCanvas.Children.Add(resizeHandle);
            
            AttachResizeEvents(resizeHandle);
        }
    }
    
    // Create resize ellipse
    private Ellipse CreateResizeHandle()
    {
        return new Ellipse
        {
            Width = 10,
            Height = 10,
            Fill = Brushes.Cyan,
            Cursor = Cursors.SizeAll
        };
    }

    // Set handler position
    private void PositionResizeHandle(Ellipse handle, Shape shape)
    {
        Canvas.SetLeft(handle, Canvas.GetLeft(shape) + shape.Width - 5);
        Canvas.SetTop(handle, Canvas.GetTop(shape) + shape.Height - 5);
    }
    
    private void AttachResizeEvents(Ellipse handle)
    {
        handle.MouseDown += (s, e) =>
        {
            _isResizing = true;
            _originalSize = e.GetPosition(drawingCanvas);
            handle.CaptureMouse();
        };

        handle.MouseMove += (s, e) =>
        {
            if (_isResizing) 
                ResizeCurrentShape(e.GetPosition(drawingCanvas));
        };

        handle.MouseUp += (s, e) =>
        {
            _isResizing = false;
            handle.ReleaseMouseCapture();
            drawingCanvas.Children.Remove(handle);
        };
    }
    
    private void ResizeCurrentShape(Point newPoint)
    {
        double deltaX = newPoint.X - _originalSize.X;
        double deltaY = newPoint.Y - _originalSize.Y;

        if (_currentShape.Width + deltaX > 0 && _currentShape.Height + deltaY > 0)
        {
            _currentShape.Width += deltaX;
            _currentShape.Height += deltaY;

            _originalSize = newPoint;
        }
    }
    
    #endregion

    #region Save

    // Save image
    private void Save_Click(object sender, RoutedEventArgs e)
    {
        var saveDialog = new SaveFileDialog
        {
            Filter = "Bitmap Image|*.bmp",
            FileName = "drawing.bmp"
        };
        if (saveDialog.ShowDialog() == true) 
            SaveCanvasToBmp(saveDialog.FileName);
    }

    // SaveToBmp
    private void SaveCanvasToBmp(string filePath)
    {
        var rect = new Rect(drawingCanvas.RenderSize);
        var renderTarget = new RenderTargetBitmap(
            (int)rect.Width, (int)rect.Height, 96d, 96d, PixelFormats.Pbgra32);

        drawingCanvas.Measure(rect.Size);
        drawingCanvas.Arrange(rect);

        renderTarget.Render(drawingCanvas);

        var encoder = new BmpBitmapEncoder();
        encoder.Frames.Add(BitmapFrame.Create(renderTarget));

        using (var fileStream = System.IO.File.Create(filePath))
        {
            encoder.Save(fileStream);
        }
    }

    #endregion

    
}