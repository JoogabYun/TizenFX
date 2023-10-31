﻿using System;
using Tizen.NUI;
using Tizen.NUI.Components;
using Tizen.NUI.BaseComponents;
using System.ComponentModel;
using System.Runtime.InteropServices;


namespace VideoPlayDemo
{
    class Program : NUIApplication
    {
        class LogOutput : ScrollableBase
        {
            public LogOutput() : base()
            {
                SizeWidth = 500;
                BackgroundColor = Color.AntiqueWhite;
                // WidthSpecification = LayoutParamPolicies.MatchParent;
                HeightSpecification = LayoutParamPolicies.MatchParent;
                // HideScrollbar = false;
                ScrollingDirection = ScrollableBase.Direction.Vertical;

                ContentContainer.Layout = new LinearLayout
                {
                    LinearOrientation = LinearLayout.Orientation.Vertical,
                    VerticalAlignment = VerticalAlignment.Top,
                };
            }

            public void AddLog(string log)
            {
                Console.WriteLine($"{log}\n");
                var txt = new TextLabel
                {
                    Text = log
                };

                ContentContainer.Add(txt);
                if (ContentContainer.Children.Count > 30)
                {
                    var remove = ContentContainer.Children.GetRange(0, 10);
                    foreach (var child in remove)
                    {
                        ContentContainer.Remove(child);
                    }
                }
                ElmSharp.EcoreMainloop.Post(() =>
                {
                    ScrollTo((ContentContainer.Children.Count) * (txt.NaturalSize.Height), true);
                });
            }
            public override View GetNextFocusableView(View currentFocusedView, View.FocusDirection direction, bool loopEnabled)
            {
                return null;
            }
        }

        private TapGestureDetector[] tapGestureDetector = new TapGestureDetector[4];
        private PanGestureDetector[] panGestureDetector = new PanGestureDetector[4];
        private LongPressGestureDetector[] longPressGestureDetector = new LongPressGestureDetector[4];
        private GestureProcessor[] gestureProcessor = new GestureProcessor[4];
        private Color backgroundColor = Color.White;


        private PanGestureDetector tempPanGestureDetector;

        private Window window;
        private View blueView;
        private View yellowView;
        private View redView;
        private View orangeView;
        private LogOutput log;

        public Program() : base()
        {
        }

        protected override void OnCreate()
        {
            base.OnCreate();
            Initialize();
        }

        void Initialize()
        {
            window = NUIApplication.GetDefaultWindow();
            window.BackgroundColor = Color.Grey;

            var root = new View
            {
                WidthResizePolicy = ResizePolicyType.FillToParent,
                HeightResizePolicy = ResizePolicyType.FillToParent,

            };
            root.Name = "root";

            // default hittest, do not parent-child
            blueView = new View
            {
                Name = "blueView",
                Size2D = new Size2D(700, 500),
                Position2D = new Position2D(500, 10),
                BackgroundColor = Color.Blue,
            };
            var blueTxt = new TextLabel
            {
                Text = "Blue",
            };
            blueView.Add(blueTxt);

            yellowView = new View
            {
                Name = "yellowView",
                Size2D = new Size2D(600, 350),
                Position2D = new Position2D(550, 60),
                BackgroundColor = Color.Yellow,
            };
            var yellowTxt = new TextLabel
            {
                Text = "Yellow",
            };
            yellowView.Add(yellowTxt);


            redView = new View
            {
                Name = "redView",
                Size2D = new Size2D(300, 150),
                Position2D = new Position2D(590, 120),
                BackgroundColor = Color.Red,
                // ClippingMode = ClippingModeType.ClipChildren,
            };
            var redTxt = new TextLabel
            {
                Text = "Red",
            };
            redView.Add(redTxt);


            orangeView = new View
            {
                Name = "orangeView",
                Size2D = new Size2D(700, 100),
                Position2D = new Position2D(50, 30),
                BackgroundColor = Color.Orange,
            };
            var orangeTxt = new TextLabel
            {
                Text = "Orange",
            };
            orangeView.Add(orangeTxt);


            log = new LogOutput();

            redView.Add(orangeView);
            root.Add(blueView);
            root.Add(yellowView);
            root.Add(redView);
            window.Add(root);
            window.Add(log);

            for(int i =0; i<4; i++)
            {
                string name = GetName(i);
                tapGestureDetector[i] = new TapGestureDetector();
                tapGestureDetector[i].Detected += (s, e) =>
                {
                    Tizen.Log.Error("NUI", $"{name} tapGestureDetectorn");
                    log.AddLog($" ->{name} OnTap {e.TapGesture.State}\n");
                };
                panGestureDetector[i] = new PanGestureDetector();
                panGestureDetector[i].Detected += (s, e) =>
                {
                    Tizen.Log.Error("NUI", $"{name} panGestureDetector");
                    log.AddLog($" ->{name} OnPan {e.PanGesture.State}\n");
                };
                longPressGestureDetector[i] = new LongPressGestureDetector();
                longPressGestureDetector[i].Detected += (s, e) =>
                {
                    Tizen.Log.Error("NUI", $"{name} longPressGestureDetector");
                    log.AddLog($" ->{name} OnLong {e.LongPressGesture.State} {s}\n");

                    var senderView = e.View;
                    if(e.LongPressGesture.State == Gesture.StateType.Started)
                    {
                        backgroundColor = new Color(senderView.BackgroundColor);
                        senderView.BackgroundColor = senderView.BackgroundColor * 0.7f;
                    }

                    if(e.LongPressGesture.State == Gesture.StateType.Finished || e.LongPressGesture.State == Gesture.StateType.Cancelled)
                     {
                        senderView.BackgroundColor = backgroundColor;
                     }
                };
                gestureProcessor[i] = new GestureProcessor();
            }

            MakeInterceptTouchList();
            MakeTouchList();
            MakeGestureList();

            // LongPanGestureTest();
        }

