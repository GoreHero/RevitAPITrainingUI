using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Commands;

namespace RevitAPI_W1_MadeButton
{
    internal class MainViewViewModel
    {
        private ExternalCommandData _commandData;

        public DelegateCommand SelectedCommandPipe { get; } //Свойство
        public DelegateCommand SelectedCommandWall { get; } //Свойство
        public DelegateCommand SelectedCommandDoor { get; } //Свойство

        public MainViewViewModel(ExternalCommandData commandData)
        {
            _commandData = commandData; //обращение к открытому проекту Revit
            SelectedCommandPipe = new DelegateCommand(OnSelectPipe); //команда выполняемая при нажатии на кнопку
            SelectedCommandWall = new DelegateCommand(OnSelectWall); //команда выполняемая при нажатии на кнопку
            SelectedCommandDoor = new DelegateCommand(OnSelectDoor); //команда выполняемая при нажатии на кнопку
        }

        public event EventHandler HideRequest; //Событие временного скрытия окна

        private void RaiseHideRequest()
        {
            HideRequest?.Invoke(this, EventArgs.Empty); 
        }

        public event EventHandler ShowRequest;

        private void RaiseShowRequest()
        {
            ShowRequest?.Invoke(this, EventArgs.Empty);
        }

        private void OnSelectDoor() //логика при нажатии на кнопку
        {
            RaiseHideRequest(); //Скрывает временно окно 

            UIApplication uiapp = _commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document; 
            //собираем список экземпляров дверей в проекте
            List<FamilyInstance> familyInstances = new FilteredElementCollector(doc)
               .OfCategory(BuiltInCategory.OST_Doors)
               .WhereElementIsNotElementType()
               .Cast<FamilyInstance>()
               .ToList();

            TaskDialog.Show("Количество дверей", $"Количество дверей: {familyInstances.Count.ToString()}");

            RaiseShowRequest();
        }

        private void OnSelectWall() //логика при нажатии на кнопку
        {
            RaiseHideRequest();

            UIApplication uiapp = _commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;
            //Сбор всех дверей
            var walls = new FilteredElementCollector(doc)
               .OfClass(typeof(Wall))
               .Cast<Wall>()
               .ToList();

            double Total = 0;
            foreach (var wall in walls)
            {
                Parameter volumeParameter = wall.get_Parameter(BuiltInParameter.HOST_VOLUME_COMPUTED);
                if (volumeParameter.StorageType == StorageType.Double)
                {
                    double volumeValue = UnitUtils.ConvertFromInternalUnits(volumeParameter.AsDouble(), UnitTypeId.CubicMeters);
                    Total += Math.Round(volumeValue, 2);
                }
            }

            TaskDialog.Show("Объем стен", $"Объем стен: {Total} м³");

            RaiseShowRequest();
        }

        private void OnSelectPipe() //логика при нажатии на кнопку
        {
            RaiseHideRequest();

            UIApplication uiapp = _commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            //сбор всех экземпляров труб
            var pipes = new FilteredElementCollector(doc)
                .OfClass(typeof(Pipe))
                .Cast<Pipe>()
                .ToList();

            TaskDialog.Show("Количество труб", $"Количество труб: {pipes.Count.ToString()}");

            RaiseShowRequest();
        }
    }
}
