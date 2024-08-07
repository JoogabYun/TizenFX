/*
 * Copyright (c) 2016 Samsung Electronics Co., Ltd All Rights Reserved
 *
 * Licensed under the Apache License, Version 2.0 (the License);
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an AS IS BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;

namespace ElmSharp
{
    /// <summary>
    /// An instance to the ColorSelector item gets added.
    /// </summary>
    /// <since_tizen> preview </since_tizen>
    [Obsolete("This has been deprecated in API12")]
    public class ColorSelectorItem : ItemObject
    {
        internal ColorSelectorItem() : base(IntPtr.Zero)
        {
        }

        internal ColorSelectorItem(EvasObject parent) : base(IntPtr.Zero, parent)
        {
        }

        /// <summary>
        /// Gets or sets the palette item's color.
        /// </summary>
        /// <since_tizen> preview </since_tizen>
        [Obsolete("This has been deprecated in API12")]
        public Color Color
        {
            get
            {
                int r, g, b, a;
                Interop.Elementary.elm_colorselector_palette_item_color_get(Handle, out r, out g, out b, out a);
                return Color.FromRgba(r, g, b, a);
            }
            set
            {
                if (Handle != IntPtr.Zero)
                {
                    Interop.Elementary.elm_colorselector_palette_item_color_set(Handle, value.R, value.G, value.B, value.A);
                }
            }
        }
    }
}