        public void LongPanGestureTest()
        {
            gestureProcessor[3].AddDetector(longPressGestureDetector[3]);
            orangeView.TouchEvent += (s, e) =>
            {
                log.AddLog($" ->orangeView touch11 {e.Touch.GetState(0)}\n");
                gestureProcessor[3].FeedTouch(s, e.Touch);
                return true;
            };


            bool isDetected = false;
            var tempGestureProcessor = new GestureProcessor();
            tempPanGestureDetector = new PanGestureDetector();
            tempGestureProcessor.AddDetector(tempPanGestureDetector);
            Color tempColor = yellowView.BackgroundColor * 0.7f;;
            tempPanGestureDetector.Detected += (s, e) =>
            {
                log.AddLog($" ->yellowView OnPan\n");
                if(e.PanGesture.State == Gesture.StateType.Finished || e.PanGesture.State == Gesture.StateType.Cancelled)
                {
                    isDetected = false;
                    yellowView.BackgroundColor = Color.Yellow;
                }
                else
                {
                    isDetected = true;
                    yellowView.BackgroundColor = tempColor;
                }
            };
            yellowView.InterceptTouchEvent += (s, e) =>
            {
                tempGestureProcessor.FeedTouch(s, e.Touch);
                return isDetected;
            };
            yellowView.TouchEvent += (s, e) =>
            {
                tempGestureProcessor.FeedTouch(s, e.Touch);
                return true;
            };
        }

        private bool blueInterceptConsumed = false;
        private bool yellowInterceptConsumed = false;
        private bool redInterceptConsumed = false;
        private bool orangeInterceptConsumed = false;
        public void MakeInterceptTouchEvent(View list, View targetView, string name, Tizen.NUI.EventHandlerWithReturnType<object, Tizen.NUI.BaseComponents.View.TouchEventArgs, bool> interceptEvent)
        {
            var interceptButtonList = new View()
            {
                Size2D = new Size2D(300, 70),
                Layout = new LinearLayout
                {
                    LinearOrientation = LinearLayout.Orientation.Horizontal,
                },
            };
            var checkBox = new CheckBox
            {
                Text = name
            };
            checkBox.SelectedChanged += (object sender, SelectedChangedEventArgs args) =>
            {
                if(args.IsSelected == true)
                {
                    targetView.InterceptTouchEvent += interceptEvent;
                }
                else
                {
                    targetView.InterceptTouchEvent -= interceptEvent;
                }
            };
            var consumedCheck = new CheckBox
            {
                Text = "Consumed"
            };
            if(name == "Blue")
            {
                consumedCheck.SelectedChanged += (object sender, SelectedChangedEventArgs args) =>
                {
                    blueInterceptConsumed = args.IsSelected;
                };
            }
            else if(name == "Yellow")
            {
                consumedCheck.SelectedChanged += (object sender, SelectedChangedEventArgs args) =>
                {
                    yellowInterceptConsumed = args.IsSelected;
                };
            }
            else if(name == "Red")
            {
                consumedCheck.SelectedChanged += (object sender, SelectedChangedEventArgs args) =>
                {
                    redInterceptConsumed = args.IsSelected;
                };
            }
            else if(name == "Orange")
            {
                consumedCheck.SelectedChanged += (object sender, SelectedChangedEventArgs args) =>
                {
                    orangeInterceptConsumed = args.IsSelected;
                };
            }
            interceptButtonList.Add(checkBox);
            interceptButtonList.Add(consumedCheck);
            list.Add(interceptButtonList);
        }

