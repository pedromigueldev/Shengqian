using Microsoft.Maui.ApplicationModel;

namespace Shengqian.Android;

public class LocalNetworkPermission : Permissions.BasePlatformPermission
{
	public override (string androidPermission, bool isRuntime)[] RequiredPermissions =>
		OperatingSystem.IsAndroidVersionAtLeast(37)
			? [("android.permission.ACCESS_LOCAL_NETWORK", true)]
			: [];
}
