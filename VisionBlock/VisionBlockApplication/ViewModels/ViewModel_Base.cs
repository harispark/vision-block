using System.ComponentModel;

namespace VisionBlockApplication.ViewModels
{
    public class ViewModel_Base : INotifyPropertyChanged
    {
        /// <summary>
        /// Réfléchir à un héritage pour le property Changed car on en a besoin dans tout les viewModels
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string info)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(info));
            }
        }
    }
}
