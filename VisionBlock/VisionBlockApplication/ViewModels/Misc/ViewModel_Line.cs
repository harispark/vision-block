using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisionBlockApplication.ViewModels.Controls;
using VisionBlockApplication.ViewModels;

namespace VisionBlockApplication.Models
{
    public class ViewModel_Line : ViewModel_Base
    {
        private Point _from;
        public Point From {
            get {
                return _from;
            }
            set
            {
                this._from = value;
                FromControl = new Point(From.X + 100, From.Y);
                OnPropertyChanged(nameof(this.From));
            }
        }

        private Point _to;
        public Point To
        {
            get
            {
                return _to;
            }
            set
            {
                this._to = value;
                ToControl = new Point(To.X - 100, To.Y);
                OnPropertyChanged(nameof(this.To));
            }
        }

        private Point _fromControl;
        public Point FromControl
        {
            get
            {
                return _fromControl;
            }
            set
            {
                _fromControl = value;
                this.OnPropertyChanged(nameof(FromControl));
            }
        }

        private Point _toControl;
        public Point ToControl
        {
            get
            {
                return _toControl;
            }
            set
            {
                _toControl = value;
                this.OnPropertyChanged(nameof(ToControl));
            }
        }

        public ViewModel_EmptyBlock _viewModelFromRef;
        public ViewModel_EmptyBlock _viewModelToRef;

        public void setViewModelFromRef(ViewModel_EmptyBlock vm)
        {
            this._viewModelFromRef = vm;
            this.From = vm.FromPointPosition;
        }

        public void setViewModelToRef(ViewModel_EmptyBlock vm)
        {
            this._viewModelToRef = vm;
            this.To = vm.ToPointPosition;
        }

        public void setViewModelFromAndTo(ViewModel_EmptyBlock vmFrom, ViewModel_EmptyBlock vmTo)
        {
            this._viewModelFromRef = vmFrom;
            this._viewModelToRef = vmTo;
            this.To = vmTo.ToPointPosition;
            this.From = vmFrom.ToPointPosition;
        }

        private int _thickness = 6;
        public int Thickness
        {
            get
            {
                return this._thickness;
            }
            set
            {
                this._thickness = value;
                this.OnPropertyChanged(nameof(this.Thickness));
            }
        }
    }
}
