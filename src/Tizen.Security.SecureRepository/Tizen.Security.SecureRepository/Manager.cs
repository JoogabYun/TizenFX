﻿/*
 *  Copyright (c) 2016 Samsung Electronics Co., Ltd All Rights Reserved
 *
 *  Licensed under the Apache License, Version 2.0 (the "License");
 *  you may not use this file except in compliance with the License.
 *  You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 *  Unless required by applicable law or agreed to in writing, software
 *  distributed under the License is distributed on an "AS IS" BASIS,
 *  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *  See the License for the specific language governing permissions and
 *  limitations under the License
 */

using System;

namespace Tizen.Security.SecureRepository
{
    /// <summary>
    /// This class is a base class of XxxManager classes. It provides the common methods
    /// for all sub classes.
    /// </summary>
    public class Manager
    {
        /// <summary>
        /// Creates a new full alias which is concatenation of owner id and alias.
        /// </summary>
        /// <param name="ownerId">Data owner's id. This should be package id if data
        /// owner is application. If you want to access data stored by system services,
        /// use CreateFullSystemAlias() instead.</param>
        /// <param name="alias">Data alias.</param>
        static public string CreateFullAlias(string ownerId, string alias)
        {
            return ownerId + Manager.OwnerIdSeperator + alias;
        }

        /// <summary>
        /// Creates a new full alias which is concatenation of system service's
        /// owner id and alias.
        /// </summary>
        /// <param name="alias">Data alias which is owned by system service.</param>
        static public string CreateFullSystemAlias(string alias)
        {
            return Manager.CreateFullAlias(Manager.SystemOwnerId, alias);
        }

        /// <summary>
        /// Removes a an entry (no matter of type) from the key manager.
        /// </summary>
        /// <param name="alias">Item alias to be removed.</param>
        /// <exception cref="ArgumentNullException">alias is null.</exception>
        /// <exception cref="ArgumentException">alias is invalid format.</exception>
        /// <exception cref="InvalidOperationException">alias does not exist.</exception>
        /// <remarks>
        /// To remove item, client must have remove permission to the specified item.
        /// </remarks>
        /// <remarks>The item owner can remove by default.</remarks>
        static public void RemoveAlias(string alias)
        {
            if (alias == null)
                throw new ArgumentNullException("alias should not be null");

            Interop.CheckNThrowException(
                Interop.CkmcManager.RemoveAlias(alias),
                "Failed to remove alias. alias=" + alias);
        }

        /// <summary>
        /// Allows another application to access client's application data.
        /// </summary>
        /// <param name="alias">Item alias for which access will be granted.</param>
        /// <param name="otherPackageId">
        /// Package id of the application that will gain access rights.
        /// </param>
        /// <param name="permissions">
        /// Mask of permissions(Permission enum) granted for an application with
        /// otherPackageId.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// alias or otherPackageId is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// alias or otherPackageId is invalid format.
        /// </exception>
        /// <exception cref="InvalidOperationException">alias does not exist.</exception>
        /// <remarks>Data identified by alias should exist.</remarks>
        /// <remarks>The item owner can set permissions.</remarks>
        static public void SetPermission(
            string alias, string otherPackageId, int permissions)
        {
            if (alias == null || otherPackageId == null)
                throw new ArgumentNullException("alias or otherPackageId is null");

            Interop.CheckNThrowException(
                Interop.CkmcManager.SetPermission(alias, otherPackageId, permissions),
                "Failed to set permission. alias=" + alias);
        }

        // to being static base class
        internal Manager()
        {
        }

        private const string OwnerIdSeperator = " ";
        private const string SystemOwnerId = "/System";
    }
}
