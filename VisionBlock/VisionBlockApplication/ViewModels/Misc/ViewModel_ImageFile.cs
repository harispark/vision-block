using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace VisionBlockApplication.ViewModels.Misc
{
    public class ViewModel_ImageFile : ViewModel_Base
    {
        public ViewModel_ImageFile(string filename)
        {
            BitmapImage image = new BitmapImage(new System.Uri(filename));
            BitmapData = new WriteableBitmap(image);
            Name = filename.Substring(filename.LastIndexOf("\\") + 1);
            Filename = filename;
        }

        #region Name
        private string _name;
        public string Name
        {
            get { return this._name; }
            set {
                this._name = value;
                this.OnPropertyChanged(nameof(this.Name));
            }
        }
        #endregion

        #region BitmapData
        private WriteableBitmap _bitmapData;
        public WriteableBitmap BitmapData
        {
            get { return this._bitmapData; }
            set { this._bitmapData = value; }
        }
        #endregion

        public string Filename;
    }
}
