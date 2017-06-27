//  
// Copyright (c) Nick Guletskii and Arseniy Aseev. All rights reserved.  
// Licensed under the MIT License. See LICENSE file in the solution root for full license information.  
//
namespace WixWPFWizardBA.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.InteropServices;
    using System.Text;
    using Microsoft.Win32;

    public static class RegistryUtilities
    {
        public const uint KEY_READ = 0x20019u;
        public const uint KEY_WRITE = 0x20006u;
        public const uint KEY_QUERY_VALUE = 0x0001u;
        public const uint KEY_SET_VALUE = 0x0002u;
        public const uint KEY_WOW64_64KEY = 0x0100u;
        public const uint KEY_WOW64_32KEY = 0x0200u;

        public const uint REG_NONE = 0u;
        public const uint REG_SZ = 1u;
        public const uint REG_EXPAND_SZ = 2u;
        public const uint REG_BINARY = 3u;
        public const uint REG_DWORD = 4u;
        public const uint REG_DWORD_BIG_ENDIAN = 5u;
        public const uint REG_LINK = 6u;
        public const uint REG_MULTI_SZ = 7u;
        public const uint REG_RESOURCE_LIST = 8u;
        public const uint REG_FULL_RESOURCE_DESCRIPTOR = 9u;
        public const uint REG_RESOURCE_REQUIREMENTS_LIST = 10u;
        public const uint REG_QWORD = 11u;

        private const int ERROR_MORE_DATA = 234;
        private const int ERROR_FILE_NOT_FOUND = 2;
        private const int ERROR_SUCCESS = 0;

        [DllImport("Advapi32.dll", EntryPoint = "RegOpenKeyExW", CharSet = CharSet.Unicode)]
        private static extern int RegOpenKeyEx(UIntPtr hKey, [In] string lpSubKey, uint ulOptions, uint samDesired,
            out UIntPtr phkResult);

        [DllImport("Advapi32.dll", EntryPoint = "RegQueryValueExW", CharSet = CharSet.Unicode)]
        private static extern int RegQueryValueEx(UIntPtr hKey, [In] string lpValueName, IntPtr lpReserved,
            out uint lpType,
            [Out] byte[] lpData, ref int lpcbData);

        [DllImport("advapi32.dll")]
        private static extern int RegCloseKey(UIntPtr hKey);

        private static object RegQueryValue(UIntPtr key, string value)
        {
            return RegQueryValue(key, value, null);
        }

        private static object RegQueryValue(UIntPtr key, string value, object defaultValue)
        {
            int error;
            uint type = 0;
            var dataLength = 64 * 1024;
            var returnLength = dataLength;
            var data = new byte[dataLength];

            while ((error = RegQueryValueEx(key, value, IntPtr.Zero, out type, data, ref returnLength)) ==
                   ERROR_MORE_DATA)
            {
                dataLength *= 2;
                returnLength = dataLength;
                data = new byte[dataLength];
            }

            if (error == ERROR_FILE_NOT_FOUND)
                return defaultValue;
            if (error != ERROR_SUCCESS)
                throw new Win32Exception(error);

            switch (type)
            {
                case REG_NONE:
                case REG_BINARY:
                    return data;
                case REG_DWORD:
                    if (!BitConverter.IsLittleEndian)
                        Array.Reverse(data);
                    return BitConverter.ToUInt32(data, 0);
                case REG_DWORD_BIG_ENDIAN:
                    if (BitConverter.IsLittleEndian)
                        Array.Reverse(data);
                    return BitConverter.ToUInt32(data, 0);
                case REG_QWORD:
                {
                    if (!BitConverter.IsLittleEndian)
                        Array.Reverse(data);
                    return BitConverter.ToUInt64(data, 0);
                }
                case REG_SZ:
                    return Encoding.Unicode.GetString(data, 0, returnLength);
                case REG_EXPAND_SZ:
                    return Environment.ExpandEnvironmentVariables(Encoding.Unicode.GetString(data, 0, returnLength));
                case REG_MULTI_SZ:
                {
                    var strings = new List<string>();
                    var packed = Encoding.Unicode.GetString(data, 0, returnLength);
                    var start = 0;
                    var end = packed.IndexOf('\0', start);
                    while (end > start)
                    {
                        strings.Add(packed.Substring(start, end - start));
                        start = end + 1;
                        end = packed.IndexOf('\0', start);
                    }
                    return strings.ToArray();
                }
                default:
                    throw new NotSupportedException();
            }
        }

        public static object GetRegistryValue32Bit(RegistryHive hive, string path, string v)
        {
            UIntPtr key;
            int error;
            if ((error = RegOpenKeyEx(new UIntPtr((uint) hive), path, 0, KEY_READ | KEY_WOW64_32KEY, out key)) != 0)
            {
                if (error == ERROR_FILE_NOT_FOUND)
                    return null;
                throw new Win32Exception(error);
            }
            try
            {
                return RegQueryValue(key, v);
            }
            finally
            {
                RegCloseKey(key);
            }
        }

        public static object GetRegistryValue64Bit(RegistryHive hive, string path, string v)
        {
            UIntPtr key;
            int error;
            if ((error = RegOpenKeyEx(new UIntPtr((uint) hive), path, 0, KEY_READ | KEY_WOW64_64KEY, out key)) != 0)
            {
                if (error == ERROR_FILE_NOT_FOUND)
                    return null;
                throw new Win32Exception(error);
            }
            try
            {
                return RegQueryValue(key, v);
            }
            finally
            {
                RegCloseKey(key);
            }
        }
    }
}