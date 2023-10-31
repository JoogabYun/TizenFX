/*
 * Copyright (c) 2020 Samsung Electronics Co., Ltd.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *
 */
using System.ComponentModel;
using System.Collections.Generic;
using Tizen.NUI.BaseComponents;

namespace Tizen.NUI
{
    /// <summary>
    /// This is a class for detects various gestures.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class GestureProcessor : Disposable
    {
        private List<GestureDetector> gestureList = new List<GestureDetector>();
        /// <summary>
        ///  Creates a GestureProcessor with the user listener.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public GestureProcessor()
        {
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public void AddDetector(GestureDetector detector)
        {
            gestureList.Add(detector);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public void RemoveDetector(GestureDetector detector)
        {
            detector.DetachAll();
            gestureList.Remove(detector);
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool FeedTouch(object sender, Touch touch)
        {
            if(gestureList.Count == 0)
            {
                return false;
            }
            if(touch.GetState(0) == PointStateType.Down)
            {
                // GestureOptions.Instance.ClearAllGestureDetector();
                View view = sender as View;
                foreach (GestureDetector detector in gestureList)
                {
                    detector.Attach(view);
                }
            }

            GestureOptions.Instance.FeedTouch(sender, touch);

            if (touch.GetState(0) == PointStateType.Finished || touch.GetState(0) == PointStateType.Interrupted)
            {
                foreach (GestureDetector detector in gestureList)
                {
                    detector.DetachAll();
                }
            }
            return true;
        }


    }
}
