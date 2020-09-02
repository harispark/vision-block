using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using VisionBlockApplication.Core;
using VisionBlockApplication.Divers;
using VisionBlockApplication.ViewModels.Controls;
using VisionBlockApplication.ViewModels.Pages;
using VisionBlockApplication.Views.Controls.Blocks;

namespace VisionBlockApplication.ViewModels.Misc
{
    public enum ProcessingCategoryEnum
    {
        Color, GreyScale, Binaire, None, All, Multiple, RGBA
    }

    public enum ProcessingRunningStateEnum
    {
        MustRun, HasAlreadyRun,IsRunning
    }

    public class ViewModel_NavigationButton : ViewModel_Base
    {
        public string NameButton
        {
            get; set;
        }

        public Type BlockType { get; set; }

        public string Description { get; set; }

        public int ProcessingCategoryInput { get; set; }

        public int ProcessingCategoryOutput { get; set; }

        #region Binding Enable
        private bool _enable = false;
        public bool Enable
        {
            get
            {
                return this._enable;
            }
            set
            {
                this._enable = value;
                this.OnPropertyChanged(nameof(this.Enable));
            }
        }
        #endregion

        #region CreateSpecificBlock_Command
        private ICommand _createSpecificBlock_Command;
        public ICommand CreateSpecificBlock_Command
        {
            get
            {
                if (_createSpecificBlock_Command == null)
                {
                    _createSpecificBlock_Command = new RelayCommand(p => { this.Execute_CreateSpecificBlock_Command(); }, p => CanExecute_CreateSpecificBlock_Command);
                }
                return _createSpecificBlock_Command;
            }
        }

        private Boolean CanExecute_CreateSpecificBlock_Command = true;
        private void Execute_CreateSpecificBlock_Command()
        {
            ViewModel_MainWindow vm = (ViewModel_MainWindow)Application.Current.MainWindow.DataContext;
            ViewModel_EmptyBlock.i = 0;
            vm.EmptyBlocks.Add((UserControl)Activator.CreateInstance(BlockType));
        }
        #endregion
    }
}