        public void MakeInterceptTouchList()
        {
            var buttonLayer = new View()
            {
                Size2D = new Size2D(700, 400),
                Position2D = new Position2D(500, 560),
                Layout = new LinearLayout
                {
                    LinearOrientation = LinearLayout.Orientation.Vertical,
                    VerticalAlignment = VerticalAlignment.Top,
                },
            };

            var interceptText = new TextLabel()
            {
                Text = "InterceptTouch"
            };
            buttonLayer.Add(interceptText);

            MakeInterceptTouchEvent(buttonLayer, blueView, "Blue", BlueInterceptTouched);
            MakeInterceptTouchEvent(buttonLayer, yellowView, "Yellow", YellowInterceptTouched);
            MakeInterceptTouchEvent(buttonLayer, redView, "Red", RedInterceptTouched);
            MakeInterceptTouchEvent(buttonLayer, orangeView, "Orange", OrangeInterceptTouched);
            window.Add(buttonLayer);
        }

        private bool blueConsumed = false;
        private bool yellowConsumed = false;
        private bool redConsumed = false;
        private bool orangeConsumed = false;
        public void MakeTouchEvent(View list, View targetView, string name, Tizen.NUI.EventHandlerWithReturnType<object, Tizen.NUI.BaseComponents.View.TouchEventArgs, bool> touchEvent)
        {
            var buttonList = new View()
            {
                Size2D = new Size2D(300, 70),
                Layout = new LinearLayout
                {
                    LinearOrientation = LinearLayout.Orientation.Horizontal,
                },
            };
            var checkBox = new CheckBox
            {
                Text = name
            };
            checkBox.SelectedChanged += (object sender, SelectedChangedEventArgs args) =>
            {
                if(args.IsSelected == true)
                {
                    targetView.TouchEvent += touchEvent;
                }
                else
                {
                    targetView.TouchEvent -= touchEvent;
                }
            };
            var consumedCheck = new CheckBox
            {
                Text = "Consumed"
            };
            if(name == "Blue")
            {
                consumedCheck.SelectedChanged += (object sender, SelectedChangedEventArgs args) =>
                {
                    blueConsumed = args.IsSelected;
                };
            }
            else if(name == "Yellow")
            {
                consumedCheck.SelectedChanged += (object sender, SelectedChangedEventArgs args) =>
                {
                    yellowConsumed = args.IsSelected;
                };
            }
            else if(name == "Red")
            {
                consumedCheck.SelectedChanged += (object sender, SelectedChangedEventArgs args) =>
                {
                    redConsumed = args.IsSelected;
                };
            }
            else if(name == "Orange")
            {
                consumedCheck.SelectedChanged += (object sender, SelectedChangedEventArgs args) =>
                {
                    orangeConsumed = args.IsSelected;
                };
            }
            buttonList.Add(checkBox);
            buttonList.Add(consumedCheck);
            list.Add(buttonList);
        }
        public void MakeTouchList()
        {
            var buttonLayer = new View()
            {
                Size2D = new Size2D(700, 400),
                Position2D = new Position2D(850, 560),
                Layout = new LinearLayout
                {
                    LinearOrientation = LinearLayout.Orientation.Vertical,
                    VerticalAlignment = VerticalAlignment.Top,
                },
            };

            var title = new TextLabel()
            {
                Text = "Touch"
            };
            buttonLayer.Add(title);

            MakeTouchEvent(buttonLayer, blueView, "Blue", BlueTouched);
            MakeTouchEvent(buttonLayer, yellowView, "Yellow", YellowTouched);
            MakeTouchEvent(buttonLayer, redView, "Red", RedTouched);
            MakeTouchEvent(buttonLayer, orangeView, "Orange", OrangeTouched);
            window.Add(buttonLayer);
        }

