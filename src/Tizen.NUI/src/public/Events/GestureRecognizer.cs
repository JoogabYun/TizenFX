/*
 * Copyright(c) 2023 Samsung Electronics Co., Ltd.
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
using System.Runtime.InteropServices;
using Tizen.NUI.BaseComponents;
using System.ComponentModel;

namespace Tizen.NUI
{

    /// <summary>
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class GestureRecognizer : BaseHandle
    {

        /// <summary>
        /// Creates a GestureRecognizer
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public GestureRecognizer() : this(Interop.GestureRecognizer.New(), true)
        {
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
        }

        internal GestureRecognizer(global::System.IntPtr cPtr, bool cMemoryOwn) : base(cPtr, cMemoryOwn)
        {
        }

        /// <summary>
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void AddDetector(GestureDetector detector)
        {

        }

        /// <summary>
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void RemoveDetector(GestureDetector detector)
        {

        }

        /// <summary>
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool FeedTouch(object sender, View.TouchEventArgs e)
        {
            return false;
        }


        /// <summary>
        /// Gets the GestureRecognizer from the pointer.
        /// </summary>
        /// <param name="cPtr">The pointer to cast.</param>
        /// <returns>The GestureRecognizer object.</returns>
        internal static GestureRecognizer GetGestureRecognizerFromPtr(global::System.IntPtr cPtr)
        {
            GestureRecognizer ret = new GestureRecognizer(cPtr, false);
            if (NDalicPINVOKE.SWIGPendingException.Pending) throw NDalicPINVOKE.SWIGPendingException.Retrieve();
            return ret;
        }

        /// This will not be public opened.
        [EditorBrowsable(EditorBrowsableState.Never)]
        protected override void ReleaseSwigCPtr(System.Runtime.InteropServices.HandleRef swigCPtr)
        {
            Interop.GestureRecognizer.DeleteGestureRecognizer(swigCPtr);
        }
    }
}
