using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.NUI.Components;
using Tizen.NUI.Events;


namespace Tizen.NUI.Samples
{
    public class TouchGestureSample : IExample
    {

        public void Activate()
        {
            NUIApplication.SetGeometryHittestEnabled(true);
            Window window = NUIApplication.GetDefaultWindow();

            var view1 = new View()
            {
                Size2D = new Size2D(700, 700),
                BackgroundColor = Color.Red,
            };

            var view2 = new View()
            {
                Size2D = new Size2D(500, 500),
                BackgroundColor = Color.Blue,
            };

            var view3 = new View()
            {
                Size2D = new Size2D(300, 300),
                BackgroundColor = Color.Yellow,
            };

            view1.TouchEvent += (s, e) =>
            {
                Tizen.Log.Error("NUI", $" view1 {e.Touch.GetState(0)}\n");

                return true;
            };

            view2.TouchEvent += (s, e) =>
            {
                Tizen.Log.Error("NUI", $" view2 {e.Touch.GetState(0)}\n");
                // if(e.Touch.GetState(0) == PointStateType.Motion)
                // {
                //     return true;
                // }
                return false;
            };

            view3.TouchEvent += (s, e) =>
            {
                Tizen.Log.Error("NUI", $" view3 {e.Touch.GetState(0)}\n");

                return false;
            };


            window.Add(view1);
            window.Add(view2);
            window.Add(view3);


        }

        public void Deactivate()
        {
        }
    }
}
