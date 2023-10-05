﻿using System.Runtime.InteropServices;
using System;
using System.IO;
using System.Reflection;

namespace DuckDB.NET.Test.Helpers;

static class NativeLibraryHelper
{
    public static bool TryLoad()
    {
        if (GetRid() is not { } rid)
        {
            return false;
        }

        return NativeLibrary.TryLoad(Path.Join("runtimes", rid, "native", "duckdb"), Assembly.GetExecutingAssembly(), DllImportSearchPath.AssemblyDirectory, out _) ||
               NativeLibrary.TryLoad(Path.Join("runtimes", rid, "native", "libduckdb"), Assembly.GetExecutingAssembly(), DllImportSearchPath.AssemblyDirectory, out _);
    }

    private static string GetRid()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return Environment.Is64BitProcess ? "win-x64" : "win-x86";
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            return RuntimeInformation.ProcessArchitecture switch
            {
                Architecture.X64 => "linux-x64",
                Architecture.Arm64 => "linux-arm64",
                _ => null,
            };
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            return "osx";
        }

        return null;
    }
}