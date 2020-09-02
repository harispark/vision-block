using VisionBlockApplication.ViewModels.Pages;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using VisionBlockApplication.Views.Controls;
using VisionBlockApplication.ViewModels.Controls;
using System.Linq;
using System.Collections.ObjectModel;
using VisionBlockApplication.Models;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Windows.Media.Effects;
using System.Windows.Media;
using VisionBlockApplication.Views.Controls.Blocks;
using VisionBlockApplication.ViewModels.Misc;
using System.Timers;
using System.Threading.Tasks;
using System.Threading;

namespace VisionBlockApplication.Views.Pages
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>


    public partial class MainWindow : Window
    {
        public static bool tracageLigne = false;
        public static bool deplacementBlock = false;
        public static bool dragCanvas = false;
        private System.Windows.Point lastPositionDragCanvas = new System.Windows.Point();
        public static Type blockToCreateType;
        ViewModel_Line selectedLine;

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new ViewModel_MainWindow();
            Uri iconUri = new Uri("pack://application:,,,/resources/images/icons/favicoVB.ico", System.UriKind.Absolute);
            this.Icon = BitmapFrame.Create(iconUri);
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void Canvas2_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            ViewModel_MainWindow vm = this.DataContext as ViewModel_MainWindow;

            if (MainWindow.deplacementBlock)
            {
                foreach (UserControl a in vm.EmptyBlocks)
                {
                    ViewModel_EmptyBlock vmBlock = a.DataContext as ViewModel_EmptyBlock;

                    if (vmBlock.isMoving == true)
                    {
                        List<ViewModel_Line> fromList = new List<ViewModel_Line>();
                        List<ViewModel_Line> toList = new List<ViewModel_Line>();

                        foreach (ViewModel_Line l in vm.Lines)
                        {
                            //remplir une liste avec tout les from qui correspondent à ce point.
                            if (object.ReferenceEquals(l._viewModelFromRef, vmBlock))
                                fromList.Add(l);
                            if (object.ReferenceEquals(l._viewModelToRef, vmBlock))
                                toList.Add(l);
                        }

                        vmBlock.Position = new System.Windows.Point(e.GetPosition(canvas2).X - vmBlock.deltaPositionForDragDrop.X, e.GetPosition(canvas2).Y - vmBlock.deltaPositionForDragDrop.Y);
                        vmBlock.FromPointPosition = new System.Windows.Point(vmBlock.Position.X + a.ActualWidth, vmBlock.Position.Y + a.ActualHeight / 2);
                        vmBlock.ToPointPosition = new System.Windows.Point(vmBlock.Position.X, vmBlock.Position.Y + a.ActualHeight / 2);

                        if (fromList.Count != 0)
                            foreach (ViewModel_Line l in fromList)
                                l.setViewModelFromRef(vmBlock);

                        if (toList.Count != 0)
                            foreach (ViewModel_Line l in toList)
                                l.setViewModelToRef(vmBlock);
                    }
                }
            }
            else
            if (MainWindow.tracageLigne)
            {
                vm.Lines[vm.Lines.Count - 1].To = e.GetPosition(canvas2);

                ViewModel_EmptyBlock vmBlockFrom = vm.Lines[vm.Lines.Count - 1]._viewModelFromRef;

                foreach (UserControl u in vm.EmptyBlocks)
                {
                    ViewModel_EmptyBlock vmBlockTo = u.DataContext as ViewModel_EmptyBlock;

                    if (!Object.ReferenceEquals(vmBlockTo, vmBlockFrom))
                    {
                        if (vmBlockTo.CanBeMapped(vmBlockFrom.ProcessingCategoryEnumOutput))
                        {
                            vmBlockFrom.ShadowColor = System.Windows.Media.Colors.Green;
                            vmBlockTo.ShadowColor = System.Windows.Media.Colors.Green;
                            vmBlockFrom.ShadowOpacity = 0.5;
                            vmBlockTo.ShadowOpacity = 0.5;
                        }
                        else
                        {
                            vmBlockFrom.ShadowColor = System.Windows.Media.Colors.Green;
                            vmBlockTo.ShadowColor = System.Windows.Media.Colors.Red;
                            vmBlockFrom.ShadowOpacity = 0.5;
                            vmBlockTo.ShadowOpacity = 0.5;
                        }
                    }
                }

                foreach (UserControl u in vm.EmptyBlocks)
                {
                    if (e.GetPosition(u).X > 0 && e.GetPosition(u).X < u.ActualWidth && e.GetPosition(u).Y > 0 && e.GetPosition(u).Y < u.ActualHeight)
                    {
                        vmBlockFrom.ShadowOpacity = 1;
                        ((ViewModel_EmptyBlock)u.DataContext).ShadowOpacity = 1;
                    }
                }
            }
            else if (dragCanvas)
            {
                Point deltaCanvasDrag = new Point(e.GetPosition(this).X - lastPositionDragCanvas.X, e.GetPosition(this).Y - lastPositionDragCanvas.Y);
                vm.TranslateX = vm.TranslateX + deltaCanvasDrag.X;
                vm.TranslateY = vm.TranslateY + deltaCanvasDrag.Y;
                this.translateCanvas(deltaCanvasDrag);
                lastPositionDragCanvas = e.GetPosition(this);
            }
            else
            {
                foreach (UserControl u in vm.EmptyBlocks)
                {
                    //test de la présence de block en dessous de la souris
                    if (e.GetPosition(u).X > 0 && e.GetPosition(u).X < u.ActualWidth && e.GetPosition(u).Y > 0 && e.GetPosition(u).Y < u.ActualHeight)
                    {
                        ((ViewModel_EmptyBlock)u.DataContext).ShadowOpacity = 0.5;
                        ((ViewModel_EmptyBlock)u.DataContext).ShadowColor = System.Windows.Media.Colors.Black;
                        ((ViewModel_EmptyBlock)u.DataContext).ActionButtonsVisibility = Visibility.Visible;
                    }
                    else
                    {
                        ((ViewModel_EmptyBlock)u.DataContext).ShadowOpacity = 0;
                        ((ViewModel_EmptyBlock)u.DataContext).ActionButtonsVisibility = Visibility.Collapsed;
                    }
                }
                lastPositionDragCanvas = e.GetPosition(this);
            }
        }

        private void Canvas2_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            dragCanvas = false;
            if (MainWindow.tracageLigne == true)
            {
                MainWindow.tracageLigne = false;

                //Si la ligne est sur un block > association
                //Sinon supression
                ViewModel_MainWindow vm = this.DataContext as ViewModel_MainWindow;
                foreach (UserControl u in vm.EmptyBlocks)
                {
                    ViewModel_EmptyBlock vmBlockTo = (ViewModel_EmptyBlock)u.DataContext;

                    vmBlockTo.UpdateErrorMessage();

                    ViewModel_EmptyBlock vmBlockFrom = vm.Lines[vm.Lines.Count - 1]._viewModelFromRef;

                    if (!Object.ReferenceEquals(vmBlockTo, vmBlockFrom))
                        if (e.GetPosition(u).X > 0 && e.GetPosition(u).X < u.ActualWidth && e.GetPosition(u).Y > 0 && e.GetPosition(u).Y < u.ActualHeight)
                        {

                            if (vmBlockTo.CanBeMapped(vmBlockFrom.ProcessingCategoryEnumOutput))
                            {
                                //ViewModel_EmptyBlock vmFrom = vm.Lines[vm.Lines.Count - 1].;

                                if (vmBlockFrom.enfants == null)
                                    vmBlockFrom.enfants = new List<ViewModel_EmptyBlock>();
                                vmBlockFrom.enfants.Add(vmBlockTo);
                                vmBlockFrom.textEnfants = "";

                                if (vmBlockTo.parents == null)
                                    vmBlockTo.parents = new List<ViewModel_EmptyBlock>();
                                vmBlockTo.parents.Add(vmBlockFrom);
                                vmBlockTo.textParents = "";
                                vmBlockTo.UpdateErrorMessage();
                                vm.Lines[vm.Lines.Count - 1].setViewModelToRef(vmBlockTo);
                                return;
                            }
                        }
                }
                vm.Lines.RemoveAt(vm.Lines.Count - 1);
            }
        }

        private void canvas2_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            var element = sender as UIElement;
            var position = e.GetPosition(element);
            var scale = e.Delta >= 0 ? 1.1 : (1.0 / 1.1); // choose appropriate scaling factor
            zoom(scale);
        }

        private void canvas2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            IInputElement element = this.InputHitTest(e.GetPosition(this));
            if (!(element is Canvas))
                return;

            dragCanvas = true;
            ParentCanvas.Focus();
            (this.DataContext as ViewModel_MainWindow).RemoveSelectedItem();
        }

        public void resetZoom()
        {
            //A Corriger la translation ne revient pas au point d'origine
            //ViewModel_MainWindow vm = (this.DataContext as ViewModel_MainWindow);
            //this.translateCanvas(new Point(-vm.TranslateX, -vm.TranslateY));
            //vm.TranslateX = 0;
            //vm.TranslateY = 0;
            //var transform = canvas2.RenderTransform as MatrixTransform;
            //var matrix = transform.Matrix;
            //transform.Matrix = new Matrix(1, 0, 0, 1, 0, 0);
        }

        private void CanvasDragButtons_MouseMove(object sender, MouseEventArgs e)
        {
            Canvas.SetLeft(button, e.GetPosition(canvasDragButtons).X);
            Canvas.SetTop(button, e.GetPosition(canvasDragButtons).Y);
        }

        private void canvasDragButtons_MouseUp(object sender, MouseButtonEventArgs e)
        {
            button.Visibility = Visibility.Hidden;
            button.ReleaseMouseCapture();

            if (blockToCreateType == null)
                return;

            ViewModel_MainWindow vm = (ViewModel_MainWindow)this.DataContext;
            UserControl newBlock = (UserControl)Activator.CreateInstance(blockToCreateType);
            ViewModel_EmptyBlock vmBlock = (ViewModel_EmptyBlock)newBlock.DataContext;
            Point p = new System.Windows.Point(Mouse.GetPosition(canvas2).X, Mouse.GetPosition(canvas2).Y);
            if (p.X < 0)
                p = new System.Windows.Point(10, 10);
            if (p.Y < 0)
                p = new System.Windows.Point(10, 10);
            vmBlock.Position = p;
            vm.EmptyBlocks.Add(newBlock);
        }

        private void Button_MouseDown(object sender, MouseButtonEventArgs e)
        {

            System.Windows.Point positionButton = Mouse.GetPosition(this.canvasDragButtons);
            Canvas.SetLeft(this.button, positionButton.X);
            Canvas.SetTop(this.button, positionButton.Y);

            ViewModel_NavigationButton vmNavigationButton = (ViewModel_NavigationButton)((Button)sender).DataContext;
            blockToCreateType = vmNavigationButton.BlockType;

            button.Visibility = Visibility.Visible;
            button.Content = vmNavigationButton.NameButton;
            button.CaptureMouse();
        }

        private void ParentCanvas_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.C && Keyboard.Modifiers == ModifierKeys.Control)
            {
                //MessageBox.Show("CTRL + C Pressed!"+ blockToCreateType.ToString());

            }
            if (e.Key == Key.V && Keyboard.Modifiers == ModifierKeys.Control)
            {
                if (blockToCreateType == null)
                    return;
                ViewModel_MainWindow vm = (ViewModel_MainWindow)this.DataContext;
                UserControl newBlock = (UserControl)Activator.CreateInstance(blockToCreateType);
                ViewModel_EmptyBlock vmBlock = (ViewModel_EmptyBlock)newBlock.DataContext;
                Point p = new System.Windows.Point(Mouse.GetPosition(canvas2).X, Mouse.GetPosition(canvas2).Y);
                if (p.X < 0)
                    p = new System.Windows.Point(10, p.Y);
                if (p.Y < 0)
                    p = new System.Windows.Point(p.X, 10);
                vmBlock.Position = p;
                vm.EmptyBlocks.Add(newBlock);
            }
        }

        private void Path_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void Path_MouseEnter(object sender, MouseEventArgs e)
        {
            if (tracageLigne == false)
            {
                if ((sender as System.Windows.Shapes.Path).DataContext == null)
                    return;
                selectedLine = ((sender as System.Windows.Shapes.Path).DataContext as ViewModel_Line);
                selectedLine.Thickness = 8;
            }

        }

        private void Path_MouseLeave(object sender, MouseEventArgs e)
        {
            if(selectedLine!=null)
                selectedLine.Thickness = 6;
        }

        private void Path_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            ViewModel_MainWindow vm = this.DataContext as ViewModel_MainWindow;
            vm.Lines.Remove(selectedLine);

            vm.EmptyBlocks.Where(x => (x.DataContext as ViewModel_EmptyBlock).parents.First() == selectedLine._viewModelToRef);
            foreach (UserControl block in vm.EmptyBlocks)
            {
                ViewModel_EmptyBlock viewModel_EmptyBlock = (block.DataContext as ViewModel_EmptyBlock);
                viewModel_EmptyBlock.parents?.Remove(selectedLine._viewModelToRef);
                viewModel_EmptyBlock.enfants?.Remove(selectedLine._viewModelToRef);
                viewModel_EmptyBlock.parents?.Remove(selectedLine._viewModelFromRef);
                viewModel_EmptyBlock.enfants?.Remove(selectedLine._viewModelFromRef);
                CommandManager.InvalidateRequerySuggested();
            }
        }

        private void translateCanvas(Point translation)
        {
            ViewModel_MainWindow vm = Application.Current.MainWindow.DataContext as ViewModel_MainWindow;
            foreach (UserControl u in vm.EmptyBlocks)
            {
                ViewModel_EmptyBlock vmBlock = (ViewModel_EmptyBlock)u.DataContext;
                vmBlock.Position = new System.Windows.Point(vmBlock.Position.X + translation.X, vmBlock.Position.Y + translation.Y);
            }
            foreach (ViewModel_Line line in vm.Lines)
            {
                line.From = new System.Windows.Point(line.From.X + translation.X, line.From.Y + translation.Y);
                line.To = new System.Windows.Point(line.To.X + translation.X, line.To.Y + translation.Y);
            }
        }

        private void zoom(double scale)
        {
            var transform = canvas2.RenderTransform as MatrixTransform;
            var matrix = transform.Matrix;
            matrix.ScaleAtPrepend(scale, scale, ParentCanvas.ActualWidth / 2, ParentCanvas.ActualHeight / 2);
            transform.Matrix = matrix;
        }

        private void ZoomIn(object sender, RoutedEventArgs e)
        {
            zoom(1.1);
        }

        private void ZoomOut(object sender, RoutedEventArgs e)
        {
            zoom(0.9);
        }
    }
}