        public int GetIndex(string name)
        {
            if(name == "Blue")
            {
                return 0;
            }
            else if(name == "Yellow")
            {
                return 1;
            }
            else if(name == "Red")
            {
                return 2;
            }
            else if(name == "Orange")
            {
                return 3;
            }
            return -1;
        }

        public string GetName(int index)
        {
            switch(index)
            {
                case 0 :
                    return "Blue";
                case 1 :
                    return "Yellow";
                case 2 :
                    return "Red";
                case 3 :
                    return "Orange";
                default :
                    return "Unknown";
            }
        }

        public void MakeGestureEvent(View list, View targetView, string name)
        {
            var buttonList = new View()
            {
                Size2D = new Size2D(300, 70),
                Layout = new LinearLayout
                {
                    LinearOrientation = LinearLayout.Orientation.Horizontal,
                    VerticalAlignment = VerticalAlignment.Center,
                },
            };
            // var viewText = new TextLabel
            // {
            //     Text = name+" : "
            // };
            // buttonList.Add(viewText);

            var tapCheckBox = new CheckBox
            {
                Text = "Tap"
            };

            tapCheckBox.SelectedChanged += (object sender, SelectedChangedEventArgs args) =>
            {
                if(args.IsSelected)
                {
                    gestureProcessor[GetIndex(name)].AddDetector(tapGestureDetector[GetIndex(name)]);
                }
                else
                {
                    gestureProcessor[GetIndex(name)].RemoveDetector(tapGestureDetector[GetIndex(name)]);
                }
            };

            var longCheckBox = new CheckBox
            {
                Text = "LongPress"
            };
            longCheckBox.SelectedChanged += (object sender, SelectedChangedEventArgs args) =>
            {
                if(args.IsSelected)
                {
                    gestureProcessor[GetIndex(name)].AddDetector(longPressGestureDetector[GetIndex(name)]);
                }
                else
                {
                    gestureProcessor[GetIndex(name)].RemoveDetector(longPressGestureDetector[GetIndex(name)]);
                }
            };

            var panCheckBox = new CheckBox
            {
                Text = "Pan"
            };
            panCheckBox.SelectedChanged += (object sender, SelectedChangedEventArgs args) =>
            {
                if(args.IsSelected)
                {
                    gestureProcessor[GetIndex(name)].AddDetector(panGestureDetector[GetIndex(name)]);
                }
                else
                {
                    gestureProcessor[GetIndex(name)].RemoveDetector(panGestureDetector[GetIndex(name)]);
                }
            };

            buttonList.Add(tapCheckBox);
            buttonList.Add(longCheckBox);
            buttonList.Add(panCheckBox);
            list.Add(buttonList);
        }

        public void MakeGestureList()
        {
            var buttonLayer = new View()
            {
                Size2D = new Size2D(700, 400),
                Position2D = new Position2D(1200, 560),
                Layout = new LinearLayout
                {
                    LinearOrientation = LinearLayout.Orientation.Vertical,
                    VerticalAlignment = VerticalAlignment.Top,
                },
            };
            var title = new TextLabel()
            {
                Text = "Gesture"
            };
            buttonLayer.Add(title);
            MakeGestureEvent(buttonLayer, blueView, "Blue");
            MakeGestureEvent(buttonLayer, yellowView, "Yellow");
            MakeGestureEvent(buttonLayer, redView, "Red");
            MakeGestureEvent(buttonLayer, orangeView, "Orange");
            window.Add(buttonLayer);
        }

        private bool BlueInterceptTouched(object sender, View.TouchEventArgs args)
        {
            Tizen.Log.Error("NUI", $"blueView InterceptTouchEvent {args.Touch.GetState(0)} return {blueInterceptConsumed}\n");
            log.AddLog($" ->InterceptBlue View {args.Touch.GetState(0)} return {blueInterceptConsumed}\n");
            return blueInterceptConsumed;
        }

