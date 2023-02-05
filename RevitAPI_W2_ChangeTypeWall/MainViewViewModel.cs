using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Prism.Commands;
using RevitAPILibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAPI_W2_ChangeTypeWall
{
    internal class MainViewViewModel
    {
        private ExternalCommandData _commandData;

        public DelegateCommand SaveCommand { get; } //Свойство
        public List<Element> PickedObjects { get; } = new List<Element>(); // из библиотеки. Сбор элементов
        public List<WallType> WallTypes { get; } = new List<WallType>(); //из библиотеки Типы стен

        public WallType SelectedWallTypes { get; set; } //сбор всех выбраных стен

        public MainViewViewModel(ExternalCommandData commandData)
        {
            _commandData = commandData;
            SaveCommand = new DelegateCommand(OnSaveCommand); //исполнтельная кнопки изменить тип
            PickedObjects = SelectionUtils.PickObjects(commandData); //
            WallTypes = RevitAPILibrary.WallUtils.GetWallTypes(commandData);
        }

        private void OnSaveCommand()
        {
            UIApplication uiapp = _commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            if (PickedObjects.Count == 0 || SelectedWallTypes == null)
                return;

            using (var ts = new Transaction(doc, "Set type"))
            {
                ts.Start();

                foreach (var pickedObject in PickedObjects)
                {
                    if (pickedObject is Wall)
                    {
                        var oWall = pickedObject as Wall;
                        oWall.WallType = SelectedWallTypes;
                    }
                }
                ts.Commit();
            }
            RaiseCloseRequest();
        }

        public event EventHandler CloseRequest;
        private void RaiseCloseRequest()
        {
            CloseRequest?.Invoke(this, EventArgs.Empty);
        }
    }
}
