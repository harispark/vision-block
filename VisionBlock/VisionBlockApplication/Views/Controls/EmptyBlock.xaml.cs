using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using VisionBlockApplication.Divers;
using VisionBlockApplication.ViewModels.Controls;
using VisionBlockApplication.ViewModels.Pages;
using VisionBlockApplication.Views.Pages;

namespace VisionBlockApplication.Views.Controls
{
    public partial class EmptyBlock : UserControl
    {
        public EmptyBlock()
        {
            InitializeComponent();
            this.Loaded += EmptyBlock_Loaded;
        }

        void reCalculatePoints()
        {
            ViewModel_EmptyBlock vmBlock = ((ViewModel_EmptyBlock)this.DataContext);
            vmBlock.FromPointPosition = new System.Windows.Point(vmBlock.Position.X + this.ActualWidth, vmBlock.Position.Y + this.ActualHeight / 2);
            vmBlock.ToPointPosition = new System.Windows.Point(vmBlock.Position.X, vmBlock.Position.Y + this.ActualHeight / 2);

            List<Models.ViewModel_Line> fromList = new List<Models.ViewModel_Line>();
            List<Models.ViewModel_Line> toList = new List<Models.ViewModel_Line>();

            ViewModel_MainWindow vmMainWindow = (ViewModel_MainWindow)Application.Current.MainWindow.DataContext;
            foreach (Models.ViewModel_Line l in vmMainWindow.Lines)
            {
                //remplir une liste avec tout les from qui correspondent à ce point.
                if (object.ReferenceEquals(l._viewModelFromRef, vmBlock))
                    fromList.Add(l);
                if (object.ReferenceEquals(l._viewModelToRef, vmBlock))
                    toList.Add(l);
            }

            if (fromList.Count != 0)
                foreach (Models.ViewModel_Line l in fromList)
                    l.setViewModelFromRef(vmBlock);

            if (toList.Count != 0)
                foreach (Models.ViewModel_Line l in toList)
                    l.setViewModelToRef(vmBlock);
        }

        private void Grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
                return;
            reCalculatePoints();
        }

        private void EmptyBlock_Loaded(object sender, RoutedEventArgs e)
        {
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
                return;
            reCalculatePoints();
        }

        private void Grid_MouseDown_1(object sender, MouseButtonEventArgs e)
        {    
            //sélection du block
            ViewModel_EmptyBlock vmBlock = (this.DataContext) as ViewModel_EmptyBlock;
            if (vmBlock.IsSelected != true)
            {
                vmBlock.IsSelected = true;
                MainWindow.blockToCreateType = (this.DataContext as ViewModel_EmptyBlock).xamlBlockType;
            }

            //déplacement du block
            IInputElement element = this.InputHitTest(e.GetPosition(this));
            if (element is System.Windows.Shapes.Ellipse || element is System.Windows.Controls.Image || element is System.Windows.Shapes.Rectangle)
                return;
            
            MainWindow.deplacementBlock = true;
            vmBlock.isMoving = true;
            vmBlock.deltaPositionForDragDrop = Mouse.GetPosition(this);
        }

        private void Grid_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            MainWindow.deplacementBlock = false;
            ((ViewModel_EmptyBlock)this.DataContext).isMoving = false;
        }

        private void Grid_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow.tracageLigne = true;
            Models.ViewModel_Line l = new Models.ViewModel_Line();
            l.setViewModelFromRef((ViewModel_EmptyBlock)this.DataContext);
            ViewModel_EmptyBlock vm = this.DataContext as ViewModel_EmptyBlock;
            l.From = new Point(vm.Position.X + this.Width, vm.Position.Y + this.ActualHeight / 2);
            l.To = Mouse.GetPosition(((MainWindow)Application.Current.MainWindow).canvas2);
            ((ViewModel_MainWindow)Application.Current.MainWindow.DataContext).Lines.Add(l);
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ViewModel_MainWindow vmMainWindow = ((Application.Current.MainWindow as MainWindow).DataContext as ViewModel_MainWindow);

            (Application.Current.MainWindow as MainWindow).FullScreenImage.Width = 100;
            (Application.Current.MainWindow as MainWindow).FullScreenImage.Height = 100;

            (Application.Current.MainWindow as MainWindow).FullScreenImage.Source = ((Image)sender).Source;

            Canvas.SetTop((Application.Current.MainWindow as MainWindow).FullScreenImage, (Application.Current.MainWindow as MainWindow).ParentCanvas.ActualHeight/2);
            Canvas.SetLeft((Application.Current.MainWindow as MainWindow).FullScreenImage, (Application.Current.MainWindow as MainWindow).ParentCanvas.ActualWidth/2);

            var a = (Application.Current.MainWindow as MainWindow).ParentCanvas.ActualWidth / (Application.Current.MainWindow as MainWindow).FullScreenImage.Width;
            var b = (Application.Current.MainWindow as MainWindow).ParentCanvas.ActualHeight / (Application.Current.MainWindow as MainWindow).FullScreenImage.Height;
            if (a > b)
                vmMainWindow.ImageFullScreenSize = b;
            else
                vmMainWindow.ImageFullScreenSize = a;

            vmMainWindow.ImageFullScreenDisplay = Visibility.Visible;

            ((Storyboard)(Application.Current.MainWindow as MainWindow).Resources["StoryBoard_DisplayFullImage"]).Begin(Application.Current.MainWindow as FrameworkElement);
        }
    }
}