        private bool YellowInterceptTouched(object sender, View.TouchEventArgs args)
        {
            Tizen.Log.Error("NUI", $"yellowView InterceptTouchEvent {args.Touch.GetState(0)} return {yellowInterceptConsumed}\n");
            log.AddLog($" ->InterceptYellow View {args.Touch.GetState(0)} return {yellowInterceptConsumed}\n");
            return yellowInterceptConsumed;
        }


        private bool RedInterceptTouched(object sender, View.TouchEventArgs args)
        {
            Tizen.Log.Error("NUI", $"redView InterceptTouchEvent {args.Touch.GetState(0)} return {redInterceptConsumed}\n");
            log.AddLog($" ->InterceptRed View {args.Touch.GetState(0)} return {redInterceptConsumed}\n");
            return redInterceptConsumed;
        }


        private bool OrangeInterceptTouched(object sender, View.TouchEventArgs args)
        {
            Tizen.Log.Error("NUI", $"orangeView InterceptTouchEvent {args.Touch.GetState(0)} return {orangeInterceptConsumed}\n");
            log.AddLog($" ->InterceptOrange View {args.Touch.GetState(0)} return {orangeInterceptConsumed}\n");
            return orangeInterceptConsumed;
        }

        private bool BlueTouched(object sender, View.TouchEventArgs args)
        {
            Tizen.Log.Error("NUI", $"blueView TouchEvent {args.Touch.GetState(0)}\n");
            log.AddLog($" ->Blue View {args.Touch.GetState(0)}\n");
            return blueConsumed | gestureProcessor[0].FeedTouch(sender, args.Touch);
        }

        private bool YellowTouched(object sender, View.TouchEventArgs args)
        {
            Tizen.Log.Error("NUI", $"yellowView TouchEvent {args.Touch.GetState(0)}\n");
            log.AddLog($" ->Yellow View {args.Touch.GetState(0)}\n");
            return yellowConsumed | gestureProcessor[1].FeedTouch(sender, args.Touch);
        }


        private bool RedTouched(object sender, View.TouchEventArgs args)
        {
            Tizen.Log.Error("NUI", $"redView TouchEvent {args.Touch.GetState(0)}\n");
            log.AddLog($" ->Red View {args.Touch.GetState(0)}\n");
            return redConsumed | gestureProcessor[2].FeedTouch(sender, args.Touch);
        }


        private bool OrangeTouched(object sender, View.TouchEventArgs args)
        {
            Tizen.Log.Error("NUI", $"orangeView TouchEvent {args.Touch.GetState(0)}\n");
            log.AddLog($" ->Orange View {args.Touch.GetState(0)}\n");
            return orangeConsumed | gestureProcessor[3].FeedTouch(sender, args.Touch);
        }


        static View CreateButton(int index)
        {
            var rnd = new Random();

            var btn = new Button
            {
                Focusable = true,
                FocusableInTouch = true,
                Text = $"Item {index}",
                LeaveRequired = true,
                BackgroundColor = Color.DarkOrange,
            };
            btn.HoverEvent += (s, e) =>
            {
                if(e.Hover.GetState(0) == PointStateType.Started)
                {
                    btn.BackgroundColor = Color.Red;
                }
                else if (e.Hover.GetState(0) == PointStateType.Leave)
                {
                    btn.BackgroundColor = Color.DarkOrange;
                }
                return true;
            };

            var item = Wrapping(btn);
            item.SizeWidth = 200;
            item.SizeHeight = 90;

            item.Position = new Position(200, 100 * (index / 3) );

            if (item is Button button)
            {
                button.Text = $"[{button.Text}]";
            }

            return item;
        }

        static View Wrapping(View view)
        {
            int cnt = new Random().Next(0, 4);

            for (int i = 0; i < cnt; i++)
            {
                var wrapper = new View();
                view.WidthSpecification = LayoutParamPolicies.MatchParent;
                view.HeightSpecification = LayoutParamPolicies.MatchParent;
                wrapper.Add(view);
                view = wrapper;
            }

            return view;
        }


        static void Main(string[] args)
        {
            var app = new Program();
            app.Run(args);
        }
    }
}
