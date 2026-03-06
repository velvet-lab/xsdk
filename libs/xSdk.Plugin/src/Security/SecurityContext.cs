using System.Runtime.InteropServices;
using System.Security.Principal;

namespace xSdk.Security
{
    public static class SecurityContext
    {
        [DllImport("libc", SetLastError = true)]
        private static extern uint getuid();

        [DllImport("libc", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern nint getgrgid(uint id);

        [DllImport("libc", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern nint getpwuid(uint id);

        internal struct Group
        {
            public string Name;
            public string Password;
            public uint Gid;
            public nint Members;
        }

        internal struct Passwd
        {
            public string Name;
            public string Password;
            public uint Uid;
            public uint Gid;
            public string GECOS;
            public string Directory;
            public string Shell;
        }

        public static bool IsSuperUser()
        {
            var isAdmin = false;
            try
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    var identity = WindowsIdentity.GetCurrent();
                    if (identity != null)
                    {
                        var principal = new WindowsPrincipal(identity);
                        isAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator);
                    }
                }
                else
                {
                    var currentUserId = getuid();
                    if (currentUserId == 0)
                    {
                        isAdmin = true;
                    }
                    else
                    {
                        // Read all Members from Group root
                        var groupInfoPtr = getgrgid(0);
                        var members = new List<string>();
                        if (groupInfoPtr != nint.Zero)
                        {
                            var groupInfo = Marshal.PtrToStructure<Group>(groupInfoPtr);
                            if (groupInfo.Members != nint.Zero)
                            {
                                nint currentPtr = groupInfo.Members;
                                while (true)
                                {
                                    nint memberPtr = Marshal.ReadIntPtr(currentPtr);
                                    if (memberPtr == nint.Zero)
                                    {
                                        break;
                                    }

                                    var member = Marshal.PtrToStringAnsi(memberPtr);
                                    if (string.IsNullOrEmpty(member))
                                    {
                                        break;
                                    }

                                    members.Add(member);
                                    currentPtr += nint.Size;
                                }
                            }
                        }

                        var userInfoPtr = getpwuid(currentUserId);
                        if (userInfoPtr != nint.Zero)
                        {
                            var userInfo = Marshal.PtrToStructure<Passwd>(userInfoPtr);
                            if (members.Any(x => string.Compare(x, userInfo.Name, true) == 0))
                            {
                                isAdmin = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new SdkException("Admin Rights could not determined for current User.", ex);
            }

            return isAdmin;
        }
    }
}
