/*
 * Copyright(c) 2024 Samsung Electronics Co., Ltd.
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

using System;
using System.ComponentModel;
using System.Collections.Generic;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;

namespace Tizen.NUI.PenWave
{
    public class SizeIcon : Icon
    {
        private readonly float mSize;

        public SizeIcon(float size) : base()
        {
            mSize = size;

            string url = $"{FrameworkInformation.ResourcePath}images/light/color_icon_base.png";

            InitializeIcon(url, new Color("#17234d"));
            mImgView.Size2D = new Size2D((int)(size * 1.5f), (int)(size * 1.5f));
            this.TouchEvent += IconClick;
        }

        public bool IconClick(object sender, View.TouchEventArgs args)
        {
            if (base.IconClick(sender, args))
            {
                PWEngine.SetStrokeSize(GetSize());

            }
            return true;
        }

        public float GetSize() => mSize;
    }
}
